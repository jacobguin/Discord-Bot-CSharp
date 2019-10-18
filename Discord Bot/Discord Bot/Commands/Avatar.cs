using Discord.Commands;
using System.Threading.Tasks;
using Discord;

namespace Discord_Bot.Commands
{
    public class Avatar : ModuleBase<SocketCommandContext>
    {
        [Command("Avatar"), Summary("Gets a user avatar")]
        public async Task Get(params IUser[] user)
        {
            if (user.Length == 1)
            {
                EmbedBuilder E = new EmbedBuilder()
                    .WithImageUrl(user[0].GetAvatarUrl());
                await Context.Channel.SendMessageAsync(null, false, E.Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync("Too many parameters were given.");
            }
        }
    }
}
