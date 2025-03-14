using System;
using Firebase.Crashlytics;
using UnityEngine;

namespace Infrastructure.Services
{
    public class CrashlyticsService : IExceptionLoggerService
    {
        public void Initialize()
        {
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;
            Debug.Log("Crashlytics service initialized");
        }

        public void LogError(string message) =>
            Crashlytics.Log(message);

        public void LogException(Exception exception) =>
            Crashlytics.LogException(exception);

        public void Log(string message) =>
            Crashlytics.Log(message);
    }
}