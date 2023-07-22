using Serilog.Sinks.MSSqlServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SampleProject.Core.Utilities.IoC;
using SampleProject.Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using SampleProject.Core.Utilities.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace SampleProject.Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class MsSqlLogger : LoggerServiceBase
    {
        public MsSqlLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:MsSqlConfiguration")
                                .Get<MsSqlConfiguration>() ??
                            throw new Exception(SerilogMessages.NullOptionsMessage);
            var sinkOpts = new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true };

            var columnOpts = new ColumnOptions();

            SqlColumn userId = new SqlColumn { DataType = SqlDbType.BigInt, ColumnName = "UserId", AllowNull = true };
            SqlColumn methodName = new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "Method", AllowNull = true };

            columnOpts.AdditionalColumns = new List<SqlColumn>
            {
                userId,
                methodName,
            };

            columnOpts.Store.Remove(StandardColumn.MessageTemplate);
            columnOpts.Store.Remove(StandardColumn.Properties);
            columnOpts.Store.Remove(StandardColumn.Level);

            var seriLogConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.MSSqlServer(connectionString: logConfig.ConnectionString, sinkOptions: sinkOpts, columnOptions: columnOpts)
                .CreateLogger();

            Logger = seriLogConfig;
        }
    }
}
