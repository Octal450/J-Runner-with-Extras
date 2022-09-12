using System;
using System.Windows.Forms;

namespace JRunner.Forms
{

    public partial class Timing : Form
    {
        public Timing()
        {
            InitializeComponent();
            btnA.Text = "Xenon";
            btnB.Text = "Zephyr";
            btnC.Text = "Falcon";
            btnD.Text = "Jasper";
            btnE.Text = "Trinity";
            btnF.Text = "Corona";
            btn1.Hide();
            btn1.Text = "";
            btn2.Hide();
            btn2.Text = "";
            btn3.Hide();
            btn3.Text = "";
            btn4.Hide();
            btn4.Text = "";
            btn5.Hide();
            btn5.Text = "";
            instructionLabel.Hide();
        }

        private string console = "no";

        private void btnReset_Click(object sender, EventArgs e)
        {
            console = "no";
            btnA.Show();
            btnA.Text = "Xenon";
            btnB.Show();
            btnB.Text = "Zephyr";
            btnC.Show();
            btnC.Text = "Falcon";
            btnD.Show();
            btnD.Text = "Jasper";
            btnE.Show();
            btnE.Text = "Trinity";
            btnF.Show();
            btnF.Text = "Corona";
            btn1.Hide();
            btn1.Text = "";
            btn2.Hide();
            btn2.Text = "";
            btn3.Hide();
            btn3.Text = "";
            btn4.Hide();
            btn4.Text = "";
            btn5.Hide();
            btn5.Text = "";
            btnReset.Visible = false;
            doneLabel.Hide();
            instructionLabel.Hide();
            topLabel.Text = "Select Board Type";
        }

        private void btnA_Click(object sender, EventArgs e)
        {
            if (console == "no")
            {
                console = "Xenon";
                btnA.Hide();
                btnB.Hide();
                btnC.Hide();
                btnD.Hide();
                btnE.Hide();
                btnF.Hide();
                btn3.Text = "EXT_CLK\n------------\nGlitch2";
                btn3.Show();
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
            }
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            if (console == "no")
            {
                console = "Zephyr";
                btnA.Hide();
                btnB.Hide();
                btnC.Hide();
                btnD.Hide();
                btnE.Hide();
                btnF.Hide();
                btn2.Text = "RGH1\n------------\nGlitch";
                btn2.Show();
                btn4.Text = "EXT_CLK\n------------\nGlitch2";
                btn4.Show();
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
            }
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            if (console == "no")
            {
                console = "Falcon";
                btnA.Hide();
                btnB.Hide();
                btnC.Hide();
                btnD.Hide();
                btnE.Hide();
                btnF.Hide();
                btn1.Text = "RGH1\n------------\nGlitch";
                btn1.Show();
                btn3.Text = "RGH1.2\n------------\nGlitch2";
                btn3.Show();
                btn5.Text = "R-JTOP\n------------\nJTAG";
                btn5.Show();
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
            }
        }

        private void btnD_Click(object sender, EventArgs e)
        {
            if (console == "no")
            {
                console = "Jasper";
                btnA.Hide();
                btnB.Hide();
                btnC.Hide();
                btnD.Hide();
                btnE.Hide();
                btnF.Hide();
                btn1.Text = "RGH1\n------------\nGlitch";
                btn1.Show();
                btn3.Text = "RGH1.2\n------------\nGlitch2";
                btn3.Show();
                btn5.Text = "R-JTOP\n------------\nJTAG";
                btn5.Show();
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
            }
        }

        private void btnE_Click(object sender, EventArgs e)
        {
            if (console == "no")
            {
                console = "Trinity";
                btnA.Hide();
                btnB.Hide();
                btnC.Hide();
                btnD.Hide();
                btnE.Hide();
                btnF.Hide();
                btn2.Text = "RGH1.2\n------------\nGlitch2";
                btn2.Show();
                btn4.Text = "S-RGH\n------------\nGlitch2";
                btn4.Show();
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
            }
        }

