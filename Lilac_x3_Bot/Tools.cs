using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac_x3_Bot
{
    public class Tools
    {
        public string test;
        public void CWLTextColor(string text, ConsoleColor textColor)
        {
            Console.ForegroundColor = textColor;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public string RemoveSpecificCharFromString(string yourString, char character)
        {
            string newString = "";
            for (int i = 0; i < yourString.Length; i++)
            {
                if (yourString[i] == character)
                {
                    continue;
                }
                else
                {
                    newString += yourString[i];
                }
            }
            return newString;
        }
    }
}
