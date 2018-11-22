using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BradsCSVProcessor
{
    public static class CSVProcessor
    {
        private static string ProcessCSVFile(string fileString)
        {

            List<string> finalList = new List<string>();

            //find first row
            var firstRowList = new List<int>();
            for (int i = 0; i < 300; i++)
            {
                if (fileString[i] == ',')
                {
                    firstRowList.Add(i);
                }
            }

            var firstRowEnd = firstRowList.Skip(3).FirstOrDefault() + 1;
            var firstRow = fileString.Substring(0, firstRowEnd);

            finalList.Add(firstRow);
            string fileStringWFR = fileString.Remove(0, firstRow.Length).Replace(" ", "");

            //find all commas
            int length = 0;
            int commaCount = 0;
            int startPoint = 0;
            string startOfRow = "\"";
            string endOfRow = "\"";
            bool firstRowBool = true;
            foreach (var letter in fileStringWFR)
            {
                length += +1;
                if (letter == ',')
                {
                    commaCount += +1;
                    if (IsFourtheenthComma(commaCount))
                    {
                        // logic to not add comma on the first row
                        startOfRow = (firstRowBool) ? "\"" : ",\"";
                        firstRowBool = false;
                        var rowToAdd = fileStringWFR.Substring(startPoint, length - 1);

                        fileStringWFR = fileStringWFR.Remove(startPoint, length);
                        finalList.Add(startOfRow + rowToAdd + endOfRow);
                        length = 0;
                    }
                }
            }

            int finalCommaChecker = 0;
            foreach (var letter in fileStringWFR)
            {
                if (letter == ',')
                {
                    finalCommaChecker++;          
                }
            }
            if (finalCommaChecker == 14)
            {
                finalList.Add(",\"" + fileStringWFR + "\"");
            }
            else
            {
                finalList.Add("," + fileStringWFR);
            }
            //var debug = string.Join("\n", finalList);    
            return string.Join("", finalList);
        }

        private static bool IsFourtheenthComma(int x)
        {
            return (x % 15) == 0;
        }

        public static bool StoreCSV(string filePath, List<string> csvRows)
        {
            try
            {
                var outputName = Path.Combine(Environment.CurrentDirectory, "output", Path.GetFileName(filePath));

                using (TextWriter tw = new StreamWriter(outputName))
                {
                    foreach (var item in csvRows)
                    {
                        tw.WriteLine(item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

       
        private static string fileLine;
        public static int RunCSVProcessing(string filePath)
        {
            List<String> csvRows = new List<string>();
            int counter = 1;
            Console.Write($"Found {filePath} !");
            try
            {
                Console.WriteLine();
                Console.Write($"Processing ....");
                MemoryStream fileMemory = new MemoryStream(Encoding.ASCII.GetBytes(File.ReadAllText(filePath)));
                StreamReader fileStream = new StreamReader(fileMemory);
                while ((fileLine = fileStream.ReadLine()) != null)
                {
                    counter++;
                    if (counter % 50 == 0)
                    {
                        Console.Write(".");
                    }
                    csvRows.Add(CSVProcessor.ProcessCSVFile(fileLine));
                }

                Console.WriteLine($"Processed {filePath}");
                StoreCSV(filePath, csvRows);
                Console.WriteLine($"Stored {Path.GetFileName(filePath)} \n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting {filePath}: " + ex.Message);
                throw;
            }
            return 1;
        }
        public static void CheckOrCreateOutputDirectory()
        {
            if (!Directory.Exists("output"))
            {
                try
                {
                    System.IO.Directory.CreateDirectory("output");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to create or find Output folder, please create one within the application context");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }
    }
}
