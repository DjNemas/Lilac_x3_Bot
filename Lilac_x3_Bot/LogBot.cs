using System;
using System.IO;

namespace Lilac_x3_Bot
{
    public class LogBot
    {
        private static string environmentPath = Environment.CurrentDirectory;
        private static string logFolder = "/logs/";
        private static string formatDate = "dd.MM.yyyy";
        private static string formatTime = "HH:mm:ss";
        private static string formatFullDate = "dd.MM.yyyy HH:mm:ss";

        public static void LogMain(string msg, LogLevel mode)
        {
            string strMode = "";
            switch (mode)
            {
                case LogLevel.Log:
                    strMode = "[Log]";
                    break;
                case LogLevel.Warning:
                    strMode = "[Warning]";
                    break;
                case LogLevel.Debug:
                    strMode = "[Debug]";
                    break;
                case LogLevel.Error:
                    strMode = "[Error]";
                    break;
                case LogLevel.Dev:
                    strMode = "[Dev]";
                    break;
            }
            try
            {
                File.AppendAllText(environmentPath + logFolder + "main.log", DateTime.Now.ToString(formatFullDate) + " " + strMode + " " + msg + "\n");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(DateTime.Now.ToString(formatFullDate) + " [Error] Unable to write into Logfile\n" + e);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void DiscordAPIConsoleLog(string msg)
        {
            try
            {
                File.AppendAllText(environmentPath + logFolder + "discordAPI.log", DateTime.Now.ToString(formatDate) + " " + msg);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(DateTime.Now.ToString(formatFullDate) + " [Error] Unable to write into Logfile\n" + e);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void LogInit()
        {
            CreateFolder();
        }

        private static void CreateFolder()
        {
            if (!Directory.Exists(environmentPath + logFolder))
            {
                try
                {
                    var result = Directory.CreateDirectory(environmentPath + logFolder);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(DateTime.Now.ToString(formatFullDate) + " [Debug] Log folder created. " + result.FullName);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(DateTime.Now.ToString(formatFullDate) + " [Error] Can't create folder\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
            }
        }
        public enum LogLevel
        {
            Log,
            Warning,
            Debug,
            Error,
            Dev
        }
    }
}
