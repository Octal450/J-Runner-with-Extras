using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using RenameRegistryKey;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace JRunner
{
    public partial class CpuKeyDB : Form
    {
        int index = 0;
        enum DataTableColumns
        {
            ID = 0,
            Serial = 1,
            CPUKey = 2,
            CRCKV = 3,
            Comment = 4,
            DVDKey = 5,
            Region = 6,
            OSIG = 7
        }
        Regex objAlphaPattern = new Regex("[a-fA-F0-9]{32}$");
        public CpuKeyDB()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
        }

        private void cpukeydb_Load(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EventHandler(cpukeydb_Load), new object[] { sender, e });
                return;
            }

            rbtnSerial.Checked = variables.cpuKeyDbSerial;

            DataTable cputable = dataSet1.DataTable1;
            RegistryKey cpukeydb = Registry.CurrentUser.CreateSubKey("CPUKey_DB", RegistryKeyPermissionCheck.ReadWriteSubTree);
            foreach (string valueName in cpukeydb.GetValueNames())
            {
                if (valueName == "Index")
                {
                    index = Convert.ToInt32(cpukeydb.GetValue(valueName));
                    for (int i = 1; i <= index; i++)
                    {
                        try
                        {
                            RegistryKey cpukeys = cpukeydb.OpenSubKey(i.ToString(), true);
                            if (cpukeys.GetValue("Deleted") != null)
                            {
                                RegistryUtilities.RenameSubKey(cpukeydb, index.ToString(), i.ToString());
                                index = index - 1;
                                cpukeys.SetValue("Index", i); ;
                                cpukeydb.SetValue("Index", index);
                                cpukeys.DeleteValue("Deleted");
                            }
                            DataRow cpurow = cputable.NewRow();
                            cpurow[0] = Convert.ToInt32(cpukeys.GetValue("Index"));
                            cpurow[1] = cpukeys.GetValue("Serial").ToString();
                            cpurow[2] = cpukeys.GetValue("cpukey").ToString();
                            cpurow[3] = cpukeys.GetValue("CRC_KV").ToString();
                            cpurow[4] = cpukeys.GetValue("Mobo", "").ToString();
                            cpurow[5] = cpukeys.GetValue("DVDKey", "").ToString();
                            cpurow[6] = cpukeys.GetValue("Region", "").ToString();
                            cpurow[7] = cpukeys.GetValue("OSIG", "").ToString();
                            cputable.Rows.Add(cpurow);
                        }
                        catch (SystemException ex)
                        {
                            if (variables.debugMode) Console.WriteLine(ex.ToString());
                            continue;
                        }
                        catch (Exception ex)
                        {
                            if (variables.debugMode) Console.WriteLine(ex.ToString());
                            continue;
                        }
                        lblNumber.Text = cputable.Rows.Count.ToString();
                    }
                }
            }
        }

        public static bool addkey_s(regentries entry, DataSet1 db, bool stealth = false, bool updDialog = false)
        {
            if (string.IsNullOrEmpty(entry.kvcrc)) return false;

            DataTable cputable = db.DataTable1;
            RegistryKey cpukeydb = Registry.CurrentUser.CreateSubKey("CPUKey_DB");
            foreach (string subkey in cpukeydb.GetSubKeyNames())
            {
                if (cpukeydb.OpenSubKey(subkey).GetValue("CRC_KV") != null)
                {
                    if (cpukeydb.OpenSubKey(subkey).GetValue("CRC_KV").ToString() == entry.kvcrc || cpukeydb.OpenSubKey(subkey).GetValue("Serial").ToString() == entry.serial)
                    {
                        if (!stealth) Console.WriteLine("Key already exists");
                        return false;
                    }
                }
            }

            int index = Convert.ToInt32(cpukeydb.GetValue("Index")) + 1;
            if (updDialog) MainForm.mainForm.cpukeydb.index = index; // Update nonstatically if the dialog calls it, dirty but I don't have time to rewrite this dialog

            cpukeydb.SetValue("Index", index);
            RegistryKey cpukeys = cpukeydb.CreateSubKey(index.ToString());
            cpukeys.SetValue("Index", index);
            cpukeys.SetValue("Serial", entry.serial);
            cpukeys.SetValue("cpukey", entry.cpukey);
            cpukeys.SetValue("CRC_KV", entry.kvcrc);
            cpukeys.SetValue("DVDKey", entry.dvdkey);
            cpukeys.SetValue("Region", entry.region);
            cpukeys.SetValue("OSIG", entry.osig);
            cpukeys.SetValue("Mobo", entry.extra);


            DataRow cpurow = cputable.NewRow();
            cpurow[0] = index;
            cpurow[1] = entry.serial;
            cpurow[2] = entry.cpukey;
            cpurow[3] = entry.kvcrc;
            cpurow[4] = entry.extra;
            cpurow[5] = entry.dvdkey;
            cpurow[6] = entry.region;
            cpurow[7] = entry.osig;

            try
            {
                cputable.Rows.Add(cpurow);

            }
            catch (System.Data.ConstraintException) { }
            if (!stealth) Console.WriteLine("Added Key to Database");
            return true;
        }
        public static string getkey_s(long kvcrc, DataSet1 hi)
        {
            if (kvcrc == 0) return "";

            DataTable cputable = hi.DataTable1;
            RegistryKey cpukeydb = Registry.CurrentUser.CreateSubKey("CPUKey_DB");
            foreach (string subkey in cpukeydb.GetSubKeyNames())
            {
                if (cpukeydb.OpenSubKey(subkey).GetValue("CRC_KV") != null)
                {

                    if (cpukeydb.OpenSubKey(subkey).GetValue("CRC_KV").ToString() == kvcrc.ToString("X"))
                    {
                        return cpukeydb.OpenSubKey(subkey).GetValue("cpukey").ToString();
                    }
                }
            }
            return "";
        }
        public static bool getkey_s(string cpukey, DataSet1 hi)
        {
            DataTable cputable = hi.DataTable1;
            RegistryKey cpukeydb = Registry.CurrentUser.CreateSubKey("CPUKey_DB");
            foreach (string subkey in cpukeydb.GetSubKeyNames())
            {
                if (cpukeydb.OpenSubKey(subkey).GetValue("cpukey") != null)
                {
                    if (cpukeydb.OpenSubKey(subkey).GetValue("cpukey").ToString() == cpukey) return true;
                }
            }
            return false;
        }

        #region delete
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="indexrow"></param>
        public void deletekey(int indexrow)
        {
            if (variables.debugMode) Console.WriteLine("Deleting Key");
            --index;
            DataTable cputable = dataSet1.DataTable1;
            RegistryKey cpukeydb = Registry.CurrentUser.CreateSubKey("CPUKey_DB");
            if (variables.debugMode) Console.WriteLine("Index Row {0} | Index {1}", indexrow, index);
            if (indexrow == index)
            {
                if (variables.debugMode) Console.WriteLine("Last one");
                cpukeydb.SetValue("Index", index);
                cpukeydb.DeleteSubKeyTree(cputable.Rows[indexrow][0].ToString());
                if (variables.debugMode) Console.WriteLine("Done");
            }
            else
            {
                if (variables.debugMode) Console.WriteLine("Setting deleted");
                RegistryKey cpukeys = cpukeydb.CreateSubKey(cputable.Rows[indexrow][0].ToString());
                foreach (string valueN in cpukeys.GetValueNames())
                {
                    if (valueN != "Index")
                    {
                        cpukeys.DeleteValue(valueN);
                    }
                }
                cpukeys.SetValue("Deleted", 1);
                if (variables.debugMode) Console.WriteLine("Done");
            }
            cputable.Rows.Remove(cputable.Rows[indexrow]);
            lblNumber.Text = cputable.Rows.Count.ToString();
            if (variables.debugMode) Console.WriteLine("Finished");
            //this.Refresh(); // Uncomment if below is removed
            MainForm.mainForm.restartCpuKeyDb(); // Fix the sorting in registry and table, there is probably a better way to do this
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (variables.debugMode) Console.WriteLine("UserDeletingRow {0}", e.Row.ToString());
            //Console.WriteLine(e.Row.Index);
            deletekey(e.Row.Index);
            //cpukeydb.DeleteSubKeyTree(cputable.Rows[e.Row.Index][0].ToString(), false);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(dataGridView1.CurrentRow);
            //Console.WriteLine(dataGridView1.CurrentRow.Index);
            //int indexrow = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            //Console.WriteLine(indexrow);
            //deletekey(indexrow);
            deletekey(dataGridView1.CurrentRow.Index);
        }
        #endregion


        /// <summary>
        /// Search by Serial or CPU Key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateSearch();
        }

        private void rbtnSerial_CheckedChanged(object sender, EventArgs e)
        {
            updateSearch();
            variables.cpuKeyDbSerial = rbtnSerial.Checked;
        }

        private void rbtnCPU_CheckedChanged(object sender, EventArgs e)
        {
            updateSearch();
        }

        private void updateSearch()
        {
            DataTable cputable = dataSet1.DataTable1;
            dataGridView1.CurrentCell = null;
            int column = 1;
            //if (rbtnComment.Checked) column = 4;
            if (rbtnCPU.Checked) column = 2;
            else column = 1;
            for (int i = 0; i != index; i++)
            {
                if (cputable.Rows[i][column].ToString().ToUpper().Contains(txtSearch.Text.ToUpper())) dataGridView1.Rows[i].Visible = true;
                else dataGridView1.Rows[i].Visible = false;
            }
        }

        /// <summary>
        /// Export
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            string xmlfile;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Registry file|*.reg";
            saveFileDialog1.Title = "Save as Registry File";
            string strRegistrySection = "HKEY_CURRENT_USER\\CPUKey_DB";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                xmlfile = saveFileDialog1.FileName;
                if (xmlfile != null) ExportKey(strRegistrySection, xmlfile);

            }
        }
        public void ExportKey(string RegKey, string SavePath)
        {
            string path = "\"" + SavePath + "\"";
            string key = "\"" + RegKey + "\"";

            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "regedit.exe";
                proc.StartInfo.UseShellExecute = false;
                proc = Process.Start("regedit.exe", "/e " + path + " " + key + "");

                if (proc != null) proc.WaitForExit();
            }
            finally
            {
                if (proc != null) proc.Dispose();
            }
            Console.WriteLine("CPU Key Database Export Complete");
            Console.WriteLine("");
        }

        enum STATES
        {
            NONE,
            INDEX,
            KEY
        }
        public struct regentries
        {
            public enum REGENTRY
            {
                Serial,
                CPUKey,
                CRC_KV,
                Comment,
                DVDKey,
                Region,
                OSIG
            }

            public string serial;
            public string cpukey;
            public string kvcrc;
            public string extra;
            public string dvdkey;
            public string region;
            public string osig;
        }
        private void importReg(string regfile)
        {
            Forms.ProgressIndeterminate pi = new Forms.ProgressIndeterminate();

            this.BeginInvoke(new Action(() =>
            {
                this.Hide();
                pi.Show();
                pi.updateTitle("Importing");
            }));
            MainForm.mainForm.BeginInvoke(new Action(() => {
                MainForm.mainForm.Enabled = false;
            }));

            Console.WriteLine("Importing");
            string[] lines = File.ReadAllLines(regfile);
            int keystoadd = 0;
            STATES state = STATES.NONE;
            List<regentries> entries = new List<regentries>();
            regentries tempentry = new regentries();

            foreach (string line in lines)
            {
                if (line.StartsWith("["))
                {
                    if (line.Equals("[HKEY_CURRENT_USER\\CPUKey_DB]")) state = STATES.INDEX;
                    else state = STATES.KEY;
                    continue;
                }

                switch (state)
                {
                    case STATES.INDEX:
                        Int32.TryParse(line.Substring(line.IndexOf(":") + 1), out keystoadd);
                        state = STATES.NONE;
                        break;
                    case STATES.KEY:
                        if (line.StartsWith("\""))
                        {
                            if (line.Contains("Index"))
                            {
                                tempentry = new regentries();
                                tempentry.extra = "";
                                tempentry.dvdkey = "";
                                tempentry.osig = "";
                                tempentry.region = "";
                                continue;
                            }
                            else if (line.Contains("Serial"))
                            {
                                tempentry.serial = (line.Substring(line.IndexOf("=\"") + 1)).Replace("\"", "");
                            }
                            else if (line.Contains("cpukey"))
                            {
                                tempentry.cpukey = (line.Substring(line.IndexOf("=\"") + 1)).Replace("\"", "");
                            }
                            else if (line.Contains("CRC_KV"))
                            {
                                tempentry.kvcrc = (line.Substring(line.IndexOf("=\"") + 1)).Replace("\"", "");
                            }
                            else if (line.Contains("Mobo"))
                            {
                                tempentry.extra = (line.Substring(line.IndexOf("=\"") + 1)).Replace("\"", "");
                            }
                            else if (line.Contains("DVDKey"))
                            {
                                tempentry.dvdkey = (line.Substring(line.IndexOf("=\"") + 1)).Replace("\"", "");
                            }
                            else if (line.Contains("Region"))
                            {
                                tempentry.region = (line.Substring(line.IndexOf("=\"") + 1)).Replace("\"", "");
                            }
                            else if (line.Contains("OSIG"))
                            {
                                tempentry.osig = (line.Substring(line.IndexOf("=\"") + 1)).Replace("\"", "");
                            }
                        }
                        else
                        {
                            entries.Add(tempentry);
                            state = STATES.NONE;
                        }
                        break;
                    default:
                        break;
                }
            }
            Console.WriteLine("Checking {0} keys", keystoadd);
            int counter = 0;
            foreach (regentries entry in entries)
            {
                long crc;
                if (!long.TryParse(entry.kvcrc, System.Globalization.NumberStyles.HexNumber, System.Threading.Thread.CurrentThread.CurrentCulture, out crc)) continue;
                if (!string.IsNullOrWhiteSpace(getkey_s(crc, dataSet1))) continue;
                if (getkey_s(entry.cpukey, dataSet1)) continue;


                if (addkey_s(entry, dataSet1, true, true)) counter++;
            }
            DataTable cputable = dataSet1.DataTable1;
            lblNumber.Text = cputable.Rows.Count.ToString();
            Console.WriteLine("Added {0} keys", counter);
            Console.WriteLine("");

            MainForm.mainForm.BeginInvoke(new Action(() => {
                MainForm.mainForm.Enabled = true;
            }));
            this.BeginInvoke(new Action(() =>
            {
                this.Show();
                pi.Close();
            }));
        }

        #region Buttons
        private void EditMobotoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string EdID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string Edcpukey = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            string Edserial = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            string Edcomment = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            CPUkeydb.Editmobo YetMoreForms = new CPUkeydb.Editmobo(EdID, Edserial, Edcpukey, Edcomment, dataSet1);
            YetMoreForms.ShowDialog();

            // YetMoreForms.// dataGridView1.CurrentRow.Cells[0].Value.ToString()
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hitTestInfo;
            if (e.Button == MouseButtons.Right)
            {
                hitTestInfo = dataGridView1.HitTest(e.X, e.Y);
                dataGridView1.ClearSelection();
                if (hitTestInfo.RowIndex == -1 || hitTestInfo.ColumnIndex == -1) return;
                //dataGridView1.CurrentCell.Selected = true;//
                //dataGridView1.Rows[hitTestInfo.RowIndex].Selected = true;
                dataGridView1.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex];
                //dataGridView1.Rows[hitTestInfo.RowIndex].Cells[2].Selected = true;
                contextMenuStrip1.Show(dataGridView1, new System.Drawing.Point(e.X, e.Y));
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataTable cputable = dataSet1.DataTable1;
            //DataTable cputable = dataSet1.DataTable1;
            //int indexrow = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            //if (cputable.Rows[indexrow][2] != null) Clipboard.SetText(cputable.Rows[indexrow][2].ToString());
            this.dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            if (this.dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {

                    // Add the selection to the clipboard.
                    Clipboard.SetDataObject(this.dataGridView1.GetClipboardContent());
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    //
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            try
            {
                Clipboard.SetText(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                variables.cpukey = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                //Console.WriteLine(Path.Combine(Directory.GetParent(variables.outfolder).FullName, dataGridView1.CurrentRow.Cells[1].Value.ToString()));
                variables.FindFolder = Path.Combine(Path.Combine(Directory.GetParent(variables.outfolder).FullName, dataGridView1.CurrentRow.Cells[1].Value.ToString()));
                if (variables.debugMode) Console.WriteLine((variables.FindFolder));
                if (Directory.Exists(variables.FindFolder))
                {
                    Console.WriteLine("Select Load Source to open the folder: {0}", (variables.FindFolder));
                }
                else variables.FindFolder = "";
                this.Close();
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
        }
        #endregion

        private void scan(string folder)
        {
            Forms.ProgressIndeterminate pi = new Forms.ProgressIndeterminate();

            this.BeginInvoke(new Action(() =>
            {
                this.Hide();
                pi.Show();
                pi.updateTitle("Scanning");
            }));
            MainForm.mainForm.BeginInvoke(new Action(() => {
                MainForm.mainForm.Enabled = false;
            }));

            Console.WriteLine("Scanning Files...");
            try
            {
                int counter = 0, nandfiles = 0, median = 0, textfiles = 0, percent = 0, previous = 0;
                List<float> lengths = new List<float>();
                List<string> blacklist = new List<string>();
                lengths.Add(0x4200000); lengths.Add(0x1080000); lengths.Add(0x10800000); lengths.Add(0x21000000); lengths.Add(0x3000000); lengths.Add(0xE0400000);

                string[] filePaths = Directory.GetFiles(folder, "*.txt", SearchOption.AllDirectories);
                foreach (string file in filePaths)
                {
                    if (file.ToUpper().Contains("FUSES") || file.ToUpper().Contains("INFO") || file.ToUpper().Contains("KV") || file.ToUpper().Contains("CPU"))
                    {
                        textfiles++;
                        string[] nandPaths = Directory.GetFiles(Path.GetDirectoryName(file), "*.bin", SearchOption.TopDirectoryOnly);
                        counter += nandPaths.Length;

                        foreach (string nand in nandPaths)
                        {
                            if (variables.debugMode) Console.WriteLine(nand);

                            #region percent
                            if (variables.debugMode)
                            {
                                Console.WriteLine("counter: {0} - i: {1}", counter, textfiles);
                                Console.WriteLine("nandpaths.length: {0}", nandPaths.Length);
                                Console.WriteLine("j: {0} - filepaths.length: {1}", nandfiles, filePaths.Length);
                            }
                            median = (counter) / textfiles;
                            if (median == 0) median = 1;
                            percent = ((nandfiles + (textfiles - median)) * 100) / Math.Max((filePaths.Length - textfiles) * median, 1);
                            if (percent > previous && percent < 100)
                            {
                                if (percent % 5 == 0) Console.WriteLine("\rCompleted: {0}%", percent);
                                Console.Out.Flush();
                                previous = percent;
                            }
                            nandfiles++;
                            #endregion

                            if (nand.ToUpper().Contains("NANDFLASH") || nand.ToUpper().Contains("UPDFLASH")) continue;

                            FileInfo fl = new FileInfo(nand);
                            if (lengths.Contains((fl.Length)))
                            {
                                string cpukey = "";
                                string[] textlines = File.ReadAllLines(file);

                                foreach (string line in textlines)
                                {
                                    cpukey = (objAlphaPattern.Match(line).Value);

                                    try
                                    {
                                        if (blacklist.Contains(nand)) break;
                                        if (Nand.Nand.imageknown(nand, false))
                                        {
                                            if (variables.debugMode) Console.WriteLine("Verifying key");
                                            if (Nand.Nand.cpukeyverification(nand, cpukey, true)) break;
                                            if (variables.debugMode) Console.WriteLine("Key not verified");
                                        }
                                        if (variables.debugMode) Console.WriteLine("Image not known");
                                    }
                                    catch (Exception ex) { blacklist.Add(nand); if (variables.debugMode) Console.WriteLine(ex.ToString()); continue; }
                                }

                                bool sts = objAlphaPattern.IsMatch(cpukey);
                                bool check = false;

                                if (sts)
                                    try
                                    {
                                        check = Nand.Nand.cpukeyverification(nand, cpukey);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (variables.debugMode) Console.WriteLine(nand.ToString());
                                        if (variables.debugMode) Console.WriteLine(ex.ToString());
                                    }

                                if (check)
                                {
                                    Nand.PrivateN nan = new Nand.PrivateN(nand, cpukey);
                                    DataTable cputable = dataSet1.DataTable1;
                                    try
                                    {
                                        bool found = false;
                                        for (int c = 0; c != index; c++)
                                        {
                                            if (!string.IsNullOrWhiteSpace(nan.ki.serial) && cputable.Rows[c]["Serial"].ToString().ToUpper().Contains(nan.ki.serial.ToUpper()))
                                            {
                                                if (string.IsNullOrWhiteSpace(cputable.Rows[c]["Comment"].ToString()) || string.IsNullOrWhiteSpace(cputable.Rows[c]["Region"].ToString())
                                                    || string.IsNullOrWhiteSpace(cputable.Rows[c]["DVDKey"].ToString()) || string.IsNullOrWhiteSpace(cputable.Rows[c]["OSIG"].ToString()))
                                                {
                                                    try
                                                    {
                                                        RegistryKey cpukeydb = Registry.CurrentUser.CreateSubKey("CPUKey_DB");
                                                        RegistryKey cpukeys = cpukeydb.CreateSubKey(cputable.Rows[c]["ID"].ToString());
                                                        //RegistryKey cpukeys = cpukeydb.OpenSubKey(Nand.Nand.getConsoleName(nan, variables.flashconfig), true);
                                                        string moboname = Nand.Nand.getConsoleName(nan, variables.flashconfig);
                                                        cpukeys.SetValue("Mobo", moboname);
                                                        cpukeys.SetValue("DVDKey", nan.ki.dvdkey);
                                                        cpukeys.SetValue("Region", nan.ki.region);
                                                        cpukeys.SetValue("OSIG", nan.ki.osig);
                                                        //Console.WriteLine("Adding {0}", Nand.Nand.getConsoleName(nan, variables.flashconfig) + " to entry found blank in CPUKey db");
                                                        cputable.Rows[c]["Comment"] = moboname;
                                                        cputable.Rows[c]["DVDKey"] = nan.ki.dvdkey;
                                                        cputable.Rows[c]["Region"] = nan.ki.region;
                                                        cputable.Rows[c]["OSIG"] = nan.ki.osig;
                                                    }
                                                    catch (Exception ex1)
                                                    {
                                                        if (variables.debugMode) Console.WriteLine(ex1.ToString());
                                                    }
                                                }
                                                found = true;
                                            }
                                            else if (cputable.Rows[c]["CPU Key"].ToString().ToUpper().Contains(cpukey.ToUpper()))
                                            {
                                                found = true;
                                            }
                                        }
                                        if (variables.debugMode) Console.WriteLine("found {0}", found);
                                        if (!found)
                                        {
                                            regentries entry = new regentries();
                                            entry.kvcrc = nan.kvcrc().ToString("X");
                                            entry.serial = nan.ki.serial;
                                            entry.cpukey = cpukey;
                                            entry.extra = Nand.Nand.getConsoleName(nan, variables.flashconfig);
                                            entry.osig = nan.ki.osig;
                                            entry.region = nan.ki.region;
                                            entry.dvdkey = nan.ki.dvdkey;

                                            addkey_s(entry, dataSet1, true, true);
                                        }
                                        found = false;
                                    }
                                    catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
                                    lblNumber.Text = cputable.Rows.Count.ToString();
                                }
                                else if (variables.debugMode) Console.WriteLine("2nd time veri failed");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            this.Refresh();
            Console.WriteLine("\rCompleted: 100%");
            Console.WriteLine("");

            MainForm.mainForm.BeginInvoke(new Action(() => {
                MainForm.mainForm.Enabled = true;
            }));
            this.BeginInvoke(new Action(() =>
            {
                this.Show();
                pi.Close();
            }));
        }

        private void scan_cpukey(string folder, string cpukey)
        {
            Forms.ProgressIndeterminate pi = new Forms.ProgressIndeterminate();

            this.BeginInvoke(new Action(() =>
            {
                this.Hide();
                pi.Show();
                pi.updateTitle("Scanning");
            }));
            MainForm.mainForm.BeginInvoke(new Action(() => {
                MainForm.mainForm.Enabled = false;
            }));

            Console.WriteLine("Scanning Files...");
            try
            {
                int i = 0, percent = 0, previous = 0;
                List<float> lengths = new List<float>();
                List<string> blacklist = new List<string>();
                lengths.Add(0x4200000); lengths.Add(0x1080000); lengths.Add(0x10800000); lengths.Add(0x21000000); lengths.Add(0x3000000); lengths.Add(0xE0400000);
                string[] filePaths = Directory.GetFiles(folder, "*.bin", SearchOption.AllDirectories);
                foreach (string file in filePaths)
                {
                    i++;
                    percent = (i * 100) / (filePaths.Length);
                    if (percent > previous && percent < 100)
                    {
                        if (percent % 5 == 0) Console.WriteLine("\rCompleted: {0}%", percent);
                        Console.Out.Flush();
                        previous = percent;
                    }
                    FileInfo fl = new FileInfo(file);
                    if (lengths.Contains((fl.Length)))
                    {
                        bool sts = objAlphaPattern.IsMatch(cpukey);
                        bool cpucheck = false;
                        bool check = false;
                        if ((cpukey.Length == 32 && sts)) cpucheck = true;
                        if (cpucheck) check = Nand.Nand.cpukeyverification(file, cpukey);
                        if (check)
                        {
                            Console.WriteLine(file);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            this.Refresh();
            Console.WriteLine("\rCompleted: 100%");
            Console.WriteLine("");

            MainForm.mainForm.BeginInvoke(new Action(() => {
                MainForm.mainForm.Enabled = true;
            }));
            this.BeginInvoke(new Action(() =>
            {
                this.Show();
                pi.Close();
            }));
        }

        private void scan_kv(string folder, long crc)
        {
            Forms.ProgressIndeterminate pi = new Forms.ProgressIndeterminate();

            this.BeginInvoke(new Action(() =>
            {
                this.Hide();
                pi.Show();
                pi.updateTitle("Scanning");
            }));
            MainForm.mainForm.BeginInvoke(new Action(() => {
                MainForm.mainForm.Enabled = false;
            }));

            Console.WriteLine("Scanning Files...");
            try
            {
                int i = 0, percent = 0, previous = 0;
                List<float> lengths = new List<float>();
                List<string> blacklist = new List<string>();
                lengths.Add(0x4200000); lengths.Add(0x1080000); lengths.Add(0x10800000); lengths.Add(0x21000000); lengths.Add(0x3000000); lengths.Add(0xE0400000);
                string[] filePaths = Directory.GetFiles(folder, "*.bin", SearchOption.AllDirectories);
                foreach (string file in filePaths)
                {
                    i++;
                    percent = (i * 100) / (filePaths.Length);
                    if (percent > previous && percent < 100)
                    {
                        if (percent % 5 == 0) Console.WriteLine("\rCompleted: {0}%", percent);
                        Console.Out.Flush();
                        previous = percent;
                    }
                    FileInfo fl = new FileInfo(file);
                    if (lengths.Contains((fl.Length)))
                    {
                        bool check = false;
                        if (Nand.Nand.kvcrc(file) == crc) check = true;
                        if (check)
                        {
                            Console.WriteLine(file);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
            this.Refresh();
            Console.WriteLine("\rCompleted: 100%");
            Console.WriteLine("");

            MainForm.mainForm.BeginInvoke(new Action(() => {
                MainForm.mainForm.Enabled = true;
            }));
            this.BeginInvoke(new Action(() =>
            {
                this.Show();
                pi.Close();
            }));
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
            openDialog.InitialDirectory = variables.rootfolder;
            openDialog.RestoreDirectory = false;
            openDialog.IsFolderPicker = true;

            if (openDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string fol = openDialog.FileName;
                ThreadStart readna = delegate { scan(fol); };
                Thread readnt = new Thread(readna);
                readnt.IsBackground = true;
                readnt.Start();
            }
            this.BringToFront(); // Because it goes behind?
        }

        private void btnreverse_Click(object sender, EventArgs e)
        {
            bool kv = false;
            long crc = 0;
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                if (MessageBox.Show("No CPU Key has been entered in the search box\n\nDo you want to use a nand dump?", "Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                kv = true;
            }

            if (kv)
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Nand files (*.bin;*.ecc)|*.bin;*.ecc|HEX files (*.hex)|*.hex|All files (*.*)|*.*";
                openFileDialog1.Title = "Select Nand File";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename1 = openFileDialog1.FileName;
                    crc = Nand.Nand.kvcrc(filename1);
                }
                else return;
            }

            FolderBrowserDialog folder = new FolderBrowserDialog();
            //folder.RootFolder = System.Environment.SpecialFolder.DesktopDirectory;
            DialogResult result = folder.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fol = folder.SelectedPath;
                ThreadStart readna;
                if (!kv) readna = delegate { scan_cpukey(fol, txtSearch.Text); };
                else readna = delegate { scan_kv(fol, crc); };
                Thread readnt = new Thread(readna);
                readnt.IsBackground = true;
                readnt.Start();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Registry Files (.reg)|*.reg";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                importReg(fd.FileName);
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;

            try
            {
                string id = (dataGridView1.Rows[e.RowIndex].Cells[0].Value).ToString();
                int nid;
                if (int.TryParse(id, out nid))
                {
                    nid--;
                    txtdvdkey.Text = dataSet1.DataTable1.Rows[nid][DataTableColumns.DVDKey.GetHashCode()].ToString();
                    txtregion.Text = dataSet1.DataTable1.Rows[nid][DataTableColumns.Region.GetHashCode()].ToString();
                    txtosig.Text = dataSet1.DataTable1.Rows[nid][DataTableColumns.OSIG.GetHashCode()].ToString();
                }
            }
            catch (Exception ex) { if (variables.debugMode) Console.WriteLine(ex.ToString()); }
        }
    }
}
