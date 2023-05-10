using Info.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Info.Utils;
using System.Reflection;

namespace Info
{
    internal class RetrieveData
    {
        private string _methodName = string.Empty;
        private IConfiguration _configuration;
        private SqlCommand? _sqlCommand;
        private string? spResults = string.Empty;
        protected readonly Loggers Loggers = new();

        public RetrieveData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Get all ETL enabled databases.

        public async Task<DataTable?> ETLDatabases()
        {
            try
            {
                var _dbInterface = new DbInterface(_configuration, await Utils.DbConnection.GetConnectionStringMaster(_configuration));

                _sqlCommand = await _dbInterface.DbQueries("Select DbName from dbo.Details (Nolock)" +
                    "where Is_ETL_Enabled = 1");

                var databases = await _dbInterface.ExecRecords(7, "Select DbName from dbo.Details (Nolock)" +
                    "where Is_ETL_Enabled = 1", _sqlCommand) as DataTable;

                return await Task.FromResult(databases);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().ReflectedType.Name;
                Loggers.LogMethodsErrorDetails(_methodName, ex, 0, 0);

                throw;
            }
        }

        //Get Top ETL db
        public async Task<string?> ETLDatabase()
        {
            try
            {
                var connStringMaster = await DbConnection.GetConnectionStringMaster(_configuration);
                var _dbInterface = new DbInterface(_configuration, connStringMaster);

                _sqlCommand = await _dbInterface.DbQueries("Select Top 1 DbName from dbo.Details (Nolock)" +
                    "where Is_ETL_Enabled = 1");

                var _etlDb = await _dbInterface.ExecRecords(3, "Select Top 1 DbName from dbo.Details (Nolock)" +
                    "where Is_ETL_Enabled = 1", _sqlCommand) as string;

                return await Task.FromResult(_etlDb);
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().ReflectedType.Name;
                Loggers.LogMethodsErrorDetails(_methodName, ex, 0, 0);

                throw;
            }
        }

        //Todo: Add SP
        public async Task<bool> CustomerInfoETL()
        {
            try
            {
                var _dbinterface = new DbInterface (_configuration, await DbConnection.GetConnectionString(
                    _configuration, await ETLDatabase()));
                _sqlCommand = await _dbinterface.ETLProcess(1);

                _ = _dbinterface.ExecRecords(7,"", _sqlCommand);
                spResults = (string?)(_sqlCommand?.Parameters["@Result"].Value);

                return await Task.FromResult(await CheckSpResults(spResults));
            }
            catch (Exception ex) {
                _methodName = MethodBase.GetCurrentMethod().ReflectedType.Name;
                Loggers.LogMethodsErrorDetails(_methodName, ex, 0, 0);
                throw;
            }
        }

        //public async Task<bool> TransactionsETL()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        protected async Task<bool> CheckSpResults(string? spResults)
        {
            try
            {
                Loggers.CreateLogs($"Stored Procedure Results : {spResults} ");

                var results = spResults != null;

                var subResult = spResults?[..1];
                if (subResult != "1")
                {
                    results = false;
                }

                return await Task.FromResult(results);
            }
            catch (Exception e)
            {
                _methodName = MethodBase.GetCurrentMethod().ReflectedType.Name;
                Loggers.LogMethodsErrorDetails(_methodName, e, 0, 0);
                throw;
            }
        }
    }
}
