using log4net;
using SoftFX.Extended;
using SoftFX.Extended.Storage;
using System;
using System.Linq;

namespace FdkMinimal
{
    public class FdkConnection : IDisposable
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(FdkConnection));
        private object _syncObj = new object();

        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Protocol { get; private set; }
        public bool IsConnected { get; private set; }
        public LogoutReason LastError { get; private set; }
        public bool HasError { get { return LastError != LogoutReason.None; } }
        public bool IsLrpProtocol { get { return Protocol.Trim().ToUpper() == "LRP"; } }
        public bool IsFixProtocol { get { return Protocol.Trim().ToUpper() == "FIX"; } }
        public DataTrade TradeProxy { get; set; }
        public DataFeed FeedProxy { get; set; }
        public DataFeedStorage FeedStorage { get; private set; }
        public string RootPath { get; set; }
        private ConnectionStringBuilder TradeConnection
        {
            get
            {
                return new FixConnectionStringBuilder()
                {
                    TargetCompId = "EXECUTOR",
                    ProtocolVersion = FixProtocolVersion.TheLatestVersion.ToString(),
                    SecureConnection = false,
                    Address = Address,
                    Username = Username,
                    Password = Password,
                    Port = 5002,
                    DecodeLogFixMessages = true,
                    FixEventsFileName = string.Format("{0}.trade.events.log", Username),
                    FixMessagesFileName = string.Format("{0}.trade.messages.log", Username),
                    FixLogDirectory = FdkEnvironment.LogDir
                };
            }
        }
        private ConnectionStringBuilder FeedConnection
        {
            get
            {
                return IsFixProtocol ? (ConnectionStringBuilder)
                 new FixConnectionStringBuilder()
                 {
                     TargetCompId = "EXECUTOR",
                     ProtocolVersion = FixProtocolVersion.TheLatestVersion.ToString(),
                     SecureConnection = false,
                     Port = 5001,
                     DecodeLogFixMessages = true,
                     Address = this.Address,
                     Username = this.Username,
                     Password = Password,
                     FixEventsFileName = string.Format("{0}.trade.events.log", Username),
                     FixMessagesFileName = string.Format("{0}.trade.messages.log", Username),
                     FixLogDirectory = FdkEnvironment.LogDir
                 } :
                 new LrpConnectionStringBuilder
                 {
                     Port = 5010,
                     Address = Address,
                     Username = Username,
                     Password = Password,
                     EventsLogFileName = string.Format("{0}\\{1}.lrp.feed.events.log", FdkHelper.ApplicationName, Username),
                     MessagesLogFileName = string.Format("{0}\\{1}.lrp.feed.messages.log", FdkHelper.ApplicationName, Username),
                 };
            }
        }

        public bool Connect(string address, string username, string password, string protocol)
        {
            Disconnect();

            _logger.InfoFormat("Connecting to {0} (login: {1}, protocol: {2})", address, username, protocol);

            var isSupported = (new[] { "fix", "lrp" }).Contains(protocol.ToLower());
            if (!isSupported)
                throw new ArgumentException("Unsupported protocol", "protocol");

            FdkHelper.EnsureDirectoryCreated(FdkEnvironment.StoreDir);
            FdkHelper.EnsureDirectoryCreated(FdkEnvironment.LogDir);

            Address = address;
            Username = username;
            Password = password;
            Protocol = protocol;

            TradeProxy = new DataTrade(TradeConnection.ToString());
            TradeProxy.Logout += ProxyLogout;

            FeedProxy = new DataFeed(FeedConnection.ToString());
            FeedProxy.Logout += ProxyLogout;
            FeedStorage = new DataFeedStorage(FdkEnvironment.StoreDir, StorageProvider.Ntfs, FeedProxy, true);

            if (FeedProxy.Start(FdkHelper.ConnectionTimeout) && TradeProxy.Start(FdkHelper.ConnectionTimeout))
            {
                IsConnected = true;
                _logger.InfoFormat("Connected successfully", username, address);
            }

            return IsConnected;
        }

        private void ProxyLogout(object sender, SoftFX.Extended.Events.LogoutEventArgs e)
        {
            lock (_syncObj)
            {
                if (IsConnected)
                {
                    IsConnected = false;
                    LastError = e.Reason;
                    if (HasError)
                        _logger.InfoFormat("Connection failed (Reason: {0})", LastError);
                }
            }
        }

        public void Disconnect()
        {
            if (TradeProxy != null)
            {
                TradeProxy.Logout -= ProxyLogout;
                TradeProxy.Stop();
                TradeProxy.Dispose();
                TradeProxy = null;
            }

            if (FeedProxy != null)
            {
                FeedProxy.Logout -= ProxyLogout;
                FeedProxy.Stop();
                FeedProxy.Dispose();
                FeedProxy = null;
            }

            if (FeedStorage != null)
            {
                FeedStorage.Dispose();
                FeedStorage = null;
            }

            IsConnected = false;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }

    public class ConnectionException : ApplicationException
    {
        public ConnectionException() { }
        public ConnectionException(string message) : base(message) { }
        public ConnectionException(string message, LogoutReason reason) : base(message) { }

        public LogoutReason Reason { get; set; }

        public override string ToString()
        {
            return string.Format("{0}. Reason: {1}", Message, Reason);
        }
    }
}