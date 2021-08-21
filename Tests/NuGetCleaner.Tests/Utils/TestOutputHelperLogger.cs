using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NuGetCleaner.Tests.Utils
{
    public class TestOutputHelperLogger<T> : ILogger<T> where T : class
    {
        private const string EndOfLine = "[EOL]";
        private readonly ITestOutputHelper testOutputHelper;

        public TestOutputHelperLogger(ITestOutputHelper testOutputHelper)
        {
            this.Name = typeof(T).Name;
            this.testOutputHelper = testOutputHelper;
        }

        public string Name { get; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                var message = formatter?.Invoke(state, exception);
                var messageLine = $"{DateTime.UtcNow} - {logLevel} - {this.Name} - {message} {EndOfLine}";
                this.testOutputHelper.WriteLine(messageLine);
            }
            catch (InvalidOperationException)
            {
                // TestOutputHelperLogger throws an InvalidOperationException
                // if it is no longer associated with a test case.
            }
        }
    }
}
