using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Moq;
using Xunit;
using applicationInsights.Helpers;

namespace applicationInsights.Tests
{
    public class AppInsightsHelperTests
    {
        [Fact]
        public void TrackTrace_Should_Call_TrackTrace()
        {
            // Arrange
            var telemetryClientMock = new Mock<ITelemetryClient>();
            var telemetryConfigurationMock = new Mock<TelemetryConfiguration>();

            telemetryConfigurationMock.SetupGet(c => c.DefaultTelemetrySink)
                .Returns(new TelemetrySinkConfiguration { TelemetryProcessorChainBuilder = new TelemetryProcessorChainBuilder() });

            TelemetryConfiguration.Active = telemetryConfigurationMock.Object;
            telemetryClientMock.SetupGet(tc => tc.InstrumentationKey).Returns("TestKey");

            var appInsightsHelper = new AppInsightsHelper(telemetryClientMock.Object);

            // Act
            appInsightsHelper.TrackTrace("Test message");

            // Assert
            telemetryClientMock.Verify(tc => tc.TrackTrace("Test message"), Times.Once);
        }

        [Fact]
        public void TrackException_Should_Call_TrackException()
        {
            // Arrange
            var telemetryClientMock = new Mock<ITelemetryClient>();
            var telemetryConfigurationMock = new Mock<TelemetryConfiguration>();

            telemetryConfigurationMock.SetupGet(c => c.DefaultTelemetrySink)
                .Returns(new TelemetrySinkConfiguration { TelemetryProcessorChainBuilder = new TelemetryProcessorChainBuilder() });

            TelemetryConfiguration.Active = telemetryConfigurationMock.Object;
            telemetryClientMock.SetupGet(tc => tc.InstrumentationKey).Returns("TestKey");

            var appInsightsHelper = new AppInsightsHelper(telemetryClientMock.Object);

            // Act
            appInsightsHelper.TrackException(new System.Exception("Test exception"));

            // Assert
            telemetryClientMock.Verify(tc => tc.TrackException(It.IsAny<ExceptionTelemetry>()), Times.Once);
        }
    }
}
