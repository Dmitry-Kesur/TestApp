using System;

namespace Infrastructure.Services.Log
{
    public interface IExceptionLoggerService
    {
        public void Log(string message);
        public void LogError(string message);
        public void LogException(Exception exception);
    }
}