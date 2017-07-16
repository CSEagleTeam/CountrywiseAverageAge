
namespace CountrywiseAverageAge.Entities
{
    /// <summary>
    /// Holds structure of data to be written in output file
    /// </summary>
    public class JsonOutFormat
    {
        public string country { get; set; }
        public double? average_age { get; set; }
    }
}
