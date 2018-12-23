using System;
using System.Collections.Generic;
using System.IO;

namespace Importer
{
    internal class INGImporter : IImporter
    {
        public IEnumerable<Transaction> ReadTransactions()
        {
            var result = new List<Transaction>();
            try
            {
                using (StreamReader sr = new StreamReader(@"D:\Programowanie\C#\Paygl\Exports\Lista_transakcji_nr_0039364764_221218.csv", System.Text.Encoding.GetEncoding(1250)))
                {
                    string currentLine;
                    var index = 0;
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if(index<19)
                        {
                            index++;
                            continue;
                        }
                        currentLine = currentLine.Replace("\"", "");
                        currentLine = currentLine.Replace("\'", "");
                        var cells = currentLine.Split(new char[] { ';' });
                        if (cells[0] == "")
                        {
                            break;
                        }

                        var date = DateTime.ParseExact(cells[0], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        if(cells[1]=="")
                        {
                            result.Add(new Transaction(date, cells[2], cells[3], cells[4], cells[5], cells[6], decimal.Parse(cells[10]), cells[11]));
                        }
                        else
                        {
                            result.Add(new Transaction(date, cells[2], cells[3], cells[4], cells[5], cells[6], decimal.Parse(cells[8]), cells[9]));
                        }                  
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The File could not be read:");
                Console.WriteLine(e.Message);

                Console.ReadLine();
            }

            return result;
        }
    }
}