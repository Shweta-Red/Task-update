using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Diagnostics;
using System.Reflection;

namespace applicationInsights
{
    public class AppInsightsHelper
    {
        private TelemetryClient telemetryClient;

        public AppInsightsHelper(string instrumentationKey)
        {
            TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;
            telemetryClient = new TelemetryClient();
        }

        public void TrackTrace(string message)
        {
            telemetryClient.TrackTrace(message);
            telemetryClient.Flush();
        }

        public void TrackException(Exception ex, string methodName = null, string className = null)
        {
            var exceptionTelemetry = new ExceptionTelemetry(ex);
            if (!string.IsNullOrWhiteSpace(methodName))
                exceptionTelemetry.Properties["Method"] = methodName;
            if (!string.IsNullOrWhiteSpace(className))
                exceptionTelemetry.Properties["Class"] = className;

            telemetryClient.TrackException(exceptionTelemetry);
            telemetryClient.Flush();
        }

        public void TrackDependency(string dependencyTypeName, string target, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            var dependencyTelemetry = new DependencyTelemetry
            {
                Type = dependencyTypeName,
                Target = target,
                Name = dependencyName,
                Data = data,
                Timestamp = startTime,
                Duration = duration,
                Success = success
            };

            telemetryClient.TrackDependency(dependencyTelemetry);
            telemetryClient.Flush();
        }

        public void TrackEvent(string eventName, string properties = null)
        {
            var eventTelemetry = new EventTelemetry(eventName);

            if (!string.IsNullOrWhiteSpace(properties))
            {
                var propertyArray = properties.Split(';');
                foreach (var property in propertyArray)
                {
                    var keyValue = property.Split('=');
                    if (keyValue.Length == 2)
                        eventTelemetry.Properties[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }

            telemetryClient.TrackEvent(eventTelemetry);
            telemetryClient.Flush();
        }
    }
}
