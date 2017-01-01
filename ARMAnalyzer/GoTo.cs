using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARMAnalyzer
{
    public partial class GoTo : Form
    {
        public delegate void SendMsgDelegate(string msg);
        public event SendMsgDelegate SendMsg;

        public GoTo(int count)
        {
            InitializeComponent();
            this.box_GoTo_Index.Text = count.ToString();
        }

        private void btn_GoTo_Index_Click(object sender, EventArgs e)
        {
            String temp = box_GoTo_Index.Text;
            if (temp.Count() > 2)
            {
                if (temp.Substring(0, 2) == "0x")
                {
                    int TargetAddr = Int32.Parse(temp.Substring(2), System.Globalization.NumberStyles.HexNumber);

                    SendMsg(TargetAddr.ToString());
                }
                else
                {
                    SendMsg(temp);
                }
            }
            else
            {
                SendMsg(temp);
            }

            this.Hide();
        }

        private void box_GoTo_Index_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String temp = box_GoTo_Index.Text;
                if (temp.Count() > 2) {
                    if (temp.Substring(0, 2) == "0x")
                    {
                        int TargetAddr = Int32.Parse(temp.Substring(2), System.Globalization.NumberStyles.HexNumber);

                        SendMsg(TargetAddr.ToString());
                    }
                    else
                    {
                        SendMsg(temp);
                    }
                }
                else
                {
                    SendMsg(temp);
                }

                this.Hide();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }
    }
}
