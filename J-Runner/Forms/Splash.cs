using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JRunner
{
    public partial class Splash : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr RoundCorner(
            int leftRect,
            int topRect,
            int rightRect,
            int bottomRect,
            int widthEllipse,
            int heighEllipse
        );

        public Splash()
        {
            InitializeComponent();
            Region = Region.FromHrgn(RoundCorner(0, 0, Width, Height, 21, 21));
        }

        private void Splash_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void close_Click(object sender, EventArgs e)
        {
            Program.exit();
        }
    }
}
