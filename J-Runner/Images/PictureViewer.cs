using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace JRunner
{
    public partial class PictureViewer : Form
    {
        public int i = 0;
        public List<string> list;
        public string pathforit = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public PictureViewer(List<string> mylist)
        {
            InitializeComponent();
            list = mylist;
            this.Text = "Picture Viewer - " + (i + 1) + "  out of " + mylist.Count;
            try
            {
                pictureBox1.Load(pathforit + list[i]);
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                checkBox1.Checked = true;
            }
            catch (Exception) { return; }
            this.DisplayScrollBars();
            this.SetScrollBarValues();
        }

        private void Form1_Resize(Object sender, EventArgs e)
        {
            // If the PictureBox has an image, see if it needs 
            // scrollbars and refresh the image. 
            if (pictureBox1.Image != null)
            {
                this.DisplayScrollBars();
                this.SetScrollBarValues();
                this.Refresh();
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            else pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.DisplayScrollBars();
            this.SetScrollBarValues();
            this.Refresh();

        }
        public void DisplayScrollBars()
        {
            // If the image is wider than the PictureBox, show the HScrollBar.
            if (pictureBox1.Width > pictureBox1.Image.Width - this.vScrollBar1.Width)
            {
                hScrollBar1.Visible = false;
            }
            else
            {
                hScrollBar1.Visible = true;
            }

            // If the image is taller than the PictureBox, show the VScrollBar.
            if (pictureBox1.Height >
                pictureBox1.Image.Height - this.hScrollBar1.Height)
            {
                vScrollBar1.Visible = false;
            }
            else
            {
                vScrollBar1.Visible = true;
            }
        }

        private void HandleScroll(Object sender, ScrollEventArgs se)
        {
            /* Create a graphics object and draw a portion 
               of the image in the PictureBox. */
            Graphics g = pictureBox1.CreateGraphics();

            g.DrawImage(pictureBox1.Image,
              new Rectangle(0, 0, pictureBox1.Right - vScrollBar1.Width,
              pictureBox1.Bottom - hScrollBar1.Height),
              new Rectangle(hScrollBar1.Value, vScrollBar1.Value,
              pictureBox1.Right - vScrollBar1.Width,
              pictureBox1.Bottom - hScrollBar1.Height),
              GraphicsUnit.Pixel);
            pictureBox1.Update();
        }
        public void SetScrollBarValues()
        {
            // Set the Maximum, Minimum, LargeChange and SmallChange properties.
            this.vScrollBar1.Minimum = 0;
            this.hScrollBar1.Minimum = 0;
            // If the offset does not make the Maximum less than zero, set its value. 
            if ((this.pictureBox1.Image.Size.Width - pictureBox1.ClientSize.Width) > 0)
            {
                this.hScrollBar1.Maximum =
                    this.pictureBox1.Image.Size.Width - pictureBox1.ClientSize.Width;
            }
            // If the VScrollBar is visible, adjust the Maximum of the 
            // HSCrollBar to account for the width of the VScrollBar.  
            if (this.vScrollBar1.Visible)
            {
                this.hScrollBar1.Maximum += this.vScrollBar1.Width;
            }
            this.hScrollBar1.LargeChange = this.hScrollBar1.Maximum / 10;
            this.hScrollBar1.SmallChange = this.hScrollBar1.Maximum / 20;
            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.hScrollBar1.Maximum += this.hScrollBar1.LargeChange;

            // If the offset does not make the Maximum less than zero, set its value.    
            if ((this.pictureBox1.Image.Size.Height - pictureBox1.ClientSize.Height) > 0)
            {
                this.vScrollBar1.Maximum =
                    this.pictureBox1.Image.Size.Height - pictureBox1.ClientSize.Height;
            }
            // If the HScrollBar is visible, adjust the Maximum of the 
            // VSCrollBar to account for the width of the HScrollBar.
            if (this.hScrollBar1.Visible)
            {
                this.vScrollBar1.Maximum += this.hScrollBar1.Height;
            }
            this.vScrollBar1.LargeChange = this.vScrollBar1.Maximum / 10;
            this.vScrollBar1.SmallChange = this.vScrollBar1.Maximum / 20;

            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.vScrollBar1.Maximum += this.vScrollBar1.LargeChange;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            i++;
            if (i == list.Count()) i = 0;
            try
            {
                pictureBox1.Load(pathforit + list[i]);
                this.Text = "Picture Viewer - " + (i + 1) + "  out of " + list.Count;
            }
            catch (System.IO.FileNotFoundException) { return; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            i--;
            if (i < 0) i = list.Count - 1;
            try
            {
                pictureBox1.Load(pathforit + list[i]);
                this.Text = "Picture Viewer - " + (i + 1) + "  out of " + list.Count;
            }
            catch (System.IO.FileNotFoundException) { return; }
        }

        private void PictureViewer_Load(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) this.Close();
        }
    }
}
