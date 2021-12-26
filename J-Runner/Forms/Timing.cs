using System;
using System.Windows.Forms;

namespace JRunner.Forms
{

    public partial class Timing : Form
    {
        public Timing()
        {
            InitializeComponent();
            instructionLabel.Hide();
        }

        private string console = "no";

        private void btnReset_Click(object sender, EventArgs e)
        {
            console = "no";
            btn1.Show();
            btn1.Text = "Zephyr";
            btn2.Show();
            btn2.Text = "Falcon";
            btn3.Show();
            btn3.Text = "Jasper";
            btn4.Show();
            btn4.Text = "Trinity";
            btn5.Show();
            btn5.Text = "Corona";
            btnReset.Visible = false;
            doneLabel.Hide();
            instructionLabel.Hide();
            topLabel.Text = "Select Board Type";
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (console == "no")
            {
                console = "Zephyr";
                btn1.Hide();
                btn2.Text = "RGH1\n------------\nGlitch";
                btn3.Hide();
                btn4.Text = "RGH2\n------------\nGlitch2";
                btn5.Hide();
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
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
            if (console == "no")
            {
                console = "Falcon";
                btn1.Text = "RGH1\n------------\nGlitch";
                btn2.Hide();
                btn3.Text = "RGH1.2\n------------\nGlitch2";
                btn4.Hide();
                btn5.Text = "R-JTOP\n------------\nJTAG";
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
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
            if (console == "no")
            {
                console = "Jasper";
                btn1.Text = "RGH1\n------------\nGlitch";
                btn2.Hide();
                btn3.Text = "RGH1.2\n------------\nGlitch2";
                btn4.Hide();
                btn5.Text = "R-JTOP\n------------\nJTAG";
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
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
            if (console == "no")
            {
                console = "Trinity";
                btn1.Hide();
                btn2.Text = "RGH1.2\n------------\nGlitch2";
                btn3.Hide();
                btn4.Text = "S-RGH\n------------\nGlitch2";
                btn5.Hide();
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
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
            if (console == "no")
            {
                console = "Corona";
                btn1.Hide();
                btn2.Text = "RGH1.2\n------------\nGlitch2";
                btn3.Hide();
                btn4.Text = "S-RGH\n------------\nGlitch2";
                btn5.Hide();
                btnReset.Visible = true;
                topLabel.Text = console + ": Select Hack Type";
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

        private void zephyrBtn(int btn)
        {
            if (btn == 2)
            {
                btn1.Hide();
                btn3.Hide();
                btn5.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "RGH1 only has one timing per board";
                topLabel.Text = "Zephyr RGH1 Coolrunner/Matrix";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("zephyr_rgh1");
                Activate(); // Keep us on top
            }
            else if (btn == 4)
            {
                btn2.Hide();
                btn4.Hide();
                doneLabel.Show();
                instructionLabel.Show();
                instructionLabel.Text = "Suggested Timings: 69.3 0.3, 69.3 0.5, 69.3 0.6\n\nFor 69.3 0.3, 69.3 is the timing, 0.3 is the pulse length\n\n.....##...##...................##............\nTiming too low or pulse length too large\n\n.....##...##...................##############\nTiming too high or pulse length too small";
                topLabel.Text = "Zephyr RGH2 X360ACE/DGX";
                // Now open the panel and set the timing
                MainForm.mainForm.openXsvfInfo();
                MainForm.mainForm.xsvfInfo.setTimingFromAssistant("zephyr_rgh2");
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
                btn2.Hide();
                btn4.Hide();
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
                btn2.Hide();
                btn4.Hide();
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
