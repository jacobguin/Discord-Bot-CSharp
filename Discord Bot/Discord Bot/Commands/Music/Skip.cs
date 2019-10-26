namespace Discord_Bot.Commands.Music
{
    using System;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord_Bot.Code_Support.Music;

    public class Skip : ModuleBase<SocketCommandContext>
    {
        [Command("Skip")]
        [Summary("Can skip the current song the bot is playing")]
        public async Task SkipVoid()
        {
            try
            {
                if (Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Playing") == "True")
                {
                    int skips = Database.ReadInt("Music", "Server_ID", Context.Guild.Id.ToString(), "Skip");
                    Database.Update("Music", "Skip", "Server_ID", Context.Guild.Id.ToString(), (skips + 1).ToString());
                    double meat = Math.Round((double)Playing.PeopleInCall(Context) / 2, 1);
                    if ((skips + 1) >= meat)
                    {
                        await Context.Channel.SendMessageAsync("skiping the current song");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($"You have voted to skip, you need {Math.Round(meat, 0) - (skips + 1)} more people to vote to skip");
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync("I am curently not playing music");
                }
            }
            catch (Exception ex)
            {
                await Utils.ReportError(Context, "Skip", ex);
            }
        }
    }
}