        private void btnF_Click(object sender, EventArgs e)
        {
            if (console == "no")
            {
                console = "Corona";
                btnA.Hide();
                btnB.Hide();
                btnC.Hide();
                btnD.Hide();
                btnE.Hide();
                btnF.Hide();
                btn2.Text = "RGH1.2\n------------\nGlitch2";
                btn2.Show();
                btn4.Text = "S-RGH\n------------\nGlitch2";
                btn4.Show();
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (console == "Xenon")
            {
                xenonBtn(1);
            }
            else if (console == "Zephyr")
            {
                zephyrBtn(1);
            }
            else if (console == "Falcon")
            {
                falconBtn(1);
            }
            else if (console == "Jasper")
            {
                jasperBtn(1);
            }
            else if (console == "Trinity")
            {
                trinityBtn(1);
            }
            else if (console == "Corona")
            {
                coronaBtn(1);
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (console == "Xenon")
            {
                xenonBtn(2);
            }
            else if (console == "Zephyr")
            {
                zephyrBtn(2);
            }
            else if (console == "Falcon")
            {
                falconBtn(2);
            }
            else if (console == "Jasper")
            {
                jasperBtn(2);
            }
            else if (console == "Trinity")
            {
                trinityBtn(2);
            }
            else if (console == "Corona")
            {
                coronaBtn(2);
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (console == "Xenon")
            {
                xenonBtn(3);
            }
            else if (console == "Zephyr")
            {
                zephyrBtn(3);
            }
            else if (console == "Falcon")
            {
                falconBtn(3);
            }
            else if (console == "Jasper")
            {
                jasperBtn(3);
            }
            else if (console == "Trinity")
            {
                trinityBtn(3);
            }
            else if (console == "Corona")
            {
                coronaBtn(3);
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            if (console == "Xenon")
            {
                xenonBtn(4);
            }
            else if (console == "Zephyr")
            {
                zephyrBtn(4);
            }
            else if (console == "Falcon")
            {
                falconBtn(4);
            }
            else if (console == "Jasper")
            {
                jasperBtn(4);
            }
            else if (console == "Trinity")
            {
                trinityBtn(4);
            }
            else if (console == "Corona")
            {
                coronaBtn(4);
            }
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (console == "Xenon")
            {
                xenonBtn(5);
            }
            else if (console == "Zephyr")
            {
                zephyrBtn(5);
            }
            else if (console == "Falcon")
            {
                falconBtn(5);
            }
            else if (console == "Jasper")
            {
                jasperBtn(5);
            }
            else if (console == "Trinity")
            {
                trinityBtn(5);
            }
            else if (console == "Corona")
            {
                coronaBtn(5);
            }
        }

        private void xenonBtn(int btn)
        {
            if (btn == 3)
            {
                btn3.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "Suggested Timing: 59.4 0.9 96 MHz\n\nFor 59.4 0.9, 59.4 is the timing, 0.9 is the pulse length\n\nIf the light stays on at the end of a cycle, pulse length is probably too long\n\nIf the light goes off at the end of a cycle but no boot, pulse length is probably too short";
                topLabel.Text = "Xenon EXT_CLK Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("xenon_extclk");
                //Activate(); // Keep us on top
            }
        }

        private void zephyrBtn(int btn)
        {
            if (btn == 2)
            {
                btn2.Hide();
                btn4.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "RGH1 only has one timing per board";
                topLabel.Text = "Zephyr RGH1 Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("zephyr_rgh1");
                //Activate(); // Keep us on top
            }
            else if (btn == 4)
            {
                btn2.Hide();
                btn4.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "Suggested Timing: 62.5 0.9 96 MHz\n\nFor 62.5 0.9, 62.5 is the timing, 0.9 is the pulse length\n\nIf the light stays on at the end of a cycle, pulse length is probably too long\n\nIf the light goes off at the end of a cycle but no boot, pulse length is probably too short";
                topLabel.Text = "Zephyr EXT_CLK Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("zephyr_extclk");
                //Activate(); // Keep us on top
            }
        }

        private void falconBtn(int btn)
        {
            if (btn == 1)
            {
                btn1.Hide();
                btn3.Hide();
                btn5.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "RGH1 only has one timing per board";
                topLabel.Text = "Falcon RGH1 Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("falcon_rgh1");
                //Activate(); // Keep us on top
            }
            else if (btn == 3)
            {
                btn1.Hide();
                btn3.Hide();
                btn5.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "Suggested Timings: 18-21, start with 21\n\nIf the light stays on at the end of a cycle, timing is probably too low\n\nIf the light goes off at the end of a cycle but no boot, timing is probably too high or far too low";
                topLabel.Text = "Falcon RGH1.2 Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("falconjasper_rgh12");
                //Activate(); // Keep us on top
            }
            else if (btn == 5)
            {
                btn1.Hide();
                btn3.Hide();
                btn5.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "R-JTOP only has one timing for Falcon";
                topLabel.Text = "Falcon R-JTOP Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("falcon_rjtop");
                //Activate(); // Keep us on top
            }
        }

        private void jasperBtn(int btn)
        {
            if (btn == 1)
            {
                btn1.Hide();
                btn3.Hide();
                btn5.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "RGH1 only has one timing per board\n\nIf results are inconsistent, bridge CAP/add 68-100nf cap from PLL to GND";
                topLabel.Text = "Jasper RGH1 Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("jasper_rgh1");
                //Activate(); // Keep us on top
            }
            else if (btn == 3)
            {
                btn1.Hide();
                btn3.Hide();
                btn5.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "Suggested Timings: 18-21, start with 21\n\nIf results are inconsistent, bridge CAP/add 68-100nf cap from PLL to GND\n\nIf the light stays on at the end of a cycle, timing is probably too low\n\nIf the light goes off at the end of a cycle but no boot, timing is probably too high or far too low";
                topLabel.Text = "Jasper RGH1.2 Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("falconjasper_rgh12");
                //Activate(); // Keep us on top
            }
            else if (btn == 5)
            {
                btn1.Hide();
                btn3.Hide();
                btn5.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "R-JTOP only has one timing for Jasper\n\nIf results are inconsistent, bridge CAP/add 68-100nf cap from PLL to GND";
                topLabel.Text = "Jasper R-JTOP Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("jasper_rjtop");
                //Activate(); // Keep us on top
            }
        }

        private void trinityBtn(int btn)
        {
            if (btn == 2)
            {
                btn2.Hide();
                btn4.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "Suggested Timings: 80-110, start with 95\n\nIf using an oscilator, best timing depends on the oscilator of your chip\nUsually near the middle of the working range of files is the best\n\n.....##...##...................##............\nTiming too low\n\n.....##...##...................##############\nTiming too high or far too low";
                topLabel.Text = "Trinity RGH1.2 Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("trinity_rgh12");
                //Activate(); // Keep us on top
            }
            else if (btn == 4)
            {
                btn2.Hide();
                btn4.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "Suggested Timings: 60.3 0.5, 60.6 0.5, 60.3 0.3, 60.6 0.6\n\nFor 60.3 0.5, 60.3 is the timing, 0.5 is the pulse length\n\n.....##...##...................##............\nTiming too low or pulse length too large\n\n.....##...##...................##############\nTiming too high or pulse length too small";
                topLabel.Text = "Trinity S-RGH X360ACE/DGX";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("trinity_srgh");
                //Activate(); // Keep us on top
            }
        }

        private void coronaBtn(int btn)
        {
            if (btn == 2)
            {
                btn2.Hide();
                btn4.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "Suggested Timings: 80-110, start with 95\n\nBest timing depends on the oscilator of your chip\nUsually near the middle of the working range of files is the best\n\n.....##...##...................##............\nTiming too low\n\n.....##...##...................##############\nTiming too high or far too low";
                topLabel.Text = "Corona RGH1.2 Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("corona_rgh12");
                //Activate(); // Keep us on top
            }
            else if (btn == 4)
            {
                btn2.Hide();
                btn4.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "Suggested Timings: 54.8 0.2, 55.2 0.3\n\nFor 54.8 0.2, 54.8 is the timing, 0.2 is the pulse length\n\n.....##...##...................##............\nTiming too low or pulse length too large\n\n.....##...##...................##############\nTiming too high or pulse length too small";
                topLabel.Text = "Corona S-RGH X360ACE/DGX";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("corona_srgh");
                //Activate(); // Keep us on top
            }
        }
    }
}
