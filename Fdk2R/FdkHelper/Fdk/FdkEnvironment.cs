using System;
using System.IO;

namespace FdkMinimal
{
    public static class FdkEnvironment
    {
        private static string _appDir;

        public static string GetAppName() { return "RFdk"; }
        public static string AppDir
        {
            get
            {
                return string.IsNullOrWhiteSpace(_appDir) ?
                  Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), GetAppName()) :
                  _appDir;
            }
            set
            {
                _appDir = value;
            }
        }
        public static string LogDir { get { return Path.Combine(AppDir, "Logs"); } }
        public static string StoreDir { get { return Path.Combine(AppDir, "Store"); } }
    }
}
