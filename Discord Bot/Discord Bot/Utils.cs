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
            if (!string.IsNullOrEmpty(Database.Read<string>("users", "id", id, "prefix")))
            {
                return Database.Read<string>("users", "id", id, "prefix");
            }
            else
            {
                Database.Insert("users", Database.CreateParameter("id", id), Database.CreateParameter("prefix", "?"));
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
            await (context.Client.GetChannel(880582600513716267) as ITextChannel).SendMessageAsync("", false, embed.Build());
        }
    }
}
