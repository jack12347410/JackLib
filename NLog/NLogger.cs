using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public static class NLogger
    {
        public enum LogLevelEnum
        {
            Trace,
            Debug,
            Info,
            Warn,
            Error,
            Fatal,
        }

        private readonly static ConcurrentDictionary<string, Logger> _logger = new ConcurrentDictionary<string, Logger>();
        private readonly static object _loggerLock = new object();
        public static Logger GetLogger(LogLevelEnum logLevel) => GetLoggerByName(logLevel.ToString());
        private static Logger GetLoggerByName(string name)
        {
            if (name.IsNullOrEmpty()) name = "default";
            lock (_loggerLock)
            {
                Logger logger;
                if (_logger.TryGetValue(name, out logger))
                {
                    return logger;
                }
                else
                {
                    var newLogger = CreateLogger(name);
                    logger = _logger.GetOrAdd(name, newLogger);
                    return logger;
                }
            }
        }
        private static Logger CreateLogger(string name)
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget(name)
            {
                FileName = "${basedir}/logs/" + name + ".log",
                ArchiveFileName = "${basedir}/logs/" + name + "{#}.log",
                Layout = "${longdate}[${uppercase:${level}}] ${message} ${exception:format=ToString}",
                MaxArchiveFiles = 90,
                ArchiveEvery = FileArchivePeriod.Day,
                Encoding = Encoding.UTF8,
            };

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, fileTarget, name);
            LogManager.Configuration = config;

            return LogManager.GetLogger(name);
        }
    }
}
