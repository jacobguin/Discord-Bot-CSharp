namespace Discord_Bot
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Discord_Bot.Events;
    using FileTransferProtocalLibrary;

    public partial class Load : Form
    {
        public Load()
        {
            InitializeComponent();
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

        private async void Load_Load(object sender, EventArgs e)
        {
            string path = $"C:/Users/{Environment.UserName}/Documents/Bot.accdb";
            FTP ftp = new FTP($"ftp://{Hidden_Info.Ftp.Domain}/Jacob/Program%20Files/Bot/", Hidden_Info.Ftp.Username, Hidden_Info.Ftp.Password);
            File.Delete(path);
            ftp.DownloadFile("Bot.accdb", path);

            Program.MF = new MainForm();
            Program.MF.Show();
            _ = new Load().BOT();
        }

        private void Load_Shown(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
