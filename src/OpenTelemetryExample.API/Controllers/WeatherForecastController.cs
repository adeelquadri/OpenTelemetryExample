//using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Mvc;
//using OpenTelemetryExample.API.Configs;
using OpenTelemetryExample.API.Services;

namespace OpenTelemetryExample.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger, IMetricsService metricsService) : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger = logger;
        private readonly IMetricsService _metricsService = metricsService;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Testing 123");
            _metricsService.RecordHit();

            IEnumerable<WeatherForecast> weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            _logger.LogDebug("Weather Forecasts {@WeatherForecasts}", weatherForecasts);

            foreach (WeatherForecast weatherForecast in weatherForecasts)
            {
                _metricsService.RecordColdTemp(weatherForecast);
            }

            //DiagnosticsConfig.HitCouter.Add(1,
            //    new KeyValuePair<string, object?>("test", "Random Value"),
            //    new KeyValuePair<string, object?>("bad.idea", weatherForecasts));

            //foreach (var item in weatherForecasts.Where(w => w.TemperatureC < 0))
            //{
            //    DiagnosticsConfig.Cold.Record(item.TemperatureC, new KeyValuePair<string, object?>("cold.summary", item.Summary));
            //}

            //foreach (var item in weatherForecasts)
            //{
            //    DiagnosticsConfig.ColdHistory.Record(item.TemperatureC);
            //}

            return weatherForecasts;
        }
    }
}
