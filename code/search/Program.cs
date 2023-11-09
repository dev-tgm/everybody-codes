using System;
using System.Linq;
using CommandLine;
using data;

namespace search
{
    class Program
    {
        static void Main(string[] args)
        {
            IDataProvider provider = new CsvDataProvider();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => 
                {
                    var cameras = provider.GetCameras()
                        .Where(x =>
                            // Case insensitive check if the camera name contains our search name.
                            x.Camera.Contains(o.Name, StringComparison.InvariantCultureIgnoreCase)
                        );

                    foreach (var camera in cameras) 
                    {
                        Console.WriteLine(camera.ToString());
                    }
                });
        }
    }

    /// <summary>
    /// Helper class to define cli arguments in. To be used by the CommandlineParser.
    /// </summary>
    class Options 
    {
        [Option("name", Required = true, HelpText = "Name to search for in the camera database.")]
        public string Name { get; set; }
    }
}
