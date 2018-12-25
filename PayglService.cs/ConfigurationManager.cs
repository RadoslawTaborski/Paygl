using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PayglService.cs
{
    public static class ConfigurationManager
    {
        private static RootObject _config;

        public static void ReadConfig(string pathToJson)
        {
            string json = File.ReadAllText(pathToJson);
            _config = JsonConvert.DeserializeObject<RootObject>(json);
        }

        public static List<Ignored> IgnoredTransaction()
        {
            return _config.Ignored;
        }

        public static List<Schematic> SchematicTransaction()
        {
            return _config.Schematic;
        }

        public static string Language()
        {
            return _config.Settings.Language;
        }

        public static UserData User()
        {
            return _config.Settings.UserData;
        }

        public static DataBase DataBaseData()
        {
            return _config.Settings.DataBase;
        }
    }
}
