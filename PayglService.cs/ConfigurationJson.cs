using System;
using System.Collections.Generic;
using System.Text;

namespace PayglService.cs
{
    public class UserData
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class System
    {
        public string PathToImportFiles { get; set; }
    }

    public class DataBase
    {
        public string Address { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Table { get; set; }
    }

    public class Settings
    {
        public string Language { get; set; }
        public System System { get; set; }
        public UserData UserData { get; set; }
        public DataBase DataBase { get; set; }
    }

    public class Ignored
    {
        public string DescriptionRegex { get; set; }
        public string TitleRegex { get; set; }
    }

    public class Schematic
    {
        public string DescriptionRegex { get; set; }
        public string TitleRegex { get; set; }
        public string Description { get; set; }
        public string Frequence { get; set; }
        public string Importance { get; set; }
        public List<string> Tags { get; set; }
    }

    public class RootObject
    {
        public Settings Settings { get; set; }
        public List<Ignored> Ignored { get; set; }
        public List<Schematic> Schematic { get; set; }
    }
}
