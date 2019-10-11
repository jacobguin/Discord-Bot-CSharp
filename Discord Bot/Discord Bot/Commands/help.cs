using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Commands
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("Help"), Summary("Help Command")]
        public async Task help(params string[] Command)
        {
            string Prefix = Utils.GetPrefix(Context);
            if (Command.Length == 0)
            {

                EmbedBuilder Embed = new EmbedBuilder();
                Embed.Title = "Jacob Bot Commands";

                string DEC = "```markdown\n";

                foreach (CommandInfo Info in Program.Commands.Commands)
                {
                    DEC = DEC + "- " + Prefix + Info.Name + "\n";
                }

                Embed.WithDescription($"{DEC}```");
                Embed.WithFooter("These are all the commands for the bot");

                await Context.Channel.SendMessageAsync("", false, Embed.Build());
            }
            else if (Command.Length == 1)
            {
                foreach (CommandInfo Info in Program.Commands.Commands)
                {
                    if (Command[0].ToLower() == Info.Name.ToLower())
                    {
                        await Context.Channel.SendMessageAsync(Info.Summary);
                    }
                }
            }
            else
            {
                //IDK what to do here
            }
        }
    }
}
