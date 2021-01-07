namespace SF2_Hex_Utility
{
    partial class EditHexLocationsList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditHexLocationsList));
            this.treeFolders = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.panButtons = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeFolders
            // 
            this.treeFolders.Dock = System.Windows.Forms.DockStyle.Right;
            this.treeFolders.FullRowSelect = true;
            this.treeFolders.HideSelection = false;
            this.treeFolders.ImageIndex = 5;
            this.treeFolders.ImageList = this.imageListTreeView;
            this.treeFolders.ItemHeight = 18;
            this.treeFolders.Location = new System.Drawing.Point(154, 10);
            this.treeFolders.Name = "treeFolders";
            this.treeFolders.SelectedImageIndex = 5;
            this.treeFolders.Size = new System.Drawing.Size(470, 400);
            this.treeFolders.TabIndex = 8;
            this.treeFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeFolders_AfterSelect);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "oga_book.png");
            this.imageListTreeView.Images.SetKeyName(1, "oga_open_dark.png");
            this.imageListTreeView.Images.SetKeyName(2, "icons8-file-16.png");
            this.imageListTreeView.Images.SetKeyName(3, "icons8-document-16.png");
            this.imageListTreeView.Images.SetKeyName(4, "oga_book_pink.png");
            this.imageListTreeView.Images.SetKeyName(5, "oga_dot_black.png");
            // 
            // panButtons
            // 
            this.panButtons.Controls.Add(this.btnNew);
            this.panButtons.Controls.Add(this.btnEdit);
            this.panButtons.Controls.Add(this.btnUp);
            this.panButtons.Controls.Add(this.btnDelete);
            this.panButtons.Controls.Add(this.btnDown);
            this.panButtons.Location = new System.Drawing.Point(3, 126);
            this.panButtons.Name = "panButtons";
            this.panButtons.Size = new System.Drawing.Size(151, 30);
            this.panButtons.TabIndex = 13;
            // 
            // btnNew
            // 
            this.btnNew.Image = global::SF2_Hex_Utility.Properties.Resources.oga_plus_sign;
            this.btnNew.Location = new System.Drawing.Point(35, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(23, 23);
            this.btnNew.TabIndex = 19;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::SF2_Hex_Utility.Properties.Resources.oga_pencil;
            this.btnEdit.Location = new System.Drawing.Point(64, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(23, 23);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnUp
            // 
            this.btnUp.Image = global::SF2_Hex_Utility.Properties.Resources.oga_arrow_grey_up;
            this.btnUp.Location = new System.Drawing.Point(93, 3);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 23);
            this.btnUp.TabIndex = 12;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::SF2_Hex_Utility.Properties.Resources.oga_red_x;
            this.btnDelete.Location = new System.Drawing.Point(6, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 23);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDown
            // 
            this.btnDown.Image = global::SF2_Hex_Utility.Properties.Resources.oga_arrow_grey_down;
            this.btnDown.Location = new System.Drawing.Point(122, 3);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(23, 23);
            this.btnDown.TabIndex = 11;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(406, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 37);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(505, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 37);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 410);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 52);
            this.panel1.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(624, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 452);
            this.panel2.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(634, 10);
            this.panel3.TabIndex = 21;
            // 
            // EditHexLocationsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 462);
            this.Controls.Add(this.treeFolders);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panButtons);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Name = "EditHexLocationsList";
            this.Text = "Edit Hex Location List";
            this.panButtons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeFolders;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Panel panButtons;
        private System.Windows.Forms.ImageList imageListTreeView;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}