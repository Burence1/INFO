using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Quartz;
using Info.Utils;
using Info.Implementations;
using System.Reflection;

namespace Info.Jobs
{
    internal class ETLAutomationJob : IJob
    {
        public IConfiguration Configuration { get; }

        public ETLAutomationJob(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var logger = new Loggers();
            var retrieve = new RetrieveData(Configuration);
            var result = retrieve.GetEtlParams("Is_ETL_Enabled").Result;

            try
            {
                if(result == 1)
                {
                    var source = new CancellationTokenSource();
                    var tokens = source.Token;
                    var etlService = new ETLService(Configuration);

                    var etlPauseBetweenFailure = retrieve.GetEtlParams("etlPauseBetweenFailure").Result;

                    var retryPolicyNeedsTrueResponse = Policy.HandleResult<bool>(b => b != true)
                        .WaitAndRetryForever(
                            sleepDurationProvider: iteration => TimeSpan.FromMinutes(etlPauseBetweenFailure),
                            onRetry: (results, time) =>
                            {
                                if (!results.Result)
                                {
                                    logger.CreateLogs("Job : Etl Process Retried");
                                }
                            });

                    _ = retryPolicyNeedsTrueResponse.Execute(() => etlService.EtlAutomation(tokens).Result);
                }
                else
                {
                    logger.CreateLogs("Job : Etl Process Retried");
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                var _methodName = MethodBase.GetCurrentMethod().ReflectedType.Name;
                logger.LogMethodsErrorDetails(_methodName, ex, 0, 0);
                throw;
            }
        }
    }
}
