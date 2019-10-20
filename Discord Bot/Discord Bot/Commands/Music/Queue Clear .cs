namespace Discord_Bot.Commands.Music
{
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord_Bot.Code_Support.Music;

    public class Queue : ModuleBase<SocketCommandContext>
    {
        [Command("Queue clear")]
        [Summary("Clears the Queue for music")]
        [Alias("Q C", "Q Clear", "Queue C")]
        public async Task Clear(params string[] args)
        {
            if (args.Length == 0)
            {
                Code_Support.Music.Queue q = new Code_Support.Music.Queue(Context);
                q.Clear();
                await Context.Channel.SendMessageAsync("I have cleared the queue.");
            }
            else if (args.Length == 1)
            {
                await Context.Channel.SendMessageAsync("You gave me bad arguments for the queue command.");
            }
            else
            {
                await Context.Channel.SendMessageAsync("You gave me bad arguments for the queue command.");
            }
        }
    }
}
