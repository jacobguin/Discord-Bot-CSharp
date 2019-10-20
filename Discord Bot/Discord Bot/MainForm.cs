namespace Discord_Bot
{
    using System;
    using System.Windows.Forms;
    using FileTransferProtocalLibrary;
    using MetroFramework.Forms;

    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public void AddText(string text, System.Drawing.Color color)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(text + Environment.NewLine);
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            await Program.Client.LogoutAsync();
            FTP ftp = new FTP($"ftp://{Hidden_Info.Ftp.Domain}/Jacob/Program%20Files/Bot/", Hidden_Info.Ftp.Username, Hidden_Info.Ftp.Password);
            ftp.DeleteFile("Bot.accdb");
            ftp.UploadFile("Bot.accdb", $"C:/Users/{Environment.UserName}/Documents/Bot.accdb");
            Application.Exit();
        }
    }
}
