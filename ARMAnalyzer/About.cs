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
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            /*
            UInt32 a = 0xBEFFFA5C;
            UInt32 b = 0xFFFFFFF0;
            UInt32 c = a + b;
            MessageBox.Show(String.Format("{0,8:X} + {1,8:X} = {2,8:X}", a, b, c));
             */
        }

        private void About_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
