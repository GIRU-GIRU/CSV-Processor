using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace BradsCSVProcessor
{
    class Program
    {
        public static List<String> csvRows;
        public static string[] filePaths;
        static void Main(string[] args)
        {
            Flavour.ConfigureConsole();
            CSVProcessor.CheckOrCreateOutputDirectory();

            Thread.Sleep(600);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("\n \n   Enter file path \n");
            var folderLocation = Console.ReadLine();
            Console.WriteLine("Starting .. \n");
            try
            {
               filePaths = Directory.GetFiles(folderLocation, "*.txt",
                                    SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Something's gone wrong.. \n \n" + ex.Message);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }

            int i = 0;
            foreach (var file in filePaths)
            {               
                try
                {
                    csvRows = CSVProcessor.ProcessCSVFile(File.ReadAllText(file));
                    Console.WriteLine($"Processed {file}");
                    CSVProcessor.StoreCSV(file, csvRows);
                    Console.WriteLine($"Stored {Path.GetFileName(file)} \n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting {file}: " + ex.Message);
                    throw;
                }
                i++;
            }

            Console.WriteLine($"\n\nProcessed {i} documents successfully ! :)");
            Console.WriteLine("Files will be in your output folder");
            Console.ReadKey();
        }
    }
}
