using log4net;
using SoftFX.Extended;
using SoftFX.Extended.Storage;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace FdkMinimal
{
    public static class FdkHelper
    {
        public const string DefaultAddress = "ttlive.fxopen.com";
        public const string DefaultLogin = "100";
        public const string DefaultPassword = "TTqfdeppmhDR";
        static readonly ILog Log = LogManager.GetLogger(typeof(FdkHelper));
        static FdkHelper()
        {
            Connection = new FdkConnection();
        }

        public static bool ConnectToFdk(string address, string login, string password, string protocol = "fix")
        {
            var caddress = string.IsNullOrWhiteSpace(address) ? DefaultAddress : address;
            var clogin = string.IsNullOrWhiteSpace(login) ? DefaultLogin : login;
            var cpassword = string.IsNullOrWhiteSpace(password) ? DefaultPassword : password;

            try
            {
                return Connection.Connect(caddress, clogin, cpassword, protocol);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        private static FdkConnection Connection { get; set; }

        public static DataFeed Feed
        {
            get
            {
                return Connection.FeedProxy;
            }
        }
        public static DataFeedStorage Storage
        {
            get
            {
                return Connection.FeedStorage;
            }
        }
        public static DataTrade Trade
        {
            get
            {
                return Connection.TradeProxy;
            }
        }
        public static bool IsConnected{ get { return Connection.IsConnected; } }

        public static void Disconnect()
        {
            Connection.Disconnect();
        }
        public static void WriteMessage(string message)
        {
            Console.WriteLine("FdkRLib: {0}", message);
        }

        public static Double GetCreatedEpoch(DateTime created)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            var span = (created.ToLocalTime() - epoch);
            return span.TotalSeconds;
        }

        public static Double GetCreatedEpochFromText(string createdTimeStr)
        {
            var created = DateTime.Parse(createdTimeStr, CultureInfo.InvariantCulture);
            return GetCreatedEpoch(created);
        }

        public static void DisplayDate(DateTime time)
        {
            MessageBox.Show(time.ToString());
        }


        public static DateTime GetCreatedEpoch(Double value)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            var created = epoch.AddSeconds(value);
            return created;
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        public static void EnsureDirectoryCreated(string logPath)
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
        }

        public static bool IsTimeZero(DateTime startTime)
        {
            return startTime.Year == 1970 && startTime.Month == 1;
        }

        public static string ApplicationName { get { return "RFdk"; } }
        public static int ConnectionTimeout { get { return 30000; } }

        #region Accessors

        public static T? ParseEnumStr<T>(string text) where T : struct
        {
            T result;
            if (Enum.TryParse(text, out result))
                return result;
            else
                return null;
        }

        static void ValidateAllAscii(string text)
        {
            foreach (var c in text)
            {
                if (c >= 128)
                    throw new InvalidOperationException(
                        string.Format(
                            "Field's text: '{0}' is invalid. It does not use English characters", text)
                        );
            }
        }

        public static T GetFieldByName<T>(string fieldName, bool toUpperCase = false)
        {
            ValidateAllAscii(fieldName);
            var barPeriodField = typeof(T).GetField(fieldName);
            if (barPeriodField == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                            "Field's text: '{0}' is invalid. It was not a valid value", fieldName)
                            );
            }

            var result = (T)barPeriodField.GetValue(null);

            return result;
        }
        #endregion
    }

}
