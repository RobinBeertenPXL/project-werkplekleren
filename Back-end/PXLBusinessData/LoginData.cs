using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PXLData;

namespace PXLBusinessData
{
    public static class LoginData
    {
        public static string ConnectionString = "";
        public static int UserID = -1;
        public static bool LoginSuccessFul = false;
        public static void Start()
        {
            DatabaseConnection.connectionString = LoginData.ConnectionString;
            DatabaseLauncher.Start();
        }
        public static void Stop()
        {           
            DatabaseLauncher.Stop();
        }
    }
}
