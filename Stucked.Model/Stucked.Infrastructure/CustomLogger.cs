using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Stucked.Infrastructure
{
    public interface ILogger
    {
        void Info(object message);
        void Debug(object message);
        void Error(object message, Exception ex);
        void Fatal(object message, Exception ex);

        bool IsDebugEnabled { get; }
    }

    public class CustomLogger : ILogger
    {
        private readonly log4net.ILog _logger;

        public CustomLogger(Type type)
        {
            _logger = log4net.LogManager.GetLogger(type);
        }

        public void Info(object message)
        {
            _logger.Info(message);
        }

        public void Debug(object message)
        {
            _logger.Debug(message);
        }

        public void Error(object message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        public void Fatal(object message, Exception ex)
        {
            _logger.Fatal(message, ex);
        }

        public bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }
    }

    /// <summary>
    /// Usage: private static readonly ILogger _logger = LogManager.GetLogger(typeof(YourTypeName));
    /// </summary>
    public static class LogManager
    {
        public static ILogger GetLogger(Type type)
        {
            return new CustomLogger(type);
        }
    }
}
