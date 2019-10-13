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
                await Context.Channel.SendMessageAsync("The prefix cannot have a space in it!");
            }
            else if (prefix.Length < 1)
            {
                await Context.Channel.SendMessageAsync("Please provide a prefix!");
            }
            else
            {
                if (prefix[0].Length != 1)
                {
                    await Context.Channel.SendMessageAsync("The prefix can only be one character long!");
                }
                else
                {
                    Database.Update("Users", "Prefix", "ID", $"{Context.User.Id}{Context.Guild.Id}", prefix[0]);
                    await Context.Channel.SendMessageAsync($"Your prefix is now `{prefix[0]}`");
                }
            }
        }
    }
}
