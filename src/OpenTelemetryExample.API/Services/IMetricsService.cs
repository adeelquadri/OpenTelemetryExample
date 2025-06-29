namespace OpenTelemetryExample.API.Services
{
    public interface IMetricsService
    {
        void RecordHit();
        void RecordColdTemp(WeatherForecast weatherForecast);
    }
}
