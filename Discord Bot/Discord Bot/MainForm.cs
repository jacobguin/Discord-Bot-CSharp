namespace Discord_Bot
{
    using System;
    using System.Windows.Forms;
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
            Application.Exit();
        }
    }
}
