namespace SimpleAPI.Models
{
    public class WeatherForecast
    {
        public DateTime CurrentDate { get; set; }
        public int CurrentTemperatureC { get; set; }
        public int CurrentTemperatureF => 32 + (int)(CurrentTemperatureC / 0.5556);
        public String? Summary { get; set; }
        public HourlyForecast[] HourlyForecast { get; set; } = new HourlyForecast[24];

        public override String ToString()
        {
            return $"CurrentDate {CurrentDate}, CurrentTemperatureC: {CurrentTemperatureC}, CurrentTemperatureF: {CurrentTemperatureF}, Summary: {Summary}";
        }
    }

    public class HourlyForecast
    {
        public DateTime Date { get; set; }
        public int[] HourlyTemperatureC { get; set; } = [];
        public int[] HourlyTemperatureF => HourlyTemperatureC.Select(t => 32 + (int)(t / 0.5556)).ToArray();
        public String[]? Summary { get; set; } = [];
    }
}