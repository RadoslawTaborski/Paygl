using DataBaseWithBusinessLogicConnector.Entities;
using System.Collections.Generic;

namespace PayglService.cs.Helpers
{
    public static class ConvertStringHelper
    {
        public static Frequence ConvertToFrequency(string input, List<Frequence> frequencies)
        {
            foreach(var frequency in frequencies)
            {
                if ( frequency.Text == input)
                {
                    return frequency;
                }
            }
            return null;
        }

        public static Importance ConvertToImportance(string input, List<Importance> importances)
        {
            foreach (var importance in importances)
            {
                if (importance.Text == input)
                {
                    return importance;
                }
            }
            return null;
        }

        public static Tag ConvertToTag(string input, List<Tag> tags)
        {
            foreach (var tag in tags)
            {
                if (tag.Text == input)
                {
                    return tag;
                }
            }
            return null;
        }
    }
}
