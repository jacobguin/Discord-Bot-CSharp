using Discord.Commands;
using System.Threading.Tasks;

namespace Discord_Bot.Commands
{
    public class SetPrefix : ModuleBase<SocketCommandContext>
    {
        [Command("setprefix"), Summary("Changes the user prefix")]
        public async Task sp(params string[] prefix)
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
                    Database.Update("Users", "Prefix", "ID", $"{Context.User.Id}{Context.Guild.Id}", prefix[0]);
                    await Context.Channel.SendMessageAsync($"your prefix is now '{prefix[0]}'");
                }
            }
        }
    }
}
