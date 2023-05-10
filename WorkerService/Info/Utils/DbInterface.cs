using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Configuration;
using System.Data.Common;

namespace Info.Utils
{
    internal class DbInterface
    {
        protected IConfiguration Configuration { get; }

        private readonly string _connectionString;

        private SqlConnection? MyConnection;

        protected readonly string MethodName = MethodBase.GetCurrentMethod().ReflectedType.Name;

        protected readonly Loggers Loggers = new();
        protected string? SqlConnectionString = string.Empty;
        protected readonly string CompName = Environment.MachineName;

        // Get the IP from GetHostByName method of dns class.
        private readonly string _ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].MapToIPv4().ToString();

        public DbInterface(IConfiguration configuration, string connectionStr)
        {
            Configuration = configuration;
            _connectionString = connectionStr; //Tools.GetConnectionString(Configuration);
        }

    public SqlConnection MySqlConnection()
        {
            SqlConnectionString ??= _connectionString;

            var mySqlConnection = new SqlConnection();
            try
            {
                if(mySqlConnection.State is ConnectionState.Closed or ConnectionState.Broken) {

                    mySqlConnection = new SqlConnection(SqlConnectionString);
                    mySqlConnection.Open();
                }
            }
            catch (Exception ex){
                Loggers.LogMethodsErrorDetails(MethodName, ex, 0, 0);
            }

            return mySqlConnection;
        }

