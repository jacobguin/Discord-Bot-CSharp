using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Discord_Bot
{
    public static class Utils
    {
        public static string GetPrefix(SocketCommandContext Context)
        {
            string ID = $"{Context.User.Id}{Context.Guild.Id}";
            if (!string.IsNullOrEmpty(Database.Read("Users", "ID", ID, "Prefix")))
            {
                return Database.Read("Users", "ID", ID, "Prefix");
            }
            else
            {
                Database.Write("Users", "ID", ID);
                return "?";
            }
        }

        public static async Task RepotError(SocketCommandContext Context, string Command, Exception Error)
        {
            Console.WriteLine(Error.Message, System.Drawing.Color.Red);
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.WithTitle("Error");
            Embed.WithColor(Color.DarkRed);
            Embed.WithDescription(Error.Message);
            Embed.WithFooter($"Error with the Command: {Command}");
            await Context.Client.GetGuild(543962547217498150).GetTextChannel(544837859362996274).SendMessageAsync("", false, Embed.Build());
        }
    }
}
