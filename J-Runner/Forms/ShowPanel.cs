using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class ShowPanel : Form
    {
        public ShowPanel()
        {
            InitializeComponent();
        }

        public void addPanel(Control c)
        {
            panel1.Controls.Add(c);
        }
    }
}
