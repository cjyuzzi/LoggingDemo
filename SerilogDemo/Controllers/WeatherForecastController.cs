using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SerilogDemo.Controllers
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
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Route("/Log")]
        [HttpGet]
        public IActionResult Log()
        {
            var ID = 1;
            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;

            _logger.LogInformation("[{id}]Processed {@Position} in {elapsedMs:000} ms.", ID, position, elapsedMs);

            return Ok();
        }

        [Route("/Enrich")]
        [HttpGet]
        public IActionResult Enrich()
        {
            using (LogContext.PushProperty("Key", "Value"))
            {
                _logger.LogInformation("Enrich with serilog!");
            }
            return Ok();
        }
    }
}
