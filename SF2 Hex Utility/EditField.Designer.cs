namespace SF2_Hex_Utility
{
    partial class EditField
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.lblFormat = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(192, 57);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "123456789012345678901234567890";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtValue
            // 
            this.txtValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtValue.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValue.Location = new System.Drawing.Point(196, 19);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(294, 20);
            this.txtValue.TabIndex = 1;
            this.txtValue.Text = "12 34 56 78 12 34 56 78 12 34 56 78 12 34 56 78";
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // lblFormat
            // 
            this.lblFormat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFormat.BackColor = System.Drawing.Color.Blue;
            this.lblFormat.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormat.ForeColor = System.Drawing.Color.White;
            this.lblFormat.Location = new System.Drawing.Point(492, 19);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(34, 20);
            this.lblFormat.TabIndex = 4;
            this.lblFormat.Text = "HEX";
            this.lblFormat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EditField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lblName);
            this.Name = "EditField";
            this.Size = new System.Drawing.Size(528, 57);
            this.MouseEnter += new System.EventHandler(this.EditField_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.EditField_MouseLeave);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EditFieldInert_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label lblFormat;
    }
}
