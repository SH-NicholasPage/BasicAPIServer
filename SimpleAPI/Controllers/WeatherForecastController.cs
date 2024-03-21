using Microsoft.AspNetCore.Mvc;
using SimpleAPI.Models;

namespace SimpleAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private const int MAX_TEMP = 55;
        private const int MIN_TEMP = -20;
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
            // Ensure lastRandomNum is within the range of MIN_TEMP and MAX_TEMP
            lastRandomNum = Math.Max(lastRandomNum, MIN_TEMP);
            lastRandomNum = Math.Min(lastRandomNum, MAX_TEMP);
            return lastRandomNum;
        }

        private String GenerateSummery(int tempC)
        {
            return Summaries.FirstOrDefault(s => tempC <= s.Item2).Item1 ?? "Scorching";
        }

        private (int[], String[]) GenerateHourlyForecast()
        {
            int[] hourlyTemps = new int[24];
            String[] hourlySummaries = new String[24];
            for (int i = 0; i < 24; i++)
            {
                hourlyTemps[i] = GenerateRandomNumber(lastRandomNum - 5, lastRandomNum + 5);
                hourlySummaries[i] = GenerateSummery(hourlyTemps[i]);
            }

            return (hourlyTemps, hourlySummaries);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public WeatherForecast Get(int? numToGet = 5)
        {
            WeatherForecast wf = new WeatherForecast
            {
                CurrentDate = DateTime.Now,
                CurrentTemperatureC = GenerateRandomNumber(MIN_TEMP, MAX_TEMP),
                Summary = GenerateSummery(lastRandomNum),
                HourlyForecast = new HourlyForecast[numToGet!.Value]
            };

            for (int i = 0; i < numToGet; i++)
            {
                (int[], String[]) hourly = GenerateHourlyForecast();
                wf.HourlyForecast[i] = new HourlyForecast
                {
                    Date = DateTime.Now.AddDays(i),
                    HourlyTemperatureC = hourly.Item1,
                    Summary = hourly.Item2,
                };
            }

            return wf;
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