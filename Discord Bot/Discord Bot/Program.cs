using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Events;
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
        public static DiscordSocketClient Client;
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
            Message_Received.Load();
            Ready.Load();
            Log.Load();

            await Client.LoginAsync(TokenType.Bot, Hidden_Info.Tokens.Bot);
            await Client.StartAsync();
            await Task.Delay(-1);
        }
    }
}
