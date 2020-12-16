using System;
using System.Data.SQLite;

namespace Lilac_x3_Bot.Database
{
    public class DatabaseInit
    {
        //member
        string pathDB = @"database.db";
        SQLiteConnection connection;

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
    }
}
