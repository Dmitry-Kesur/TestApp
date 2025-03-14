using System;

namespace Infrastructure.Services
{
    public class EditorExceptionLoggerService : IExceptionLoggerService
    {
        public void Initialize() { }

        public void LogError(string message) { }

        public void LogException(Exception exception) { }
    }
}