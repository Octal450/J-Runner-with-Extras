using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JRunner
{
    public partial class XBOptions : Form
    {
        string filename = Path.Combine(variables.pathforit, @"xebuild\options_edited.ini");
        string file = Path.Combine(variables.pathforit, @"xebuild\options.ini");
        string[] delete = { };

        public XBOptions()
        {
            InitializeComponent();
            if (!File.Exists(filename))
            {
                try
                {
                    File.Copy(file, filename);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
        }

        private void XBOptions_Load(object sender, EventArgs e)
        {
            if (File.Exists(filename))
            {

                Control.ControlCollection coll = this.Controls;
                var d = GetAll(this, typeof(TextBox));
                var c = GetAll(this, typeof(CheckBox));

                string[] options = parse_ini.parse(filename);

                foreach (string option in options)
                {
                    foreach (CheckBox col in c)
                    {
                        if (option.Substring(0, option.IndexOf('=')) == (col.Text))
                        {
                            if (option.Contains("true")) col.Checked = true;
                            else if (option.Contains("false")) col.Checked = false;
                        }
                    }
                    foreach (TextBox col in d)
                    {
                        if (option.Substring(0, option.IndexOf('=')) == (col.Name))
                        {
                            if (option.Contains("=")) col.Text = option.Substring(option.IndexOf("=") + 1);
                            if (option.Substring(0, option.IndexOf('=')) == "xellbutton")
                            {
                                if (option.Substring(option.IndexOf("=") + 1) == "power")
                                {
                                    rbtnPower.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "eject")
                                {
                                    rbtnDVD.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "remopower")
                                {
                                    rbtnIRp.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "remox")
                                {
                                    rbtnIRX.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "winbutton")
                                {
                                    rbtnIRW.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "wirelessx")
                                {
                                    rbtnWlx.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "wiredx")
                                {
                                    rbtnWX.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "kiosk")
                                {
                                    rbtnKiosk.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "")
                                {
                                    rbtnBlank.Checked = true;
                                }
                            }
                            else if (option.Substring(0, option.IndexOf('=')) == "xellbutton2")
                            {
                                if (option.Substring(option.IndexOf("=") + 1) == "power")
                                {
                                    rbtnPower2.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "eject")
                                {
                                    rbtnDVD2.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "remopower")
                                {
                                    rbtnIRp2.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "remox")
                                {
                                    rbtnIRX2.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "winbutton")
                                {
                                    rbtnIRW2.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "wirelessx")
                                {
                                    rbtnWlx2.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "wiredx")
                                {
                                    rbtnWX2.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "kiosk")
                                {
                                    rbtnKiosk2.Checked = true;
                                }
                                else if (option.Substring(option.IndexOf("=") + 1) == "")
                                {
                                    rbtnBlank2.Checked = true;
                                }
                            }
                            else if (option.Substring(0, option.IndexOf('=')) == "cpufan")
                            {
                                try
                                {
                                    trackBar1.Value = Convert.ToInt32(option.Substring(option.IndexOf("=") + 1));
                                }
                                catch (Exception) { }
                            }
                            else if (option.Substring(0, option.IndexOf('=')) == "gpufan")
                            {
                                try
                                {
                                    trackBar2.Value = Convert.ToInt32(option.Substring(option.IndexOf("=") + 1));
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }
            }
        }

        private void btnaccept_Click(object sender, EventArgs e)
        {
            if (!chksettings.Checked)
            {
                List<string> options = new List<string>();

                var d = GetAll(this, typeof(TextBox));
                d.Reverse();
                foreach (TextBox col in d)
                {
                    options.Add(col.Name + " = " + col.Text);
                }
                var c = GetAll(this, typeof(CheckBox));
                c.Reverse();
                foreach (CheckBox col in c)
                {
                    if (col != chksettings)
                    {
                        if (col != null)
                            if (col.Checked)
                                options.Add(col.Text + " = true");
                            else
                                options.Add(col.Text + " = false");
                    }
                }
                File.Copy(file, filename, true);
                parse_ini.edit_ini(filename, options.ToArray(), delete);
            }
            else
            {
                File.Copy(file, filename, true);
            }
            Console.WriteLine("Options.ini saved successfully");
            this.Close();
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }

        enum buttons
        {
            power = 0,
            eject = 1,
            remopower = 2,
            remox = 3,
            winbutton = 4,
            wirelessx = 5,
            wiredx = 6,
            kiosk = 7,
            blank = 8
        };


        #region shit
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton.Text = buttons.power.ToString();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton.Text = buttons.eject.ToString();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton.Text = buttons.remopower.ToString();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton.Text = buttons.remox.ToString();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton.Text = buttons.winbutton.ToString();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton.Text = buttons.wirelessx.ToString();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton.Text = buttons.wiredx.ToString();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton.Text = buttons.kiosk.ToString();
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton.Text = "";
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton2.Text = buttons.power.ToString();
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton2.Text = buttons.eject.ToString();
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton2.Text = buttons.remopower.ToString();
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton2.Text = buttons.remox.ToString();
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton2.Text = buttons.winbutton.ToString();
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton2.Text = buttons.wirelessx.ToString();
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton2.Text = buttons.wiredx.ToString();
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton2.Text = buttons.kiosk.ToString();
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            xellbutton2.Text = "";
        }
        #endregion

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            cpufan.Text = trackBar1.Value.ToString();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            gpufan.Text = trackBar2.Value.ToString();
        }


    }
}
