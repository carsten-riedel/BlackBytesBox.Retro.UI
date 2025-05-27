using System.Drawing;
using System.Windows.Forms;

namespace BlackBytesBox.Retro.UI.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            gifSpinner1.GifImage = Image.FromFile("spinner.gif");
            gifSpinner1.Start();
        }

        private void placeholderTextBox1_EnterKeyPressed(object sender, System.EventArgs e)
        {
            placeholderTextBox1.Text = "";
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            richTextBoxExtendend1.AppendTextLine("Tick at " + System.DateTime.Now.ToString("HH:mm:ss"));
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            splitContainerEx1.CollapsePanel1();
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            splitContainerEx1.ExpandPanel1();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            gifSpinner1.StopAndShowFirstFrame();
            gifSpinner1.Start();
        }
    }
}
