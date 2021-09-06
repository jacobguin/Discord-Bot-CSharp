namespace Discord_Bot.Commands.Music
{
    using System;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord_Bot.Code_Support.Music;

    public class Stop : ModuleBase<SocketCommandContext>
    {
        [Command("Stop")]
        [Summary("Stops the bot from playing music")]
        public async Task StopMusic()
        {
            try
            {
                if (Database.Read<bool>("music", "server_id", Context.Guild.Id.ToString(), "playing"))
                {
                    string raw = Database.Read<string>("music", "server_id", Context.Guild.Id.ToString(), "stop");
                    if (raw.Contains(Context.Message.Author.Id.ToString()))
                    {
                        await Context.Channel.SendMessageAsync("you have already voted to stop.");
                    }
                    else
                    {
                        string[] people = raw.Split('|');
                        int skips = people.Length - 1;
                        Database.Update("music", "server_id", Context.Guild.Id.ToString(), Database.CreateParameter("stop", $"{raw}{Context.Message.Author.Id}|".ToString()));
                        double meat = Math.Round((double)Playing.PeopleInCall(Context) / 2, 1);
                        if ((skips + 1) >= meat)
                        {
                            await Context.Channel.SendMessageAsync("Stoping the music");
                        }
                        else
                        {
                            double amt = Math.Round(meat, 0) - (skips + 1);
                            await Context.Channel.SendMessageAsync($"You have voted to skip, you need {amt} more {(amt == 1 ? "person" : "people")} to vote to Stop");
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
                await Utils.ReportError(Context, "Stop", ex);
            }
        }
    }
}
