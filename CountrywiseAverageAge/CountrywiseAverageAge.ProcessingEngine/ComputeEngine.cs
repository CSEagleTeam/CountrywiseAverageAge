using System.Collections.Generic;
using System.Linq;
using CountrywiseAverageAge.Entities;

namespace CountrywiseAverageAge.ProcessingEngine
{
    public class ComputeEngine
    {
        /// <summary>
        /// This method will process the input data and return results with country and average age
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public IList<JsonOutFormat> CalculateAverageAge(IList<InputFileFormat> inputData)
        {
            if (inputData == null)
                return null;
            var outJsonData = from item in inputData
                              where item.age != null
                              group item by item.country into countryGroup
                              select new JsonOutFormat
                              {
                                  country = countryGroup.Key,
                                  average_age = countryGroup.Average(x => x.age)
                              };

            return outJsonData.ToList<JsonOutFormat>();
        }

        
    }
}
