namespace Discord_Bot.Commands
{
    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    public class Avatar : ModuleBase<SocketCommandContext>
    {
        [Command("Avatar")]
        [Summary("Gets a user avatar")]
        public async Task Get(params IUser[] user)
        {
            try
            {
                if (user.Length == 1)
                {
                    EmbedBuilder embed = new EmbedBuilder()
                        .WithImageUrl(user[0].GetAvatarUrl());
                    await Context.Channel.SendMessageAsync(null, false, embed.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Too many parameters were given.");
                }
            }
            catch (Exception ex)
            {
                await Utils.ReportError(Context, "Avatar", ex);
            }
        }
    }
}
