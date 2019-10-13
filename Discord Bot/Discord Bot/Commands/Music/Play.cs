using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Discord_Bot.Code_Support.Music;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discord_Bot.Commands.Music
{
    public class Play : ModuleBase<SocketCommandContext>
    {
        JArray items = null;
        IVoiceChannel Channel = null;
        int Success = 0;
        [Command("Play", RunMode = RunMode.Async), Summary("Music")]
        public async Task play(params string[] Search)
        {
            try
            {
                if (Search.Length == 0)
                {
                    if (string.IsNullOrEmpty(Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue")))
                    {
                        await Context.Channel.SendMessageAsync("The queue is empty. Please provide a song.");
                    }
                    else
                    {
                        Channel = Channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
                        if (Channel == null)
                        {
                            await Context.Channel.SendMessageAsync("You must be in a voice channel.");
                        }
                        else
                        {

                            if (Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Playing") != "True")
                            {
                                await Playing.StartPlaying(await Channel.ConnectAsync(), Context, Channel);
                            }
                            else
                            {
                                await Context.Channel.SendMessageAsync("The bot is already playing something.");
                            }
                        }
                    }
                }
                else
                {
                    IEnumerable<IMessage> msgs = await Context.Channel.GetMessagesAsync().FlattenAsync();
                    if (msgs.Where(m => m.Author.Id == Context.Client.CurrentUser.Id && m.Content.StartsWith("**Song Results:**")).Count() > 0)
                    {
                        await Context.Channel.SendMessageAsync("There is already a message to select a song. Please select one in order to add more songs.");
                        return;
                    }

                    string[] emojiArr = { "1⃣", "2⃣", "3⃣", "4⃣", "5⃣" };

                    Channel = Channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
                    if (Channel == null)
                    {
                        await Context.Channel.SendMessageAsync("You must be in a voice channel.");
                    }
                    else
                    {

                        string url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={Uri.EscapeUriString(string.Join(" ", Search))}&key={Uri.EscapeUriString(Hidden_Info.API_Keys.Youtube)}";

                        WebClient webClient = new WebClient();
                        string value = webClient.DownloadString(url);

                        dynamic Json = JsonConvert.DeserializeObject<dynamic>(value);
                        items = Json.items;

                        if (items.Count == 0)
                        {
                            await Context.Channel.SendMessageAsync("No search results found.");
                        }
                        else
                        {

                            int index = 0;
                            dynamic[] titles = items.Select(i =>
                            {
                                index += 1;
                                return $"{index}: {i.Value<dynamic>().snippet.title}";
                            }).ToArray();
                            string msg = $"**Song Results:**\n```\n{String.Join("\n", titles)}\n```**This message will expire in 10 seconds!**";
                            RestUserMessage m = await Context.Channel.SendMessageAsync(msg);
                            int results = items.Count;
                            int current = 1;
                            Context.Client.ReactionAdded += Client_ReactionAdded;
                            try
                            {
                                foreach (string em in emojiArr)
                                {
                                    if (current <= results)
                                    {
                                        current++;
                                        var e = new Emoji(em);
                                        await m.AddReactionAsync(e);
                                    }
                                }

                                for (int i = 0; i < 9; i++)
                                {
                                    await Task.Delay(1000);
                                    await m.ModifyAsync(h => h.Content = msg.Replace("**This message will expire in 10 seconds!**", $"**This message will expire in {9 - i} seconds!**"));
                                }

                                await Task.Delay(1000);
                                Context.Client.ReactionAdded -= Client_ReactionAdded;
                                await m.DeleteAsync();
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Utils.RepotError(Context, "Play", ex);
            }
        }

        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> Message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            string[] emojiArr = { "1⃣", "2⃣", "3⃣", "4⃣", "5⃣" };

            int num = Array.IndexOf(emojiArr, reaction.Emote.Name) + 1;

            if (reaction.UserId == Context.Client.CurrentUser.Id) return;
            IUserMessage msg = await Message.DownloadAsync();
            if (msg.Author.Id != Context.Client.CurrentUser.Id) return;
            if (!emojiArr.Contains(reaction.Emote.Name)) return;
            if (!msg.ToString().StartsWith("**Song Results:**")) return;
            if (items.Count < num) { await msg.Channel.SendMessageAsync("No result with that number found."); return; };

            Context.Client.ReactionAdded -= Client_ReactionAdded;
            await msg.DeleteAsync();
            string id = items.Value<dynamic>(num - 1);

            if (Success != 0)
            {
                _ = Start(id);
            }
        }
        private async Task Start(string id)
        {
            Queue.AddSongToQueue(Context, id);
            if (Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Playing") != "True")
            {
                IAudioClient AudioClient = await Channel.ConnectAsync();
                _ = Playing.StartPlaying(AudioClient, Context, Channel);
            }
            else
            {
                await Context.Channel.SendMessageAsync("The bot is already playing something but i added your song to the Queue");
            }
        }
    }
}
