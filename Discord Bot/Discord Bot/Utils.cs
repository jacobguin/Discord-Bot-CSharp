namespace Discord_Bot
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Discord;
    using Discord.Commands;

    public static class Utils
    {
        public static string GetPrefix(SocketCommandContext context)
        {
            string id = $"{context.User.Id}{context.Guild.Id}";
            if (!string.IsNullOrEmpty(Database.Read("Users", "ID", id, "Prefix")))
            {
                return Database.Read("Users", "ID", id, "Prefix");
            }
            else
            {
                Database.Write("Users", "ID", id);
                return "?";
            }
        }

        public static async Task ReportError(SocketCommandContext context, string command, Exception error)
        {
            Program.MF.Invoke(new MethodInvoker(() => { Program.MF.AddText(error.Message, System.Drawing.Color.Red); }));
            EmbedBuilder embed = new EmbedBuilder()
             .WithTitle("Error")
             .WithColor(Color.DarkRed)
             .WithDescription(error.Message)
             .WithFooter($"Error with the Command: {command}");
            await (context.Client.GetChannel(544837859362996274) as ITextChannel).SendMessageAsync("", false, embed.Build());
        }
    }
}
