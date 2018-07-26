using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TemplateReaderAdHocTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: <input config file> [output file]");
                return;
            }

            Console.WriteLine("Attach debugger & press return.");
            Console.ReadLine();

            string configFilename = args[0];
            TemplateData.TemplateData templateData = ReadConfig(configFilename);

            if (args.Length > 1)
            {
                string outputFilename = args[1];
                WriteTemplateDataToFile(templateData, outputFilename);
            }
            else
            {
                WriteTemplateDataToConsole(templateData);
            }
        }

        private static TemplateData.TemplateData ReadConfig(string configFileName)
        {
            if (!File.Exists(configFileName))
            {
                throw new FileNotFoundException($"File [{configFileName}] doesn't exist.");
            }

            string configString = File.ReadAllText(configFileName);
            JObject configSource = JObject.Parse(configString);

            TemplateData.TemplateData templateData = TemplateReader.TemplateReader.ReadTemplateData(configSource);
            return templateData;
        }

        private static void WriteTemplateDataToFile(TemplateData.TemplateData templateData, string outputFilename)
        {
            JObject serialized = JObject.FromObject(templateData);
            File.WriteAllText(outputFilename, serialized.ToString());
        }

        private static void WriteTemplateDataToConsole(TemplateData.TemplateData templateData)
        {
            JObject serialized = JObject.FromObject(templateData);
            Console.WriteLine(serialized.ToString());
        }
    }
}
