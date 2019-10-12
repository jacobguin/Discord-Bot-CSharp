using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discord_Bot
{
    public partial class Load : Form
    {
        public Load()
        {
            InitializeComponent();
        }

        private async void Load_Load(object sender, EventArgs e)
        {
            Program.MF = new MainForm();
            Program.MF.Show();
            new Load().BOT();
        }

        public async Task BOT()
        {
            
            Program.Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Debug });
            Program.Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug,
            });

            await Program.Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Message_Received.Load();
            Ready.Load();
            Log.Load();

            await Program.Client.LoginAsync(TokenType.Bot, Hidden_Info.Tokens.Bot);
            await Program.Client.StartAsync();
            await Task.Delay(-1);
        }

        private void Load_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
