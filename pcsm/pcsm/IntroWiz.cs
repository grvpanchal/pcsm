using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace pcsm
{
    public partial class IntroWiz : Form
    {
        public IntroWiz()
        {
            InitializeComponent();
        }

        private void back_b_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex > 0)
            {
                tabControl.SelectedIndex--;
            }
            if (tabControl.SelectedIndex == 5)
            {
                next_b.Text = "Next";
            }
        }

        private void next_b_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex < tabControl.TabCount - 1)
            {
                tabControl.SelectedIndex++;
            }
            if (next_b.Text == "Finish")
            {
                this.Close();
            }
            if (tabControl.SelectedIndex == 6)
            {
                next_b.Text = "Finish";
            }
            
        }

        private void close_b_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.pcstarters.net/pcsm/video");
        }
    }

    class WizardPages : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }

        protected override void OnKeyDown(KeyEventArgs ke)
        {   
            if (ke.Control && ke.KeyCode == Keys.Tab)
                return;
            base.OnKeyDown(ke);
        }
    }
}
