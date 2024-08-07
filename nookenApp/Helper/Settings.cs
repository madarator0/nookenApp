using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nookenApp.Helper
{
    public static class Settings
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["connectionString"] ?? "";
            }
        }

        public static string PortName
        {
            get
            {
                return ConfigurationManager.AppSettings["PortName"] ?? "";
            }
        }

        public static string ApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiUrl"] ?? "";
            }
        }

        public static string MconnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["MconnectionString"] ?? "";
            }
        }
    }
}