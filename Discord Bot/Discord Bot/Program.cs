using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Discord_Bot
{
    class Program
    {
        public DiscordSocketClient Client;
        public static CommandService Commands;

        static void Main(string[] args)
        {
            new Program().BOT().GetAwaiter().GetResult();
        }

        public async Task BOT()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });
            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug,
            });

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Client.MessageReceived += Client_MessageReceived;
            Client.Ready += Client_Ready;
            Client.Log += Client_Log;

            await Client.LoginAsync(TokenType.Bot, Hidden_Info.Tokens.Bot);
            await Client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task Client_Log(LogMessage Message)

        {
            Console.WriteLine($"[{DateTime.Now} at {Message.Source}] {Message.Message}", System.Drawing.Color.Gold);
        }

        private async Task Client_Ready()
        {
            await Client.SetGameAsync(Client.Guilds.Count.ToString() + " Servers", null, ActivityType.Watching);
        }

        private async Task Client_MessageReceived(SocketMessage MessagePram)
        {
            SocketUserMessage Message = MessagePram as SocketUserMessage;
            SocketCommandContext Context = new SocketCommandContext(Client, Message);

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;

            string Prefix = Utils.GetPrefix(Context);

            int ArgPos = 0;
            if (!(Message.HasStringPrefix(Prefix, ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))) return;
            if (Context.Message.ToString() == Prefix) return;

            IResult Result = await Commands.ExecuteAsync(Context, ArgPos, null);

            if (!Result.IsSuccess)
            {
                Console.WriteLine($"{DateTime.Now} at Commands] Something went wrong with a command  Text: {Context.Message.Content} | Error: {Result.ErrorReason}", System.Drawing.Color.DarkRed);
                string ERROR = Result.ErrorReason;
                if (ERROR == "Unknown command.")
                {
                    ERROR = $"Unknown Command! Use {Prefix}Help to see the commands.";
                }
                await Context.Channel.SendMessageAsync(ERROR);
            }
        }
    }
}
