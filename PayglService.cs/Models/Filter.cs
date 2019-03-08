using System;

namespace PayglService.cs.Models
{
    [Serializable]
    public class Filter: IFilter
    {
        public string Description { get; private set; }
        public string Query { get; private set; }

        public Filter(string description, string query)
        {
            Description = description;
            Query = query;
        }

        public void SetQuery(string query)
        {
            Query = query;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}
