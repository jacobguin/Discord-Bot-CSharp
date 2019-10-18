using Discord;
using Discord.Commands;
using Discord_Bot.Code_Support.Music;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Discord_Bot.Commands.Music
{
    
    public class Queue_Clear : ModuleBase<SocketCommandContext>
    {
        [Command("Queue"), Summary("Queue for music"), Alias("Q")]
        public async Task queue()
        {
            Queue Q = new Queue(Context);
            string desc = $"```markdown{Environment.NewLine}";
            for (int i = 0; i < Q.Items.Length - 1; i++)
            {
                WebClient web = new WebClient();
                string text = web.DownloadString($"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={Q.Items[i].YtVideo.ID}&key={Uri.EscapeUriString(Hidden_Info.API_Keys.Youtube)}");
                string title = Json.Parse(text, "items[0].snippet.title");
                desc += $"{i + 1}. [{title}]{Environment.NewLine}";
            }
            EmbedFooterBuilder F = new EmbedFooterBuilder()
                .WithText($"Requested by { Context.Message.Author.Username}#{Context.Message.Author.Discriminator}")
                .WithIconUrl(Context.Message.Author.GetAvatarUrl());
            EmbedBuilder E = new EmbedBuilder()
                .WithDescription($"{desc}```")
                .WithTitle("Current Queue")
                .WithFooter(F);
            await Context.Channel.SendMessageAsync(null, false, E.Build());
        }
    }
}