        public async Task<object?> ExecRecords(int action, string sqlStr,
        SqlCommand? cmdObject = null)
        {
            try
            {
                if (!ReferenceEquals(cmdObject, null))
                {
                    SqlConnectionString = cmdObject.Connection.ConnectionString;
                }

                MyConnection = MySqlConnection();

                if (MyConnection.State is ConnectionState.Broken or ConnectionState.Closed)
                {
                    return null;
                }

                var dt = new DataTable();
                var ds = new DataSet();
                SqlCommand? myCommand;
                SqlDataAdapter? myAdapter = null;

                switch (action)
                {
                    case 1:
                        myAdapter?.Fill(dt);
                        MyConnection.Close();
                        return dt;

                    case 2:
                        myCommand = new SqlCommand(sqlStr, MyConnection);
                        myCommand.ExecuteNonQuery();
                        MyConnection.Close();

                        dt.Clear();
                        dt.Columns.Add("Successful", typeof(string));
                        return dt;

                    case 3:
                        myCommand = new SqlCommand(sqlStr, MyConnection);
                        var scalarItem = myCommand.ExecuteScalar();
                        MyConnection.Close();
                        return scalarItem;

                    case 4:
                        myCommand = MyConnection.CreateCommand();
                        myCommand.CommandText = sqlStr;
                        return await myCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                    case 5:
                        myAdapter = new SqlDataAdapter(sqlStr, MyConnection);
                        var myDataSet = new DataSet();
                        myAdapter.Fill(myDataSet);
                        MyConnection.Close();
                        return myDataSet;

                    case 6:

                        if (cmdObject == null) return dt;
                        cmdObject.Connection = MyConnection;
                        cmdObject.CommandTimeout = 10000;
                        cmdObject.ExecuteNonQuery();

                        MyConnection.Close();

                        dt.Clear();
                        dt.Columns.Add("Successful", typeof(string));

                        return dt;

                    case 7:
                        if (cmdObject == null) return dt;
                        cmdObject.Connection = MyConnection;
                        cmdObject.CommandTimeout = 10000;
                        myAdapter = new SqlDataAdapter(cmdObject);
                        myAdapter.Fill(dt);
                        MyConnection.Close();

                        return dt;

                    case 8:
                        myAdapter = new SqlDataAdapter(sqlStr, MyConnection);
                        return myAdapter;

                    case 9:
                        if (cmdObject == null) return dt;
                        cmdObject.Connection = MyConnection;
                        cmdObject.CommandTimeout = 10000;
                        myAdapter = new SqlDataAdapter(cmdObject);
                        myAdapter.Fill(ds);
                        MyConnection.Close();
                        return ds;

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                MyConnection?.Close();
                Loggers.LogMethodsErrorDetails(MethodName, ex, 1, action);
                return null;
            }
        }

        public async Task<SqlCommand?> DbQueries(string command)
        {
            var myConnection = new SqlConnection(_connectionString);

            try
            {
                myConnection.Open();
                SqlCommand myCmd = new()
                {
                    CommandText = command,
                    Connection = myConnection,
                    CommandTimeout = 120,
                    CommandType = CommandType.Text
                };
                myConnection.Close();
                return myCmd;
            }
            catch (Exception ex)
            {
                Loggers.LogMethodsErrorDetails(MethodName, ex, 0, 0);
                myConnection.Close();
                return null;
            }
        }

        public async Task<string> GetParams(string itemCode, string paramCat)
        {
            try
            {
                var functionReturnValue = string.Empty;
                if (itemCode.Trim().Length <= 0)
                {
                    return functionReturnValue;
                }
                const string columns = " ItemCode ";
                const string tableName = "Params";
                var whereClause = "ParamCode='" + itemCode + "' and ParamCat = '" + paramCat + "'";
                var dtSetup = await SpExecuteStatements(0, tableName, columns, "", whereClause);

                if (dtSetup.Rows.Count > 0)
                {
                    functionReturnValue = (string.IsNullOrEmpty(dtSetup.Rows[0]["ItemCode"].ToString().Trim()) ? "" : dtSetup.Rows[0]["ItemCode"].ToString().Trim());
                }

                return functionReturnValue;
            }
            catch (Exception ex)
            {
                Loggers.LogMethodsErrorDetails(MethodName, ex, 0, 0);
                return null;
            }
        }

        public async Task<DataTable> SpExecuteStatements(int mode, string tableName = "", string columns = "",
        string values = "", string condition = "")
        {
            var myConnection = new SqlConnection(_connectionString);
            var spExecuteResult = new DataTable();
            try
            {
                myConnection.Open();
                var myCmd = new SqlCommand()
                {
                    CommandText = "[dbo].[spExecuteStatements]",
                    CommandTimeout = 120,
                    CommandType = CommandType.StoredProcedure,
                    Connection = myConnection
                };

                myCmd.Parameters.Add("@Mode", SqlDbType.Int).Value = mode;
                myCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 1000).Value = tableName;
                myCmd.Parameters.Add("@Columns", SqlDbType.VarChar, 7000).Value = columns;
                myCmd.Parameters.Add("@Values", SqlDbType.VarChar, 7000).Value = values;
                myCmd.Parameters.Add("@Condition", SqlDbType.VarChar, 7000).Value = condition;

                switch (mode)
                {
                    case 0:
                        {
                            var dataTable = new DataTable();
                            var adapter = new SqlDataAdapter(myCmd);

                            adapter.Fill(dataTable);
                            spExecuteResult = dataTable;
                            break;
                        }
                    case 1:
                        {
                            myCmd.ExecuteNonQuery();
                            spExecuteResult = null;
                            break;
                        }
                    default:
                        myConnection.Close();
                        // return await Task.FromResult(myCmd);
                        break;
                }
                return await Task.FromResult(spExecuteResult);
            }
            catch (Exception ex)
            {
                Loggers.LogMethodsErrorDetails(MethodName, ex, 0, 0);
                myConnection.Close();
                return null;
            }
        }

        public async Task<SqlCommand?> ETLProcess(int Mode, string processDesc = "", int etlStatus = 0)
        {
            var connString = new SqlConnection(_connectionString);

            try
            {
                connString.Open();
                var cmd = new SqlCommand()
                {
                    CommandText = "[dbo].[ETLProcess]",
                    CommandTimeout = 0,
                    CommandType = CommandType.StoredProcedure,
                    Connection = connString
                };

                cmd.Parameters.Add("@Mode",SqlDbType.Int).Value = Mode;
                cmd.Parameters.Add("@processDesc", SqlDbType.VarChar, 100).Value = processDesc;
                cmd.Parameters.Add("@etlStatus", SqlDbType.Int).Value = etlStatus;
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                connString.Close();
                return await Task.FromResult(cmd);
            }
            catch (Exception ex)
            {
                Loggers.LogMethodsErrorDetails(MethodName, ex, 0, 0);
                connString.Close();
                return null;
            }
        }
    }
}
