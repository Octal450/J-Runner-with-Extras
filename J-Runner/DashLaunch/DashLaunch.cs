using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace JRunner
{
    public partial class DashLaunch : Form
    {
        public DashLaunch()
        {
            InitializeComponent();
            string defaultpath = Path.Combine(variables.launchpath, "launch_default.ini");
            if (File.Exists(defaultpath))
            {
                getDL(defaultpath);
            }
            else
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox10.Checked = true;
                checkBox5.Checked = true;
                checkBox17.Checked = true;
                checkBox7.Checked = true;
                checkBox8.Checked = true;
                checkBox9.Checked = true;
                ftpport.Text = "21";
                Default.Text = @"Usb:\FSD\default.xex";
                BUT_X.Text = @"Hdd:\Content\0000000000000000\C0DE9999\00080000\C0DE99990F586558";

            }
        }

        void getDL(string path)
        {
            var d = GetAll(this, typeof(TextBox));
            var c = GetAll(this, typeof(CheckBox));

            string[] dl_options = parse_ini.parse(path);

            foreach (string dl_option in dl_options)
            {
                foreach (CheckBox col in c)
                {
                    if (dl_option.Contains(col.Text))
                    {
                        if (dl_option.Contains("True")) col.Checked = true;
                        else if (dl_option.Contains("False")) col.Checked = false;
                    }
                }
                foreach (TextBox col in d)
                {
                    if (dl_option.Contains(col.Name))
                    {
                        if (dl_option.Contains("=")) col.Text = dl_option.Substring(dl_option.IndexOf("=") + 1);
                    }
                }
            }
        }
        void saveDL(string path)
        {
            List<string> options = new List<string>();
            IEnumerable<Control> c;
            string tooltip;

            string text = "; launch.xex V3.0 config file" + Environment.NewLine +
@"; parsed by simpleIni http://code.jellycan.com/simpleini/" + Environment.NewLine +
@"; currently supported devices and paths:" + Environment.NewLine +
@"; internal hard disk    Hdd:\" + Environment.NewLine +
@"; usb memory stick      Usb:\" + Environment.NewLine +
@"; memory unit           Mu:\" + Environment.NewLine +
@"; USB memory unit       UsbMu:\" + Environment.NewLine +
@"; big block NAND mu     FlashMu:\" + Environment.NewLine +
@"; internal slim 4G mu	IntMu:\" + Environment.NewLine +
@"; internal corona 4g mu MmcMu:\" + Environment.NewLine +
@"; CD/DVD                Dvd:\     (not recommended to use this one)" + Environment.NewLine +
@"; buttons can point to any xex, or any CON with default.xex in it on any of the above devices" + Environment.NewLine +
@"; note that Right Bumper is ALWAYS default to return NXE" + Environment.NewLine +
@"; if you want to assign an additional one, use the path Sfc:\dash.xex ie:" + Environment.NewLine +
@"; BUT_A = Sfc:\dash.xex" + Environment.NewLine + Environment.NewLine +
@"; all comments and fields in this file are optional, you can remove anything you don't need" + Environment.NewLine +
@"; sorry for any double negatives :p" + Environment.NewLine +
@"; njoy, cOz";
            options.Add(text);

            options.Add("");
            options.Add("; example entry" + Environment.NewLine + @"; Default = Hdd:\FreeStyle\default.xex");
            options.Add("[Paths]");
            var d = GetAll(tabPaths, typeof(TextBox));
            d = d.Reverse();

            foreach (TextBox col in d)
            {
                if (col != null)
                {
                    tooltip = toolTip1.GetToolTip(col);
                    if (!String.IsNullOrWhiteSpace(tooltip)) options.Add(Environment.NewLine + "; " + tooltip.Replace(Environment.NewLine, Environment.NewLine + "; "));
                    options.Add(col.Name + " = " + col.Text);
                }
            }

            options.Add("");
            options.Add("; example plugin entry" + Environment.NewLine + @"; plugin1 = Usb:\plugin\aplugin.xex");
            options.Add("[Plugins]");
            d = GetAll(tabPlugins, typeof(TextBox));
            d = d.Reverse();

            foreach (TextBox col in d)
            {
                if (col != null)
                {
                    tooltip = toolTip1.GetToolTip(col);
                    if (!String.IsNullOrWhiteSpace(tooltip)) options.Add(Environment.NewLine + "; " + tooltip.Replace(Environment.NewLine, Environment.NewLine + "; "));
                    options.Add(col.Name + " = " + col.Text);
                }
            }

            options.Add("");
            options.Add("; these options are never used directly by dash launch but serve to give the configuration program" + Environment.NewLine + @"; a place to keep these values; note that things like ftp and udp servers are only running while installer is running");
            options.Add("[Externals]");
            d = GetAll(tabExternals, typeof(TextBox));
            c = GetAll(tabExternals, typeof(CheckBox));
            c = c.Reverse();

            foreach (TextBox col in d)
            {
                if (col != null)
                {

                    options.Add(col.Name + " = " + col.Text);
                }
            }

            foreach (CheckBox col in c)
            {
                if (col != null)
                {
                    options.Add(col.Text + " = " + col.Checked);
                }
            }

            options.Add("");
            options.Add("[Settings]");
            d = GetAll(tabSettings, typeof(TextBox));
            c = GetAll(tabSettings, typeof(CheckBox));
            c = c.Reverse();
            d = d.Reverse();

            foreach (CheckBox col in c)
            {
                if (col != null)
                {
                    tooltip = toolTip1.GetToolTip(col);
                    if (!String.IsNullOrWhiteSpace(tooltip)) options.Add(Environment.NewLine + "; " + tooltip.Replace(Environment.NewLine, Environment.NewLine + "; "));
                    options.Add(col.Text + " = " + col.Checked);
                }
            }

            foreach (TextBox col in d)
            {
                if (col != null)
                {
                    if (!checkBox7.Checked && col == Regio) continue;
                    else if (!checkBox18.Checked && col == hddtimer) continue;
                    tooltip = toolTip1.GetToolTip(col);
                    if (!String.IsNullOrWhiteSpace(tooltip)) options.Add(Environment.NewLine + "; " + tooltip.Replace(Environment.NewLine, Environment.NewLine + "; "));
                    if (col == Regio) options.Add("region = " + col.Text);
                    else options.Add(col.Name + " = " + col.Text);

                }
            }

            options.Add("; if some titles benefit from fakelive, add them here - only 10 may be listed");
            d = GetAll(tabAutoFake, typeof(TextBox));
            //d = d.Reverse();
            foreach (TextBox col in d)
            {
                if (col != null)
                {
                    options.Add(col.Name + " = " + col.Text);
                }
            }


            parse_ini.write_ini(path, options.ToArray());
        }

        void btn_Create_Click(object sender, System.EventArgs e)
        {
            string defaultpath = Path.Combine(variables.launchpath, "launch.ini");
            saveDL(defaultpath);
            MessageBox.Show("Launch.ini Creation Completed Successfully");
        }
        void btn_Save_Click(object sender, System.EventArgs e)
        {
            string defaultpath = Path.Combine(variables.launchpath, @"launch_default.ini");
            saveDL(defaultpath);
            MessageBox.Show("Settings saved successfuly");
        }

        void linkLabel1_Click(object sender, System.EventArgs e)
        {
            DashLaunch_Help myNewForm = new DashLaunch_Help();
            myNewForm.ShowDialog();
        }

        enum default_true
        {
            nxemini, noupdater, exchandler, liveblock, xhttp, fakelive, nonetstore, nohealth, nooobe
        };

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }

    }
}
