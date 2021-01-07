namespace SF2_Hex_Utility
{
    partial class EditFieldList
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
            this.components = new System.ComponentModel.Container();
            this.panList = new System.Windows.Forms.Panel();
            this.contextMenuStripEditField = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmContextEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmContextDuplicate = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripEditField.SuspendLayout();
            this.SuspendLayout();
            // 
            // panList
            // 
            this.panList.AutoScroll = true;
            this.panList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panList.Location = new System.Drawing.Point(0, 0);
            this.panList.Name = "panList";
            this.panList.Size = new System.Drawing.Size(522, 531);
            this.panList.TabIndex = 0;
            // 
            // contextMenuStripEditField
            // 
            this.contextMenuStripEditField.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmContextEdit,
            this.tsmContextDuplicate});
            this.contextMenuStripEditField.Name = "contextMenuStripEditField";
            this.contextMenuStripEditField.Size = new System.Drawing.Size(125, 48);
            // 
            // tsmContextEdit
            // 
            this.tsmContextEdit.Name = "tsmContextEdit";
            this.tsmContextEdit.Size = new System.Drawing.Size(124, 22);
            this.tsmContextEdit.Text = "&Edit";
            this.tsmContextEdit.Click += new System.EventHandler(this.tsmContextEdit_Click);
            // 
            // tsmContextDuplicate
            // 
            this.tsmContextDuplicate.Name = "tsmContextDuplicate";
            this.tsmContextDuplicate.Size = new System.Drawing.Size(124, 22);
            this.tsmContextDuplicate.Text = "&Duplicate";
            this.tsmContextDuplicate.Click += new System.EventHandler(this.tsmContextDuplicate_Click);
            // 
            // EditFieldList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panList);
            this.Name = "EditFieldList";
            this.Size = new System.Drawing.Size(522, 531);
            this.contextMenuStripEditField.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripEditField;
        private System.Windows.Forms.ToolStripMenuItem tsmContextEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmContextDuplicate;
    }
}
