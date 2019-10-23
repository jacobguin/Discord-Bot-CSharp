namespace Discord_Bot.Code_Support.Music
{
    using System;
    using System.Linq;
    using Discord.Commands;

    public class Queue
    {
        public Queue(SocketCommandContext context)
        {
            try
            {
                Items = List(context);
                Context = context;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong Creating the [Queue]", ex);
            }
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
            try
            {
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), "");
                Items = null;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Clear] void", ex);
            }
        }

        public void Add(Type type, string input)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Add] void", ex);
            }
        }

        public void Refresh()
        {
            try
            {
                Items = List(Context);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Refresh] void", ex);
            }
        }

        private static Queue_Item[] List(SocketCommandContext context)
        {
            try
            {
                string raw_ = Database.Read("Music", "Server_ID", context.Guild.Id.ToString(), "Queue");
                if (string.IsNullOrEmpty(raw_))
                {
                    return null;
                }

                raw_ = raw_.Replace("|yt|", "|yt").Replace("|playlist|", "|playlist");
                string[] raw = raw_.Split('|');
                Queue_Item[] output = new Queue_Item[raw.Count() - 2];

                for (int i = -1; i < raw.Count() - 2; i++)
                {
                    if (i != -1 && i != raw.Count() - 2)
                    {
                        if (raw[i + 1].StartsWith("yt"))
                        {
                            output[i] = new Queue_Item(Type.Youtube, raw[i + 1].Replace("yt", ""), context);
                        }
                        else if (raw[i + 1].StartsWith("playlist"))
                        {
                            // Will not occur yet, this may change in the future.
                            output[i] = new Queue_Item(Type.Playlist, raw[i + 1].Replace("playlist", ""), context);
                        }
                        else
                        {
                            output[i] = new Queue_Item(Type.End, null, context);
                        }
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong Creating the [List] of Queue_Items", ex);
            }
        }

        public class Queue_Item
        {
            public Queue_Item(Type type, string result, SocketCommandContext context)
            {
                try
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
                catch (Exception ex)
                {
                    throw new Exception("Something Went wrong creating the [Queue_Item] Class", ex);
                }
            }

            public Type Type { get; }

            public YtVideo YtVideo { get; }

            public QueuePlaylist Playlist { get; }

            private SocketCommandContext Context { get; }

            public void Remove()
            {
                try
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
                catch (Exception ex)
                {
                    throw new Exception("Something Went wrong in the [Remove] void", ex);
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
