using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Threading;

namespace ARMAnalyzer
{
    public partial class TaintResult : Form
    {
        List<Entry> rtree = new List<Entry>();
        TreeNode LastFound;

        delegate void addBoxLineCallback(TreeNode root, TreeNode child);
        private void addBoxLineInvoke(TreeNode root, TreeNode child)
        {
            if ( root == null )
            {
                this.treeView_TaintResult_Result.Nodes.Add(child);
            }
            else
            {
                root.Nodes.Add(child);
            }
        }
        private void addBoxLine(TreeNode root, TreeNode child)
        {
            if (this.treeView_TaintResult_Result.InvokeRequired)
            {
                this.Invoke(new addBoxLineCallback(addBoxLineInvoke), new object[] { root, child });
            }
            else
            {
                // Nothing
            }
        }

        public TaintResult()
        {
            InitializeComponent();
            
            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(TaintResult_KeyPress);
        }

        private void TaintResult_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Hide();
            /*
            if (e.KeyChar == (int)Keys.Escape)
            {

            }
             */
        }

        private void TaintResult_Load(object sender, EventArgs e)
        {
            this.Update();
        }

        public void ok_target(string data, string line)
        {
            label1.Text = data + "\n" + line;
        }

        public void add_tree(Entry item, string DstSrc, string getLine)
        {
            string key = item.Count.ToString();
            string line = String.Format("   {0} | {1}\n", DstSrc, getLine);

            this.LastFound = new TreeNode();
            this.LastFound.Text = line;
            this.LastFound.ImageIndex = 0;
            this.LastFound.SelectedImageIndex = 0;
            addBoxLine(null, LastFound);
        }
        public void adding(List<Entry> items, Node visit)
        {
            TreeNode childNode;
            while (visit.next != null)
            {
                childNode = new TreeNode();
                childNode.Text = String.Format("{0,10} : {1,10} | {2}\n",
                        visit.dst, visit.src, items[visit.index].GetLine()
                    );
                addBoxLine(this.LastFound, childNode);

                visit = visit.next;
            }
            childNode = new TreeNode();
            childNode.Text = String.Format("{0,10} : {1,10} | {2}\n",
                    visit.dst, visit.src, items[visit.index].GetLine()
                );
            addBoxLine(this.LastFound, childNode);
        }

        private void btn_TaintResult_OK_Click(object sender, EventArgs e)
        {
            //this.Close();
            this.Hide();
        }

        private void btn_TaintResult_SAVE_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Taint Result File (*.txt)|*.txt";
            this.saveFileDialog.InitialDirectory = System.Environment.CurrentDirectory;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                /*
                FileStream sFile = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite);

                string byte_buffer_string = treeView_TaintResult_Result.Text;
                int byte_buffer = Encoding.Default.GetByteCount(byte_buffer_string);
                byte[] bData = new Byte[byte_buffer];

                bData = Encoding.Default.GetBytes(treeView_TaintResult_Result.Text);
                sFile.Seek(0, SeekOrigin.Begin);
                sFile.Write(bData, 0, bData.Length);

                sFile.Close();
                 */
                StringBuilder sb = new StringBuilder();
                foreach (TreeNode node in treeView_TaintResult_Result.Nodes)
                {
                    sb.AppendLine(node.Text);
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        sb.AppendLine(node2.Text);
                    }
                }

                File.WriteAllText(saveFileDialog.FileName, sb.ToString());
            }
        }
    }
}
