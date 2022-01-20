using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static Lilac_x3_Bot.LogBot;

namespace Lilac_x3_Bot
{
    public class ConfigXML
    {
        // Member
        private Tools t = new Tools();
        XDocument _configXML;
        static private string configFolder = @"./config/";
        static private string configFileName = "config.xml";
        private string _configPath = configFolder + configFileName;
       
        public XDocument LoadConfigXML()
        {
            try
            {
                this._configXML = XDocument.Load(this._configPath);
                return this._configXML;
            }
            catch (Exception)
            {
                bool create = this.CreateConfigXML();
                if (create == true)
                {
                    this._configXML = XDocument.Load(this._configPath);
                    return this._configXML;
                }
                else
                {
                    return null;
                }  
            }
        }

        private bool CreateConfigXML()
        {
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }
            else
            {
                LogMain("Config Ordner konnte nicht erstellt werden!", LogLevel.Error);
            }

            try
            {
                XDocument config = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                                    new XElement("Config",
                                                        new XElement("Init",
                                                            new XElement("Token", "Token Here")
                                                        ), 
                                                        new XElement("General",
                                                            new XElement("Prefix", "!"),
                                                            new XElement("ModRoleID", new XAttribute("id", "0")),
                                                            new XElement("WriteIntoChannelAll", new XAttribute("id", "0")),
                                                            new XElement("WriteIntoChannelAdmin", new XAttribute("id", "0")),
                                                            new XElement("ReadFromChannelAll", new XAttribute("id", "0")),
                                                            new XElement("ReadFromChannelAdmin", new XAttribute("id", "0"))
                                                        ),
                                                        new XElement("Feature1337",
                                                            new XElement("WriteIntoChannel", new XAttribute("id", "0")),
                                                            new XElement("ReadFromChannel", new XAttribute("id", "0")),
                                                            new XElement("Listen1337FromChannel", new XAttribute("id", "0"))
                                                        )
                                                     )
                                                );
                config.Save(this._configPath);
                t.CWLTextColor("config.xml war nicht vorhanden und wurde erstellt.", ConsoleColor.Yellow);
                LogMain("config.xml war nicht vorhanden und wurde erstellt.", LogLevel.Dev);
                LogMain("Bitte die config.xml anpassen!", LogLevel.Error);
                return true;
            }
            catch (Exception e)
            {
                t.CWLTextColor("config.xml konnte nicht erstellt werden.\nMore Details: " + e, ConsoleColor.Red, true);
                LogMain("config.xml konnte nicht erstellt werden.\nMore Details: " + e, LogLevel.Error);
                return false;
            }
        }

        public string GetToken(XDocument configFile)
        {
            var tokenQuery = from config in configFile.Descendants("Init")
                             select config;
            try
            {
                string token = "";
                foreach (var item in tokenQuery)
                {
                    token = item.Element("Token").Value;
                }
                return token;
            }
            catch (Exception)
            {
                LogMain("Can't get Token from config.xml", LogLevel.Error);
                return null;
            }
        }

        public void ChangePrefix(char prefix)
        {
            this._configXML = LoadConfigXML();
            var prefixQuery = from pre in this._configXML.Descendants("General")
                              select pre;
            foreach (var item in prefixQuery)
            {
                item.Element("Prefix").Value = prefix.ToString();
            }
            this._configXML.Save(this._configPath);
        }

        public void ChangeWriteIntoChannelID(ulong id, string modul, string moduleid)
        {
            this._configXML = LoadConfigXML();
            var prefixQuery = from pre in this._configXML.Descendants(modul)
                              select pre;
            foreach (var item in prefixQuery)
            {
                item.Element(moduleid).Attribute("id").Value = id.ToString();
            }
            this._configXML.Save(this._configPath);
        }

        public char GetPrefix()
        {
            char prefix = ' ';

            this._configXML = LoadConfigXML();
            var prefixQuery = from pre in this._configXML.Descendants("General")
                              select pre;

            foreach (var item in prefixQuery)
            {
                prefix = item.Element("Prefix").Value[0];
            }
            return prefix;
        }

        public ulong GetChannelUID(string feature, string element)
        {
            this._configXML = LoadConfigXML();
            var chID = from id in this._configXML.Descendants(feature)
                       select id;

            ulong channelID = 0;
            foreach (var item in chID)
            {
                if (item.Element(element).Attribute("id").Value != "0")
                {
                    channelID = Convert.ToUInt64(item.Element(element).Attribute("id").Value);
                }
                else
                {
                    InitBot init = new InitBot();
                    if (init._devMode) LogMain($"{feature} {element} ID nicht vorhanden", LogLevel.Warning);
                }
            }
            return channelID;
        }
    }
}
