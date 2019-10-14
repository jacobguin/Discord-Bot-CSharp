﻿using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Discord_Bot.Code_Support.Music
{
    public static class Queue
    {
 /*       public enum Type
        {
            Playlist,
            Youtube,
            End,
        }

        public static class Playlist
        {
            public static string[] Songs()
            {
                return new string[] { "Not Setup yet" };
            }
        }

        public static Queue_Item[] List(SocketCommandContext Context)
        {
            string RAW = Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue");
            if (string.IsNullOrEmpty(RAW))
            {
                return null;
            }
            else
            {
                RAW = RAW.Replace("|yt|", "|yt").Replace("|playlist|", "|playlist");
                string[] raw = RAW.Split('|');
                Queue_Item[] Output = new Queue_Item[raw.Count() - 2];

                for (int i = -1; i < raw.Count() - 2; i++)
                {
                    if (i != -1 && i != raw.Count() - 2)
                    {
                        if (raw[i + 1].StartsWith("yt"))
                        {
                            Output[i] = new Queue_Item(Type.Youtube, raw[i + 1].Replace("yt", ""));
                        }
                        else if (raw[i + 1].StartsWith("playlist"))
                        {
                            // Will not occur yet, this may change in the future.
                            Output[i] = new Queue_Item(Type.Playlist, raw[i + 1].Replace("playlist", ""));
                        }
                        else
                        {
                            Output[i] = new Queue_Item(Type.End, null);
                        }
                    }
                }
                return Output;
            }
        }

        public static Queue_Item FirstInQueue(SocketCommandContext Context)
        {
            return List(Context)[0];
        }

        public static async Task RemoveFirst(SocketCommandContext Context)
        {
            Queue_Item remove = FirstInQueue(Context);
            if (remove.Type == Type.Youtube)
            {
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue").Remove(0, 14));
            }
            else if (remove.Type == Type.Playlist)
            {
                //not here yet
            }
            else
            {
                await Clear(Context);
            }
        }

        public class Queue_Item
        {
            public Queue_Item(Type type, string Result)
            {
                Type = type;
                if (Type == Type.Youtube)
                {
                    YtVideo = new YtVideo(Result);
                }
                else
                {
                    //wip
                }
            }

            public Type Type { get; }

            public YtVideo YtVideo { get; }

            public QueuePlaylist Playlist { get; }
        }

        public class QueuePlaylist
        {
            public ulong Author { get; }
            public string Title { get; }
        }

        public class YtVideo
        {
            public YtVideo(string Id)
            {
                ID = Id;
            }
            public string ID { get; }
        }

        public static async Task Clear(SocketCommandContext Context)
        {
            Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), "");
        }

        public static async Task AddSongToQueue(SocketCommandContext Context, string ID)
        {
            string old = Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue");
            if (!string.IsNullOrEmpty(old))
            {
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), old.Replace("|End|", "") + $"|yt|{ID}|End|");
            }
            else
            {
                if (Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Playing") == null)
                {
                    Database.Write("Music", "Server_ID", Context.Guild.Id.ToString());
                }
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), $"|yt|{ID}|End|");
            }
        }*/
    }
}
