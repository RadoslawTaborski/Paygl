using System;
using System.Collections.Generic;
using System.IO;

namespace Importer
{
    internal class IngImporter : IImporter
    {
        public IEnumerable<Transaction> ReadTransactions(string path)
        {
            var result = new List<Transaction>();
            try
            {
                using (var sr = new StreamReader(path, System.Text.Encoding.GetEncoding(1250)))
                {
                    string currentLine;
                    var flag = false;
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (currentLine.Contains("\"Data transakcji\";\"Data księgowania\";"))
                        {
                            flag = true;
                            continue;
                        }
                        if(!flag)
                        {
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
                        result.Add(cells[8] == ""
                            ? new Transaction(date, cells[2], cells[3], cells[4], cells[5], cells[6],
                                decimal.Parse(cells[10]), cells[11])
                            : new Transaction(date, cells[2], cells[3], cells[4], cells[5], cells[6],
                                decimal.Parse(cells[8]), cells[9]));
                    }
                }
            }
            catch (Exception)
            {
                throw new IOException(string.Format(Properties.strings.ExCouldNotBeRead, path));
            }

            return result;
        }
    }
}