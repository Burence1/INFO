using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Info.Utils
{
    internal class DbConnection
    {
        private string _methodName = string.Empty;
        private readonly Loggers _logger = new();


        public static async Task<string> GetSqlDate(DateTime dateTime)
        {
            var dateString = dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day;

            return dateString;
        }

        public static async Task<string> GetConnectionString(IConfiguration configuration,string? databaseName)
        {
            DbConnection _dbConn = new DbConnection();

            var encryptedSv = configuration["DBConnKeys:SV"];
            var encryptedDb = configuration["DBConnKeys:DB"];
            var encryptedUi = configuration["DBConnKeys:UI"];
            var encryptedPw = configuration["DBConnKeys:PW"];
            var dbConn = configuration["AppSettings:DBConn"];

            return await Task.FromResult(await _dbConn.ConfigureConn(dbConn, encryptedSv, encryptedDb, encryptedUi, encryptedPw));
        }

        public async Task<string> ConfigureConn(string dbConn, string encrSv, string encrDb, string encrUi, string encrPw)
        {
            var connectionString = string.Empty;

            try
            {
                connectionString = dbConn;

                if (connectionString.Trim().Length == 0 || string.IsNullOrEmpty(connectionString))
                {
                    _logger.CreateLogs(_methodName + "-> Connection String Missing");
                    return connectionString;
                }

                var connSrv = encrSv;
                var connDb = encrDb;
                var connUi = encrUi;
                var connPass = encrPw;

                //------- Server Name
                connectionString = connectionString.Contains("[{SV}]") && connSrv != ""
                    ? connectionString.Replace("[{SV}]", connSrv)
                    : "";

                //------- Database Name
                connectionString = connectionString.Contains("[{DB}]") && connDb != ""
                    ? connectionString.Replace("[{DB}]", connDb)
                    : "";

                //------- User Name
                connectionString = connectionString.Contains("[{UI}]") && connUi != ""
                    ? connectionString.Replace("[{UI}]", connUi)
                    : "";

                //------- User password
                connectionString = connectionString.Contains("[{PW}]") && connPass != ""
                    ? connectionString.Replace("[{PW}]", connPass)
                    : "";

                return connectionString;
            }
            catch (Exception ex)
            {
                _methodName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name;
                _logger.CreateLogs(_methodName + "->" + ex.Message);
                return connectionString;
            }
        }
    }
}
