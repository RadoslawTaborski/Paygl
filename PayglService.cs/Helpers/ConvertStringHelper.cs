using DataBaseWithBusinessLogicConnector.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayglService.cs.Helpers
{
    public static class ConvertStringHelper
    {
        public static Frequence ConvertToFrequence(string input, List<Frequence> frequencies)
        {
            foreach(var frequence in frequencies)
            {
                if ( frequence.Text == input)
                {
                    return frequence;
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
