namespace Discord_Bot.Events
{
    using System;
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord.WebSocket;

    public static class Message_Received
    {
        public static void Load()
        {
            Program.Client.MessageReceived += Client_MessageReceived;
        }

        private static async Task Client_MessageReceived(SocketMessage messagePram)
        {
            SocketUserMessage message = messagePram as SocketUserMessage;
            SocketCommandContext context = new SocketCommandContext(Program.Client, message);
            if (context.Message == null || context.Message.Content == "") return;
            if (context.User.IsBot) return;

            string prefix = Utils.GetPrefix(context);

            int argPos = 0;
            if (!(message.HasStringPrefix(prefix, ref argPos) || message.HasMentionPrefix(Program.Client.CurrentUser, ref argPos))) return;
            if (context.Message.ToString() == prefix) return;

            IResult result = await Program.Commands.ExecuteAsync(context, argPos, null);

            if (!result.IsSuccess)
            {
                Program.MF.AddText($"{DateTime.Now} at Commands] Something went wrong with a command  Text: {context.Message.Content} | Error: {result.ErrorReason}", System.Drawing.Color.DarkRed);
                string err = result.ErrorReason;
                if (err == "Unknown command.")
                {
                    err = $"Unknown Command! Use {prefix}Help to see the commands.";
                }

                await context.Channel.SendMessageAsync(err);
            }
        }
    }
}
