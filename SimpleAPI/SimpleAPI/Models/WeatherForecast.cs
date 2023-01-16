namespace SimpleAPI.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int CurrentTemperatureC { get; set; }
        public int CurrentTemperatureF => 32 + (int)(CurrentTemperatureC / 0.5556);
        public int[] HourlyTemperatureC { get; set; } = { };
        public int[] HourlyTemperatureF => HourlyTemperatureC.Select(t => 32 + (int)(t / 0.5556)).ToArray();
        public string? Summary { get; set; }
    }
}