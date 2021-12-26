using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace JRunner
{
    public class TextBoxStreamWriter : TextWriter
    {
        TextBox _output = null;
        delegate void SetTextCallback(string text);
        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            //MethodInvoker action = delegate { _output.AppendText(value.ToString()); };
            //_output.BeginInvoke(action);
            base.Write(value);
            try
            {
                if (this._output.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Write);
                    _output.BeginInvoke(d, new object[] { value.ToString() });
                }
                else
                {
                    _output.AppendText(value.ToString());
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
