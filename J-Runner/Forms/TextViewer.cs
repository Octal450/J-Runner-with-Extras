using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class TextViewer : Form
    {
        public TextViewer()
        {
            InitializeComponent();
            FormClosing += TextViewer_FormClosing;
        }

        private bool FileOpen = false;
        private FileStream fileStream;

        private void TextViewer_FormClosing(object sender, EventArgs e)
        {
            fileStream.Dispose();
        }

        public void LoadFile(string file)
        {
            if (!File.Exists(file))
            {
                MessageBox.Show("File doesn't exist", "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string extn = Path.GetExtension(file);
            if (extn != ".txt" && extn != ".rtf" && extn != ".log" && extn != ".ini")
            {
                MessageBox.Show("I have absolutely no idea how to handle this file type: " + extn, "Can't", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (FileOpen) fileStream.Dispose();

            RichTextBoxStreamType type = RichTextBoxStreamType.RichText;
            if (extn == ".txt" || extn == ".log" || extn == ".ini") type = RichTextBoxStreamType.PlainText;

            fileStream = new FileStream(file, FileMode.Open);
            viewer.LoadFile(fileStream, type);
            FileOpen = true;

            setUI(true);
            this.Text = Path.GetFileName(file) + " - Text Viewer";
        }

        private void setUI(bool m)
        {
            copyToolStripMenuItem.Enabled = m;
            selectAllToolStripMenuItem.Enabled = m;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = "";
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "All Supported Files (*.rtf, *.txt, *.log, *.ini)|*.rtf;*.txt;*.log|Rich Text Files (*.rtf)|*.rtf|Plain Text Files (*.txt)|*.txt|Log Files (*.log)|*.log|Configuration Files (*.ini)|*.ini";
            openDialog.Title = "Open";
            openDialog.InitialDirectory = Environment.CurrentDirectory;
            openDialog.RestoreDirectory = false;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                file = openDialog.FileName;
            }
            if (file != "") LoadFile(file);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileOpen)
            {
                fileStream.Dispose();
                viewer.Clear();
                FileOpen = false;
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewer.SelectAll();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewer.Copy();
        }
    }
}
