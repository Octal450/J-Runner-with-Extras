using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace JRunner.Forms
{
    public partial class demon_uart : Form
    {

        public static TextWriter _writer = null;
        public demon_uart()
        {
            InitializeComponent();
        }

        private bool StartDemon()
        {
            Demon _demon = new Demon();
            _demon.NewSerialDataRecieved += MonitorOnNewSerialDataRecieved;
            return _demon.Start();
        }
        private void MonitorOnNewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {

            var tmp = Encoding.UTF8.GetString(e.Data);
            Console.Write(tmp);

        }
        private void demon_uart_Load(object sender, EventArgs e)
        {
            _writer = new TextBoxStreamWriter(txtConsole);
            Console.SetOut(_writer);
            StartDemon();
        }
    }
}
