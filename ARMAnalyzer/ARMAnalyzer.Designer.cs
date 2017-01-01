namespace ARMAnalyzer
{
    partial class ARMAnalyzer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ARMAnalyzer));
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Analysis = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Analysis_TaintRange = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Analysis_TaintResult = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Analysis_FastMode = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Help_About = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ListBox = new System.Windows.Forms.ListBox();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.taintOfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.Status_Progress = new System.Windows.Forms.ToolStripProgressBar();
            this.Status_Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.bg_Tainting = new System.ComponentModel.BackgroundWorker();
            this.bg_Reading = new System.ComponentModel.BackgroundWorker();
            this.bg_Adding = new System.ComponentModel.BackgroundWorker();
            this.LoadTime_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_Search = new System.Windows.Forms.Panel();
            this.ck_Disassem_Ops = new System.Windows.Forms.CheckBox();
            this.tb_Disassem_OpStr = new System.Windows.Forms.TextBox();
            this.tb_Disassem_Mne = new System.Windows.Forms.TextBox();
            this.ck_Disassem_OpStr = new System.Windows.Forms.CheckBox();
            this.ck_Disassem_Real = new System.Windows.Forms.CheckBox();
            this.label_Op_Val = new System.Windows.Forms.Label();
            this.tb_Op_Val = new System.Windows.Forms.TextBox();
            this.ck_Addr_All = new System.Windows.Forms.CheckBox();
            this.tb_Addr_End = new System.Windows.Forms.TextBox();
            this.tb_Addr_Start = new System.Windows.Forms.TextBox();
            this.label_Addr_End = new System.Windows.Forms.Label();
            this.label_Addr_Start = new System.Windows.Forms.Label();
            this.ck_Index_All = new System.Windows.Forms.CheckBox();
            this.tb_Index_End = new System.Windows.Forms.TextBox();
            this.gb_State = new System.Windows.Forms.GroupBox();
            this.rb_Thumb = new System.Windows.Forms.RadioButton();
            this.rb_Arm = new System.Windows.Forms.RadioButton();
            this.rb_All = new System.Windows.Forms.RadioButton();
            this.ck_Disassem_Mne = new System.Windows.Forms.CheckBox();
            this.bt_Save = new System.Windows.Forms.Button();
            this.ck_Disassem_Src = new System.Windows.Forms.CheckBox();
            this.ck_Disassem_Dst = new System.Windows.Forms.CheckBox();
            this.tb_Disassem_Real = new System.Windows.Forms.TextBox();
            this.tb_Index_Start = new System.Windows.Forms.TextBox();
            this.label_Index_End = new System.Windows.Forms.Label();
            this.label_Index_Start = new System.Windows.Forms.Label();
            this.bt_Cont = new System.Windows.Forms.Button();
            this.bt_Search = new System.Windows.Forms.Button();
            this.bt_Stop = new System.Windows.Forms.Button();
            this.ck_Search = new System.Windows.Forms.CheckBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.bg_Searching = new System.ComponentModel.BackgroundWorker();
            this.MenuBar.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.panel_Search.SuspendLayout();
            this.gb_State.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuBar
            // 
            this.MenuBar.BackColor = System.Drawing.SystemColors.MenuBar;
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Analysis,
            this.Menu_Help});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(800, 24);
            this.MenuBar.TabIndex = 0;
            this.MenuBar.Text = "Menu";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_Open,
            this.Menu_File_Clear,
            this.Menu_File_Exit});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // Menu_File_Open
            // 
            this.Menu_File_Open.Name = "Menu_File_Open";
            this.Menu_File_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.Menu_File_Open.Size = new System.Drawing.Size(146, 22);
            this.Menu_File_Open.Text = "Open";
            this.Menu_File_Open.Click += new System.EventHandler(this.Menu_File_Open_Click);
            // 
            // Menu_File_Clear
            // 
            this.Menu_File_Clear.Name = "Menu_File_Clear";
            this.Menu_File_Clear.Size = new System.Drawing.Size(146, 22);
            this.Menu_File_Clear.Text = "Clear";
            this.Menu_File_Clear.Click += new System.EventHandler(this.Menu_File_Clear_Click);
            // 
            // Menu_File_Exit
            // 
            this.Menu_File_Exit.Name = "Menu_File_Exit";
            this.Menu_File_Exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.Menu_File_Exit.Size = new System.Drawing.Size(146, 22);
            this.Menu_File_Exit.Text = "Exit";
            this.Menu_File_Exit.Click += new System.EventHandler(this.Menu_File_Exit_Click);
            // 
            // Menu_Analysis
            // 
            this.Menu_Analysis.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Analysis_TaintRange,
            this.Menu_Analysis_TaintResult,
            this.Menu_Analysis_FastMode});
            this.Menu_Analysis.Name = "Menu_Analysis";
            this.Menu_Analysis.Size = new System.Drawing.Size(62, 20);
            this.Menu_Analysis.Text = "Analysis";
            // 
            // Menu_Analysis_TaintRange
            // 
            this.Menu_Analysis_TaintRange.Name = "Menu_Analysis_TaintRange";
            this.Menu_Analysis_TaintRange.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.Menu_Analysis_TaintRange.Size = new System.Drawing.Size(174, 22);
            this.Menu_Analysis_TaintRange.Text = "TaintRange";
            this.Menu_Analysis_TaintRange.Click += new System.EventHandler(this.Menu_Analysis_TaintRange_Click);
            // 
            // Menu_Analysis_TaintResult
            // 
            this.Menu_Analysis_TaintResult.Enabled = false;
            this.Menu_Analysis_TaintResult.Name = "Menu_Analysis_TaintResult";
            this.Menu_Analysis_TaintResult.Size = new System.Drawing.Size(174, 22);
            this.Menu_Analysis_TaintResult.Text = "TaintResult";
            this.Menu_Analysis_TaintResult.Click += new System.EventHandler(this.Menu_Analysis_TaintResult_Click);
            // 
            // Menu_Analysis_FastMode
            // 
            this.Menu_Analysis_FastMode.Checked = true;
            this.Menu_Analysis_FastMode.CheckOnClick = true;
            this.Menu_Analysis_FastMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Menu_Analysis_FastMode.Name = "Menu_Analysis_FastMode";
            this.Menu_Analysis_FastMode.Size = new System.Drawing.Size(174, 22);
            this.Menu_Analysis_FastMode.Text = "FastMode";
            // 
            // Menu_Help
            // 
            this.Menu_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Help_About});
            this.Menu_Help.Name = "Menu_Help";
            this.Menu_Help.Size = new System.Drawing.Size(44, 20);
            this.Menu_Help.Text = "Help";
            // 
            // Menu_Help_About
            // 
            this.Menu_Help_About.Name = "Menu_Help_About";
            this.Menu_Help_About.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.Menu_Help_About.Size = new System.Drawing.Size(150, 22);
            this.Menu_Help_About.Text = "About";
            this.Menu_Help_About.Click += new System.EventHandler(this.Menu_Help_About_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // ListBox
            // 
            this.ListBox.AllowDrop = true;
            this.ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBox.ContextMenuStrip = this.contextMenu;
            this.ListBox.Font = new System.Drawing.Font("Consolas", 11F);
            this.ListBox.FormattingEnabled = true;
            this.ListBox.HorizontalScrollbar = true;
            this.ListBox.ItemHeight = 18;
            this.ListBox.Items.AddRange(new object[] {
            "1. File",
            "2. File -> Open",
            "3. Select File (Dump.log)",
            "4. (Auto) Waiting",
            "5. (Auto) Print Instruction List"});
            this.ListBox.Location = new System.Drawing.Point(0, 190);
            this.ListBox.Margin = new System.Windows.Forms.Padding(0);
            this.ListBox.Name = "ListBox";
            this.ListBox.Size = new System.Drawing.Size(800, 274);
            this.ListBox.TabIndex = 2;
            this.ListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListBox_MouseClick);
            this.ListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListBox_DragDrop);
            this.ListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListBox_DragEnter);
            this.ListBox.DoubleClick += new System.EventHandler(this.ListBox_DoubleClick);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.taintOfToolStripMenuItem,
            this.goToToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(149, 48);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // taintOfToolStripMenuItem
            // 
            this.taintOfToolStripMenuItem.Enabled = false;
            this.taintOfToolStripMenuItem.Name = "taintOfToolStripMenuItem";
            this.taintOfToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.taintOfToolStripMenuItem.Text = "Taint Of";
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.goToToolStripMenuItem.Text = "Go To";
            this.goToToolStripMenuItem.Click += new System.EventHandler(this.goToToolStripMenuItem_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status_Progress,
            this.Status_Label});
            this.StatusBar.Location = new System.Drawing.Point(0, 474);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(800, 22);
            this.StatusBar.TabIndex = 3;
            this.StatusBar.Text = "statusStrip1";
            // 
            // Status_Progress
            // 
            this.Status_Progress.MarqueeAnimationSpeed = 500;
            this.Status_Progress.Name = "Status_Progress";
            this.Status_Progress.Size = new System.Drawing.Size(100, 16);
            // 
            // Status_Label
            // 
            this.Status_Label.Name = "Status_Label";
            this.Status_Label.Size = new System.Drawing.Size(39, 17);
            this.Status_Label.Text = "Ready";
            // 
            // bg_Tainting
            // 
            this.bg_Tainting.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bg_Tainting_DoWork);
            this.bg_Tainting.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bg_Tainting_ProgressChanged);
            this.bg_Tainting.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bg_Tainting_RunWorkerCompleted);
            // 
            // bg_Reading
            // 
            this.bg_Reading.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bg_Reading_DoWork);
            this.bg_Reading.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bg_Reading_ProgressChanged);
            this.bg_Reading.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bg_Reading_RunWorkerCompleted);
            // 
            // bg_Adding
            // 
            this.bg_Adding.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bg_Adding_DoWork);
            this.bg_Adding.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bg_Adding_ProgressChanged);
            this.bg_Adding.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bg_Adding_RunWorkerCompleted);
            // 
            // LoadTime_Label
            // 
            this.LoadTime_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadTime_Label.AutoSize = true;
            this.LoadTime_Label.Location = new System.Drawing.Point(609, 480);
            this.LoadTime_Label.Name = "LoadTime_Label";
            this.LoadTime_Label.Size = new System.Drawing.Size(0, 12);
            this.LoadTime_Label.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(584, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "[State]     [Index]   [Address]     [Opcode]               [Disassembly]";
            // 
            // panel_Search
            // 
            this.panel_Search.Controls.Add(this.ck_Disassem_Ops);
            this.panel_Search.Controls.Add(this.tb_Disassem_OpStr);
            this.panel_Search.Controls.Add(this.tb_Disassem_Mne);
            this.panel_Search.Controls.Add(this.ck_Disassem_OpStr);
            this.panel_Search.Controls.Add(this.ck_Disassem_Real);
            this.panel_Search.Controls.Add(this.label_Op_Val);
            this.panel_Search.Controls.Add(this.tb_Op_Val);
            this.panel_Search.Controls.Add(this.ck_Addr_All);
            this.panel_Search.Controls.Add(this.tb_Addr_End);
            this.panel_Search.Controls.Add(this.tb_Addr_Start);
            this.panel_Search.Controls.Add(this.label_Addr_End);
            this.panel_Search.Controls.Add(this.label_Addr_Start);
            this.panel_Search.Controls.Add(this.ck_Index_All);
            this.panel_Search.Controls.Add(this.tb_Index_End);
            this.panel_Search.Controls.Add(this.gb_State);
            this.panel_Search.Controls.Add(this.ck_Disassem_Mne);
            this.panel_Search.Controls.Add(this.bt_Save);
            this.panel_Search.Controls.Add(this.ck_Disassem_Src);
            this.panel_Search.Controls.Add(this.ck_Disassem_Dst);
            this.panel_Search.Controls.Add(this.tb_Disassem_Real);
            this.panel_Search.Controls.Add(this.tb_Index_Start);
            this.panel_Search.Controls.Add(this.label_Index_End);
            this.panel_Search.Controls.Add(this.label_Index_Start);
            this.panel_Search.Controls.Add(this.bt_Cont);
            this.panel_Search.Controls.Add(this.bt_Search);
            this.panel_Search.Controls.Add(this.bt_Stop);
            this.panel_Search.Location = new System.Drawing.Point(0, 27);
            this.panel_Search.Name = "panel_Search";
            this.panel_Search.Size = new System.Drawing.Size(800, 133);
            this.panel_Search.TabIndex = 6;
            this.panel_Search.Visible = false;
            // 
            // ck_Disassem_Ops
            // 
            this.ck_Disassem_Ops.AutoSize = true;
            this.ck_Disassem_Ops.Enabled = false;
            this.ck_Disassem_Ops.Font = new System.Drawing.Font("Consolas", 10F);
            this.ck_Disassem_Ops.Location = new System.Drawing.Point(459, 103);
            this.ck_Disassem_Ops.Name = "ck_Disassem_Ops";
            this.ck_Disassem_Ops.Size = new System.Drawing.Size(91, 21);
            this.ck_Disassem_Ops.TabIndex = 61;
            this.ck_Disassem_Ops.Text = "Operands";
            this.ck_Disassem_Ops.UseVisualStyleBackColor = true;
            // 
            // tb_Disassem_OpStr
            // 
            this.tb_Disassem_OpStr.Enabled = false;
            this.tb_Disassem_OpStr.Font = new System.Drawing.Font("Consolas", 10F);
            this.tb_Disassem_OpStr.Location = new System.Drawing.Point(561, 38);
            this.tb_Disassem_OpStr.Name = "tb_Disassem_OpStr";
            this.tb_Disassem_OpStr.Size = new System.Drawing.Size(143, 23);
            this.tb_Disassem_OpStr.TabIndex = 60;
            // 
            // tb_Disassem_Mne
            // 
            this.tb_Disassem_Mne.Enabled = false;
            this.tb_Disassem_Mne.Font = new System.Drawing.Font("Consolas", 10F);
            this.tb_Disassem_Mne.Location = new System.Drawing.Point(561, 6);
            this.tb_Disassem_Mne.Name = "tb_Disassem_Mne";
            this.tb_Disassem_Mne.Size = new System.Drawing.Size(80, 23);
            this.tb_Disassem_Mne.TabIndex = 59;
            // 
            // ck_Disassem_OpStr
            // 
            this.ck_Disassem_OpStr.AutoSize = true;
            this.ck_Disassem_OpStr.Font = new System.Drawing.Font("Consolas", 10F);
            this.ck_Disassem_OpStr.Location = new System.Drawing.Point(435, 37);
            this.ck_Disassem_OpStr.Name = "ck_Disassem_OpStr";
            this.ck_Disassem_OpStr.Size = new System.Drawing.Size(115, 21);
            this.ck_Disassem_OpStr.TabIndex = 58;
            this.ck_Disassem_OpStr.Text = "Operand Str";
            this.ck_Disassem_OpStr.UseVisualStyleBackColor = true;
            this.ck_Disassem_OpStr.CheckedChanged += new System.EventHandler(this.ck_Disassem_OpStr_CheckedChanged);
            // 
            // ck_Disassem_Real
            // 
            this.ck_Disassem_Real.AutoSize = true;
            this.ck_Disassem_Real.Font = new System.Drawing.Font("Consolas", 10F);
            this.ck_Disassem_Real.Location = new System.Drawing.Point(435, 75);
            this.ck_Disassem_Real.Name = "ck_Disassem_Real";
            this.ck_Disassem_Real.Size = new System.Drawing.Size(107, 21);
            this.ck_Disassem_Real.TabIndex = 57;
            this.ck_Disassem_Real.Text = "Real Value";
            this.ck_Disassem_Real.UseVisualStyleBackColor = true;
            this.ck_Disassem_Real.CheckedChanged += new System.EventHandler(this.ck_Disasem_Real_CheckedChanged);
            // 
            // label_Op_Val
            // 
            this.label_Op_Val.AutoSize = true;
            this.label_Op_Val.Font = new System.Drawing.Font("Consolas", 10F);
            this.label_Op_Val.Location = new System.Drawing.Point(280, 77);
            this.label_Op_Val.Name = "label_Op_Val";
            this.label_Op_Val.Size = new System.Drawing.Size(48, 17);
            this.label_Op_Val.TabIndex = 56;
            this.label_Op_Val.Text = "Value";
            // 
            // tb_Op_Val
            // 
            this.tb_Op_Val.Enabled = false;
            this.tb_Op_Val.Font = new System.Drawing.Font("Consolas", 11F);
            this.tb_Op_Val.Location = new System.Drawing.Point(283, 97);
            this.tb_Op_Val.Name = "tb_Op_Val";
            this.tb_Op_Val.Size = new System.Drawing.Size(87, 25);
            this.tb_Op_Val.TabIndex = 55;
            // 
            // ck_Addr_All
            // 
            this.ck_Addr_All.AutoSize = true;
            this.ck_Addr_All.Checked = true;
            this.ck_Addr_All.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_Addr_All.Font = new System.Drawing.Font("Consolas", 10F);
            this.ck_Addr_All.Location = new System.Drawing.Point(179, 7);
            this.ck_Addr_All.Name = "ck_Addr_All";
            this.ck_Addr_All.Size = new System.Drawing.Size(51, 21);
            this.ck_Addr_All.TabIndex = 54;
            this.ck_Addr_All.Text = "All";
            this.ck_Addr_All.UseVisualStyleBackColor = true;
            this.ck_Addr_All.CheckedChanged += new System.EventHandler(this.ck_Addr_All_CheckedChanged);
            // 
            // tb_Addr_End
            // 
            this.tb_Addr_End.Enabled = false;
            this.tb_Addr_End.Font = new System.Drawing.Font("Consolas", 10F);
            this.tb_Addr_End.Location = new System.Drawing.Point(179, 100);
            this.tb_Addr_End.Name = "tb_Addr_End";
            this.tb_Addr_End.Size = new System.Drawing.Size(76, 23);
            this.tb_Addr_End.TabIndex = 53;
            // 
            // tb_Addr_Start
            // 
            this.tb_Addr_Start.Enabled = false;
            this.tb_Addr_Start.Font = new System.Drawing.Font("Consolas", 10F);
            this.tb_Addr_Start.Location = new System.Drawing.Point(179, 50);
            this.tb_Addr_Start.Name = "tb_Addr_Start";
            this.tb_Addr_Start.Size = new System.Drawing.Size(76, 23);
            this.tb_Addr_Start.TabIndex = 52;
            // 
            // label_Addr_End
            // 
            this.label_Addr_End.AutoSize = true;
            this.label_Addr_End.Font = new System.Drawing.Font("Consolas", 10F);
            this.label_Addr_End.Location = new System.Drawing.Point(176, 80);
            this.label_Addr_End.Name = "label_Addr_End";
            this.label_Addr_End.Size = new System.Drawing.Size(32, 17);
            this.label_Addr_End.TabIndex = 51;
            this.label_Addr_End.Text = "END";
            // 
            // label_Addr_Start
            // 
            this.label_Addr_Start.AutoSize = true;
            this.label_Addr_Start.Font = new System.Drawing.Font("Consolas", 10F);
            this.label_Addr_Start.Location = new System.Drawing.Point(176, 31);
            this.label_Addr_Start.Name = "label_Addr_Start";
            this.label_Addr_Start.Size = new System.Drawing.Size(48, 17);
            this.label_Addr_Start.TabIndex = 50;
            this.label_Addr_Start.Text = "START";
            // 
            // ck_Index_All
            // 
            this.ck_Index_All.AutoSize = true;
            this.ck_Index_All.Checked = true;
            this.ck_Index_All.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_Index_All.Font = new System.Drawing.Font("Consolas", 10F);
            this.ck_Index_All.Location = new System.Drawing.Point(89, 7);
            this.ck_Index_All.Name = "ck_Index_All";
            this.ck_Index_All.Size = new System.Drawing.Size(51, 21);
            this.ck_Index_All.TabIndex = 49;
            this.ck_Index_All.Text = "All";
            this.ck_Index_All.UseVisualStyleBackColor = true;
            this.ck_Index_All.CheckedChanged += new System.EventHandler(this.ck_Index_All_CheckedChanged);
            // 
            // tb_Index_End
            // 
            this.tb_Index_End.Enabled = false;
            this.tb_Index_End.Font = new System.Drawing.Font("Consolas", 10F);
            this.tb_Index_End.Location = new System.Drawing.Point(89, 100);
            this.tb_Index_End.Name = "tb_Index_End";
            this.tb_Index_End.Size = new System.Drawing.Size(76, 23);
            this.tb_Index_End.TabIndex = 42;
            // 
            // gb_State
            // 
            this.gb_State.Controls.Add(this.rb_Thumb);
            this.gb_State.Controls.Add(this.rb_Arm);
            this.gb_State.Controls.Add(this.rb_All);
            this.gb_State.Font = new System.Drawing.Font("Consolas", 10F);
            this.gb_State.Location = new System.Drawing.Point(1, 7);
            this.gb_State.Name = "gb_State";
            this.gb_State.Size = new System.Drawing.Size(79, 116);
            this.gb_State.TabIndex = 48;
            this.gb_State.TabStop = false;
            this.gb_State.Text = "[State]";
            // 
            // rb_Thumb
            // 
            this.rb_Thumb.AutoSize = true;
            this.rb_Thumb.Font = new System.Drawing.Font("Consolas", 10F);
            this.rb_Thumb.Location = new System.Drawing.Point(6, 87);
            this.rb_Thumb.Name = "rb_Thumb";
            this.rb_Thumb.Size = new System.Drawing.Size(66, 21);
            this.rb_Thumb.TabIndex = 2;
            this.rb_Thumb.Text = "Thumb";
            this.rb_Thumb.UseVisualStyleBackColor = true;
            // 
            // rb_Arm
            // 
            this.rb_Arm.AutoSize = true;
            this.rb_Arm.Font = new System.Drawing.Font("Consolas", 10F);
            this.rb_Arm.Location = new System.Drawing.Point(6, 58);
            this.rb_Arm.Name = "rb_Arm";
            this.rb_Arm.Size = new System.Drawing.Size(50, 21);
            this.rb_Arm.TabIndex = 1;
            this.rb_Arm.Text = "ARM";
            this.rb_Arm.UseVisualStyleBackColor = true;
            // 
            // rb_All
            // 
            this.rb_All.AutoSize = true;
            this.rb_All.Checked = true;
            this.rb_All.Font = new System.Drawing.Font("Consolas", 10F);
            this.rb_All.Location = new System.Drawing.Point(6, 29);
            this.rb_All.Name = "rb_All";
            this.rb_All.Size = new System.Drawing.Size(50, 21);
            this.rb_All.TabIndex = 0;
            this.rb_All.TabStop = true;
            this.rb_All.Text = "All";
            this.rb_All.UseVisualStyleBackColor = true;
            // 
            // ck_Disassem_Mne
            // 
            this.ck_Disassem_Mne.AutoSize = true;
            this.ck_Disassem_Mne.Font = new System.Drawing.Font("Consolas", 10F);
            this.ck_Disassem_Mne.Location = new System.Drawing.Point(435, 7);
            this.ck_Disassem_Mne.Name = "ck_Disassem_Mne";
            this.ck_Disassem_Mne.Size = new System.Drawing.Size(91, 21);
            this.ck_Disassem_Mne.TabIndex = 47;
            this.ck_Disassem_Mne.Text = "Mnemonic";
            this.ck_Disassem_Mne.UseVisualStyleBackColor = true;
            this.ck_Disassem_Mne.CheckedChanged += new System.EventHandler(this.ck_Disassem_Mne_CheckedChanged);
            // 
            // bt_Save
            // 
            this.bt_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Save.Location = new System.Drawing.Point(724, 94);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(75, 23);
            this.bt_Save.TabIndex = 46;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // ck_Disassem_Src
            // 
            this.ck_Disassem_Src.AutoSize = true;
            this.ck_Disassem_Src.Enabled = false;
            this.ck_Disassem_Src.Font = new System.Drawing.Font("Consolas", 10F);
            this.ck_Disassem_Src.Location = new System.Drawing.Point(622, 74);
            this.ck_Disassem_Src.Name = "ck_Disassem_Src";
            this.ck_Disassem_Src.Size = new System.Drawing.Size(51, 21);
            this.ck_Disassem_Src.TabIndex = 45;
            this.ck_Disassem_Src.Text = "Src";
            this.ck_Disassem_Src.UseVisualStyleBackColor = true;
            // 
            // ck_Disassem_Dst
            // 
            this.ck_Disassem_Dst.AutoSize = true;
            this.ck_Disassem_Dst.Enabled = false;
            this.ck_Disassem_Dst.Font = new System.Drawing.Font("Consolas", 10F);
            this.ck_Disassem_Dst.Location = new System.Drawing.Point(561, 75);
            this.ck_Disassem_Dst.Name = "ck_Disassem_Dst";
            this.ck_Disassem_Dst.Size = new System.Drawing.Size(51, 21);
            this.ck_Disassem_Dst.TabIndex = 44;
            this.ck_Disassem_Dst.Text = "Dst";
            this.ck_Disassem_Dst.UseVisualStyleBackColor = true;
            // 
            // tb_Disassem_Real
            // 
            this.tb_Disassem_Real.Enabled = false;
            this.tb_Disassem_Real.Font = new System.Drawing.Font("Consolas", 10F);
            this.tb_Disassem_Real.Location = new System.Drawing.Point(561, 102);
            this.tb_Disassem_Real.Name = "tb_Disassem_Real";
            this.tb_Disassem_Real.Size = new System.Drawing.Size(143, 23);
            this.tb_Disassem_Real.TabIndex = 43;
            // 
            // tb_Index_Start
            // 
            this.tb_Index_Start.Enabled = false;
            this.tb_Index_Start.Font = new System.Drawing.Font("Consolas", 10F);
            this.tb_Index_Start.Location = new System.Drawing.Point(89, 50);
            this.tb_Index_Start.Name = "tb_Index_Start";
            this.tb_Index_Start.Size = new System.Drawing.Size(76, 23);
            this.tb_Index_Start.TabIndex = 41;
            // 
            // label_Index_End
            // 
            this.label_Index_End.AutoSize = true;
            this.label_Index_End.Font = new System.Drawing.Font("Consolas", 10F);
            this.label_Index_End.Location = new System.Drawing.Point(86, 80);
            this.label_Index_End.Name = "label_Index_End";
            this.label_Index_End.Size = new System.Drawing.Size(32, 17);
            this.label_Index_End.TabIndex = 40;
            this.label_Index_End.Text = "END";
            // 
            // label_Index_Start
            // 
            this.label_Index_Start.AutoSize = true;
            this.label_Index_Start.Font = new System.Drawing.Font("Consolas", 10F);
            this.label_Index_Start.Location = new System.Drawing.Point(86, 31);
            this.label_Index_Start.Name = "label_Index_Start";
            this.label_Index_Start.Size = new System.Drawing.Size(48, 17);
            this.label_Index_Start.TabIndex = 39;
            this.label_Index_Start.Text = "START";
            // 
            // bt_Cont
            // 
            this.bt_Cont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Cont.Enabled = false;
            this.bt_Cont.Location = new System.Drawing.Point(724, 65);
            this.bt_Cont.Name = "bt_Cont";
            this.bt_Cont.Size = new System.Drawing.Size(75, 23);
            this.bt_Cont.TabIndex = 38;
            this.bt_Cont.Text = "Continue";
            this.bt_Cont.UseVisualStyleBackColor = true;
            this.bt_Cont.Click += new System.EventHandler(this.bt_Cont_Click);
            // 
            // bt_Search
            // 
            this.bt_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Search.Location = new System.Drawing.Point(724, 7);
            this.bt_Search.Name = "bt_Search";
            this.bt_Search.Size = new System.Drawing.Size(75, 23);
            this.bt_Search.TabIndex = 37;
            this.bt_Search.Text = "Search";
            this.bt_Search.UseVisualStyleBackColor = true;
            this.bt_Search.Click += new System.EventHandler(this.bt_Search_Click);
            // 
            // bt_Stop
            // 
            this.bt_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Stop.Enabled = false;
            this.bt_Stop.Location = new System.Drawing.Point(724, 36);
            this.bt_Stop.Name = "bt_Stop";
            this.bt_Stop.Size = new System.Drawing.Size(75, 23);
            this.bt_Stop.TabIndex = 36;
            this.bt_Stop.Text = "Pause";
            this.bt_Stop.UseVisualStyleBackColor = true;
            this.bt_Stop.Click += new System.EventHandler(this.bt_Stop_Click);
            // 
            // ck_Search
            // 
            this.ck_Search.AutoSize = true;
            this.ck_Search.Enabled = false;
            this.ck_Search.Font = new System.Drawing.Font("Consolas", 10F);
            this.ck_Search.Location = new System.Drawing.Point(691, 3);
            this.ck_Search.Name = "ck_Search";
            this.ck_Search.Size = new System.Drawing.Size(75, 21);
            this.ck_Search.TabIndex = 25;
            this.ck_Search.Text = "Search";
            this.ck_Search.UseVisualStyleBackColor = true;
            this.ck_Search.CheckedChanged += new System.EventHandler(this.ck_Search_CheckedChanged);
            // 
            // bg_Searching
            // 
            this.bg_Searching.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bg_Searching_DoWork);
            this.bg_Searching.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bg_Searching_ProgressChanged);
            this.bg_Searching.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bg_Searching_RunWorkerCompleted);
            // 
            // ARMAnalyzer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 496);
            this.Controls.Add(this.ck_Search);
            this.Controls.Add(this.panel_Search);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoadTime_Label);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.ListBox);
            this.Controls.Add(this.MenuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuBar;
            this.MinimumSize = new System.Drawing.Size(816, 397);
            this.Name = "ARMAnalyzer";
            this.Text = "ARM Analyzer";
            this.Load += new System.EventHandler(this.ARMAnalyzer_Load);
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.panel_Search.ResumeLayout(false);
            this.panel_Search.PerformLayout();
            this.gb_State.ResumeLayout(false);
            this.gb_State.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Open;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Exit;
        private System.Windows.Forms.ToolStripMenuItem Menu_Analysis;
        private System.Windows.Forms.ToolStripMenuItem Menu_Analysis_TaintRange;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help;
        private System.Windows.Forms.ToolStripMenuItem Menu_Help_About;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ListBox ListBox;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripProgressBar Status_Progress;
        private System.Windows.Forms.ToolStripStatusLabel Status_Label;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem taintOfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bg_Tainting;
        private System.ComponentModel.BackgroundWorker bg_Reading;
        private System.ComponentModel.BackgroundWorker bg_Adding;
        private System.Windows.Forms.Label LoadTime_Label;
        private System.Windows.Forms.ToolStripMenuItem Menu_Analysis_TaintResult;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Clear;
        private System.Windows.Forms.ToolStripMenuItem Menu_Analysis_FastMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_Search;
        private System.Windows.Forms.CheckBox ck_Search;
        private System.Windows.Forms.TextBox tb_Disassem_OpStr;
        private System.Windows.Forms.TextBox tb_Disassem_Mne;
        private System.Windows.Forms.CheckBox ck_Disassem_OpStr;
        private System.Windows.Forms.CheckBox ck_Disassem_Real;
        private System.Windows.Forms.Label label_Op_Val;
        private System.Windows.Forms.TextBox tb_Op_Val;
        private System.Windows.Forms.CheckBox ck_Addr_All;
        private System.Windows.Forms.TextBox tb_Addr_End;
        private System.Windows.Forms.TextBox tb_Addr_Start;
        private System.Windows.Forms.Label label_Addr_End;
        private System.Windows.Forms.Label label_Addr_Start;
        private System.Windows.Forms.CheckBox ck_Index_All;
        private System.Windows.Forms.TextBox tb_Index_End;
        private System.Windows.Forms.GroupBox gb_State;
        private System.Windows.Forms.RadioButton rb_Thumb;
        private System.Windows.Forms.RadioButton rb_Arm;
        private System.Windows.Forms.RadioButton rb_All;
        private System.Windows.Forms.CheckBox ck_Disassem_Mne;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.CheckBox ck_Disassem_Src;
        private System.Windows.Forms.CheckBox ck_Disassem_Dst;
        private System.Windows.Forms.TextBox tb_Disassem_Real;
        private System.Windows.Forms.TextBox tb_Index_Start;
        private System.Windows.Forms.Label label_Index_End;
        private System.Windows.Forms.Label label_Index_Start;
        private System.Windows.Forms.Button bt_Cont;
        private System.Windows.Forms.Button bt_Search;
        private System.Windows.Forms.Button bt_Stop;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker bg_Searching;
        private System.Windows.Forms.CheckBox ck_Disassem_Ops;
    }
}