namespace ARMAnalyzer
{
    partial class TaintRange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaintRange));
            this.label_Start = new System.Windows.Forms.Label();
            this.label_End = new System.Windows.Forms.Label();
            this.box_Start = new System.Windows.Forms.TextBox();
            this.box_End = new System.Windows.Forms.TextBox();
            this.btn_Add = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Offset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Start = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_End = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_Remove = new System.Windows.Forms.Button();
            this.bt_OK = new System.Windows.Forms.Button();
            this.box_Offset = new System.Windows.Forms.TextBox();
            this.label_Offset = new System.Windows.Forms.Label();
            this.bt_SAVE = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.box_Index = new System.Windows.Forms.TextBox();
            this.label_Index = new System.Windows.Forms.Label();
            this.col_Index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label_Start
            // 
            this.label_Start.AutoSize = true;
            this.label_Start.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Start.Location = new System.Drawing.Point(206, 9);
            this.label_Start.Name = "label_Start";
            this.label_Start.Size = new System.Drawing.Size(54, 19);
            this.label_Start.TabIndex = 0;
            this.label_Start.Text = "START";
            // 
            // label_End
            // 
            this.label_End.AutoSize = true;
            this.label_End.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_End.Location = new System.Drawing.Point(299, 9);
            this.label_End.Name = "label_End";
            this.label_End.Size = new System.Drawing.Size(36, 19);
            this.label_End.TabIndex = 1;
            this.label_End.Text = "END";
            // 
            // box_Start
            // 
            this.box_Start.Font = new System.Drawing.Font("Consolas", 11F);
            this.box_Start.Location = new System.Drawing.Point(210, 31);
            this.box_Start.Name = "box_Start";
            this.box_Start.Size = new System.Drawing.Size(87, 25);
            this.box_Start.TabIndex = 2;
            this.box_Start.Text = "0xBBBBBBBB";
            // 
            // box_End
            // 
            this.box_End.Font = new System.Drawing.Font("Consolas", 11F);
            this.box_End.Location = new System.Drawing.Point(303, 31);
            this.box_End.Name = "box_End";
            this.box_End.Size = new System.Drawing.Size(87, 25);
            this.box_End.TabIndex = 3;
            this.box_End.Text = "0xCCCCCCCC";
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(401, 7);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Add.TabIndex = 4;
            this.btn_Add.Text = "Add";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.col_Index,
            this.col_Offset,
            this.col_Start,
            this.col_End,
            this.col_Size});
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(12, 62);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(544, 300);
            this.listView.TabIndex = 5;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 0;
            // 
            // col_Offset
            // 
            this.col_Offset.Text = "InputData Offset";
            this.col_Offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_Offset.Width = 100;
            // 
            // col_Start
            // 
            this.col_Start.Text = "Start";
            this.col_Start.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_Start.Width = 100;
            // 
            // col_End
            // 
            this.col_End.Text = "End";
            this.col_End.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_End.Width = 100;
            // 
            // col_Size
            // 
            this.col_Size.Text = "Size";
            this.col_Size.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_Size.Width = 130;
            // 
            // btn_Remove
            // 
            this.btn_Remove.Location = new System.Drawing.Point(401, 31);
            this.btn_Remove.Name = "btn_Remove";
            this.btn_Remove.Size = new System.Drawing.Size(75, 23);
            this.btn_Remove.TabIndex = 5;
            this.btn_Remove.Text = "Remove";
            this.btn_Remove.UseVisualStyleBackColor = true;
            this.btn_Remove.Click += new System.EventHandler(this.btn_Remove_Click);
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(482, 31);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 23);
            this.bt_OK.TabIndex = 6;
            this.bt_OK.Text = "OK";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // box_Offset
            // 
            this.box_Offset.Font = new System.Drawing.Font("Consolas", 11F);
            this.box_Offset.Location = new System.Drawing.Point(114, 31);
            this.box_Offset.Name = "box_Offset";
            this.box_Offset.Size = new System.Drawing.Size(87, 25);
            this.box_Offset.TabIndex = 1;
            this.box_Offset.Text = "0xAAAAAAAA";
            // 
            // label_Offset
            // 
            this.label_Offset.AutoSize = true;
            this.label_Offset.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Offset.Location = new System.Drawing.Point(110, 9);
            this.label_Offset.Name = "label_Offset";
            this.label_Offset.Size = new System.Drawing.Size(63, 19);
            this.label_Offset.TabIndex = 8;
            this.label_Offset.Text = "OFFSET";
            // 
            // bt_SAVE
            // 
            this.bt_SAVE.Location = new System.Drawing.Point(482, 7);
            this.bt_SAVE.Name = "bt_SAVE";
            this.bt_SAVE.Size = new System.Drawing.Size(75, 23);
            this.bt_SAVE.TabIndex = 9;
            this.bt_SAVE.Text = "SAVE";
            this.bt_SAVE.UseVisualStyleBackColor = true;
            this.bt_SAVE.Click += new System.EventHandler(this.bt_SAVE_Click);
            // 
            // box_Index
            // 
            this.box_Index.Font = new System.Drawing.Font("Consolas", 11F);
            this.box_Index.Location = new System.Drawing.Point(16, 31);
            this.box_Index.Name = "box_Index";
            this.box_Index.Size = new System.Drawing.Size(87, 25);
            this.box_Index.TabIndex = 10;
            this.box_Index.Text = "123456789";
            // 
            // label_Index
            // 
            this.label_Index.AutoSize = true;
            this.label_Index.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Index.Location = new System.Drawing.Point(12, 9);
            this.label_Index.Name = "label_Index";
            this.label_Index.Size = new System.Drawing.Size(54, 19);
            this.label_Index.TabIndex = 11;
            this.label_Index.Text = "INDEX";
            // 
            // col_Index
            // 
            this.col_Index.Text = "Index";
            this.col_Index.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_Index.Width = 100;
            // 
            // TaintRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 373);
            this.Controls.Add(this.box_Index);
            this.Controls.Add(this.label_Index);
            this.Controls.Add(this.bt_SAVE);
            this.Controls.Add(this.box_Offset);
            this.Controls.Add(this.label_Offset);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.btn_Remove);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.box_End);
            this.Controls.Add(this.box_Start);
            this.Controls.Add(this.label_End);
            this.Controls.Add(this.label_Start);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "TaintRange";
            this.Text = "TaintRange";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Start;
        private System.Windows.Forms.Label label_End;
        private System.Windows.Forms.TextBox box_Start;
        private System.Windows.Forms.TextBox box_End;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Button btn_Remove;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.ColumnHeader col_Start;
        private System.Windows.Forms.ColumnHeader col_End;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader col_Size;
        private System.Windows.Forms.ColumnHeader col_Offset;
        private System.Windows.Forms.TextBox box_Offset;
        private System.Windows.Forms.Label label_Offset;
        private System.Windows.Forms.Button bt_SAVE;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ColumnHeader col_Index;
        private System.Windows.Forms.TextBox box_Index;
        private System.Windows.Forms.Label label_Index;
    }
}