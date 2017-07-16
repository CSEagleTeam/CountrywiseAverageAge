using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CountrywiseAverageAge.Entities;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace CountrywiseAverageAge
{
    class Utility
    {
        private static object thisLock = new object();//Makes execution thread safe

        /// <summary>
        /// This method takes json filepath as input and returns file data
        /// </summary>
        /// <param name="inputfilePath">physical path of the json file</param>
        /// <returns>Formatted data</returns>
        private static IList<InputFileFormat> GetJsonFileData(string inputfilePath)
        {
            IList<InputFileFormat> dataCollection = null;
            var sb = new StringBuilder();

            lock (thisLock)
            {
                using (var streamReader = new StreamReader(inputfilePath))
                {
                    string jsonString = streamReader.ReadToEnd();
                    try
                    {
                        dataCollection = JsonConvert.DeserializeObject<List<InputFileFormat>>(jsonString);
                        var nullAgeData = from record in dataCollection
                                     where record.age.HasValue == false
                                     select record;
                        if (nullAgeData.Count() != 0)
                        {
                            sb.Append("Following records were not processed.......");
                            sb.Append(string.Format("File {0} has records where age is null", inputfilePath));
                            foreach (var item in nullAgeData)
                            {
                                sb.Append(string.Format("Record details:\n Name :{0}, Country : {1}", item.first_name + " " + item.last_name, item.country));
                            }
                            LogInformation(sb.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Error occured while processing file : {0}", inputfilePath));
                        Console.WriteLine("Exception raised " + ex.Message);
                    }
                }
            }
            return dataCollection;
        }

        /// <summary>
        /// This method will write the json data into the output file path
        /// </summary>
        /// <param name="outFilePath">physical path of output file</param>
        /// <param name="JsonData">Data to be written in the output file</param>
        public static void WriteOutputJsonFile(string outFilePath, object JsonData)
        {
            var jsonString = JsonConvert.SerializeObject(JsonData);
            //File.SetAttributes(outFilePath, FileAttributes.Normal);
            File.WriteAllText(outFilePath, jsonString);
        }

        /// <summary>
        /// This method will read the input files in parallel and return file data as a collection
        /// </summary>
        /// <param name="files">collection of file names</param>
        /// <returns></returns>
        public static List<InputFileFormat> ReadInputFiles(string[] files)
        {
            var allFilesData = new List<InputFileFormat>();
            Task<IList<InputFileFormat>>[] taskArray = new Task<IList<InputFileFormat>>[files.Length];

            for (var i = 0; i < files.Length; i++)
            {
                var fileName = files[i];
                taskArray[i] = Task<IList<InputFileFormat>>.Factory.StartNew(() => GetJsonFileData(fileName));
            }

            foreach (var task in taskArray)
            {
                allFilesData.AddRange(task.Result);
            }

            return allFilesData;
        }

        /// <summary>
        /// This method writes to log file
        /// </summary>
        /// <param name="message"></param>
        private static void LogInformation(string message)        
        {
            File.WriteAllText("LogFile.txt", message);            
        }
    }
}
