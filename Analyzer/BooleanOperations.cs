using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public static class BooleanOperations
    {
        public static string Conjunction => "AND";
        public static string Disjunction => "OR";
    }

    public static class SetOperations
    {
        public static string NotIn => "NOT IN";
        public static string AllIn => "ALL IN";
        public static string OneIn => "ONE IN";
        public static string In => "IN";
    }

    public static class KeyWords
    {
        public static List<string> List = new List<string> { "Frequence", "Importance", "Tags", "TransactionType" };
        public static Dictionary<string, string> OperationProperty = new Dictionary<string, string>
        {
            ["Frequence"] = "Frequence",
            ["Importance"] = "Importance",
            ["Tags"] = "Tags",
            ["TransactionType"] = "TransactionType",
        };
    }
}
