using Lilac_x3_Bot.Database.Backup;
using System;
using System.Data.SQLite;

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
            this.CreateDBWIthTables();
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

        private void CreateDBWIthTables()
        {
            if (!System.IO.File.Exists(pathDB))
            {
                SQLiteConnection.CreateFile(pathDB);
                this.t.CWLTextColor("Datenbank wurde erstellt", ConsoleColor.Yellow);

                var con = this.connect();
                con.Open();

                string stringQuery = "CREATE TABLE \"1337\" " +
                    "(\"userid\" INTEGER UNIQUE," +
                      "\"username\"  TEXT," +
                      " \"counter_all\" INTEGER," +
                      " \"counter_streak\" INTEGER," +
                      " \"counter_longest_streak\" INTEGER," +
                      " \"date_begin\" TEXT," +
                      " \"date_last\" TEXT," +
                      " PRIMARY KEY(\"userid\"))";
                var cmd = new SQLiteCommand(stringQuery, con);

                try
                { 
                    cmd.ExecuteNonQuery();
                    con.Close();
                    this.t.CWLTextColor("Tabelle in der Datenbank angelegt", ConsoleColor.Yellow);
                }
                catch (Exception e)
                {
                    this.t.CWLTextColor(e, ConsoleColor.Red);
                }
            }
        }
    }
}
