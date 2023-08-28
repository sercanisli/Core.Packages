﻿using Core.CrossCuttingConcerns.Serilog.ConfigurationModels;
using Core.CrossCuttingConcerns.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Serilog.Logger
{
    public class MsSqlLogger:LoggerServiceBase
    {
        public MsSqlLogger(IConfiguration configuration)
        {
            MsSqlLogConfiguration logConfiguration = configuration.GetSection("SeriLogConfiguration:MsSqlConfiguration")
               .Get<MsSqlLogConfiguration>() ?? throw new Exception(SerilogMessages.NullOptionsMessage);

            MSSqlServerSinkOptions sinkOptions = new()
            {
                TableName = logConfiguration.TableName,
                AutoCreateSqlDatabase = logConfiguration.AutoCreateSqlTable
            };

            ColumnOptions columnOptions = new();

            global::Serilog.Core.Logger serilogConfiguration = new LoggerConfiguration().WriteTo
                .MSSqlServer(logConfiguration.ConnectionString, sinkOptions, columnOptions:columnOptions).CreateLogger();

            Logger = serilogConfiguration;
        }
    }
}
