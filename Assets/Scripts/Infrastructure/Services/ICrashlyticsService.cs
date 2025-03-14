using System;

namespace Infrastructure.Services
{
    public interface IExceptionLoggerService : IFirebaseInitialize
    {
        public void LogError(string message);
        public void LogException(Exception exception);
    }
}