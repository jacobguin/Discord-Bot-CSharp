using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Discord_Bot.Code_Support.Music
{
    public static class Playing
    {
        private static IAudioClient c;
        private static IVoiceChannel v;
        private static SocketCommandContext s;

        public static async Task StartPlaying(IAudioClient Client, SocketCommandContext Context, IVoiceChannel Channel, Queue Q)
        {
            Database.Update("Music", "Playing", "Server_ID", Context.Guild.Id.ToString(), true);
            v = Channel;
            s = Context;
            c = Client;
            Program.Client.UserVoiceStateUpdated += Client_UserVoiceStateUpdated;
            Program.Client.LoggedOut += Client_LoggedOut;
            while (true)
            {
                Q.Refresh();
                try
                {
                    try
                    {
                        if (Q.Items[0].Type == Queue.Type.End)
                        {
                            await Context.Channel.SendMessageAsync("I am done playing music");
                            Q.Clear();
                            await Stop(Client, Context);
                            break;
                        }
                        else if (Q.Items[0].Type == Queue.Type.Youtube)
                        {
                            await Context.Channel.SendMessageAsync("", false, Youtube_video_embed(Q.Items[0].YtVideo.ID.Replace("https://www.youtube.com/watch?v=", "")));
                            await Backend.SendUrlAsync(Client, Q.Items[0].YtVideo.ID);
                        }
                        else if (Q.Items[0].Type == Queue.Type.Playlist)
                        {
                            //not here yet
                        }
                    }
                    catch (Exception ex)
                    {
                        await Utils.ReportError(Context, "Play", ex);
                    }
                }
                finally
                {
                    Q.Items[0].Remove();
                }
            }
            await Stop(Client, Context);
        }

        private async static Task Client_LoggedOut()
        {
            await Stop(c, s);
        }

        private async static Task Client_UserVoiceStateUpdated(SocketUser User, SocketVoiceState BeforeState, SocketVoiceState AfterState)
        {
            if (User.Id == 508008523146199061)
            {
                if (AfterState.VoiceChannel == null)
                {
                    await Stop(c, s);
                }
            }
            else
            {
                int count = PeopleInCall(s);
                if (count == 0)
                {
                    await Stop(c, s);
                }
            }
        }

        private static async Task Stop(IAudioClient Client, SocketCommandContext Context)
        {
            Database.Update("Music", "Playing", "Server_ID", Context.Guild.Id.ToString(), false);
            await Client.StopAsync();
        }

        private static Embed Youtube_video_embed(string ID)
        {
            EmbedBuilder e = new EmbedBuilder();
            WebClient web = new WebClient();
            string text = web.DownloadString($"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={ID}&key={Uri.EscapeUriString(Hidden_Info.API_Keys.Youtube)}");
            e.Title = "Now Playing: " + '"' + Json.Parse(text, "items[0].snippet.title") + '"' + " : by: " + Json.Parse(text, "items[0].snippet.channelTitle");
            e.WithThumbnailUrl(Json.Parse(text, "items[0].snippet.thumbnails.standard.url"));
            e.WithColor(255, 0, 0);
            e.WithFooter($"https://www.youtube.com/watch?v={ID}");
            return e.Build();
        }

        private static int PeopleInCall(SocketCommandContext Context)
        {
            return Context.Client.GetGuild(Context.Guild.Id).GetUser(508008523146199061).VoiceChannel.Users.Count - 1;
        }
    }
}
