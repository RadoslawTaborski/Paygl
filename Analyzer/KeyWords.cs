using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public static class KeyWords
    {
        public static List<string> List = new List<string> { "Frequence", "Importance", "Tags", "TransactionType", "Name" };
        public static Dictionary<string, string> OperationProperty = new Dictionary<string, string>
        {
            ["Frequence"] = "Frequence",
            ["Importance"] = "Importance",
            ["Tags"] = "Tags",
            ["TransactionType"] = "TransactionType",
            ["Name"] = "Name"
        };
    }
}
