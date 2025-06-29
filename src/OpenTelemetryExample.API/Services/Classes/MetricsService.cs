using System.Diagnostics.Metrics;

namespace OpenTelemetryExample.API.Services.Classes
{
    public class MetricsService : IMetricsService
    {
        private readonly Counter<int> _hitCouter;
        private readonly Gauge<int> _coldTemps;
        private readonly Histogram<int> _tempHistory;

        public MetricsService(IMeterFactory meterFactory)
        {
            Meter meter = meterFactory.Create(AppDomain.CurrentDomain.FriendlyName);

            _hitCouter = meter.CreateCounter<int>("hits.count", description: "No of times accessed weather Get method");
            _coldTemps = meter.CreateGauge<int>("cold.temps", "c", "Temps below 0c");
            _tempHistory = meter.CreateHistogram<int>("temp.history", "c", "History of temps");
        }

        public void RecordHit()
        {
            _hitCouter.Add(1);
        }

        public void RecordColdTemp(WeatherForecast weatherForecast)
        {
            if (weatherForecast.TemperatureC < 0)
            {
                _coldTemps.Record(weatherForecast.TemperatureC
                    , new KeyValuePair<string, object?>("weatherforecast.summary", weatherForecast.Summary)
                    , new KeyValuePair<string, object?>("weatherforecast.temperaturef", weatherForecast.TemperatureF));
            }
        }
    }
}
