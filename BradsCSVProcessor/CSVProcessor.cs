using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BradsCSVProcessor
{
    public static class CSVProcessor
    {
        public static List<string> ProcessCSVFile(string fileString)
        {
            using (TextWriter log = new StreamWriter("log"))
            {
                log.WriteLine("------ begin file process -------");
                var indexofCommaList = new List<int>();
                for (int i = 0; i < fileString.Length; i++)
                {
                    if (fileString[i] == ',')
                    {
                        indexofCommaList.Add(i);
                    }
                }

                int[] commaLocations = indexofCommaList.ToArray();
                int firstRowLocation = commaLocations.Skip(3).FirstOrDefault();

                var firstRow = fileString.Substring(0, firstRowLocation);
                firstRow = "\"" + firstRow + "\"";

                var restOfCSV = fileString;
                var restOfCommaLocations = commaLocations.Skip(3).ToArray();
                commaLocations = indexofCommaList.Skip(3).ToArray();

                List<string> csvRows = new List<string>();
                csvRows.Add(firstRow);


                List<int> everyRow = new List<int>();
                for (int i = 0; i < restOfCommaLocations.Length; i++)
                {
                    if (i % 14 == 0)
                    {
                        everyRow.Add(restOfCommaLocations[i]);
                    }
                }

                log.WriteLine("rows are inbetween: " + string.Join(", ", everyRow));

                int end = 1;
                for (int start = 0; start < everyRow.Count; start++)
                {
                    if (end == everyRow.Count)
                    {
                        break;
                    }
                    var endSkipComma = everyRow[end] - (everyRow[start] - 1);
                    var itemToAdd = "\"" + restOfCSV.Substring(everyRow[start] + 1, endSkipComma) + "\"";
                    csvRows.Add(itemToAdd);
                    log.WriteLine($"\n {itemToAdd}");
                    end++;
                }
                log.WriteLine("------ end file process ------- \n \n \n");
                return csvRows;
            }
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
