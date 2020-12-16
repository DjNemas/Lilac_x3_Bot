using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Text;

namespace Lilac_x3_Bot.Database.Tables
{
    public class TablesHeader : DataContext
    {
        public Table<Table1337> Table1337;
        // add tables here!
        public TablesHeader(IDbConnection connection) : base(connection) { }
    }
}
