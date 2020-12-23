using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ILoggerDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            //_logger = loggerFactory.CreateLogger<WeatherForecastController>();
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
            // 取得所有記錄等級。
            var allLevels = (LogLevel[])Enum.GetValues(typeof(LogLevel));
            allLevels = allLevels.Where(l => l != LogLevel.None).ToArray();

            // 模擬記錄日誌。
            var eventID = 1;
            for (int i = 0; i < allLevels.Length; i++)
            {
                var level = allLevels[i];

                _logger.Log(level, eventID++, $"This is a/an {level} log message.");
            }

            return Ok();
        }

        [Route("/Log2")]
        [HttpGet]
        public IActionResult Log2()
        {
            _logger.Log(LogLevel.None, "Log none");

            _logger.LogTrace("Log trace.");
            _logger.LogDebug("Log debug.");
            _logger.LogInformation("Log information.");
            _logger.LogWarning("Log warning.");
            _logger.LogError("Log error.");
            _logger.LogCritical("Log critical.");

            return Ok();
        }

        [Route("/LogScope")]
        [HttpGet]
        public async Task<IActionResult> LogScope()
        {
            using (_logger.BeginScope($"Begin scope[{Guid.NewGuid()}]"))
            {
                var stopwatch = Stopwatch.StartNew();

                await Task.Delay(1000);
                _logger.LogInformation($"foo completes at {stopwatch.Elapsed}");

                await Task.Delay(1500);
                _logger.LogInformation($"bar completes at {stopwatch.Elapsed}");

                await Task.Delay(500);
                _logger.LogInformation($"baz completes at {stopwatch.Elapsed}");
            }

            return Ok();
        }

        [Route("/LogTemplte")]
        [HttpGet]
        public IActionResult LogMessageTemplate()
        {
            var ID = 1;
            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;

            _logger.LogInformation("[{id}]Processed {@Position} in {elapsedMs:000} ms.", ID, position, elapsedMs);

            return Ok();
        }
    }
}
