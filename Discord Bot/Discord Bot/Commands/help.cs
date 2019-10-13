using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Discord_Bot.Commands
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("Help"), Summary("Shows a list of commands and gives a summary of them.")]
        public async Task help(params string[] Command)
        {
            string Prefix = Utils.GetPrefix(Context);
            if (Command.Length == 0)
            {

                EmbedBuilder Embed = new EmbedBuilder
                {
                    Title = "Jacob Bot Commands"
                };

                string DEC = "```markdown\n";

                foreach (CommandInfo Info in Program.Commands.Commands)
                {
                    DEC += $"- {Prefix}{Info.Name}\n";
                }

                Embed.WithDescription($"{DEC}```");
                Embed.WithFooter("Bot Command List");

                await Context.Channel.SendMessageAsync("", false, Embed.Build());
            }
            else if (Command.Length == 1)
            {
                foreach (CommandInfo Info in Program.Commands.Commands)
                {
                    if (Command[0].ToLower() == Info.Name.ToLower()) await Context.Channel.SendMessageAsync(Info.Summary);
                }
            }
        }
    }
}
