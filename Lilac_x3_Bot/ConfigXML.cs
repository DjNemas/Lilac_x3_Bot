using System;
using System.Linq;
using System.Xml.Linq;

namespace Lilac_x3_Bot
{
    public class ConfigXML
    {
        // Member
        private Tools t = new Tools();
        XDocument configXML;
        static private string configFolder = @"E:\Visual Studio Projekte\dev\configs\";
        static private string configFileName = "config.xml";
        private string configPath = configFolder + configFileName;
       
        public XDocument LoadConfigXML()
        {
            try
            {
                configXML = XDocument.Load(configPath);
                return configXML;
            }
            catch (Exception)
            {
                bool create = this.CreateConfigXML();
                if (create == true)
                {
                    configXML = XDocument.Load(configPath);
                    return configXML;
                }
                else
                {
                    return null;
                }  
            }
        }

        private bool CreateConfigXML()
        {
            try
            {
                XDocument config = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                                    new XElement("Init",
                                                        new XElement("Token", "Token Here")
                                                    )
                                                );
                config.Save(configPath);
                Console.WriteLine("config.xml war nicht vorhanden und wurde erstellt.");
                this.t.CWLTextColor("Bitte die config.xml anpassen!", ConsoleColor.Yellow);
                return true;
            }
            catch (Exception e)
            {
                this.t.CWLTextColor("config.xml konnte nicht erstellt werden.\nMore Details:", ConsoleColor.Red);
                Console.WriteLine(e);
                return false;
            }
        }

        public string GetToken(XDocument configFile)
        {
            var token = from config in configFile.Descendants("Init")
                        select config;
            try
            {
                string tmp = "";
                foreach (var item in token)
                {
                    tmp = item.Element("Token").Value;
                }
                return tmp;
            }
            catch (Exception)
            {
                t.CWLTextColor("Can't get Token from config.xml", ConsoleColor.Red);
                return null;
            }
        }
    }
}
