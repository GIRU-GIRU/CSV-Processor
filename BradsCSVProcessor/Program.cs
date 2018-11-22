using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.VisualBasic.CompilerServices;

namespace BradsCSVProcessor
{
    class Program
    {
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
            Thread.Sleep(300);

            bool pathInvalid = true;
            while (pathInvalid)
            {
                try
                {
                    filePaths = Directory.GetFiles(folderLocation, "*.csv",
                        SearchOption.TopDirectoryOnly);
                    pathInvalid = false;
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine("Uhm..");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("\n \n   Enter file path \n");
                    folderLocation = Console.ReadLine();
                    Console.WriteLine("Starting .. \n");
                }
            }

            int csvProcessedCount = 0;
            foreach (var filePath in filePaths)
            {
                try
                {
                    csvProcessedCount += CSVProcessor.RunCSVProcessing(filePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error occured processing {filePath}!!:" + e.Message);
                    Console.WriteLine("Continuing");
                    continue;
                }             
            }

            Console.WriteLine($"\n\nProcessed {csvProcessedCount} documents successfully ! :)");
            Console.WriteLine("Files will be in your output folder");
            var dir = Environment.CurrentDirectory + "\\output";
            
            Process.Start("explorer.exe", $"/open, {dir}");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }
    }
}
