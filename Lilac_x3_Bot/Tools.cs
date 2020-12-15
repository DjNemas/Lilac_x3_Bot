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
    }
}
