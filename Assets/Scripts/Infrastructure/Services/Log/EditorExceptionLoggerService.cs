using System;
using UnityEngine;

namespace Infrastructure.Services.Log
{
    public class EditorExceptionLoggerService : IExceptionLoggerService
    {
        public void Log(string message) =>
            Debug.Log(message);

        public void LogError(string message) =>
            Debug.LogError(message);

        public void LogException(Exception exception) =>
            Debug.LogException(exception);
    }
}