using System;
using System.Collections.Generic;
using System.Linq;

namespace Analyzer
{
    public static class StringExtensions
    {
        public static (List<string> substrings, List<string> separators) SplitWithSeparators(this string value, string[] separators)
        {
            var separators1 = new List<string>();

            var substrings1 = value.Split(separators, StringSplitOptions.None).ToList();
            for (var i = 0; i < substrings1.Count; i++)
            {
                substrings1[i] = substrings1[i].Trim();
            }

            var indexes = new List<KeyValuePair<int, string>>();
            foreach (var item in separators)
            {
                indexes.AddRange(IndexesOfString(value.AllIndexesOf(item), item));
                var tmp = "";
                for (var i = 0; i < item.Length; ++i)
                {
                    tmp += "X";
                }
                value = value.Replace(item, tmp);
            }

            indexes.Sort((x, y) => x.Key.CompareTo(y.Key));

            foreach (var item in indexes)
            {
                separators1.Add(item.Value.Trim());
            }

            return (substrings1, separators1);
        }

        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(Properties.strings.ExStringToFindNotEmpty, "value");
            var indexes = new List<int>();
            for (var index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index, StringComparison.Ordinal);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        private static List<KeyValuePair<int,string>> IndexesOfString(List<int> indexes, string word){
            var result = new List<KeyValuePair<int, string>>();

            foreach(var item in indexes)
            {
                result.Add(new KeyValuePair<int, string>(item, word));
            }

            return result;
        }
    }
}
