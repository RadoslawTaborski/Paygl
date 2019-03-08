using System.Collections.Generic;

namespace Analyzer
{
    public static class KeyWords
    {
        public static List<string> List = new List<string> { "Frequency", "Importance", "Tags", "TransactionType", "Name", "TransferType" };
        public static Dictionary<string, string> OperationProperty = new Dictionary<string, string>
        {
            ["Frequency"] = "Frequency",
            ["Importance"] = "Importance",
            ["Tags"] = "Tags",
            ["TransactionType"] = "TransactionType",
            ["Name"] = "Name",
            ["TransferType"] = "TransferType"
        };
    }
}
