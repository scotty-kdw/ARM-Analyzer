namespace ARMAnalyzer
{
    partial class TaintResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaintResult));
            this.btn_TaintResult_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_TaintResult_SAVE = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.treeView_TaintResult_Result = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // btn_TaintResult_OK
            // 
            this.btn_TaintResult_OK.Location = new System.Drawing.Point(12, 12);
            this.btn_TaintResult_OK.Name = "btn_TaintResult_OK";
            this.btn_TaintResult_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_TaintResult_OK.TabIndex = 0;
            this.btn_TaintResult_OK.Text = "OK";
            this.btn_TaintResult_OK.UseVisualStyleBackColor = true;
            this.btn_TaintResult_OK.Click += new System.EventHandler(this.btn_TaintResult_OK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(108, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Target Data";
            // 
            // btn_TaintResult_SAVE
            // 
            this.btn_TaintResult_SAVE.Location = new System.Drawing.Point(12, 41);
            this.btn_TaintResult_SAVE.Name = "btn_TaintResult_SAVE";
            this.btn_TaintResult_SAVE.Size = new System.Drawing.Size(75, 23);
            this.btn_TaintResult_SAVE.TabIndex = 4;
            this.btn_TaintResult_SAVE.Text = "SAVE";
            this.btn_TaintResult_SAVE.UseVisualStyleBackColor = true;
            this.btn_TaintResult_SAVE.Click += new System.EventHandler(this.btn_TaintResult_SAVE_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(85, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(658, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "[Dst]  :  [Src]     | [State]    [Index]   [Address]     [Opcode]               [" +
    "Disassembly]";
            // 
            // treeView_TaintResult_Result
            // 
            this.treeView_TaintResult_Result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView_TaintResult_Result.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView_TaintResult_Result.Location = new System.Drawing.Point(12, 91);
            this.treeView_TaintResult_Result.Name = "treeView_TaintResult_Result";
            this.treeView_TaintResult_Result.Size = new System.Drawing.Size(964, 296);
            this.treeView_TaintResult_Result.TabIndex = 6;
            // 
            // TaintResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 399);
            this.Controls.Add(this.treeView_TaintResult_Result);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_TaintResult_SAVE);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_TaintResult_OK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TaintResult";
            this.Text = "TaintResult";
            this.Load += new System.EventHandler(this.TaintResult_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TaintResult_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_TaintResult_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_TaintResult_SAVE;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeView_TaintResult_Result;
    }
}