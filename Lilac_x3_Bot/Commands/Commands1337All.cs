using Discord.Commands;
using Lilac_x3_Bot.Database.Tables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lilac_x3_Bot.Commands
{
    public class Commands1337All : CommandHeader
    {
        private const int NAME_LENGTH = 20;

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
                    "`.\n Deine längste Serie bisher war `" + counter_longest_streak + "`.");
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
        public async Task Feature1337HighscoreAsync([Remainder] string args = null)
        {
            bool check = ReadChannel1337();
            if (!check) return;

            if (args == null)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Du hast zu wenige Argumente angegeben. Bitte nutze den Befehl wie folgt:\n>>> `" +
                    Prefix + "1337highscore <Anzahl User (1-30)>`");
                return;
            }
            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            int userCount = 0;
            try
            {
                userCount = Convert.ToInt32(countArgs[0]);
            }
            catch (Exception)
            {
                await SendToGeneralChannelAdminAsync("<Anzahl User (1-30)> war keine Zahl!");
                return;
            }

            if (userCount > 30)
            {
                userCount = 30;
            }
            if (userCount < 1)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + "Was zum henker willst du mit weniger als 1 User sehen? o.O");
                return;
            }

            if (countArgs.Length > 1)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu viele Agumente!");
            }
            else
            {
                var db = dbClass.connect();
                TablesHeader dbTable = new TablesHeader(db);
                db.Close();
                var streakQuery = from user in dbTable.Table1337
                                  orderby user.counter_streak descending, user.username ascending
                                  select user;

                if (streakQuery.Count() == 0)
                {
                    await SendTo1337ChannelAsync(Context.User.Mention + " Noch keine Daten vorhanden. ");
                    return;
                }
                if (userCount > streakQuery.Count())
                {
                    await SendTo1337ChannelAsync("```Es gibt aktuell nur " + streakQuery.Count() + " Meowies in der Liste.``` <a:LilacxCryGif:708767296205881416>");
                    userCount = streakQuery.Count();
                }

                List<Table1337> users = streakQuery.ToList();

                for (int i = 0; i < users.Count; i++)
                {
                    if (users.ElementAt(i).username.Length > 20)
                    {
                        users.ElementAt(i).username = t.ShortenString(users.ElementAt(i).username, 20);
                    }
                }
                // Header
                List<string> sbList = new List<string>();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("```");
                sb.Append('-', 31).Append("Highscore List").Append('-', 32).AppendLine();
                sb.AppendFormat("|{0,4}|{1,14}|{2,22}|{3,11}|{4,10}|{5,10}\n",
                     "Rank",
                     "Counter Streak",
                     "Counter Longest Streak",
                     "Counter All",
                     "Date Begin",
                     "Date Last");

                sbList.Add(sb.ToString());

                // Index of sbList
                int index = 0;
                // Content
                for (int i = 0; i < userCount; i++)
                {
                    sb = new StringBuilder();
                    sb.AppendLine("User: " + users.ElementAt(i).username + "");

                    sb.AppendFormat("|{0,4}|{1,14}|{2,22}|{3,11}|{4,10}|{5,10}\n",
                        i + 1,
                        users.ElementAt(i).counter_streak.ToString(),
                        users.ElementAt(i).counter_longest_streak.ToString(),
                        users.ElementAt(i).counter_all.ToString(),
                        users.ElementAt(i).date_begin,
                        users.ElementAt(i).date_last
                        );

                    // Check if String is less or equal 2000 - 3 (for the last 3 ```)
                    if ((sbList[index].Length + sb.ToString().Length) <= (2000 - 3))
                    {
                        sbList[index] += sb.ToString();
                    }
                    // Add new string to sbList
                    else
                    {
                        index++;
                        string str = "```";
                        str = str + sb.ToString();
                        sbList.Add(str);
                    }
                }
                // Add ``` on every string in sbList
                for (int i = 0; i < sbList.Count; i++)
                {
                    sbList[i] += "```";
                }
                // Post every string
                foreach (var item in sbList)
                {
                    await SendTo1337ChannelAsync(item);
                }

                ////  BACKUP
                //if (streakQuery.Count() < 10)
                //{
                //    for (int i = 0; i < streakQuery.Count(); i++)
                //    {
                //        sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,14}|{4,22}|{5,11}|{6,10}|{7,10}",
                //            "#",
                //            i + 1,
                //            users.ElementAt(i).username,
                //            users.ElementAt(i).counter_streak.ToString(),
                //            users.ElementAt(i).counter_longest_streak.ToString(),
                //            users.ElementAt(i).counter_all.ToString(),
                //            users.ElementAt(i).date_begin,
                //            users.ElementAt(i).date_last
                //            );
                //        sb.AppendLine();
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < 10; i++)
                //    {
                //        sb.AppendFormat("{0,1}|{1,4}|{2,20}|{3,14}|{4,22}|{5,11}|{6,10}|{7,10}",
                //            "#",
                //            i + 1,
                //            users.ElementAt(i).username,
                //            users.ElementAt(i).counter_streak.ToString(),
                //            users.ElementAt(i).counter_longest_streak.ToString(),
                //            users.ElementAt(i).counter_all.ToString(),
                //            users.ElementAt(i).date_begin,
                //            users.ElementAt(i).date_last
                //            );
                //        sb.AppendLine();
                //    }
                //}


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
            sb.Append('-', 11).Append("Highscore List Streak").Append('-', 11).AppendLine();
            sb.AppendFormat("|{0,4}|{1,14}|{2,22}\n",
                 "Rank",
                 "Counter Streak",
                 "Counter Longest Streak"
                 );

            if (streakQuery.Count() < 10)
            {
                for (int i = 0; i < streakQuery.Count(); i++)
                {
                    sb.AppendLine("User: " + users.ElementAt(i).username + "");
                    sb.AppendFormat("|{0,4}|{1,14}|{2,22}",
                        i + 1,
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
                    sb.AppendLine("User: " + users.ElementAt(i).username + "");
                    sb.AppendFormat("|{0,4}|{1,14}|{2,22}",
                        i + 1,
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
            sb.Append('-', 8).Append("Highscore List Counter").Append('-', 9).AppendLine();
            sb.AppendFormat("|{0,4}|{1,11}|{2,10}|{3,10}\n",
                 "Rank",
                 "Counter All",
                 "Date Begin",
                 "Date Last");


            if (streakQuery.Count() < 10)
            {
                for (int i = 0; i < streakQuery.Count(); i++)
                {
                    sb.AppendLine("User: " + users.ElementAt(i).username + "");
                    sb.AppendFormat("|{0,4}|{1,11}|{2,10}|{3,10}",
                        i + 1,
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
                    sb.AppendLine("User: " + users.ElementAt(i).username + "");
                    sb.AppendFormat("|{0,4}|{1,11}|{2,10}|{3,10}",
                        i + 1,
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
            channelID = _configXML.GetChannelUID("Feature1337", "Listen1337FromChannel");

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
