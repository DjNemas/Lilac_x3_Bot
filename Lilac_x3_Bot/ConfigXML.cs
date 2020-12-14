using System;
using System.Xml.Linq;

namespace Lilac_x3_Bot
{
    public class ConfigXML
    {
        // Member
        private Tools t = new Tools();
        private string configPath = "config.xml";

        public XDocument LoadConfigXML()
        {
            try
            {
                XDocument configXML = XDocument.Load(configPath);
                return configXML;
            }
            catch (Exception)
            {
                this.CreateConfigXML();

                return null;
            }
        }

        private void CreateConfigXML()
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
            }
            catch (Exception e)
            {
                this.t.CWLTextColor("config.xml konnte nicht erstellt werden.\nMore Details:", ConsoleColor.Red);
                Console.WriteLine(e);
            }
        }

    }
}
