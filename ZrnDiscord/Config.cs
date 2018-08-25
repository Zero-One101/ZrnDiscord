using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZrnDiscord
{
    static class Config
    {
        public static string AuthToken { get; private set; }
        public static char ControlChar { get; private set; }

        public static void ReadConfig()
        {
            var config = File.ReadAllLines("config.cfg");
            Dictionary<String, String> props = new Dictionary<string, string>();

            for (var i = 0; i < config.Length; i++)
            {
                var prop = config[i].Split('=');
                props.Add(prop[0], prop[1]);
            }

            AuthToken = props["AuthToken"];
            ControlChar = props["ControlChar"].ToCharArray()[0];
        }
    }
}
