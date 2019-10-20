namespace Discord_Bot.Code_Support.Music
{
    using System.Linq;
    using Discord.Commands;

    public class Queue
    {
        public Queue(SocketCommandContext context)
        {
            Items = List(context);
            Context = context;
        }

        public enum Type
        {
            Playlist,
            Youtube,
            End,
        }

        public Queue_Item[] Items { get; internal set; }

        private SocketCommandContext Context { get; }

        public void Clear()
        {
            Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), "");
            Items = null;
        }

        public void Add(Type type, string input)
        {
            if (type == Type.Youtube)
            {
                string old = Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue");
                if (!string.IsNullOrEmpty(old))
                {
                    Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), old.Replace("|End|", "") + $"|yt|{input}|End|");
                }
                else
                {
                    if (Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Playing") == null)
                    {
                        Database.Write("Music", "Server_ID", Context.Guild.Id.ToString());
                    }

                    Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), $"|yt|{input}|End|");
                }
            }

            Refresh();
        }

        public void Refresh()
        {
            Items = List(Context);
        }

        private static Queue_Item[] List(SocketCommandContext context)
        {
            string raw_ = Database.Read("Music", "Server_ID", context.Guild.Id.ToString(), "Queue");
            if (string.IsNullOrEmpty(raw_))
            {
                return null;
            }
            else
            {
                raw_ = raw_.Replace("|yt|", "|yt").Replace("|playlist|", "|playlist");
                string[] raw = raw_.Split('|');
                Queue_Item[] output = new Queue_Item[raw.Count() - 2];

                for (int i = -1; i < raw.Count() - 2; i++)
                {
                    if (i != -1)
                    {
                        if (i != raw.Count() - 2)
                        {
                            if (raw[i + 1].StartsWith("yt"))
                            {
                                output[i] = new Queue_Item(Type.Youtube, raw[i + 1].Replace("yt", ""), context);
                            }
                            else if (raw[i + 1].StartsWith("playlist"))
                            {
                                // Will not ocure Yet, this may change in the fucture.
                                output[i] = new Queue_Item(Type.Playlist, raw[i + 1].Replace("playlist", ""), context);
                            }
                            else
                            {
                                output[i] = new Queue_Item(Type.End, null, context);
                            }
                        }
                    }
                }

                return output;
            }
        }

        public class Queue_Item
        {
            public Queue_Item(Type type, string result, SocketCommandContext context)
            {
                Type = type;
                Context = context;
                if (Type == Type.Youtube)
                {
                    YtVideo = new YtVideo(result);
                }
                else
                {
                    // wip
                }
            }

            public Type Type { get; }

            public YtVideo YtVideo { get; }

            public QueuePlaylist Playlist { get; }

            private SocketCommandContext Context { get; }

            public void Remove()
            {
                if (Type == Type.Youtube)
                {
                    string old = Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue");
                    if (old.Contains("|yt|"))
                    {
                        Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), old.Replace($"|yt|{YtVideo.ID}", ""));
                    }
                }
                else if (Type == Type.Playlist)
                {
                    // wip
                }
            }
        }

        public class QueuePlaylist
        {
            // WIP
            public ulong Author { get; }

            public string Title { get; }
        }

        public class YtVideo
        {
            public YtVideo(string id)
            {
                ID = id;
            }

            public string ID { get; }
        }
    }
}
