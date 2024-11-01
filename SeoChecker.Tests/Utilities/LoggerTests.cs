using SeoChecker.Utilities;
using System;
using System.IO;
using Xunit;
using static SeoChecker.Utilities.Logger;

namespace SeoChecker.Tests.Utilities
{
    public class LoggerTests : IDisposable
    {
        private readonly TextWriter _originalConsoleOut;

        public LoggerTests()
        {
            // Store the original Console.Out to restore after each test
            _originalConsoleOut = Console.Out;
        }

        [Fact]
        public void Log_InfoLevel_WritesExpectedMessage()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            Logger.Log(LogLevel.Info, "Test info message");

            var result = sw.ToString().Trim();
            Assert.Contains("[INFO]", result);
            Assert.Contains("Test info message", result);
        }

        [Fact]
        public void Log_WarningLevel_WritesExpectedMessage()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            Logger.Log(LogLevel.Warning, "Test warning message");

            var result = sw.ToString().Trim();
            Assert.Contains("[WARNING]", result);
            Assert.Contains("Test warning message", result);
        }

        [Fact]
        public void Log_ErrorLevel_WritesExpectedMessageAndException()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);
            var exception = new Exception("Test exception message");

            Logger.Log(LogLevel.Error, "Test error message", exception);

            var result = sw.ToString().Trim();
            Assert.Contains("[ERROR]", result);
            Assert.Contains("Test error message", result);
            Assert.Contains("Test exception message", result);
        }

        [Fact]
        public void Log_CacheHitLevel_WritesExpectedMessage()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            Logger.Log(LogLevel.CacheHit, "Cache hit for keywords and URL");

            var result = sw.ToString().Trim();
            Assert.Contains("[CACHEHIT]", result);
            Assert.Contains("Cache hit for keywords and URL", result);
        }

        [Fact]
        public void Log_SearchLevel_WritesExpectedMessage()
        {
            using var sw = new StringWriter();
            Console.SetOut(sw);

            Logger.Log(LogLevel.Search, "Searching for keywords and URL");

            var result = sw.ToString().Trim();
            Assert.Contains("[SEARCH]", result);
            Assert.Contains("Searching for keywords and URL", result);
        }

        public void Dispose()
        {
            // Restore the original Console.Out after each test
            Console.SetOut(_originalConsoleOut);
        }
    }
}
