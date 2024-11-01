using System;

namespace SeoChecker.Utilities
{
    public enum LogLevel { Info, Warning, Error, CacheHit, Search }

    public static class Logger
    {
        public static void Log(LogLevel level, string message, Exception ex = null)
        {
            var levelString = level.ToString().ToUpper();
            Console.WriteLine($"[{levelString}] {DateTime.Now}: {message}");

            if (level == LogLevel.Error && ex != null)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
