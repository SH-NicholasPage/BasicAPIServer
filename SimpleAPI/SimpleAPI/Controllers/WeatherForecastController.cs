using Microsoft.AspNetCore.Mvc;
using SimpleAPI.Models;

namespace SimpleAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private int lastRandomNum = 0;
        
        private static readonly (String, Int32)[] Summaries = 
        {
            ("Freezing", 0), ("Bracing", 5), ("Chilly", 10), ("Cool", 15), 
            ("Mild", 20), ("Warm", 25), ("Balmy", 28), ("Hot", 30), ("Sweltering", 34), ("Scorching", 38)
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        private int GenerateRandomNumber(int min, int max)
        {
            lastRandomNum = Random.Shared.Next(min, max);
            return lastRandomNum;
        }

        private String GenerateSummery(int tempC)
        {
            return Summaries.FirstOrDefault(s => tempC <= s.Item2).Item1 ?? "Scorching";
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get(int? numToGet = 5)
        {
            return Enumerable.Range(1, numToGet!.Value).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                CurrentTemperatureC = GenerateRandomNumber(-20, 50),
                Summary = GenerateSummery(lastRandomNum),
                HourlyTemperatureC = Enumerable.Range(1, 24).Select(i => Random.Shared.Next(-20, 50)).ToArray()
            })
            .ToArray();
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public Object Post([FromBody] WeatherForecast weatherForecast)
        {
            try
            {
                _logger.LogInformation("Weather forecast posted: {weatherForecast}", weatherForecast);
                return base.Ok("Post succeeded!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting weather forecast: {weatherForecast}", weatherForecast);
                return base.Problem("Post failed! " + ex.Message);
            }
        }
    }
}