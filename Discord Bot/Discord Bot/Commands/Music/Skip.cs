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
                    string raw = Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Skip");
                    if (raw.Contains(Context.Message.Author.Id.ToString()))
                    {
                        await Context.Channel.SendMessageAsync("you have already voted to skip.");
                    }
                    else
                    {
                        string[] people = raw.Split('|');
                        int skips = people.Length - 1;
                        Database.Update("Music", "Skip", "Server_ID", Context.Guild.Id.ToString(), $"{raw}{Context.Message.Author.Id}|".ToString());
                        double meat = Math.Round((double)Playing.PeopleInCall(Context) / 2, 1);
                        if ((skips + 1) >= meat)
                        {
                            await Context.Channel.SendMessageAsync("skiping the current song");
                        }
                        else
                        {
                            double amt = Math.Round(meat, 0) - (skips + 1);
                            await Context.Channel.SendMessageAsync($"You have voted to skip, you need {amt} more {(amt == 1 ? "person" : "people")} to vote to skip");
                        }
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
