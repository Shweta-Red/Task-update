using System;
using System.Reflection;

namespace applicationInsights
{
    class Program
    {
        static void Main(string[] args)
        {
            string instrumentationKey = "Your-Instrumentation-Key-Here";

            
            var appInsightsHelper = new applicationInsights.AppInsightsHelper(instrumentationKey);

            try
            {
                
                appInsightsHelper.TrackTrace("Application started.");

                int result = 10 / 0;

           
                appInsightsHelper.TrackTrace("Application completed successfully.");
            }
            catch (Exception ex)
            {
          
                appInsightsHelper.TrackException(ex, MethodBase.GetCurrentMethod().Name, typeof(Program).FullName);
            }
        }
    }
}
