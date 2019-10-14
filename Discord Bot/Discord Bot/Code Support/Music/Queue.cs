using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Code_Support.Music
{
    public class Queue
    {

        public Queue(SocketCommandContext Context)
        {
            Items = List(Context);
            context = Context;
        }

        public Queue_Item[] Items { get; internal set; }

        private SocketCommandContext context { get; }

        public enum Type
        {
            Playlist,
            Youtube,
            End,
        }

        private static Queue_Item[] List(SocketCommandContext Context)
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
                    if (i != -1)
                    {
                        if (i != raw.Count() - 2)
                        {
                            if (raw[i + 1].StartsWith("yt"))
                            {
                                Output[i] = new Queue_Item(Type.Youtube, raw[i + 1].Replace("yt", ""), Context);
                            }
                            else if (raw[i + 1].StartsWith("playlist"))
                            {
                                //Will not ocure Yet, this may change in the fucture.
                                Output[i] = new Queue_Item(Type.Playlist, raw[i + 1].Replace("playlist", ""), Context);
                            }
                            else
                            {
                                Output[i] = new Queue_Item(Type.End, null, Context);
                            }
                        }
                    }
                }
                return Output;
            }
        }

        public class Queue_Item
        {
            public Queue_Item(Type type, string Result, SocketCommandContext Context)
            {
                Type = type;
                context = Context;
                if (Type == Type.Youtube)
                {
                    YtVideo = new YtVideo(Result);
                }
                else
                {
                    //wip
                }
            }

            public void Remove()
            {
                if (Type == Type.Youtube)
                {
                    string old = Database.Read("Music", "Server_ID", context.Guild.Id.ToString(), "Queue");
                    if (old.Contains("|yt|"))
                    {
                        Database.Update("Music", "Queue", "Server_ID", context.Guild.Id.ToString(), old.Replace($"|yt|{YtVideo.ID}", ""));
                    }
                }
                else if (Type == Type.Playlist)
                {
                    //not here yet WIP
                }
            }

            private SocketCommandContext context { get; }

            public Type Type { get; }

            public YtVideo YtVideo { get; }

            public QueuePlaylist Playlist { get; }
        }

        public class QueuePlaylist
        {
            //WIP
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

        public void Clear()
        {
            Database.Update("Music", "Queue", "Server_ID", context.Guild.Id.ToString(), "");
            Items = null;
        }

        public void Add(Type type, string input)
        {
            if (type == Type.Youtube)
            {
                string old = Database.Read("Music", "Server_ID", context.Guild.Id.ToString(), "Queue");
                if (!string.IsNullOrEmpty(old))
                {
                    Database.Update("Music", "Queue", "Server_ID", context.Guild.Id.ToString(), old.Replace("|End|", "") + $"|yt|{input}|End|");
                }
                else
                {
                    if (Database.Read("Music", "Server_ID", context.Guild.Id.ToString(), "Playing") == null)
                    {
                        Database.Write("Music", "Server_ID", context.Guild.Id.ToString());
                    }
                    Database.Update("Music", "Queue", "Server_ID", context.Guild.Id.ToString(), $"|yt|{input}|End|");
                }
            }
            Refresh();
        }

        public void Refresh()
        {
            Items = List(context);
        }
    }
}
