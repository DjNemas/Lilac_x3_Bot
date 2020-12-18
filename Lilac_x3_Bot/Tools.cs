using System;
using System.Collections.Generic;
using System.Text;

namespace Lilac_x3_Bot
{
    public class Tools
    {
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

        public string ShortenString(string text, int lenght)
        {
            string newText = "";
            if (text.Length >= lenght)
            {
                for (int i = 0; i < lenght; i++)
                {
                    newText += text[i];
                }
                return newText;
            }
            else
            {
                return text;
            }
        }
    }
}
