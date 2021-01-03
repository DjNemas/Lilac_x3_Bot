using Discord;
using Discord.Commands;
using Lilac_x3_Bot.Database.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    public class Commands1337All : CommandHeader
    {
        [Command("1337streak")]
        public async Task Feature1337StreakAsync()
        {
            bool check = ReadChannel1337();
            if (!check) return;

            var db = dbClass.connect();
            TablesHeader dbTable = new TablesHeader(db);

            var streakQuery = from user in dbTable.Table1337
                              where user.userid == Context.User.Id
                              select user;

            if (streakQuery.Count() == 0)
            {
                await SendTo1337ChannelAsync(Context.User.Mention + " Du hast noch nie am 1337 Event teilgenommen. <:SadCat:766665234273402910> ");
                return;
            }
            else 
            {
                uint count_streak = 0;
                uint counter_longest_streak = 0;
                foreach (var item in streakQuery)
                {
                    count_streak = item.counter_streak;
                    counter_longest_streak = item.counter_longest_streak;
                }
                await SendTo1337ChannelAsync(">>> " + Context.User.Mention + "\n Deine aktuelle Serie liegt bei `" + count_streak +
                    "`.\n Deine längste Serie bisher war `" + counter_longest_streak + "`." );
            }
        }

        [Command("1337count")]
        public async Task Feature1337CountAsync()
        {
            bool check = ReadChannel1337();
            if (!check) return;

            var db = dbClass.connect();
            TablesHeader dbTable = new TablesHeader(db);

            var streakQuery = from user in dbTable.Table1337
                              where user.userid == Context.User.Id
                              select user;

            uint countAll = 0;
            string date_begin = "";
            string date_last = "";
            foreach (var item in streakQuery)
            {
                countAll = item.counter_all;
                date_begin = item.date_begin;
                date_last = item.date_last;
            }

            if (streakQuery.Count() == 0)
            {
                await SendTo1337ChannelAsync(Context.User.Mention + " Du hast noch nie am 1337 Event teilgenommen. <:SadCat:766665234273402910> ");
                return;
            }
            else
            {
                await SendTo1337ChannelAsync(">>> " + Context.User.Mention + "\nDu wurdest Insgesamt `" + countAll + "` mal gezählt.\n" +
                    "Deine erste Zählung war am `" + date_begin + "`.\n" +
                    "Deine letzte Zählung war am `" + date_last + "`.");
            }
        }

        [Command("1337highscore")]
        public async Task Feature1337HighscoreAsync()
        {
            bool check = ReadChannel1337();
            if (!check) return;

            var db = dbClass.connect();
            TablesHeader dbTable = new TablesHeader(db);
            db.Close();
            var streakQuery = from user in dbTable.Table1337
                              orderby user.counter_streak descending, user.username ascending
                              select user;
            List<Table1337> users = streakQuery.ToList();

            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).username.Length > 20)
                {
                    users.ElementAt(i).username = t.ShortenString(users.ElementAt(i).username, 20);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("```");
            sb.Append('-', 43).Append("Highscore List").Append('-', 42).AppendLine();
            sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,14}|{4,22}|{5,11}|{6,10}|{7,10}\n",
                 " ",
                 "Rank",
                 "Username",                 
                 "Counter Streak",
                 "Counter Longest Streak",
                 "Counter All",
                 "Date Begin",
                 "Date Last");


            if (streakQuery.Count() < 10)
            {
                for (int i = 0; i < streakQuery.Count(); i++)
                {
                    sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,14}|{4,22}|{5,11}|{6,10}|{7,10}",
                        "#",
                        i + 1,
                        users.ElementAt(i).username,                        
                        users.ElementAt(i).counter_streak.ToString(),
                        users.ElementAt(i).counter_longest_streak.ToString(),
                        users.ElementAt(i).counter_all.ToString(),
                        users.ElementAt(i).date_begin,
                        users.ElementAt(i).date_last
                        );
                    sb.AppendLine();
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,14}|{4,22}|{5,11}|{6,10}|{7,10}",
                        "#",
                        i + 1,
                        users.ElementAt(i).username,
                        users.ElementAt(i).counter_streak.ToString(),
                        users.ElementAt(i).counter_longest_streak.ToString(),
                        users.ElementAt(i).counter_all.ToString(),
                        users.ElementAt(i).date_begin,
                        users.ElementAt(i).date_last
                        );
                    sb.AppendLine();
                }
            }
            sb.Append("```");
            if (streakQuery.Count() == 0)
            {
                await SendTo1337ChannelAsync(Context.User.Mention + " Noch keine Daten vorhanden. ");
                return;
            }
            else
            {
                await SendTo1337ChannelAsync(sb.ToString());
            }

        }

        [Command("1337hstreak")]
        public async Task Feature1337HStreakAsync() 
        {
            bool check = ReadChannel1337();
            if (!check) return;

            var db = dbClass.connect();
            TablesHeader dbTable = new TablesHeader(db);
            db.Close();
            var streakQuery = from user in dbTable.Table1337
                              orderby user.counter_streak descending, user.username ascending
                              select user;
            List<Table1337> users = streakQuery.ToList();

            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).username.Length > 20)
                {
                    users.ElementAt(i).username = t.ShortenString(users.ElementAt(i).username, 20);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("```");
            sb.Append('-', 22).Append("Highscore List Streak").Append('-', 22).AppendLine();
            sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,14}|{4,22}\n",
                 " ",
                 "Rank",
                 "Username",
                 "Counter Streak",
                 "Counter Longest Streak"
                 );


            if (streakQuery.Count() < 10)
            {
                for (int i = 0; i < streakQuery.Count(); i++)
                {
                    sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,14}|{4,22}",
                        "#",
                        i + 1,
                        users.ElementAt(i).username,
                        users.ElementAt(i).counter_streak.ToString(),
                        users.ElementAt(i).counter_longest_streak.ToString()
                        );
                    sb.AppendLine();
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,14}|{4,22}",
                        "#",
                        i + 1,
                        users.ElementAt(i).username,
                        users.ElementAt(i).counter_streak.ToString(),
                        users.ElementAt(i).counter_longest_streak.ToString()
                        );
                    sb.AppendLine();
                }
            }
            sb.Append("```");
            if (streakQuery.Count() == 0)
            {
                await SendTo1337ChannelAsync(Context.User.Mention + " Noch keine Daten vorhanden. ");
                return;
            }
            else
            {
                await SendTo1337ChannelAsync(sb.ToString());
            }
        }

        [Command("1337hcount")]
        public async Task Feature1337HCountAsync()
        {
            bool check = ReadChannel1337();
            if (!check) return;

            var db = dbClass.connect();
            TablesHeader dbTable = new TablesHeader(db);
            db.Close();
            var streakQuery = from user in dbTable.Table1337
                              orderby user.counter_all descending, user.username ascending
                              select user;
            List<Table1337> users = streakQuery.ToList();

            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).username.Length > 20)
                {
                    users.ElementAt(i).username = t.ShortenString(users.ElementAt(i).username, 20);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("```");
            sb.Append('-', 20).Append("Highscore List Counter").Append('-', 19).AppendLine();
            sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,11}|{4,10}|{5,10}\n",
                 " ",
                 "Rank",
                 "Username",
                 "Counter All",
                 "Date Begin",
                 "Date Last");


            if (streakQuery.Count() < 10)
            {
                for (int i = 0; i < streakQuery.Count(); i++)
                {
                    sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,11}|{4,10}|{5,10}",
                        "#",
                        i + 1,
                        users.ElementAt(i).username,
                        users.ElementAt(i).counter_all.ToString(),
                        users.ElementAt(i).date_begin,
                        users.ElementAt(i).date_last
                        );
                    sb.AppendLine();
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,11}|{4,10}|{5,10}",
                        "#",
                        i + 1,
                        users.ElementAt(i).username,
                        users.ElementAt(i).counter_all.ToString(),
                        users.ElementAt(i).date_begin,
                        users.ElementAt(i).date_last
                        );
                    sb.AppendLine();
                }
            }
            sb.Append("```");
            if (streakQuery.Count() == 0)
            {
                await SendTo1337ChannelAsync(Context.User.Mention + " Noch keine Daten vorhanden. ");
                return;
            }
            else
            {
                await SendTo1337ChannelAsync(sb.ToString());
            }
        }

        [Command("1337info")]
        public async Task Feature1337InfoAsync()
        {
            bool check = ReadChannel1337();
            if (!check) return;

            ulong channelID;
            channelID =  _configXML.GetChannelUID("Feature1337", "Listen1337FromChannel");

            var eb = new StringBuilder();
            eb.AppendLine(">>> Feature 1337 Info\n");
            eb.AppendLine("Was ist dieses 1337 Feature?");
            if (channelID == 0)
            {
                eb.AppendLine("Genau um `13:37:00 - 13:37:59 Uhr` , werden in allen Channels `1337 und @1337`\n");
                eb.AppendLine(" pro User und Tag genau 1x gezählt.");
            }
            else
            {
                eb.Append("Genau um `13:37:00 - 13:37:59 Uhr` , werden im Channel `" + Context.Guild.GetChannel(channelID).Name + "` , ");
                eb.AppendLine("`1337 und @1337` pro User und Tag genau 1x gezählt.");
            }
            
            await SendTo1337ChannelAsync(eb.ToString());

        }
    }
}
