namespace Discord_Bot.Commands
{
    using System;
    using System.Threading.Tasks;
    using Discord.Commands;

    public class SetPrefix : ModuleBase<SocketCommandContext>
    {
        [Command("setprefix")]
        [Summary("Changes the user prefix")]
        public async Task Sp(params string[] prefix)
        {
            try
            {
                if (prefix.Length > 1)
                {
                    await Context.Channel.SendMessageAsync("That is not a valid prefix!");
                }
                else if (prefix.Length < 1)
                {
                    await Context.Channel.SendMessageAsync("Please provide a prefix!");
                }
                else
                {
                    if (prefix[0].Length != 1)
                    {
                        await Context.Channel.SendMessageAsync("That is not a valid prefix!");
                    }
                    else
                    {
                        Database.Update("users", "id", $"{Context.User.Id}{Context.Guild.Id}", Database.CreateParameter("Prefix", prefix[0]));
                        await Context.Channel.SendMessageAsync($"your prefix is now '{prefix[0]}'");
                    }
                }
            }
            catch (Exception ex)
            {
                await Utils.ReportError(Context, "SetPrefix", ex);
            }
        }
    }
}
