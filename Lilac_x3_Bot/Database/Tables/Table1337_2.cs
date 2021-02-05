using System.Data.Linq.Mapping;

namespace Lilac_x3_Bot.Database.Tables
{
    [Table(Name = "1337_2")]
    public class Table1337_2
    {
        // User ID
        [Column(Name = "userid", IsPrimaryKey = true)]
        public ulong userid { get; set; }
        // User Username
        [Column(Name = "username")]
        public string username { get; set; }
        // Count all 1337 ever wrote for user
        [Column(Name = "counter_all")]
        public uint counter_all { get; set; }
        // Count the actual streak for user
        [Column(Name = "counter_streak")]
        public uint counter_streak { get; set; }
        // set the longest streak ever had
        [Column(Name = "counter_longest_streak")]
        public uint counter_longest_streak { get; set; }
        // set date when the user first time wrote 1337
        [Column(Name = "date_begin")]
        public string date_begin { get; set; }
        // set the last date when the user wrote 1337
        [Column(Name = "date_last")]
        public string date_last { get; set; }
    }
}
