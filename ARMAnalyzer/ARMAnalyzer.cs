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
    public partial class ARMAnalyzer : Form
    {
        int listbox_base_idx = 0;

        public string file_path = null;
        public string target_path = null;
        List<Entry> items;
        TaintRange Taint_Range;
        TaintResult child;

        private string target_string;
        private string target_operand;
        private uint target_base_address;
        Queue<Tuple<int, string>> searchQ = new Queue<Tuple<int, string>>();

        Chain Result;

        public ARMAnalyzer()
        {
            InitializeComponent();
            this.Size = new Size(816, 397);
            this.label1.Location = new Point(0, 27);
            this.ListBox.Location = new Point(0, 52);
            this.ListBox.Size = new Size(800, this.Size.Height - 107);
        }

        private void Menu_File_Clear_Click(object sender, EventArgs e)
        {
            this.items.Clear();
            this.Taint_Range = new TaintRange();
            if (this.child != null)
            {
                this.child.Close();
            }
            this.Menu_Analysis_TaintResult.Enabled = false;
            this.searchQ.Clear();
            this.Result = new Chain();

            this.ListBox.Items.Clear();
            this.Status_Label.Text = "";
            this.LoadTime_Label.Text = "";
            this.Status_Progress.Value = 0;
            this.Menu_Analysis_FastMode.Enabled = true;

            this.ck_Search.Checked = false;
            this.ck_Search.Enabled = false;
        }

        private void Menu_Help_About_Click(object sender, EventArgs e)
        {
            About child = new About();
            child.Show();
        }

        private void Menu_File_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Menu_Analysis_TaintRange_Click(object sender, EventArgs e)
        {
            this.Taint_Range.Show();
        }
        private void Menu_Analysis_TaintResult_Click(object sender, EventArgs e)
        {
            this.child.Show();
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy | DragDropEffects.Scroll;
            }
        }

        private void ListBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);

                //MessageBox.Show(file[0]);
                this.file_path = file[0];
                string file_name = file_path.Split('\\')[file_path.Split('\\').Length - 1];
                this.Status_Label.Text = file_name;
                this.Text = this.file_path;

                this.target_path = file_path.Substring(0, file_path.IndexOf(file_name));

                // List Box Init
                this.ListBox.Items.Clear();
                //this.items.Clear();
                items = new List<Entry>();

                this.Status_Label.Text += " [Reading...";
                this.Menu_Analysis_TaintResult.Enabled = false;
                this.bg_Reading.WorkerReportsProgress = true;
                this.bg_Adding.WorkerReportsProgress = true;
                readDump();
            }
        }

        private void Menu_File_Open_Click(object sender, EventArgs e)
        {
            //openFileDialog.InitialDirectory = ".";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.file_path = openFileDialog.FileName;
                string file_name = file_path.Split('\\')[file_path.Split('\\').Length - 1];
                this.Status_Label.Text = file_name;
                this.Text = this.file_path;

                this.target_path = file_path.Substring(0, file_path.IndexOf(file_name));

                // List Box Init
                this.ListBox.Items.Clear();
                //this.items.Clear();
                items = new List<Entry>();

                this.Status_Label.Text += " [Reading...";
                this.Menu_Analysis_TaintResult.Enabled = false;
                this.bg_Reading.WorkerReportsProgress = true;
                this.bg_Adding.WorkerReportsProgress = true;
                readDump();
            }
            //MessageBox.Show("test");
        }

        private void readDump()
        {
            this.Status_Progress.Value = 0;
            this.bg_Reading.RunWorkerAsync();
        }
        private void bg_Reading_DoWork(object sender, DoWorkEventArgs e)
        {
            // Start Read File -------------------------------------------------------------------
            FileStream fs = new FileStream(this.file_path, FileMode.Open, FileAccess.Read);

            int total = (int)(fs.Length / 160);
            int cnt = 0;
            int step = 0;

            Byte[] buffer = new Byte[160];

            int offset = 0;
            Entry item;
            Taint_Range = new TaintRange();

            /* Str Analysis
            string file_name = file_path.Split('\\')[file_path.Split('\\').Length - 1];
            file_name = file_name.Substring(0, file_name.IndexOf("."));
            System.IO.StreamWriter strFile = new System.IO.StreamWriter(this.target_path + file_name + "_str.txt");
            strFile.Close();
            strFile = new System.IO.StreamWriter(this.target_path + file_name + "_str.txt", true);
            List<int> Range_Remove_List = new List<int>();
             */

            while (fs.Length != fs.Position)
            {
                // Progress Bar
                if (step < ((cnt * 100) / total))
                {
                    step = (cnt * 100) / total;
                    //this.LoadTime_Label.Text = String.Format("{0} / {1} ({2,3}%)", cnt, total, this.Status_Progress.Value);
                    this.bg_Reading.ReportProgress(step, new Tuple<int, int> (cnt,total));
                }
                cnt++;

                buffer.Initialize();
                fs.Read(buffer, offset, 160);

                item = new Entry();
                item.SetEntry(buffer);
                this.items.Add(item);

                // Str Analysis -----------------------------------------------------------------------------------
                /*
                if (item.Mnemonic.Contains("str") || item.Mnemonic.Contains("vst") )
                {
                    if (item.Operands_Dst.Count > 0)
                    {
                        int range_total = 0;
                        List<Tuple<uint, uint>> R_range = new List<Tuple<uint, uint>>();
                        foreach (Tuple<uint, uint> temp in this.Taint_Range.GetRanges())
                        {
                            range_total++;
                            R_range.Add(temp);
                        }
                        R_range.Reverse();

                        int i = 0;
                        foreach (string dst in item.Operands_Dst)
                        {
                            // Range Check
                            UInt32 dst_addr = 0;
                            if (dst.Count() > 2)
                            {
                                if (dst.Substring(0, 2).Equals("0x"))
                                {
                                     dst_addr = Convert.ToUInt32(dst.Substring(2), 16);
                                }
                            }

                            int offset_idx = range_total;
                            foreach (Tuple<uint, uint> range in R_range)
                            {
                                offset_idx--;
                                if (range.Item1 <= dst_addr && dst_addr <= range.Item2)
                                {
                                    // Full Line
                                    strFile.Write(
                                        String.Format("{0}\n",
                                            item.GetLine())
                                    );

                                    // Dst
                                    strFile.Write(
                                        String.Format("\t{0} :",
                                            dst)
                                    );
                                    // Src
                                    foreach (string src in item.Operands_Src[i])
                                    {
                                        strFile.Write(
                                            String.Format(" {0}",
                                                src)
                                        );
                                    }
                                    strFile.Write(
                                        String.Format("\n")
                                    );

                                    // Range
                                    strFile.Write(
                                        String.Format("\t{0} : {1} : [0x{2,8:X8} - 0x{3,8:X8}]\n\n",
                                                    this.Taint_Range.GetIndex(offset_idx),
                                                    this.Taint_Range.GetOffset(offset_idx),
                                                    range.Item1, range.Item2
                                        )
                                    );

                                    if (!Range_Remove_List.Contains(offset_idx))
                                    {
                                        Range_Remove_List.Add(offset_idx);
                                    }
                                    break;
                                }
                            }

                            i++;
                        }
                    }
                }
                 */
                // Str Analysis -----------------------------------------------------------------------------------

                // Add Range -----------------------------------------------------------------
                if (((item.BitMask >> 24) & 1) == 1)
                {
                    this.Taint_Range.AddRange(item.Count, item.Operands[13], item.Operands[14], item.Operands[15]);
                }
                // Add Range -----------------------------------------------------------------
            }

            // Remove Range -----------------------------------------------------------------
            /* Str Analysis
            Range_Remove_List.Sort();
            Range_Remove_List.Reverse();
            foreach (int idx in Range_Remove_List)
            {
                //this.Taint_Range.RemoveRange(idx);
            }
             */
            //strFile.Close();
            // Remove Range -----------------------------------------------------------------

            // Add Range -----------------------------------------------------------------
            //this.Taint_Range.AddRange(this.items[this.items.Count - 1].Operands[14], this.items[this.items.Count - 1].Operands[15]);
            // Add Range -----------------------------------------------------------------

            fs.Close();
            // End Read File -------------------------------------------------------------------
        }
        private void bg_Reading_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Status_Progress.Value = e.ProgressPercentage;
            Tuple<int, int> work = e.UserState as Tuple<int, int>;
            this.LoadTime_Label.Text = String.Format("{0} / {1} ({2,3}%)", work.Item1, work.Item2, this.Status_Progress.Value);
        }
        private void bg_Reading_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Status_Label.Text += "OK > Adding...";
            this.LoadTime_Label.Text = "";
            this.Status_Progress.Value = 0;
            addDump();
        }


        private void addDump()
        {
            this.Status_Progress.Value = 0;
            this.bg_Adding.RunWorkerAsync();
        }
        private void bg_Adding_DoWork(object sender, DoWorkEventArgs e)
        {
            this.bg_Adding.ReportProgress(0);
        }
        private void bg_Adding_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Start Add Entry -------------------------------------------------------------------
            int total = items.Count;
            int cnt = 0;
            int step = 0;

            this.ListBox.BeginUpdate();
            this.listbox_base_idx = total - 100;
            foreach (Entry item in items)
            {
                // Progress Bar
                if (step < ((cnt * 100) / total))
                {
                    step = (cnt * 100) / total;
                    this.LoadTime_Label.Text = String.Format("{0} / {1} ({2,3}%)", cnt, total, this.Status_Progress.Value);
                    //this.bg_Adding.ReportProgress(step, new Tuple<int, int>(cnt, total));
                    this.Status_Progress.Value = step;
                }
                cnt++;

                if (cnt > (total - 100))
                    this.ListBox.Items.Add(item.GetLine());
            }
            this.ListBox.EndUpdate();
            // End Add Entry -------------------------------------------------------------------

            //this.Status_Progress.Value = e.ProgressPercentage;
            //Tuple<int, int> work = e.UserState as Tuple<int, int>;
            //this.LoadTime_Label.Text = String.Format("{0} / {1} ({2,3}%)", work.Item1, work.Item2, this.Status_Progress.Value);
        }
        private void bg_Adding_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Status_Label.Text += "OK]";
            this.LoadTime_Label.Text = "";
            this.Status_Progress.Value = 0;

            this.ck_Search.Enabled = true;
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoTo child = new GoTo(this.items.Count());
            child.SendMsg += new GoTo.SendMsgDelegate(goToIndex);
            child.Show();
        }
        private void goToIndex(string addr)
        {
            Int32 index = Convert.ToInt32(addr);
            int view_cnt = 100;

            if ( 0 < index )
            {
                this.ListBox.Items.Clear();
                int cnt;

                if (this.items.Count <= index)
                {
                    index = this.items.Count;
                }

                // items 갯수가 보여주려는 갯수보다 적을 때
                if (this.items.Count <= view_cnt )
                {
                    this.listbox_base_idx = 0;

                    for (cnt = 0; cnt < this.items.Count ; cnt++)
                    {
                        this.ListBox.Items.Add(this.items[cnt].GetLine());
                    }
                }
                // items 갯수가 보여주려는 갯수보다는 많을 때
                else
                {
                    if (index <= view_cnt)
                    {
                        this.listbox_base_idx = 0;
                    }
                    else
                    {
                        this.listbox_base_idx = index - view_cnt;
                    }

                    for (cnt = 0; cnt < view_cnt; cnt++)
                    {
                        this.ListBox.Items.Add(this.items[this.listbox_base_idx + cnt].GetLine());
                    }
                }

                //this.ListBox.Update();

                index = index - this.listbox_base_idx - 1;
                this.ListBox.SetSelected(index, true);
                this.ListBox.SelectedIndex = index;
            }
        }

        private void ListBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.ListBox.SelectedIndex == -1)
            {
                //ListBox.ContextMenuStrip.Enabled = false;
            }
            else
            {
                this.taintOfToolStripMenuItem.Enabled = true;
            }
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            if (this.ListBox.SelectedIndex == -1)
            {
                //ListBox.ContextMenuStrip.Enabled = false;
            }
            else
            {
                this.taintOfToolStripMenuItem.Enabled = true;
                Clipboard.SetData(System.Windows.Forms.DataFormats.Text, this.ListBox.SelectedItem.ToString());

                int index = Int32.Parse(this.ListBox.SelectedItem.ToString().Substring(7).TrimStart().Split()[0]) - 1;

                String msg = "";
                msg += String.Format("{0}\n", this.items[index].GetLine());
                msg += "\n[Operands]------------------------------------------------------\n";
                int i;
                for (i = 0; i < this.items[index].Operand_Cnt;i++ )
                {
                    msg += String.Format("{0,10} : 0x{1,8:X8}\n",
                                        this.items[index].Operands_Name[i],
                                        this.items[index].Operands[i]);
                }
                msg += "\n[Dst, Src]------------------------------------------------------\n";
                i = 0;
                foreach (string dst in this.items[index].Operands_Dst)
                {
                    msg += String.Format("{0,10} :", dst);
                    foreach (string src in this.items[index].Operands_Src[i])
                    {
                        msg += String.Format("\t{0,10}", src);
                    }
                    msg += "\n";
                    i++;
                }

                InfoBox ib = new InfoBox();
                ib.InfoView(msg);
                ib.Show();
                /*
                MessageBox.Show(

                );
                 */
            }

            // uint temp = (uint)BitConverter.ToInt32(this.items[ListBox.SelectedIndex].Opcode, 0);
            // temp = (temp >> 16) & 0xFFFF;
            // temp = temp & 0xFFFF;           // thumb op1
            // temp = (temp >> 16) & 0xFFFF;   // thumb2 op2

            /*
            string temp = this.items[ListBox.SelectedIndex].Mnemonic;
            if (temp.Equals("bx"))
            {
                if ( Char.IsDigit("13"[0]) )
                    MessageBox.Show(temp);
            }
             */
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            //int index = ListBox.SelectedIndex;
            int index = Int32.Parse(this.ListBox.SelectedItem.ToString().Substring(7).TrimStart().Split()[0]) - 1;
            if ((0 <= index) && (index < items.Count))
            {
                this.taintOfToolStripMenuItem.DropDownItems.Clear();

                // Repeat Adding ( Taint Item )
                int total = this.items[index].Operands_Dst.Count;
                int visit = 0;
                for ( visit = 0 ; visit < total ; visit++ ) {
                    string dst = this.items[index].Operands_Dst[visit];
                    foreach (string src in this.items[index].Operands_Src[visit])
                    {
                        ToolStripButton target = new ToolStripButton(
                                String.Format("{0} : {1}", dst, src)
                            );

                        //target.Width = 70;
                        target.AutoSize = true;
                        target.Click += new System.EventHandler(this.tainting);
                        this.taintOfToolStripMenuItem.DropDownItems.Add(target);
                    }
                }
                // ----------------------------
            }
        }
        private void tainting(object sender, EventArgs e)
        {
            this.Menu_Analysis_FastMode.Enabled = false;

            // Target String => "Dst : Src / Target Range : 0x00000000 - 0x00000000"
            this.target_string = String.Format("{0}", ((ToolStripButton)sender).ToString());

            //target_base_address = this.items[this.items.Count - 1].Operands[14];

            string file_name = file_path.Split('\\')[file_path.Split('\\').Length - 1];
            file_name = file_name.Substring(0, file_name.IndexOf("."));

            System.IO.StreamWriter file;
            // FastMode가 켜져 있다면
            if (this.Menu_Analysis_FastMode.Checked == true)
            {
                file = new System.IO.StreamWriter(this.target_path + file_name + "_taint.txt");
            }
            else
            {
                file = new System.IO.StreamWriter(this.target_path + file_name + "_taint_full.txt");
            }
            file.WriteLine(this.target_string);
            int index = Int32.Parse(ListBox.SelectedItem.ToString().Substring(7).TrimStart().Split()[0]) - 1;
            file.WriteLine(this.items[index].GetLine());
            file.Close();

            // Target Operand => "Src"
            this.target_operand = ((ToolStripButton)sender).ToString().Split()[2];

            // Chain Start
            Result = new Chain();
            Result.AddNode(null, new Node(index, ((ToolStripButton)sender).ToString().Split()[0], this.target_operand));

            child = new TaintResult();
            child.ok_target(this.target_string, this.items[index].GetLine());

            this.LoadTime_Label.Text = "Tainting...wait";
            this.Menu_Analysis_TaintResult.Enabled = true;

            int findex = Int32.Parse(ListBox.SelectedItem.ToString().Substring(7).TrimStart().Split()[0]) - 2;
            searchQ.Clear();
            searchQ.Enqueue(new Tuple<int, string>(findex, this.target_operand));
            this.bg_Tainting.WorkerReportsProgress = true;
            this.bg_Tainting.RunWorkerAsync(child);
            child.Show();
        }
        private void bg_Tainting_DoWork(object sender, DoWorkEventArgs e)
        {
            TaintResult child = e.Argument as TaintResult;
            //child.Show();

            int range_total = 0;
            List<Tuple<uint, uint>> R_range = new List<Tuple<uint, uint>>();
            foreach (Tuple<uint, uint> temp in this.Taint_Range.GetRanges())
            {
                range_total++;
                R_range.Add(temp);
            }
            R_range.Reverse();

            /*****************************************************************************
             * Taint Algorithm *
             *****************************************************************************/
            //----------------------------------------------------------------------------
            bool passport = false;
            bool found = false;
            //bool neon_flag = false;
            //int neon_byte_idx = 0;
            //int found_cnt = 0;

            string file_name = file_path.Split('\\')[file_path.Split('\\').Length - 1];
            file_name = file_name.Substring(0, file_name.IndexOf("."));

            System.IO.StreamWriter file;
            System.IO.StreamWriter realfile;
            // FastMode가 켜져 있다면
            if (this.Menu_Analysis_FastMode.Checked == true)
            {
                file = new System.IO.StreamWriter(this.target_path + file_name + "_taint.txt", true);
                realfile = new System.IO.StreamWriter(this.target_path + file_name + "_result.txt", false);
            }
            else
            {
                file = new System.IO.StreamWriter(this.target_path + file_name + "_taint_full.txt", true);
                realfile = new System.IO.StreamWriter(this.target_path + file_name + "_result_full.txt", false);
            }
            realfile.Close();


            int total = items.Count;
            int step = 0;
            this.bg_Tainting.ReportProgress(step, new Tuple<int, int>(0, total));

            // target < index, target_op_name >
            Tuple<int, string> target = searchQ.Dequeue();
            for (; target.Item1 >= 0; target = searchQ.Dequeue())
            {
                passport = false;

                int index = target.Item1;
                this.target_operand = target.Item2;

                /*
                if ( this.target_operand[0] == 'd' ) {
                    //neon_byte_idx = Convert.ToUInt32(this.target_operand.Substring(3), 10);
                    //this.target_operand = this.target_operand.Substring(0, this.target_operand.IndexOf('.'));
                    MessageBox.Show(this.target_operand);
                }
                */

                int i = 0;
                // 먼저, 탐색하는 Entry의 Dst 목록에서 하나씩 가져와서 검사
                foreach (string visit_dst in this.items[index].Operands_Dst)
                {
                    visit_dst.TrimEnd('!');
                    // Entry의 Dst와 찾고자 하는 값과 일치할 경우
                    if (visit_dst == target_operand)
                    {
                        passport = true;

						// 찾는 값과 Dst가 일치하는 Entry의 Src를 하나씩 가져와서 처리
                        foreach (string visit_src in this.items[index].Operands_Src[i])
                        {
                            // END 인 경우는 아예 탐색 중지
							// SP 인 경우는 탐색할 타겟은 필요 없음, PC도, FP도
                            //if (visit_src.Equals("sp") || visit_src.Equals("pc") || visit_src.Equals("fp") || visit_src.Equals("sb") || visit_src.Equals("ip"))
                            if (visit_src.Equals("END") || visit_src.Equals("pc"))
                            {
                                continue;
                            }

                            // FastMode가 켜져 있다면
                            if (this.Menu_Analysis_FastMode.Checked == true)
                            {
                                // LDR 계열의 명령어라면
                                if (this.items[index].Mnemonic.Contains("ldr"))
                                {
                                    // Src 오퍼랜드의 글자가 2글자 이하라면 통과
                                    if (visit_src.Count() <= 2)
                                    {
                                        continue;
                                    }
                                    // 그리고 그 2글자가 0x로 시작하는게 아니여도 통과
                                    if (!visit_src.Substring(0, 2).Equals("0x"))
                                    {
                                        continue;
                                    }
                                }
                            }

                            // 먼저 Src는 큐에 넣기 좋게 만들고, 큐에 중복해서 들어가있지 않나 검사
                            Tuple<int, string> new_target = new Tuple<int, string>(index - 1, visit_src);
                            if (!searchQ.Contains(new_target))
                            {
								// 파일에 탐색 Entry의 Src를 추가
                                file.WriteLine(
                                    String.Format("{0} | {1}",
                                        String.Format("{0,10} : {1,10}",
                                            visit_dst, visit_src
                                        ),
                                        items[index].GetLine()
                                    )
                                );
                                /*
                                child.add_tree(this.items[index],
                                    String.Format("{0,10} / {1,10} : {2,10}",
                                        this.target_operand, visit_dst, visit_src
                                    ),
                                    items[index].GetLine()
                                );
                                */

                                // Chain에 연결시키기
                                Result.AddNode(
                                    Result.FindNode(index, visit_dst),
                                    new Node(index, visit_dst, visit_src)
                                    );

                                // 탐색 큐에 Entry의 Src를 타겟으로 하여 삽입
                                searchQ.Enqueue(new_target);

                                // Src 오퍼랜드의 글자가 2글자 이상이라면
                                if (visit_src.Count() > 2)
                                {
									// 그리고 그 2글자가 0x로 시작한다면
                                    if (visit_src.Substring(0, 2).Equals("0x"))
                                    {
                                        UInt32 src_addr = Convert.ToUInt32(visit_src.Substring(2), 16);

                                        int offset_idx = range_total;
										// Taint Range 로부터 범위를 하나씩 가져와서
                                        foreach (Tuple<uint, uint> range in R_range)
                                        {
                                            offset_idx--;
                                            target_base_address = this.Taint_Range.GetOffset(offset_idx);
                                            uint offset = target_base_address + (src_addr - range.Item1);
                                            //offset = src_addr - this.items[this.items.Count - 1].Operands[14];

                                            if (index < this.Taint_Range.GetIndex(offset_idx))
                                            {
                                                continue;
                                            }

                                            // 탐색 Entry의 Src가 Taint Range에 포함이 된다면
                                            if ((range.Item1 <= src_addr) && (src_addr <= range.Item2))
                                            {
                                                // Result 파일에도 찾았다고 표시
                                                // FastMode가 켜져 있다면
                                                if (this.Menu_Analysis_FastMode.Checked == true)
                                                {
                                                    realfile = new System.IO.StreamWriter(this.target_path + file_name + "_result.txt", true);
                                                }
                                                else
                                                {
                                                    realfile = new System.IO.StreamWriter(this.target_path + file_name + "_result_full.txt", true);
                                                }
                                                realfile.WriteLine(
                                                   String.Format("{0} | {1}",
                                                       String.Format("{0,10} : {1,10}",
                                                           visit_dst, visit_src
                                                       ),
                                                       String.Format("Found! Probably Exploitable? // 0x{0,8:X8} = {1}",
                                                           offset,
                                                           offset
                                                       )
                                                   )
                                                );
                                                // 반복해서 한 줄 씩 추가
                                                Node visit = Result.Curr;
                                                while (visit.next != null)
                                                {
                                                    realfile.WriteLine(
                                                        String.Format("{0,10} : {1,10} | {2}",
                                                            visit.dst, visit.src, items[visit.index].GetLine()
                                                        )
                                                    );
                                                    visit = visit.next;
                                                }
                                                realfile.WriteLine(
                                                    String.Format("{0,10} : {1,10} | {2}\n",
                                                        visit.dst, visit.src, items[visit.index].GetLine()
                                                    )
                                                );
                                                realfile.Close();



                                                // 파일에 "심봤다!"는 내용을 추가
                                                file.WriteLine(
                                                    String.Format("{0} | {1}",
                                                        String.Format("{0,10} : {1,10}",
                                                            visit_dst, visit_src
                                                        ),
                                                        String.Format("Found! Probably Exploitable? // 0x{0,8:X8} = {1}",
                                                            offset,
                                                            offset
                                                        )
                                                    )
                                                );

                                                // Taint Result에도 "심봤다!"는 내용을 추가
                                                child.add_tree(this.items[index],
                                                    String.Format("{0,10} : {1,10}",
                                                        visit_dst, visit_src
                                                    ),
                                                    String.Format("Found! Probably Exploitable? // 0x{0,8:X8} = {1}",
                                                        offset,
                                                        offset
                                                    )
                                                );

                                                child.adding(items, Result.Curr);

                                                //searchQ.Enqueue(new_target);

                                                //searchQ.Clear();
                                                //searchQ.Enqueue(new Tuple<int, string>(-1, "Target Find"));

                                                found = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    i++;
                }

                // 찾고자 하는 값과 일치하는 Entry의 Dst가 존재하지 않았다면, 그 전 Entry를 탐색하기 위해 큐에 추가
                if (passport == false)
                {
                    // 먼저 Src는 큐에 넣기 좋게 만들고, 큐에 중복해서 들어가있지 않나 검사
                    Tuple<int, string> new_target = new Tuple<int, string>(index - 1, this.target_operand);
                    if (!searchQ.Contains(new_target)) {
                        searchQ.Enqueue(new_target);
                    }
                }
                if (searchQ.Count == 0)
                {
                    break;
                }

                if (step < (((total - index) * 100) / total))
                {
                    step = ((total - index) * 100) / total;
                    //this.bg_Tainting.ReportProgress(step, new Tuple<int, int>(index, total));
                    this.bg_Tainting.ReportProgress(step, new Tuple<int, int>(index, total));
                }

            } // END for ( ; target.Item1 >= 0 ; target = searchQ.Dequeue() )

            if (found == false)
            {
                // Taint Result에도 "심봤다!"는 내용을 추가
                child.add_tree(this.items[0],
                    String.Format("{0,10} : {1,10}", 0, 0),
                    String.Format("Not Found... ")
                );
            }
            //----------------------------------------------------------------------------

            //e.Result = child;
            file.Close();
        }
        private void bg_Tainting_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Status_Progress.Value = e.ProgressPercentage;
            Tuple<int, int> work = e.UserState as Tuple<int, int>;

            this.LoadTime_Label.Text = String.Format("{0} / {1} ({2,3}%)", work.Item1, work.Item2, this.Status_Progress.Value);
        }
        private void bg_Tainting_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            this.Status_Label.Text = "Analysis Complete";
            this.LoadTime_Label.Text = "";
            this.Status_Progress.Value = this.Status_Progress.Maximum;

            /*
            TaintResult child = e.Result as TaintResult;
            child.Show();
             */

            this.Menu_Analysis_FastMode.Enabled = true;
        }


        /*********************************************************************************************************
         *
         * Search Panel Control
         *
         *********************************************************************************************************/

        private int Searching = 0;

        private void ck_Search_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ck_Search.Checked == false)
            {
                this.panel_Search.Visible = false;
                this.Size = new Size(this.Size.Width, this.Size.Height - 138);
                this.label1.Location = new Point(0, 27);
                this.ListBox.Location = new Point(0, 52);
                this.ListBox.Size = new Size(this.ListBox.Size.Width, this.Size.Height - 107);
            }
            else
            {
                this.panel_Search.Visible = true;
                this.Size = new Size(this.Size.Width, this.Size.Height + 138);
                this.label1.Location = new Point(0, 165);
                this.ListBox.Location = new Point(0, 190);
                this.ListBox.Size = new Size(this.ListBox.Size.Width, this.Size.Height - 107 - 138);
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "List View File (*.txt)|*.txt";
            this.saveFileDialog.InitialDirectory = System.Environment.CurrentDirectory;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = this.saveFileDialog.FileName.ToString();
                if (filename != "")
                {
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        foreach (var item in this.ListBox.Items)
                        {
                            sw.WriteLine(item.ToString());
                        }
                        sw.Close();
                    }
                }
            }
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            this.Searching = 1;
            this.gb_State.Enabled = false;
            this.ck_Index_All.Enabled = false;
            this.tb_Index_Start.Enabled = false;
            this.tb_Index_End.Enabled = false;
            this.ck_Addr_All.Enabled = false;
            this.tb_Addr_Start.Enabled = false;
            this.tb_Addr_End.Enabled = false;
            this.ck_Disassem_Mne.Enabled = false;
            this.ck_Disassem_OpStr.Enabled = false;
            this.ck_Disassem_Real.Enabled = false;
            this.ck_Disassem_Dst.Enabled = false;
            this.ck_Disassem_Src.Enabled = false;
            this.ck_Disassem_Ops.Enabled = false;
            this.tb_Disassem_Mne.Enabled = false;
            this.tb_Disassem_OpStr.Enabled = false;
            this.tb_Disassem_Real.Enabled = false;
            this.bt_Search.Enabled = false;
            this.bt_Stop.Enabled = true;
            this.bt_Cont.Enabled = false;
            this.bt_Save.Enabled = false;

            this.bg_Searching.RunWorkerAsync();
        }

        private void bt_Stop_Click(object sender, EventArgs e)
        {
            // Stop Searching
            if ( this.Searching == 2 ) {
                this.Searching = 0;
                this.gb_State.Enabled = true;
                this.ck_Index_All.Enabled = true;
                if ( this.ck_Index_All.Checked == false ) {
                    this.tb_Index_Start.Enabled = true;
                    this.tb_Index_End.Enabled = true;
                }
                this.ck_Addr_All.Enabled = true;
                if ( this.ck_Addr_All.Checked == false ) {
                    this.tb_Addr_Start.Enabled = true;
                    this.tb_Addr_End.Enabled = true;
                }
                this.ck_Disassem_Mne.Enabled = true;
                if ( this.ck_Disassem_Mne.Checked == true ) {
                    this.tb_Disassem_Mne.Enabled = true;
                }
                this.ck_Disassem_OpStr.Enabled = true;
                if ( this.ck_Disassem_OpStr.Checked == true ) {
                    this.tb_Disassem_OpStr.Enabled = true;
                }
                this.ck_Disassem_Real.Enabled = true;
                if ( this.ck_Disassem_Real.Checked == true ) {
                    this.tb_Disassem_Real.Enabled = true;
                    this.ck_Disassem_Dst.Enabled = true;
                    this.ck_Disassem_Src.Enabled = true;
                    this.ck_Disassem_Ops.Enabled = true;
                }
                this.bt_Search.Enabled = true;
                this.bt_Stop.Text = "Pause";
                this.bt_Stop.Enabled = false;
                this.bt_Cont.Enabled = false;
                this.bt_Save.Enabled = true;
            }
            // Keep Going
            else {
                this.Searching = 2;
                this.bt_Stop.Text = "Stop";
                this.bt_Stop.Enabled = true;
                this.bt_Cont.Enabled = true;
            }

        }

        private void bt_Cont_Click(object sender, EventArgs e)
        {
            this.Searching = 3;
            this.gb_State.Enabled = false;
            this.ck_Index_All.Enabled = false;
            this.tb_Index_Start.Enabled = false;
            this.tb_Index_End.Enabled = false;
            this.ck_Addr_All.Enabled = false;
            this.tb_Addr_Start.Enabled = false;
            this.tb_Addr_End.Enabled = false;
            this.ck_Disassem_Mne.Enabled = false;
            this.ck_Disassem_OpStr.Enabled = false;
            this.ck_Disassem_Real.Enabled = false;
            this.ck_Disassem_Dst.Enabled = false;
            this.ck_Disassem_Src.Enabled = false;
            this.ck_Disassem_Ops.Enabled = false;
            this.tb_Disassem_Mne.Enabled = false;
            this.tb_Disassem_OpStr.Enabled = false;
            this.tb_Disassem_Real.Enabled = false;
            this.bt_Search.Enabled = false;
            this.bt_Stop.Text = "Pause";
            this.bt_Stop.Enabled = true;
            this.bt_Cont.Enabled = false;
            this.bt_Save.Enabled = false;
        }

        private void ck_Disassem_Mne_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ck_Disassem_Mne.Checked == true)
            {
                this.tb_Disassem_Mne.Enabled = true;
            }
            else
            {
                this.tb_Disassem_Mne.Enabled = false;
            }
        }

        private void ck_Disassem_OpStr_CheckedChanged(object sender, EventArgs e)
        {
            if ( this.ck_Disassem_OpStr.Checked == true)
            {
                this.tb_Disassem_OpStr.Enabled = true;
            }
            else
            {
                this.tb_Disassem_OpStr.Enabled = false;
            }
        }

        private void ck_Disasem_Real_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ck_Disassem_Real.Checked == true)
            {
                this.ck_Disassem_Ops.Enabled = true;
                this.ck_Disassem_Dst.Enabled = true;
                this.ck_Disassem_Src.Enabled = true;
                this.tb_Disassem_Real.Enabled = true;
            }
            else
            {
                this.ck_Disassem_Ops.Enabled = false;
                this.ck_Disassem_Dst.Enabled = false;
                this.ck_Disassem_Src.Enabled = false;
                this.tb_Disassem_Real.Enabled = false;
            }
        }

        private void ck_Index_All_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ck_Index_All.Checked == true) {
                this.tb_Index_Start.Enabled = false;
                this.tb_Index_End.Enabled = false;
            }
            else {
                this.tb_Index_Start.Enabled = true;
                this.tb_Index_End.Enabled = true;
            }
        }

        private void ck_Addr_All_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ck_Addr_All.Checked == true) {
                this.tb_Addr_Start.Enabled = false;
                this.tb_Addr_End.Enabled = false;
            }
            else {
                this.tb_Addr_Start.Enabled = true;
                this.tb_Addr_End.Enabled = true;
            }
        }

        private void bg_Searching_DoWork(object sender, DoWorkEventArgs e)
        {
            this.ListBox.Items.Clear();

            int state = 0;
            Tuple<uint, uint> index;
            Tuple<uint, uint> addr;
            String mne = "";
            String op_str = "";
            String real = "";
            int dstsrc = 0;

            // State
            if ( this.rb_All.Checked == true ) {
                state = 0;
            }
            else if ( this.rb_Arm.Checked == true ) {
                state = 1;
            }
            else {
                state = 2;
            }

            // Index
            uint start = 0;
            uint end = 0;
            if (this.ck_Index_All.Checked == true)
            {
                start = 1;
                end = (uint)(this.items.Count());
            }
            else
            {
                // start
                if (this.tb_Index_Start.Text.Count() > 2)
                {
                    // Hex
                    if (this.tb_Index_Start.Text.Substring(0, 2).Equals("0x"))
                    {
                        start = Convert.ToUInt32(this.tb_Index_Start.Text.Substring(2), 16);
                    }
                    // Decimal
                    else
                    {
                        start = Convert.ToUInt32(this.tb_Index_Start.Text, 10);
                    }
                }
                // Decimal
                else
                {
                    start = Convert.ToUInt32(this.tb_Index_Start.Text, 10);
                }
                if (start < 1)
                {
                    start = 1;
                }

                // end
                if (this.tb_Index_End.Text.Count() > 2)
                {
                    // Hex
                    if (this.tb_Index_End.Text.Substring(0, 2).Equals("0x"))
                    {
                        end = Convert.ToUInt32(this.tb_Index_End.Text.Substring(2), 16);
                    }
                    // Decimal
                    else
                    {
                        end = Convert.ToUInt32(this.tb_Index_End.Text, 10);
                    }
                }
                // Decimal
                else
                {
                    end = Convert.ToUInt32(this.tb_Index_End.Text, 10);
                }
                if (this.items.Count() < end)
                {
                    end = (uint)(this.items.Count());
                }
            }
            if (start > end)
            {
                start = (uint)(this.items.Count()) - 1;
                end = (uint)(this.items.Count());
            }
            index = new Tuple<uint, uint>(start-1, end-1);

            // Address
            if (this.ck_Addr_All.Checked == true)
            {
                start = 0;
                end = 0xFFFFFFFF;
            }
            else
            {
                if (this.tb_Addr_Start.Text.Count() > 2)
                {
                    // Hex
                    if (this.tb_Addr_Start.Text.Substring(0, 2).Equals("0x"))
                    {
                        start = Convert.ToUInt32(this.tb_Addr_Start.Text.Substring(2), 16);
                    }
                    // Decimal
                    else
                    {
                        start = Convert.ToUInt32(this.tb_Addr_Start.Text, 10);
                    }
                }
                // Decimal
                else
                {
                    start = Convert.ToUInt32(this.tb_Addr_Start.Text, 10);
                }

                if (this.tb_Addr_End.Text.Count() > 2)
                {
                    // Hex
                    if (this.tb_Addr_End.Text.Substring(0, 2).Equals("0x"))
                    {
                        end = Convert.ToUInt32(this.tb_Addr_End.Text.Substring(2), 16);
                    }
                    // Decimal
                    else
                    {
                        end = Convert.ToUInt32(this.tb_Addr_End.Text, 10);
                    }
                }
                // Decimal
                else
                {
                    end = Convert.ToUInt32(this.tb_Addr_End.Text, 10);
                }
            }
            addr = new Tuple<uint, uint>(start, end);

            // Opcode
            // To be continue...

            // Disassembly
            if (this.ck_Disassem_Mne.Checked == true)
            {
                mne = this.tb_Disassem_Mne.Text;
            }
            else
            {
                mne = "!$#@%";
            }
            if (this.ck_Disassem_OpStr.Checked == true)
            {
                op_str = this.tb_Disassem_OpStr.Text;
            }
            else
            {
                op_str = "!$#@%";
            }
            if (this.ck_Disassem_Real.Checked == true)
            {
                real = this.tb_Disassem_Real.Text;
                if (this.ck_Disassem_Dst.Checked == true)
                {
                    dstsrc = dstsrc | 0x100;
                }
                if (this.ck_Disassem_Src.Checked == true)
                {
                    dstsrc = dstsrc | 0x010;
                }
                if (this.ck_Disassem_Ops.Checked == true)
                {
                    dstsrc = dstsrc | 0x001;
                }
            }
            else
            {
                dstsrc = 0;
                real = "!$#@%";
            }

            /*
            MessageBox.Show(
                String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}",
                    state,
                    index.ToString(),
                    addr.ToString(),
                    mne,
                    op_str,
                    real,
                    dstsrc
                )
                );
             */

            // Start Searching
            bool find = false;

            int idx = (int)index.Item1;
            for (; this.Searching > 0 && idx <= (int)index.Item2; idx++ )
            {
                while (this.Searching == 2)
                {
                    Thread.Sleep(10);
                }

                find = false;
                if (mne == "!$#@%" && op_str == "!$#@%" && real == "!$#@%")
                {
                    find = true;
                }

                Entry item = this.items[idx];
                // Step 1 > Check Address
                if (addr.Item1 <= item.Address && item.Address <= addr.Item2)
                {
                    // Step 2 - 1 > Check Mnemonic
                    if (item.Mnemonic.Contains(mne))
                    {
                        find = true;
                    }

                    // Step 2 - 2 > Check Operand String
                    if (item.OperandStr.Contains(op_str))
                    {
                        find = true;
                    }

                    // Step 2 - 3 > Check Real Value
                    if ( dstsrc > 0 )
                    {
                        if ((dstsrc&0x100) == 0x100)
                        {
                            // dst
                            foreach (string dst in item.Operands_Dst)
                            {
                                if (dst.Contains(real))
                                {
                                    find = true;
                                }
                            }
                        }
                        if ((dstsrc & 0x010) == 0x010)
                        {
                            // src
                            foreach (List<string> src_list in item.Operands_Src)
                            {
                                foreach (string src in src_list)
                                {
                                    if (src.Contains(real))
                                    {
                                        find = true;
                                    }
                                }
                            }
                        }
                        if ((dstsrc & 0x001) == 0x001)
                        {
                            // just operands
                            uint real_val = 0xffffffff;
                            if (real.Count() > 2)
                            {
                                // Hex
                                if (real.Substring(0, 2).Equals("0x"))
                                {
                                    real_val = Convert.ToUInt32(real.Substring(2), 16);
                                }
                                // Decimal
                                else
                                {
                                    bool isnum = true;
                                    foreach (char a in real)
                                    {
                                        if (!Char.IsNumber(a))
                                        {
                                            isnum = false;
                                            break;
                                        }
                                    }
                                    if (isnum == true)
                                    {
                                        real_val = Convert.ToUInt32(real, 10);
                                    }
                                }
                            }
                            // Decimal
                            else
                            {
                                bool isnum = true;
                                foreach (char a in real)
                                {
                                    if (!Char.IsNumber(a))
                                    {
                                        isnum = false;
                                        break;
                                    }
                                }
                                if (isnum == true)
                                {
                                    real_val = Convert.ToUInt32(real, 10);
                                }
                            }
                            foreach (uint operand in item.Operands)
                            {
                                if (operand == real_val)
                                {
                                    find = true;
                                }
                            }
                        }
                    }
                    // Step 2 - 3 > Check Real Value End

                }

                if (find == true)
                {
                    this.ListBox.Items.Add(item.GetLine());
                }
            }
        }

        private void bg_Searching_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bg_Searching_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Searching = 0;
            this.gb_State.Enabled = true;
            this.ck_Index_All.Enabled = true;
            if ( this.ck_Index_All.Checked == false ) {
                this.tb_Index_Start.Enabled = true;
                this.tb_Index_End.Enabled = true;
            }
            this.ck_Addr_All.Enabled = true;
            if ( this.ck_Addr_All.Checked == false ) {
                this.tb_Addr_Start.Enabled = true;
                this.tb_Addr_End.Enabled = true;
            }
            this.ck_Disassem_Mne.Enabled = true;
            if ( this.ck_Disassem_Mne.Checked == true ) {
                this.tb_Disassem_Mne.Enabled = true;
            }
            this.ck_Disassem_OpStr.Enabled = true;
            if ( this.ck_Disassem_OpStr.Checked == true ) {
                this.tb_Disassem_OpStr.Enabled = true;
            }
            this.ck_Disassem_Real.Enabled = true;
            if ( this.ck_Disassem_Real.Checked == true ) {
                this.tb_Disassem_Real.Enabled = true;
                this.ck_Disassem_Dst.Enabled = true;
                this.ck_Disassem_Src.Enabled = true;
            }
            this.bt_Search.Enabled = true;
            this.bt_Stop.Text = "Pause";
            this.bt_Stop.Enabled = false;
            this.bt_Cont.Enabled = false;
            this.bt_Save.Enabled = true;
            //MessageBox.Show("Searching Complete");
        }

        private void ARMAnalyzer_Load(object sender, EventArgs e)
        {

        }
    }
}
