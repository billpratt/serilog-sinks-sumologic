using System;
using Xunit;

namespace Serilog.Sinks.SumoLogic.Tests
{
    public class LoggerConfigurationExtensionTests
    {
        [Fact]
        public void ThrowsExceptions()
        {
            var logConfiguration = new LoggerConfiguration();
            Assert.Throws<ArgumentNullException>(() => logConfiguration.WriteTo.SumoLogic(null));
        }
    }
}
