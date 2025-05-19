using System;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public interface ILoggingService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception? exception = null);
        void LogDebug(string message);
    }
} 