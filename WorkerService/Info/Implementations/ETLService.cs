using Info.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Info.Implementations
{
    internal class ETLService
    {
        public IConfiguration Configuration { get; }
        private readonly Loggers _logger = new();
        private string _methodName = string.Empty;

        public ETLService(IConfiguration configuration) {
            Configuration = configuration;
        }

        public async Task<bool> EtlAutomation(CancellationToken token)
        {
            var result = false;
            var retriever = new RetrieveData(Configuration);

            try
            {
                await retriever.CustomerInfoETL().ContinueWith(
                    async (customerInfoEtl) =>
                {
                    switch (customerInfoEtl.IsCompletedSuccessfully)
                    {
                        case true when customerInfoEtl.Result:
                            _logger.CreateLogs("Customer Info ETL has Completed Successfully!!");

                            await retriever.TransactionsETL().ContinueWith(async (transactionsEtl) =>
                            {
                                if (transactionsEtl.IsCompletedSuccessfully && transactionsEtl.Result)
                                {
                                    // Success
                                    _logger.CreateLogs("Transactions ETL has Completed Successfully!!");
                                }
                                else
                                {
                                    // Failed
                                    _logger.CreateLogs("Transactions ETL has failed!!");
                                }
                            }, token);
                            break;

                        case true when customerInfoEtl.Result == false:
                            result = false;
                            _logger.CreateLogs("Customer Info ETL has Completed Successfully failed!!");
                            break;
                    }
                        }, token);

                return result;
            }
            catch (Exception ex)
            {
                _logger.CreateLogs("ETL Service  failed");
                _methodName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name;
                _logger.LogMethodsErrorDetails(_methodName, ex, 0, 0);
                return false;
            }
        }
    }
}
