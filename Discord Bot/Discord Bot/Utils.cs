﻿using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

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

        public static async Task ReportError(SocketCommandContext Context, string Command, Exception Error)
        {
            Program.MF.AddText(Error.Message, System.Drawing.Color.Red);
            EmbedBuilder Embed = new EmbedBuilder()
             .WithTitle("Error")
             .WithColor(Color.DarkRed)
             .WithDescription(Error.Message)
             .WithFooter($"Error with the Command: {Command}");
            await (Context.Client.GetChannel(544837859362996274) as ITextChannel).SendMessageAsync("", false, Embed.Build());
        }
    }
}
