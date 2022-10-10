using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace JRunner.Panels
{
    public partial class XSVFChoice : UserControl
    {
        int hresult = 0;
        bool demon = false;
        private List<RadioButton> _radioButtonGroup = new List<RadioButton>();

        public delegate void ClickedProgramCR();
        public event ClickedProgramCR ProgramCRClick;
        public delegate void ClickedCloseCR();
        public event ClickedCloseCR CloseCRClick;

        public XSVFChoice()
        {
            InitializeComponent();
            btnProgram.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            var d = GetAll(this, typeof(RadioButton));
            foreach (RadioButton a in d)
            {
                _radioButtonGroup.Add(a);
            }
        }

        public void boardCheck(string board)
        {
            if (board != null)
            {
                if (board.Contains("Corona"))
                {
                    if (MainForm.mainForm.xPanel.getWBChecked() > 0) Rgh12CoronaWb.Checked = true;
                    else Rgh12Corona.Checked = true;
                }
                else Rgh12Trinity.Checked = true;

                if (board.Contains("Xenon") || board.Contains("Zephyr")) TimingTabs.SelectedTab = MiscTimings;
                else TimingTabs.SelectedTab = Rgh12Timings;
            }
        }

        public int heResult() // TODO: Redo this neater and programmatically?
        {
            // RGH1
            if (Rgh1Zephyr.Checked)
            {
                hresult = 1;
            }
            else if (Rgh1Falcon.Checked)
            {
                hresult = 2;
            }
            else if (Rgh1Jasper.Checked)
            {
                hresult = 3;
            }
            // RGH1.2 Falcon/Jasper
            else if (Rgh12Fj_17.Checked)
            {
                hresult = 4;
            }
            else if (Rgh12Fj_18.Checked)
            {
                hresult = 5;
            }
            else if (Rgh12Fj_19.Checked)
            {
                hresult = 6;
            }
            else if (Rgh12Fj_20.Checked)
            {
                hresult = 7;
            }
            else if (Rgh12Fj_21.Checked)
            {
                hresult = 8;
            }
            else if (Rgh12Fj_22.Checked)
            {
                hresult = 9;
            }
            else if (Rgh12Fj_23.Checked)
            {
                hresult = 10;
            }
            else if (Rgh12Fj_24.Checked)
            {
                hresult = 11;
            }
            // R-JTOP
            else if (RjtopFalcon.Checked)
            {
                hresult = 12;
            }
            else if (RjtopJasper.Checked)
            {
                hresult = 13;
            }
            // Trinity S-RGH
            else if (Trinity600_03.Checked)
            {
                hresult = 14;
            }
            else if (Trinity600_05.Checked)
            {
                hresult = 15;
            }
            else if (Trinity600_06.Checked)
            {
                hresult = 16;
            }
            else if (Trinity600_08.Checked)
            {
                hresult = 17;
            }
            else if (Trinity603_03.Checked)
            {
                hresult = 18;
            }
            else if (Trinity603_05.Checked)
            {
                hresult = 19;
            }
            else if (Trinity603_06.Checked)
            {
                hresult = 20;
            }
            else if (Trinity603_08.Checked)
            {
                hresult = 21;
            }
            else if (Trinity606_03.Checked)
            {
                hresult = 22;
            }
            else if (Trinity606_05.Checked)
            {
                hresult = 23;
            }
            else if (Trinity606_06.Checked)
            {
                hresult = 24;
            }
            else if (Trinity606_08.Checked)
            {
                hresult = 25;
            }
            else if (Trinity609_03.Checked)
            {
                hresult = 26;
            }
            else if (Trinity609_05.Checked)
            {
                hresult = 27;
            }
            else if (Trinity609_06.Checked)
            {
                hresult = 28;
            }
            else if (Trinity609_08.Checked)
            {
                hresult = 29;
            }
            // Corona S-RGH PRO
            else if (Corona545_02.Checked)
            {
                hresult = 30;
            }
            else if (Corona545_03.Checked)
            {
                hresult = 31;
            }
            else if (Corona545_05.Checked)
            {
                hresult = 32;
            }
            else if (Corona545_06.Checked)
            {
                hresult = 33;
            }
            else if (Corona548_02.Checked)
            {
                hresult = 34;
            }
            else if (Corona548_03.Checked)
            {
                hresult = 35;
            }
            else if (Corona548_05.Checked)
            {
                hresult = 36;
            }
            else if (Corona548_06.Checked)
            {
                hresult = 37;
            }
            else if (Corona552_02.Checked)
            {
                hresult = 38;
            }
            else if (Corona552_03.Checked)
            {
                hresult = 39;
            }
            else if (Corona552_05.Checked)
            {
                hresult = 40;
            }
            else if (Corona552_06.Checked)
            {
                hresult = 41;
            }
            else if (Corona555_02.Checked)
            {
                hresult = 42;
            }
            else if (Corona555_03.Checked)
            {
                hresult = 43;
            }
            else if (Corona555_05.Checked)
            {
                hresult = 44;
            }
            else if (Corona555_06.Checked)
            {
                hresult = 45;
            }
            // RGH1.2 Trinity/Corona
            else if (Rgh12Tc_60.Checked)
            {
                hresult = 46;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_65.Checked)
            {
                hresult = 47;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_70.Checked)
            {
                hresult = 48;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_75.Checked)
            {
                hresult = 49;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_80.Checked)
            {
                hresult = 50;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_85.Checked)
            {
                hresult = 51;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_90.Checked)
            {
                hresult = 52;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_95.Checked)
            {
                hresult = 53;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_100.Checked)
            {
                hresult = 54;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_105.Checked)
            {
                hresult = 55;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_110.Checked)
            {
                hresult = 56;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_115.Checked)
            {
                hresult = 57;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_120.Checked)
            {
                hresult = 58;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_125.Checked)
            {
                hresult = 59;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_130.Checked)
            {
                hresult = 60;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Rgh12Tc_135.Checked)
            {
                hresult = 61;
                if (Rgh12Corona.Checked) hresult += 16;
                else if (Rgh12CoronaWb.Checked) hresult += 32;
            }
            else if (Xenon96_594_09.Checked)
            {
                hresult = 94;
            }
            // Xenon EXT_CLK
            else if (Xenon96_594_10.Checked)
            {
                hresult = 95;
            }
            else if (Xenon192_594_09.Checked)
            {
                hresult = 96;
            }
            else if (Xenon192_594_10.Checked)
            {
                hresult = 97;
            }
            // Zephyr EXT_CLK
            else if (Zephyr96_625_09.Checked)
            {
                hresult = 98;
            }
            else if (Zephyr96_625_10.Checked)
            {
                hresult = 99;
            }
            else if (Zephyr192_625_09.Checked)
            {
                hresult = 100;
            }
            else if (Zephyr192_625_10.Checked)
            {
                hresult = 101;
            }

            else hresult = -1;
            return hresult;
        }
        public bool deResult()
        {
            if (demon) return true;
            else return false;
        }

        // This controls the radio buttons to ensure only 1 is selected at a time across groups, and hide/show/enable/disable elements
        private void enterRgh12Fj(object sender, EventArgs e)
        {
            selectedGroupUpdate("Rgh12Fj");
        }
        private void enterRgh12Tc(object sender, EventArgs e)
        {
            selectedGroupUpdate("Rgh12Tc");
        }
        private void enterTrinitySrgh(object sender, EventArgs e)
        {
            selectedGroupUpdate("TrinitySrgh");
        }
        private void enterCoronaSrgh(object sender, EventArgs e)
        {
            selectedGroupUpdate("CoronaSrgh");
        }
        private void enterXenonExtClk(object sender, EventArgs e)
        {
            selectedGroupUpdate("XenonExtClk");
        }
        private void enterZephyrExtClk(object sender, EventArgs e)
        {
            selectedGroupUpdate("ZephyrExtClk");
        }
        private void enterRgh1(object sender, EventArgs e)
        {
            selectedGroupUpdate("Rgh1");
        }
        private void enterRjtop(object sender, EventArgs e)
        {
            selectedGroupUpdate("Rjtop");
        }
        private void selectedGroupUpdate(string board)
        {
            if (board == "Rgh12Tc") Rgh12TcSelectGroup.Enabled = true;
            else Rgh12TcSelectGroup.Enabled = false;

            if (board != "Rgh12Fj")
            {
                foreach (RadioButton a in Rgh12FjGroup.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
            if (board != "Rgh12Tc")
            {
                foreach (RadioButton a in Rgh12TcGroup.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
            if (board != "TrinitySrgh")
            {
                foreach (RadioButton a in TrinitySrghGroup.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
            if (board != "CoronaSrgh")
            {
                foreach (RadioButton a in CoronaSrghGroup.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
            if (board != "XenonExtClk")
            {
                foreach (RadioButton a in XenonExtClkGroup.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
            if (board != "ZephyrExtClk")
            {
                foreach (RadioButton a in ZephyrExtClkGroup.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
            if (board != "Rgh1")
            {
                foreach (RadioButton a in Rgh1Group.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
            if (board != "Rjtop")
            {
                foreach (RadioButton a in RjtopGroup.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
        }

        public void setTimingFromAssistant(string timing)
        {
            selectedGroupUpdate("All");
            if (timing == "xenon_extclk")
            {
                TimingTabs.SelectedTab = MiscTimings;
                Xenon96_594_09.Checked = true;
            }
            else if (timing == "zephyr_rgh1")
            {
                TimingTabs.SelectedTab = MiscTimings;
                Rgh1Zephyr.Checked = true;
            }
            else if (timing == "zephyr_extclk")
            {
                TimingTabs.SelectedTab = MiscTimings;
                Zephyr96_625_09.Checked = true;
            }
            else if (timing == "falcon_rgh1")
            {
                TimingTabs.SelectedTab = MiscTimings;
                Rgh1Falcon.Checked = true;
            }
            else if (timing == "falcon_rjtop")
            {
                TimingTabs.SelectedTab = MiscTimings;
                RjtopFalcon.Checked = true;
            }
            else if (timing == "jasper_rgh1")
            {
                TimingTabs.SelectedTab = MiscTimings;
                Rgh1Jasper.Checked = true;
            }
            else if (timing == "jasper_rjtop")
            {
                TimingTabs.SelectedTab = MiscTimings;
                RjtopJasper.Checked = true;
            }
            else if (timing == "falconjasper_rgh12")
            {
                TimingTabs.SelectedTab = Rgh12Timings;
                Rgh12Fj_21.Checked = true;
            }
            else if (timing == "trinity_rgh12")
            {
                TimingTabs.SelectedTab = Rgh12Timings;
                Rgh12Tc_95.Checked = true;
                Rgh12TcSelectGroup.Enabled = true;
                Rgh12Trinity.Checked = true;
            }
            else if (timing == "corona_rgh12")
            {
                TimingTabs.SelectedTab = Rgh12Timings;
                Rgh12Tc_95.Checked = true;
                Rgh12TcSelectGroup.Enabled = true;
                if (MainForm.mainForm.xPanel.getWBChecked() > 0) Rgh12CoronaWb.Checked = true;
                else Rgh12Corona.Checked = true;
            }
            else if (timing == "trinity_srgh")
            {
                TimingTabs.SelectedTab = SrghTimings;
                Trinity603_05.Checked = true;
            }
            else if (timing == "corona_srgh")
            {
                TimingTabs.SelectedTab = SrghTimings;
                Corona548_02.Checked = true;
            }
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                CloseCRClick();
            }
            catch (Exception) { }
        }

        private void btnProgram_Click(object sender, EventArgs e)
        {
            try
            {
                ProgramCRClick();
            }
            catch (Exception) { }
        }

        private void btnAssistant_Click(object sender, EventArgs e)
        {
            MainForm.mainForm.timingAssistant();
        }

        private void selectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int timingType = MainForm.mainForm.getTimingType();
            string file = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (timingType == 2) openFileDialog.Filter = "SVF files (*.svf)|*.svf";
            else if (timingType == 1) openFileDialog.Filter = "XSVF files (*.xsvf)|*.xsvf";
            else openFileDialog.Filter = "XSVF/SVF files (*.xsvf;*.svf)|*.xsvf;*.svf";
            openFileDialog.Title = "Select a File";
            openFileDialog.RestoreDirectory = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK) file = openFileDialog.FileName;
            if (!string.IsNullOrWhiteSpace(file))
            {
                MainForm.mainForm.nandcustom("Xsvf", file, 16, 0, 0, false);
            }
        }

        private void btnMoreTimings_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Octal450/Timing-Files/releases/tag/Timings");
        }
    }
}
