using Lilac_x3_Bot.Database.Backup;
using System;
using System.Data.SQLite;
using static Lilac_x3_Bot.LogBot;

namespace Lilac_x3_Bot.Database
{
    public class DatabaseInit
    {
        //member
        string pathDB = @"database.db";
        private static bool backupTaskRunning = false;
        SQLiteConnection connection;
        Tools t = new Tools();

        public DatabaseInit()
        {
            // Starts the Backup Task for DB
            if (!backupTaskRunning)
            {
                BackupDB db = new BackupDB();
                backupTaskRunning = true;
            }
        }

        public SQLiteConnection connect()
        {

            this.connection = new SQLiteConnection("Data Source=" + pathDB);

            return connection;
        }

        public void close()
        {
            if (connection != null)
            {
                this.connection.Close();
            }
            else
            {
                Console.WriteLine("Can't close Database. Database wasn't connected.");
            }
        }

        public void InitDB()
        {
            this.CreateDB();
            this.CreatTables();
        }

        private void CreatTables()
        {
            //SELECT name FROM sqlite_master WHERE type = "table" AND name = "1337"
            var con = this.connect();
            con.Open();
            // Check if Table 1337 exist
            string stringQuery = "CREATE TABLE IF NOT EXISTS \"1337\" " +
                "(\"userid\" INTEGER UNIQUE," +
                  "\"username\"  TEXT," +
                  " \"counter_all\" INTEGER," +
                  " \"counter_streak\" INTEGER," +
                  " \"counter_longest_streak\" INTEGER," +
                  " \"date_begin\" TEXT," +
                  " \"date_last\" TEXT," +
                  " PRIMARY KEY(\"userid\"))";
            SQLiteCommand cmd = new SQLiteCommand(stringQuery, con);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    LogMain("Tabelle 1337 in der Datenbank angelegt", LogLevel.Dev);
                }
                else
                {
                    LogMain("Tabelle 1337 schon vorhanden", LogLevel.Dev);
                }
                
            }
            catch (Exception e)
            {
                LogMain(e.ToString(), LogLevel.Error);
            }

            // If not exist Create Table 1337_2 with Values
            stringQuery = "CREATE TABLE IF NOT EXISTS \"1337_2\" " +
                "(\"userid\" INTEGER UNIQUE," +
                  "\"username\"  TEXT," +
                  " \"counter_all\" INTEGER," +
                  " \"counter_streak\" INTEGER," +
                  " \"counter_longest_streak\" INTEGER," +
                  " \"date_begin\" TEXT," +
                  " \"date_last\" TEXT," +
                  " PRIMARY KEY(\"userid\"))";
            cmd = new SQLiteCommand(stringQuery, con);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                {
                    LogMain("Tabelle 1337_2 in der Datenbank angelegt", LogLevel.Dev);
                }
                else
                {
                    LogMain("Tabelle 1337_2 schon vorhanden", LogLevel.Dev);
                }
            }
            catch (Exception e)
            {
                LogMain(e.ToString(), LogLevel.Error);
            }
            con.Close();
        }

        private void CreateDB()
        {
            if (!System.IO.File.Exists(pathDB))
            {
                SQLiteConnection.CreateFile(pathDB);
                LogMain("Datenbank wurde erstellt", LogLevel.Dev);
            }
        }
    }
}
