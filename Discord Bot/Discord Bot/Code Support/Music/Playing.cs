﻿namespace Discord_Bot.Code_Support.Music
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using Discord;
    using Discord.Audio;
    using Discord.Commands;
    using Discord.WebSocket;

    public static class Playing
    {
        private static IAudioClient c;
        private static SocketCommandContext s;
        private static System.Timers.Timer t;
        private static Queue queue;

        public static async Task StartPlaying(IAudioClient client, SocketCommandContext context, Queue q)
        {
            try
            {
                queue = q;
                Database.Update("music", "server_id", context.Guild.Id.ToString(), Database.CreateParameter("playing", true));
                s = context;
                c = client;
                t = new System.Timers.Timer();
                t.Elapsed += TElapsed;
                t.Start();
                Program.Client.UserVoiceStateUpdated += Client_UserVoiceStateUpdated;
                Program.Client.LoggedOut += Client_LoggedOut;
                while (true)
                {
                    queue.Refresh();
                    try
                    {
                        if (queue.Items == null) return;
                        if (queue.Items[0].Type == Queue.Type.End)
                        {
                            await context.Channel.SendMessageAsync("I am done playing music");
                            queue.Clear();
                            await Stop(client, context);
                            break;
                        }
                        else if (queue.Items[0].Type == Queue.Type.Youtube)
                        {
                            await context.Channel.SendMessageAsync("", false, Youtube_video_embed(queue.Items[0].YtVideo.ID.Replace("https://www.youtube.com/watch?v=", "")));
                            await Backend.SendUrlAsync(client, queue.Items[0].YtVideo.ID);
                        }
                        else if (queue.Items[0].Type == Queue.Type.Playlist)
                        {
                            // not here yet
                        }
                        else
                        {
                            break;
                        }
                    }
                    finally
                    {
                        queue.Refresh();
                        if (queue.Items != null)
                        queue.Items[0].Remove();
                    }
                }

                await Stop(client, context);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [StartPlaying] Task", ex);
            }
        }

        public static int PeopleInCall(SocketCommandContext context)
        {
            try
            {
                SocketVoiceChannel channel = context.Client.GetGuild(context.Guild.Id).GetUser(Hidden_Info.Tokens.Id).VoiceChannel;
                if (channel != null) return channel.Users.Count - 1;
                else return -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [PeopleInCall] int", ex);
            }
        }

        private static async void TElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Database.Read<string>("music", "server_id", s.Guild.Id.ToString(), "skip")))
                {
                    double meat = Math.Round((double)PeopleInCall(s) / 2, 1);
                    if (Database.Read<string>("music", "server_id", s.Guild.Id.ToString(), "skip").Split('|').Length - 1 >= Math.Round(meat, 0))
                    {
                        Database.Update("music", "server_id", s.Guild.Id.ToString(), Database.CreateParameter("skip", ""));
                        await Skip();
                    }
                }

                if (!string.IsNullOrEmpty(Database.Read<string>("music", "server_id", s.Guild.Id.ToString(), "stop")))
                {
                    double meat = Math.Round((double)PeopleInCall(s) / 2, 1);
                    if (Database.Read<string>("music", "server_id", s.Guild.Id.ToString(), "stop").Split('|').Length - 1 >= Math.Round(meat, 0))
                    {
                        await Stop(c, s);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [skip and stop check] Timer", ex);
            }
        }

        private static async Task Skip()
        {
            try
            {
                await Backend.Discord.ClearAsync(CancellationToken.None);
                await Backend.Discord.FlushAsync();
                t.Stop();
                Program.Client.UserVoiceStateUpdated -= Client_UserVoiceStateUpdated;
                Program.Client.LoggedOut -= Client_LoggedOut;
                queue.Refresh();
                if (queue.Items[1].Type == Queue.Type.End)
                {
                    await s.Channel.SendMessageAsync("I am done playing music");
                    queue.Clear();
                    await Stop(c, s);
                }
                else
                {
                    queue.Items[0].Remove();
                    queue.Refresh();
                    await StartPlaying(c, s, queue);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Skip] Task", ex);
            }
        }

        private static async Task Client_LoggedOut()
        {
            try
            {
                await Stop(c, s);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Client_LoggedOut] Task", ex);
            }
        }

        private static async Task Client_UserVoiceStateUpdated(SocketUser user, SocketVoiceState beforeState, SocketVoiceState afterState)
        {
            try
            {
                if (user.Id == Hidden_Info.Tokens.Id && afterState.VoiceChannel == null)
                {
                    await Stop(c, s);
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
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Client_UserVoiceStateUpdated] event", ex);
            }
        }

        private static async Task Stop(IAudioClient client, SocketCommandContext context)
        {
            try
            {
                Database.Update("music", "server_id", s.Guild.Id.ToString(), Database.CreateParameter("stop", ""));
                Database.Update("music", "server_id", s.Guild.Id.ToString(), Database.CreateParameter("skip", ""));
                Database.Update("music", "server_id", context.Guild.Id.ToString(), Database.CreateParameter("playing", false));
                t.Stop();
                await client.StopAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [stop] Task", ex);
            }
        }

        private static Embed Youtube_video_embed(string id)
        {
            try
            {
                EmbedBuilder e = new EmbedBuilder();
                WebClient web = new WebClient();
                string text = web.DownloadString($"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={id}&key={Uri.EscapeUriString(Hidden_Info.API_Keys.Youtube)}");
                e.Title = $"Now Playing: \"{Json.Parse(text, "items[0].snippet.title")}\": by: {Json.Parse(text, "items[0].snippet.channelTitle")}";
                e.WithThumbnailUrl(Json.Parse(text, "items[0].snippet.thumbnails.standard.url"))
                 .WithColor(255, 0, 0)
                 .WithFooter($"https://www.youtube.com/watch?v={id}");
                return e.Build();
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Youtube_video_embed] Embed", ex);
            }
        }
    }
}
