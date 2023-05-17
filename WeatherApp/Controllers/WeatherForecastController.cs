using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("GetOTELCollectorUrl")]
        public Dictionary<string, string> GetOTELCollectorUrl()
        {
            var enviromentPath = Environment.GetEnvironmentVariable("OTEL_COLLECTOR_URL");
            var url = "http://localhost:1234";
            if (enviromentPath != null)
            {
                url = enviromentPath;
            }

            var dic = new Dictionary<string, string>();
            dic.Add("name", "OTEL Collector URL");
            dic.Add("url", url);

            return dic;
        }
    }
}