using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Info.Utils;

namespace Info.Implementations
{
    internal class RetrieveCronExpressions
    {
        private string _methodName = string.Empty;
        public IConfiguration Configuration { get;}
        protected readonly Loggers Loggers = new();

        public RetrieveCronExpressions(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static async Task<string> RunETLExpression(IConfiguration configuration)
        {
            var retrievecron = new RetrieveCronExpressions(configuration);

            var result = retrievecron.ETLRun().Result;
            return await Task.FromResult(result);
        }

        private async Task<string> ETLRun()
        {
            try
            {
                return await Task.FromResult("");
            }
            catch (Exception ex)
            {
                _methodName = MethodBase.GetCurrentMethod().ReflectedType.Name;
                Loggers.LogMethodsErrorDetails(_methodName, ex,0,0);
            }
        }
    }
}
