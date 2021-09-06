namespace Discord_Bot
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;

    public partial class Load : Form
    {
        public Load()
        {
            InitializeComponent();
        }

        public async Task BOT()
        {
            Program.Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
            Program.Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug,
            });

            await Program.Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            string nspace = "Discord_Bot.Events";

            IEnumerable<Type> q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && !t.Name.Contains("__1") && !t.Name.Contains("DisplayClass") && t.Namespace == nspace
                    select t;
            try
            {
                foreach (Type @class in q.ToList())
                {
                    foreach (MethodInfo method in @class.GetMethods())
                    {
                        if (method.Name == "Run")
                        {
                            object c = Activator.CreateInstance(@class);
                            method.Invoke(c, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.MF.AddText(ex.Message, System.Drawing.Color.Red);
            }

            await Program.Client.LoginAsync(TokenType.Bot, Hidden_Info.Tokens.Bot);
            await Program.Client.StartAsync();
            await Task.Delay(-1);
        }

        private async void Load_Load(object sender, EventArgs e)
        {
            string path = $"C:/Users/{Environment.UserName}/Documents/Bot.accdb";
            File.Delete(path);
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath.Replace("\\bin\\Debug", "") + "\\Copy To Output\\");
            foreach (FileInfo file in di.GetFiles())
            {
                if (File.Exists($"{Application.StartupPath}/{file.Name}")) File.Delete($"{Application.StartupPath}/{file.Name}");
                file.CopyTo($"{Application.StartupPath}/{file.Name}");
            }

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
