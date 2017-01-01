namespace ARMAnalyzer
{
    partial class GoTo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoTo));
            this.label_GoTo_Index = new System.Windows.Forms.Label();
            this.box_GoTo_Index = new System.Windows.Forms.TextBox();
            this.btn_GoTo_Index = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_GoTo_Index
            // 
            this.label_GoTo_Index.AutoSize = true;
            this.label_GoTo_Index.Font = new System.Drawing.Font("Consolas", 11F);
            this.label_GoTo_Index.Location = new System.Drawing.Point(12, 9);
            this.label_GoTo_Index.Name = "label_GoTo_Index";
            this.label_GoTo_Index.Size = new System.Drawing.Size(48, 18);
            this.label_GoTo_Index.TabIndex = 0;
            this.label_GoTo_Index.Text = "Index";
            // 
            // box_GoTo_Index
            // 
            this.box_GoTo_Index.Font = new System.Drawing.Font("Consolas", 11F);
            this.box_GoTo_Index.ForeColor = System.Drawing.SystemColors.WindowText;
            this.box_GoTo_Index.Location = new System.Drawing.Point(82, 10);
            this.box_GoTo_Index.Name = "box_GoTo_Index";
            this.box_GoTo_Index.Size = new System.Drawing.Size(100, 25);
            this.box_GoTo_Index.TabIndex = 1;
            this.box_GoTo_Index.Text = "12345";
            this.box_GoTo_Index.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.box_GoTo_Index.KeyDown += new System.Windows.Forms.KeyEventHandler(this.box_GoTo_Index_KeyDown);
            // 
            // btn_GoTo_Index
            // 
            this.btn_GoTo_Index.Location = new System.Drawing.Point(197, 8);
            this.btn_GoTo_Index.Name = "btn_GoTo_Index";
            this.btn_GoTo_Index.Size = new System.Drawing.Size(75, 23);
            this.btn_GoTo_Index.TabIndex = 2;
            this.btn_GoTo_Index.Text = "Go";
            this.btn_GoTo_Index.UseVisualStyleBackColor = true;
            this.btn_GoTo_Index.Click += new System.EventHandler(this.btn_GoTo_Index_Click);
            // 
            // GoTo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btn_GoTo_Index);
            this.Controls.Add(this.box_GoTo_Index);
            this.Controls.Add(this.label_GoTo_Index);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GoTo";
            this.Text = "GoTo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_GoTo_Index;
        private System.Windows.Forms.TextBox box_GoTo_Index;
        private System.Windows.Forms.Button btn_GoTo_Index;
    }
}