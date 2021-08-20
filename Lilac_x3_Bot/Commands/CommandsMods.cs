using Discord;
using Discord.Commands;
using Lilac_x3_Bot.Database;
using Lilac_x3_Bot.Database.Tables;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Lilac_x3_Bot.LogBot;
using Discord.WebSocket;
using System.Data.SQLite;

namespace Lilac_x3_Bot.Commands
{
    public class CommandsMods : CommandHeader
    {
        [Command("edituser")]
        public async Task ModulsAsync([Remainder] string args = null)
        {
            // Check if right Channel
            bool check = ReadChannelGeneralAdmin();
            if (!check) return;

            // If Mod Role not Set
            if (ModRoleID == 0)
            {
                await SendToGeneralChannelAdminAsync("Die Mod rolle wurde noch nicht gesetzt, bitte dem SeverAdministrator bescheid geben!");
                return;
            }

            if (args == null)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Du hast zu wenige Argumente angegeben. Bitte nutze den Befehl wie folgt:\n>>> `" +
                    Prefix + "edituser <UserID> <Counter_Streak> <Counter_Highest_Streak> <CounterAll> <Last_Date>` \n" +
                    "**BITTE BEACHTEN**: `<Last_Date>` **MUSS** im Format `DD.MM.YYYY` angegeben werden. Eine falsche Formation **WIRD** zu Fehlern bei den nächsten Zählungen führen!");
                return;
            }
            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            // Check if to less or much Arguments
            if (countArgs.Length < 5)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu wenige Agumente!");
            }
            else if (countArgs.Length > 5)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu viele Agumente!");
            }
            else
            {
                // Check if Member has ModRole
                bool memberHasModRole = false;
                foreach (var item in Context.Guild.GetRole(ModRoleID).Members)
                {
                    if (item.Id == Context.User.Id)
                    {
                        memberHasModRole = true;
                    }
                }
                // User is Mod or Admin
                if (memberHasModRole || Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator)
                {
                    var db = new DatabaseInit().connect();
                    TablesHeader dbTable = new TablesHeader(db);

                    var table1337 = dbTable.Table1337;
                    Table1337 updateUser;
                    try
                    {
                        // Check if UserID is number
                        ulong userID = 0;
                        try
                        {
                            userID = Convert.ToUInt64(countArgs[0]);
                        }
                        catch (Exception)
                        {
                            await SendToGeneralChannelAdminAsync("<UserID> war keine Zahl!");
                            return;
                        }
                        // Get User from Database
                        updateUser = dbTable.Table1337.Single((item) => item.userid == userID);
                    }
                    catch (Exception)
                    {
                        await SendToGeneralChannelAdminAsync("Es wurde kein User mit der ID: " + countArgs[0] + " gefunden");
                        return;
                    }
                    // Check If args are Numbers
                    // <Counter_Streak>
                    try
                    {
                        updateUser.counter_streak = Convert.ToUInt32(countArgs[1]);
                    }
                    catch (Exception e)
                    {
                        await SendToGeneralChannelAdminAsync("Meow:\n" + e);
                        await SendToGeneralChannelAdminAsync("<Counter_Streak> war keine Positive Zahl!");
                        return;
                    }
                    // <Counter_Highest_Streak>
                    try
                    {
                        updateUser.counter_longest_streak = Convert.ToUInt32(countArgs[2]);
                    }
                    catch (Exception)
                    {
                        await SendToGeneralChannelAdminAsync("<Counter_Highest_Streak> war keine Positive Zahl!");
                        return;
                    }
                    try
                    {
                        updateUser.counter_all = Convert.ToUInt32(countArgs[3]);
                    }
                    catch (Exception)
                    {
                        await SendToGeneralChannelAdminAsync("<CounterAll> war keine Positive Zahl!");
                        return;
                    }

                    updateUser.date_last = countArgs[4];
                    dbTable.SubmitChanges();
                    db.Close();

                    await SendToGeneralChannelAdminAsync("User: " + Context.Guild.GetUser(Convert.ToUInt64(countArgs[0])).Username + " wurde erfolgreich Editiert\n" +
                        "Seine neuen Werte lauten wie Folgt:\n" +
                        ">>> Counter Streak: `" + countArgs[1] + "`\n" +
                        "Counter Longest Streak: `" + countArgs[2] + "`\n" +
                        "Counter Alltime: `" + countArgs[3] + "`\n" +
                        "Date Last: `" + countArgs[4] + "`");
                    LogMain("User: " + Context.Guild.GetUser(Convert.ToUInt64(countArgs[0])).Username + " wurde von " + Context.User.Username + " mit der ID " + Context.User.Id + " editiert.", LogLevel.Warning);
                }
                else
                {
                    await SendToGeneralChannelAdminAsync("Du hast keine Berechtigung. Du musst im besitz der `" + Context.Guild.GetRole(ModRoleID).Name + "` Rolle sein.");
                }
            }
        }

        [Command("showusertoday")]
        public async Task ShowUserTodayAsync([Remainder] string args = null)
        {
            await ShowUser(args, ShowUserEnum.Today);
        }

        [Command("showuseryesterday")]
        public async Task ShowUserYesterdayAsync([Remainder] string args = null)
        {
            await ShowUser(args, ShowUserEnum.Yesterday);
        }

        [Command("rollback")]
        public async Task Rollback()
        {
            if (!ReadChannelGeneralAdmin()) return;

            if (ModRoleID == 0)
            {
                await SendToGeneralChannelAdminAsync("Die Mod rolle wurde noch nicht gesetzt, bitte dem SeverAdministrator bescheid geben!");
                return;
            }

            await SendToGeneralChannelAdminAsync("Bitte nutze den Command wie folgt: `" + Prefix + "rollback <DD> <MM> <YYYY>`"); ;
        }

        [Command("rollback")]
        public async Task Rollback(string day, string month, string year)
        {
            if (!ReadChannelGeneralAdmin()) return;

            if (ModRoleID == 0)
            {
                await SendToGeneralChannelAdminAsync("Die Mod rolle wurde noch nicht gesetzt, bitte dem SeverAdministrator bescheid geben!");
                return;
            }
            Console.WriteLine(day);
            if (day.Length != 2)
            {
                await SendToGeneralChannelAdminAsync("Bitte Tag im Format DD (2 Zahlen) angeben.");
                return;
            }
            if (month.Length != 2)
            {
                await SendToGeneralChannelAdminAsync("Bitte Tag im Format MM (2 Zahlen) angeben.");
                return;
            }
            if (year.Length != 4)
            {
                await SendToGeneralChannelAdminAsync("Bitte Tag im Format YYYY (4 Zahlen) angeben.");
                return;
            }

            string backupFolder = "./backup/db/";
            string[] file = Directory.GetFiles(backupFolder, $"{day}-{month}-{year}*");
            if (file.Count() == 0)
            {
                await SendToGeneralChannelAdminAsync("Backup nicht gefunden.");
                return;
            }
            else if (file.Count() == 1)
            {
                File.Delete("./database.db");
                File.Copy(file[0], "./database.db");
                await SendToGeneralChannelAdminAsync("Backup aufgespielt.");
            }
        }

        [Command("countuser")]
        public async Task countuser(SocketGuildUser user)
        {
            if (!ReadChannelGeneralAdmin()) return;

            if (ModRoleID == 0)
            {
                await SendToGeneralChannelAdminAsync("Die Mod rolle wurde noch nicht gesetzt, bitte dem SeverAdministrator bescheid geben!");
                return;
            }

            string formatDate = "dd.MM.yyyy";
            SQLiteConnection db = this.dbClass.connect();
            TablesHeader dbTable = new TablesHeader(db);

            // select the curent users userid from database
            var table1337 = dbTable.Table1337;
            var userID = from table in table1337
                         where table.userid == user.Id
                         select table.userid;

            if (userID.Count() == 0)
            {
                // Add new User to Database if not exist
                Table1337 addNewUser = new Table1337()
                {
                    userid = user.Id,
                    username = user.Username,
                    counter_all = 1,
                    counter_streak = 1,
                    counter_longest_streak = 1,
                    date_begin = DateTime.Today.ToString(formatDate),
                    date_last = DateTime.Today.ToString(formatDate)
                };

                dbTable.Table1337.InsertOnSubmit(addNewUser);
                dbTable.SubmitChanges();

                await SendToGeneralChannelAdminAsync(user.Username + " gezählt und neu in der Datenbank angelegt.");
            }
            else
            {
                

                var userID2 = from table in table1337
                              where table.userid == user.Id
                              select table;

                // temp variable for changes
                uint counter_allQ = 0;
                uint counter_streakQ = 0;
                uint counter_logest_streakQ = 0;
                string date_lastQ = " ";

                // get current data from user
                foreach (var item in userID2)
                {
                    counter_allQ = item.counter_all;
                    counter_streakQ = item.counter_streak + 1;
                    counter_logest_streakQ = item.counter_longest_streak;
                    date_lastQ = item.date_last;
                }

                // update counter longest streak if user got higher streak
                if (counter_streakQ > counter_logest_streakQ)
                {
                    counter_logest_streakQ = counter_streakQ;
                }

                // get the user which should be updated by id
                var updateTable = dbTable.Table1337.Single((item) => item.userid == user.Id);

                // update values for user
                updateTable.username = user.Username;
                updateTable.counter_all = counter_allQ + 1;
                if (date_lastQ != DateTime.Today.AddDays(-1).ToString(formatDate))
                {
                    updateTable.counter_streak = 1;
                }
                else
                {
                    updateTable.counter_streak = counter_streakQ;
                }
                // Muss noch angepasst werden | An Tag anpassen
                updateTable.counter_longest_streak = counter_logest_streakQ;
                updateTable.date_last = DateTime.Today.ToString(formatDate);

                // send data to database when SubmitChanges() is called.
                dbTable.SubmitChanges();
                await SendToGeneralChannelAdminAsync(user.Username + " gezählt.");
            }
            db.Close();
        }

        private async Task ShowUser(string args, ShowUserEnum su)
        {
            if (!ReadChannelGeneralAdmin()) return;

            if (ModRoleID == 0)
            {
                await SendToGeneralChannelAdminAsync("Die Mod rolle wurde noch nicht gesetzt, bitte dem SeverAdministrator bescheid geben!");
                return;
            }

            if (args == null)
            {
                if (su == ShowUserEnum.Today)
                {
                    await SendToGeneralChannelAdminAsync(Context.User.Mention + " Du hast zu wenige Argumente angegeben. Bitte nutze den Befehl wie folgt:\n>>> `" +
                    Prefix + "showusertoday <UserID>`");
                }
                else if(su == ShowUserEnum.Yesterday)
                {
                    await SendToGeneralChannelAdminAsync(Context.User.Mention + " Du hast zu wenige Argumente angegeben. Bitte nutze den Befehl wie folgt:\n>>> `" +
                    Prefix + "showuseryesterday <UserID>`");
                }
                return;
            }
            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            if (countArgs.Length > 1)
            {
                await SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu viele Agumente!");
            }
            else
            {
                bool memberHasModRole = false;
                foreach (var item in Context.Guild.GetRole(ModRoleID).Members)
                {
                    if (item.Id == Context.User.Id)
                    {
                        memberHasModRole = true;
                    }
                }

                if (memberHasModRole || Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator)
                {
                    var db = new DatabaseInit().connect();
                    TablesHeader dbTable = new TablesHeader(db);

                    Table1337 updateUserToday = new Table1337();
                    Table1337_2 updateUserYesterday = new Table1337_2();

                    try
                    {
                        // Check if UserID is number
                        ulong userID = 0;
                        try
                        {
                            userID = Convert.ToUInt64(countArgs[0]);
                        }
                        catch (Exception)
                        {
                            await SendToGeneralChannelAdminAsync("<UserID> war keine Zahl!");
                            return;
                        }
                        // Get User from Database
                        if (su == ShowUserEnum.Today)
                        {
                            updateUserToday = dbTable.Table1337.Single((item) => item.userid == userID);
                        }
                        else if (su == ShowUserEnum.Yesterday)
                        {
                            updateUserYesterday = dbTable.Table1337_2.Single((item) => item.userid == userID);
                        }
                    }
                    catch (Exception)
                    {
                        await SendToGeneralChannelAdminAsync("Es wurde kein User mit der ID: " + countArgs[0] + " gefunden");
                        return;
                    }
                    if (su == ShowUserEnum.Today)
                    {
                        await SendToGeneralChannelAdminAsync("User Informationen\n" +
                        ">>> Name: `" + updateUserToday.username + "`\n" +
                        "ID: `" + updateUserToday.userid.ToString() + "`\n" +
                        "Counter Streak: `" + updateUserToday.counter_streak + "`\n" +
                        "Counter Longest Streak: `" + updateUserToday.counter_longest_streak + "`\n" +
                        "Counter All: `" + updateUserToday.counter_all + "`\n" +
                        "Date Begin: `" + updateUserToday.date_begin + "`\n" +
                        "Date Last: `" + updateUserToday.date_last + "`");
                    }
                    else if (su == ShowUserEnum.Yesterday)
                    {
                        await SendToGeneralChannelAdminAsync("User Informationen\n" +
                        ">>> Name: `" + updateUserYesterday.username + "`\n" +
                        "ID: `" + updateUserYesterday.userid.ToString() + "`\n" +
                        "Counter Streak: `" + updateUserYesterday.counter_streak + "`\n" +
                        "Counter Longest Streak: `" + updateUserYesterday.counter_longest_streak + "`\n" +
                        "Counter All: `" + updateUserYesterday.counter_all + "`\n" +
                        "Date Begin: `" + updateUserYesterday.date_begin + "`\n" +
                        "Date Last: `" + updateUserYesterday.date_last + "`");
                    }
                    
                }
                else
                {
                    await SendToGeneralChannelAdminAsync("Du hast keine Berechtigung. Du musst im besitz der `" + Context.Guild.GetRole(ModRoleID).Name + "` Rolle sein.");
                }
            }
        }
        private enum ShowUserEnum
        {
            Today,
            Yesterday
        }
    }
}
