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

        //private readonly ILogger _loggerA;
        //private readonly ILogger _loggerB;

        //public WeatherForecastController(ILoggerFactory loggerFactory)
        //{
        //    _loggerA = loggerFactory.CreateLogger("CategoryA");
        //    _loggerB = loggerFactory.CreateLogger("CategoryB");
        //}

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

            _logger.LogInformation("Simulation log start by {User}[{Email}]", "Jerry", "jerryc@miniasp.com");

            LogLevel[] allLevels = GetAllLogLevels();

            for (int i = 0; i < allLevels.Length; i++)
            {
                var level = allLevels[i];
                var elapsedMs = new Random().Next(1, 1000);

                _logger.Log(level, "[{id}]Processed {@Position} in {elapsedMs:000} ms.", ID++, position, elapsedMs);
            }

            _logger.LogInformation("Simulation log end by {User}[{Email}]", "Jerry", "jerryc@miniasp.com");

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

        [Route("/Operator")]
        [HttpGet]
        public ActionResult Operator()
        {
            var position = new { Latitude = 25, Longitude = 134 };

            _logger.LogInformation("[Empty]{Position}", position);
            _logger.LogInformation("[@]{@Position}", position);
            _logger.LogInformation("[$]{$Position}");

            return Ok();
        }

        private static LogLevel[] GetAllLogLevels()
        {
            var allLevels = (LogLevel[])Enum.GetValues(typeof(LogLevel));
            allLevels = allLevels.Where(l => l != LogLevel.None).ToArray();
            return allLevels;
        }
    }
}
