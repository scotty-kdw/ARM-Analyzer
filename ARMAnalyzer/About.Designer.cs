namespace ARMAnalyzer
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.About_Label = new System.Windows.Forms.Label();
            this.About_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // About_Label
            // 
            this.About_Label.AutoSize = true;
            this.About_Label.Font = new System.Drawing.Font("D2Coding", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.About_Label.Location = new System.Drawing.Point(6, 7);
            this.About_Label.Name = "About_Label";
            this.About_Label.Size = new System.Drawing.Size(476, 255);
            this.About_Label.TabIndex = 2;
            this.About_Label.Text = resources.GetString("About_Label.Text");
            // 
            // About_OK
            // 
            this.About_OK.Location = new System.Drawing.Point(407, 236);
            this.About_OK.Name = "About_OK";
            this.About_OK.Size = new System.Drawing.Size(75, 23);
            this.About_OK.TabIndex = 3;
            this.About_OK.Text = "OK";
            this.About_OK.UseVisualStyleBackColor = true;
            this.About_OK.Click += new System.EventHandler(this.About_OK_Click);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 271);
            this.Controls.Add(this.About_OK);
            this.Controls.Add(this.About_Label);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(510, 310);
            this.MinimumSize = new System.Drawing.Size(510, 310);
            this.Name = "About";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label About_Label;
        private System.Windows.Forms.Button About_OK;

    }
}