using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using System;
using System.Reflection;

namespace data 
{
    public class CsvDataProvider : IDataProvider
    {
        public IEnumerable<CameraData> GetCameras()
        {
            // Configure CsvHelper.
            var csvReaderConfiguration = new CsvConfiguration(cultureInfo: CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                Delimiter = ";"
            };

            // Read the file from disk. Normally this should be noted in a config file or variable.
            // Using the Content Include option inside of the csproj the csv files always get copied to the bin folder..
            // Because webapps use different content root paths, we use the assembly to get the bin path location.
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "cameras-defb.csv");
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, csvReaderConfiguration)) 
            {
                // Register our helper mapping class.
                csv.Context.RegisterClassMap<CameraDataMap>();

                // Read the first (header) row
                csv.Read();
                csv.ReadHeader();

                // Start reading all rows.
                while (csv.Read())
                {
                    CameraData record = ParseCsvRow(csv);
                    if (record != null) {
                        yield return record;
                    }
                }
            }
        }

        /// <summary>
        /// Helper functions that parses the current row from the csvreader to the CameraData class
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private CameraData ParseCsvRow(CsvReader reader) 
        {
            try 
            {
                CameraData record = reader.GetRecord<CameraData>();
                record.Number = ExtractNumberFromName(record.Camera);
                return record;
            } 
            catch (Exception) 
            {
                // TODO Add useful debugging logging here.
            }

            return null;
        }
        
        /// <summary>
        /// Parses the name of a camera and returns the id.
        /// By making this it's own function it can be tested more easily
        /// </summary>
        /// <param name="cameraName"></param>
        /// <returns>The id of the camera if found, otherwise returns -1.</returns>
        private int ExtractNumberFromName(string cameraName)
        {
            var match = Regex.Match(cameraName, @"(?<=-)-*\d+(?=.)");
            if (!string.IsNullOrEmpty(match.Value) && int.TryParse(match.Value, out int cameraNumber))
            {
                return cameraNumber;
            }

            return -1;
        }
    }
    
    /// <summary>
    /// Helper class for CsvHelper to correctly map the field to the data class.
    /// </summary>
    class CameraDataMap : ClassMap<CameraData> 
    {
        public CameraDataMap()
        {
            Map(m => m.Camera);
            Map(m => m.Latitude);
            Map(m => m.Longitude);
        }
    }
}