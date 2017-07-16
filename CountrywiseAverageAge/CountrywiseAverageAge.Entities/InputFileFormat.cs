
namespace CountrywiseAverageAge.Entities
{
    /// <summary>
    /// Holds Structure of input data
    /// </summary>
    public class InputFileFormat
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string country { get; set; }
        public int? age { get; set; }
    }
}
