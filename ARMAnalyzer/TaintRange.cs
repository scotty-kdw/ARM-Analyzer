using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace ARMAnalyzer
{
    public partial class TaintRange : Form
    {
        private List<uint> Index = new List<uint>();
        private List<uint> Offset = new List<uint>();
        private List<Tuple<uint, uint>> Range = new List<Tuple<uint, uint>>();
        //private uint total_size;

        public TaintRange()
        {
            //this.total_size = 0;
            InitializeComponent();
        }

        public uint GetIndex(int idx)
        {
            return this.Index[idx];
        }

        public uint GetOffset(int idx)
        {
            return this.Offset[idx];
        }

        public List<Tuple<uint, uint>> GetRanges()
        {
            return this.Range;
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void AddRange(uint idx, uint offset, uint start, uint end)
        {
            String idx_str = String.Format("{0}", idx);
            String offset_str = String.Format("0x{0,8:X8}", offset);
            String start_str = String.Format("0x{0,8:X8}", start);
            String end_str = String.Format("0x{0,8:X8}", end);
            String size = String.Format("{0}", end - start + 1);

            ListViewItem item = new ListViewItem(offset_str + "," + start_str + "," + end_str);
            item.SubItems.Add(idx_str);
            item.SubItems.Add(offset_str);
            item.SubItems.Add(start_str);
            item.SubItems.Add(end_str);
            item.SubItems.Add(size);

            bool doing = true;
            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (this.listView.Items[i].Text == item.Text)
                {
                    //this.listView.Items[i].Selected = true;
                    //this.listView.Items[i].Focused = true;
                    doing = false;
                    break;
                }
            }
            if (doing == true)
            {
                this.Index.Add(idx);
                this.Offset.Add(offset);
                this.Range.Add(new Tuple<uint, uint>(start, end));
                this.listView.Items.Add(item);
            }
        }
        private void btn_Add_Click(object sender, EventArgs e)
        {
            if ( (box_Offset.Text.Substring(0,2).Equals("0x")) && (box_Offset.TextLength == 10)
                && (box_Start.Text.Substring(0,2).Equals("0x")) && (box_Start.TextLength == 10)
                && (box_End.Text.Substring(0,2).Equals("0x")) && (box_End.TextLength == 10))
            {
                UInt32 idx;
                if ((box_Index.Text.Substring(0, 2).Equals("0x")) && (box_Index.TextLength == 10))
                {
                    idx = Convert.ToUInt32(box_Index.Text.Substring(2), 16);
                }
                else
                {
                    idx = Convert.ToUInt32(box_Index.Text, 10);
                }
                
                UInt32 offset = Convert.ToUInt32(box_Offset.Text.Substring(2), 16);
                UInt32 start = Convert.ToUInt32(box_Start.Text.Substring(2), 16);
                UInt32 end = Convert.ToUInt32(box_End.Text.Substring(2), 16);

                this.AddRange(idx, offset, start, end);
            }
            listView.Update();
        }

        public void RemoveRange(int idx)
        {
            this.Offset.RemoveAt(idx);
            this.Range.RemoveAt(idx);
            listView.Items.RemoveAt(idx);
        }
        private void btn_Remove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem temp in listView.SelectedItems)
            {
                int idx = temp.Index;
                /*
                MessageBox.Show(
                    String.Format(
                        "({3}) 0x{0,8:X8} : 0x{1,8:X8} - 0x{2,8:X8}",
                        this.Offset[idx],
                        this.Range[idx].Item1,
                        this.Range[idx].Item2,
                        idx
                    )
                );
                 */

                this.Offset.RemoveAt(idx);
                this.Range.RemoveAt(idx);
                listView.Items.RemoveAt(idx);
            }

            listView.Update();
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*
            foreach (ListViewItem item in this.listView.Items)
            {
                MessageBox.Show(item.Text);
            }
             */
        }

        private void bt_SAVE_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Taint Range File (*.txt)|*.txt";
            this.saveFileDialog.InitialDirectory = System.Environment.CurrentDirectory;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = this.saveFileDialog.FileName.ToString();
                if (filename != "")
                {
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        foreach (ListViewItem item in this.listView.Items)
                        {
                            sw.WriteLine("{0,10} : {1} : {2} ~ {3} ({4})",
                                item.SubItems[1].Text,
                                item.SubItems[2].Text,
                                item.SubItems[3].Text,
                                item.SubItems[4].Text,
                                item.SubItems[5].Text);
                        }
                        sw.Close();
                    }
                }
                /*
                FileStream sFile = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite);
                string byte_buffer_string = listView.Text;
                int byte_buffer = Encoding.Default.GetByteCount(byte_buffer_string);
                byte[] bData = new Byte[byte_buffer];

                bData = Encoding.Default.GetBytes(listView.Text);
                sFile.Seek(0, SeekOrigin.Begin);
                sFile.Write(bData, 0, bData.Length);
                sFile.Close();
                 */
            }
        }
    }
}
