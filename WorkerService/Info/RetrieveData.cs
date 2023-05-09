using Info.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Info
{
    internal class RetrieveData
    {
        private string _methodName = string.Empty;
        private IConfiguration _configuration;
        private SqlCommand? _sqlCommand;
        private string? spResults = string.Empty;
        private readonly Loggers loggers = new();

        public RetrieveData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Get all ETL enabled databases.

        //public async Task<DataTable?> ETDatabases()
        //{
        //    try
        //    {
        //        var _dbInterface = new DbInterface(_configuration,await Tools.);
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}
    }
}
