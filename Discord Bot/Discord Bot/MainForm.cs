using MetroFramework.Forms;
using System;
using System.Windows.Forms;

namespace Discord_Bot
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public void AddText(string Text, System.Drawing.Color Color)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = Color;
            richTextBox1.AppendText(Text + Environment.NewLine);
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
