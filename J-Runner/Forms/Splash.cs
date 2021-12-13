using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
            Region = System.Drawing.Region.FromHrgn(RoundCorner(0, 0, Width, Height, 21, 21));
            ver.Text = "The Ultimate RGH/JTAG App v" + variables.version;
        }
    }
}
