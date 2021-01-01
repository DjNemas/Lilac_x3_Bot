using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Linq;
using Lilac_x3_Bot.Database;
using Lilac_x3_Bot.Database.Tables;

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
                await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Du hast zu wenige Argumente angegeben. Bitte nutze den Befehl wie folgt:\n>>> `" +
                    this.Prefix + "edituser <UserID> <Counter_Streak> <Counter_Highest_Streak> <CounterAll> <Last_Date>` \n" +
                    "**BITTE BEACHTEN**: `<Last_Date>` **MUSS** im Format `DD.MM.YYYY` angegeben werden. Eine falsche Formation **WIRD** zu Fehlern bei den nächsten Zählungen führen!");
                return;
            }
            args = args.ToLower();
            string[] countArgs = args.Split(' ');

            // Check if to less or much Arguments
            if (countArgs.Length < 5)
            {
                await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu wenige Agumente!");
            }
            else if (countArgs.Length > 5)
            {
                await this.SendToGeneralChannelAdminAsync(Context.User.Mention + " Zu viele Agumente!");
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
                        updateUser = dbTable.Table1337.Single((item) => item.userid == Convert.ToUInt64(countArgs[0]));
                    }
                    catch (Exception e)
                    {
                        await SendToGeneralChannelAdminAsync("Es wurde kein User mit der ID: " + countArgs[0] + " gefunden");
                        return;
                    }
                                        
                    updateUser.counter_streak = Convert.ToInt32(countArgs[1]);
                    updateUser.counter_longest_streak = Convert.ToInt32(countArgs[2]);
                    updateUser.counter_all = Convert.ToInt32(countArgs[3]);
                    updateUser.date_last = countArgs[4];
                    dbTable.SubmitChanges();
                    db.Close();

                    await SendToGeneralChannelAdminAsync("User: " + Context.Guild.GetUser(Convert.ToUInt64(countArgs[0])).Username + " wurde erfolgreich Editiert\n" +
                        "Seine neuen Werte lauten wie Folgt:\n" +
                        ">>> Counter Streak: `" + countArgs[1] + "`\n" +
                        "Counter Longest Streak: `" + countArgs[2] + "`\n" +
                        "Counter Alltime: `" + countArgs[3] + "`\n" +
                        "Date Last: `" + countArgs[4] + "`");
                    t.CWLTextColor("User: " + Context.Guild.GetUser(Convert.ToUInt64(countArgs[0])).Username + " wurde von " + Context.User.Username + " mit der ID " + Context.User.Id + " editiert.", ConsoleColor.Yellow);
                }
                else
                {
                    await SendToGeneralChannelAdminAsync("Du hast keine Berechtigung. Du musst im besitz der `" + Context.Guild.GetRole(ModRoleID).Name + "` Rolle sein.");
                }
            }
        }
    }
}
