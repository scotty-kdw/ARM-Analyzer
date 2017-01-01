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
    public partial class InfoBox : Form
    {
        public InfoBox()
        {
            InitializeComponent();
        }

        public void InfoView(String msg)
        {
            this.BodyBox.Text = msg;
        }

        private void BodyBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
