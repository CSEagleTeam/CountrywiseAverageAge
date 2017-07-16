using System;
using System.Configuration;
using System.IO;
using CountrywiseAverageAge.ProcessingEngine;

namespace CountrywiseAverageAge
{
    class Program
    {
        static void Main(string[] args)
        {            
            //var _sourceDirectory = ConfigurationManager.AppSettings["SourceDirectory"];
            var _destinationDirectory = ConfigurationManager.AppSettings["DestinationPath"];

            var _sourceDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\Data";

            //check source directory existence
            if (!Directory.Exists(_sourceDirectory))
            {
                Console.WriteLine("The source directory does not exist.");
                Console.ReadKey(); 
                return;
            }

            //Get all the files from source directory
            String[] files = Directory.GetFiles(_sourceDirectory);
            
            //Read all the files and get data
            var dataToBeComputed = Utility.ReadInputFiles(files);
            Console.WriteLine(string.Format("\nTotal records prior to Calculation : {0}", dataToBeComputed.Count));

            //Calculate Average age countrywise
            var computedData = new ComputeEngine().CalculateAverageAge(dataToBeComputed);
            Console.WriteLine(string.Format("Total records post Calculation : {0}", computedData.Count));
            
            //Write output data to json file
            Utility.WriteOutputJsonFile(_destinationDirectory, computedData);
            Console.ReadKey();            
        }
    }
}
