using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class About : Form
    {
        static string[] contrib = { "15432: RGH3 Exploit",
                                    "Balika011: PicoFlasher Support",
                                    "DaCukiMonsta: Nand Info Improvements/Bugfixes",
                                    "DrSchottky, Visual Studio, proferabg: RGH2 to 3",
                                    "Eaton: XL USB and XL HDD Patches",
                                    "Element18592: xFlasher Hardware",
                                    "Josh/Octal450: J-Runner with Extras \nMain Development",
                                    "Mena:\nGeneral Dev, xFlasher Speedup,\nMatrix Hex Flashing, CPU Key Gen",
                                    "Nick Stefanou: Original J-Runner \nDevelopment and Software",
                                    "Orpheus: Updates to KV Info/Bugfixes",
                                    "SGCSam: 6717/9199 XeBuild Patches",
                                    "Xvistaman2005: XDKbuild",
        };
        static int contribloc = 0;
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr RoundCorner(
            int leftRect,
            int topRect,
            int rightRect,
            int bottomRect,
            int widthEllipse,
            int heighEllipse
        );
        int counter = 0;
        int len = 0;
        string txt;

        public About()
        {
            InitializeComponent();
            Region = Region.FromHrgn(RoundCorner(0, 0, Width, Height, 21, 21));
        }

        private void About_Load(object sender, EventArgs e)
        {
            ver.Text = "V" + variables.version;
            build.Text = variables.build;
            lblCredits.Text = "";
            contribloc = 0;
            txt = contrib[0];
            len = txt.Length;

            timer1.Interval = 25;
            timer2.Interval = 25;
            timer1.Start();
        }

        private void close_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.killShade();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (len >= counter)
            {
                lblCredits.Text = txt.Substring(0, counter);
                ++counter;
            }
            else if (counter > len)
            {
                if (counter > len + 30)
                {
                    counter = counter - 30;
                    timer2.Start();
                    timer1.Stop();
                }
                else
                {
                    ++counter;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (counter != 0)
            {
                lblCredits.Text = txt.Substring(0, counter - 1);
                --counter;
            }
            else
            {
                if (contribloc >= contrib.Length - 1)
                {
                    contribloc = 0;
                    txt = contrib[contribloc];
                    len = txt.Length;
                    timer2.Stop();
                    timer1.Start();
                }
                else
                {
                    txt = contrib[++contribloc];
                    len = txt.Length;
                    timer2.Stop();
                    timer1.Start();
                }
            }
        }

        private void About_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void lostFocus(object sender, EventArgs e)
        {
            MainForm.mainForm.killShade();
            this.Close();
        }
    }
}
