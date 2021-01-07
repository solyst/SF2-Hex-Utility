namespace SF2_Hex_Utility
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmRomOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmRomSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmRomSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmRomReload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLocationsNew = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLocationsNewLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLocationsImportList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmLocationsSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLocationsSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmLocationsEditList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLocationsEditLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmToolsUnchangedRoms = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmHelpTestScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.lblRom = new System.Windows.Forms.ToolStripLabel();
            this.treeFolders = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialogRom = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogRom = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogHexList = new System.Windows.Forms.OpenFileDialog();
            this.tsmShowToolTips = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmAlwaysGenerateContext = new System.Windows.Forms.ToolStripMenuItem();
            this.editFieldDetails1 = new SF2_Hex_Utility.EditFieldDetails();
            this.editFieldList1 = new SF2_Hex_Utility.EditFieldList();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem12,
            this.toolStripMenuItem15,
            this.toolStripSeparator5,
            this.lblRom});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(6, 2, 0, 2);
            this.toolStrip1.Size = new System.Drawing.Size(1334, 27);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFileExit});
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(37, 23);
            this.toolStripMenuItem8.Text = "&File";
            // 
            // tsmFileExit
            // 
            this.tsmFileExit.Name = "tsmFileExit";
            this.tsmFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.tsmFileExit.Size = new System.Drawing.Size(135, 22);
            this.tsmFileExit.Text = "E&xit";
            this.tsmFileExit.Click += new System.EventHandler(this.tsmFileExit_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRomOpen,
            this.toolStripSeparator9,
            this.tsmRomSave,
            this.tsmRomSaveAs,
            this.toolStripSeparator10,
            this.tsmRomReload});
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(44, 23);
            this.toolStripMenuItem10.Text = "&Rom";
            // 
            // tsmRomOpen
            // 
            this.tsmRomOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsmRomOpen.Image")));
            this.tsmRomOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmRomOpen.Name = "tsmRomOpen";
            this.tsmRomOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmRomOpen.Size = new System.Drawing.Size(146, 22);
            this.tsmRomOpen.Text = "&Open";
            this.tsmRomOpen.Click += new System.EventHandler(this.tsmRomOpen_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(143, 6);
            // 
            // tsmRomSave
            // 
            this.tsmRomSave.Enabled = false;
            this.tsmRomSave.Image = ((System.Drawing.Image)(resources.GetObject("tsmRomSave.Image")));
            this.tsmRomSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmRomSave.Name = "tsmRomSave";
            this.tsmRomSave.ShortcutKeyDisplayString = "Ctrl+S ";
            this.tsmRomSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmRomSave.Size = new System.Drawing.Size(146, 22);
            this.tsmRomSave.Text = "&Save";
            this.tsmRomSave.Click += new System.EventHandler(this.tsmRomSave_Click);
            // 
            // tsmRomSaveAs
            // 
            this.tsmRomSaveAs.Enabled = false;
            this.tsmRomSaveAs.Name = "tsmRomSaveAs";
            this.tsmRomSaveAs.Size = new System.Drawing.Size(146, 22);
            this.tsmRomSaveAs.Text = "Save &As";
            this.tsmRomSaveAs.Click += new System.EventHandler(this.tsmRomSaveAs_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(143, 6);
            // 
            // tsmRomReload
            // 
            this.tsmRomReload.Enabled = false;
            this.tsmRomReload.Name = "tsmRomReload";
            this.tsmRomReload.Size = new System.Drawing.Size(146, 22);
            this.tsmRomReload.Text = "&Reload";
            this.tsmRomReload.Click += new System.EventHandler(this.tsmRomReload_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmLocationsNew,
            this.tsmLocationsNewLocation,
            this.tsmLocationsImportList,
            this.toolStripSeparator13,
            this.tsmLocationsSave,
            this.tsmLocationsSaveAll,
            this.toolStripSeparator14,
            this.tsmLocationsEditList,
            this.tsmLocationsEditLocation});
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(70, 23);
            this.toolStripMenuItem11.Text = "&Locations";
            // 
            // tsmLocationsNew
            // 
            this.tsmLocationsNew.Image = ((System.Drawing.Image)(resources.GetObject("tsmLocationsNew.Image")));
            this.tsmLocationsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmLocationsNew.Name = "tsmLocationsNew";
            this.tsmLocationsNew.Size = new System.Drawing.Size(190, 22);
            this.tsmLocationsNew.Text = "&New List";
            this.tsmLocationsNew.Click += new System.EventHandler(this.tsmLocationsNew_Click);
            // 
            // tsmLocationsNewLocation
            // 
            this.tsmLocationsNewLocation.Enabled = false;
            this.tsmLocationsNewLocation.Name = "tsmLocationsNewLocation";
            this.tsmLocationsNewLocation.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.tsmLocationsNewLocation.Size = new System.Drawing.Size(190, 22);
            this.tsmLocationsNewLocation.Text = "New Lo&cation";
            this.tsmLocationsNewLocation.Click += new System.EventHandler(this.tsmLocationsNewLocation_Click);
            // 
            // tsmLocationsImportList
            // 
            this.tsmLocationsImportList.Enabled = false;
            this.tsmLocationsImportList.Name = "tsmLocationsImportList";
            this.tsmLocationsImportList.Size = new System.Drawing.Size(190, 22);
            this.tsmLocationsImportList.Text = "&Import List";
            this.tsmLocationsImportList.Click += new System.EventHandler(this.tsmLocationsImportList_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(187, 6);
            // 
            // tsmLocationsSave
            // 
            this.tsmLocationsSave.Enabled = false;
            this.tsmLocationsSave.Image = ((System.Drawing.Image)(resources.GetObject("tsmLocationsSave.Image")));
            this.tsmLocationsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmLocationsSave.Name = "tsmLocationsSave";
            this.tsmLocationsSave.ShortcutKeyDisplayString = "";
            this.tsmLocationsSave.Size = new System.Drawing.Size(190, 22);
            this.tsmLocationsSave.Text = "&Save";
            this.tsmLocationsSave.Click += new System.EventHandler(this.tsmLocationsSave_Click);
            // 
            // tsmLocationsSaveAll
            // 
            this.tsmLocationsSaveAll.Name = "tsmLocationsSaveAll";
            this.tsmLocationsSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.tsmLocationsSaveAll.Size = new System.Drawing.Size(190, 22);
            this.tsmLocationsSaveAll.Text = "Save &All";
            this.tsmLocationsSaveAll.Click += new System.EventHandler(this.tsmLocationsSaveAll_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(187, 6);
            // 
            // tsmLocationsEditList
            // 
            this.tsmLocationsEditList.Enabled = false;
            this.tsmLocationsEditList.Image = global::SF2_Hex_Utility.Properties.Resources.oga_scroll_edit;
            this.tsmLocationsEditList.Name = "tsmLocationsEditList";
            this.tsmLocationsEditList.Size = new System.Drawing.Size(190, 22);
            this.tsmLocationsEditList.Text = "Edit &List";
            this.tsmLocationsEditList.Click += new System.EventHandler(this.tsmLocationsEditList_Click);
            // 
            // tsmLocationsEditLocation
            // 
            this.tsmLocationsEditLocation.Enabled = false;
            this.tsmLocationsEditLocation.Image = global::SF2_Hex_Utility.Properties.Resources.oga_pencil;
            this.tsmLocationsEditLocation.Name = "tsmLocationsEditLocation";
            this.tsmLocationsEditLocation.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.tsmLocationsEditLocation.Size = new System.Drawing.Size(190, 22);
            this.tsmLocationsEditLocation.Text = "&Edit Location";
            this.tsmLocationsEditLocation.Click += new System.EventHandler(this.tsmLocationsEditLocation_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAlwaysGenerateContext,
            this.tsmToolsUnchangedRoms});
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(46, 23);
            this.toolStripMenuItem12.Text = "&Tools";
            // 
            // tsmToolsUnchangedRoms
            // 
            this.tsmToolsUnchangedRoms.Name = "tsmToolsUnchangedRoms";
            this.tsmToolsUnchangedRoms.Size = new System.Drawing.Size(263, 22);
            this.tsmToolsUnchangedRoms.Text = "Set &Unchanged Rom";
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmShowToolTips,
            this.tsmHelpTestScreen,
            this.toolStripSeparator1,
            this.tsmHelpAbout});
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(44, 23);
            this.toolStripMenuItem15.Text = "&Help";
            // 
            // tsmHelpAbout
            // 
            this.tsmHelpAbout.Name = "tsmHelpAbout";
            this.tsmHelpAbout.Size = new System.Drawing.Size(152, 22);
            this.tsmHelpAbout.Text = "&About";
            this.tsmHelpAbout.Click += new System.EventHandler(this.tsmHelpAbout_Click);
            // 
            // tsmHelpTestScreen
            // 
            this.tsmHelpTestScreen.Name = "tsmHelpTestScreen";
            this.tsmHelpTestScreen.Size = new System.Drawing.Size(152, 22);
            this.tsmHelpTestScreen.Text = "Test Screen";
            this.tsmHelpTestScreen.Click += new System.EventHandler(this.tsmHelpTestScreen_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
            // 
            // lblRom
            // 
            this.lblRom.Name = "lblRom";
            this.lblRom.Size = new System.Drawing.Size(93, 20);
            this.lblRom.Text = "No Rom Loaded";
            // 
            // treeFolders
            // 
            this.treeFolders.FullRowSelect = true;
            this.treeFolders.HideSelection = false;
            this.treeFolders.ImageIndex = 3;
            this.treeFolders.ImageList = this.imageListTreeView;
            this.treeFolders.ItemHeight = 18;
            this.treeFolders.Location = new System.Drawing.Point(12, 36);
            this.treeFolders.Name = "treeFolders";
            this.treeFolders.SelectedImageIndex = 3;
            this.treeFolders.Size = new System.Drawing.Size(229, 556);
            this.treeFolders.TabIndex = 7;
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
            // 
            // openFileDialogRom
            // 
            this.openFileDialogRom.DefaultExt = "bin";
            this.openFileDialogRom.Filter = "Rom Files|*.bin|All Files|*.*";
            this.openFileDialogRom.ReadOnlyChecked = true;
            // 
            // saveFileDialogRom
            // 
            this.saveFileDialogRom.DefaultExt = "bin";
            this.saveFileDialogRom.Filter = "Rom Files|*.bin|All Files|*.*";
            // 
            // openFileDialogHexList
            // 
            this.openFileDialogHexList.DefaultExt = "hxl";
            this.openFileDialogHexList.Filter = "Hex Location List Files|*.hxl|All Files|*.*";
            // 
            // tsmShowToolTips
            // 
            this.tsmShowToolTips.Name = "tsmShowToolTips";
            this.tsmShowToolTips.Size = new System.Drawing.Size(152, 22);
            this.tsmShowToolTips.Text = "Show &Tool Tips";
            this.tsmShowToolTips.Click += new System.EventHandler(this.tsmShowToolTips_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmAlwaysGenerateContext
            // 
            this.tsmAlwaysGenerateContext.Name = "tsmAlwaysGenerateContext";
            this.tsmAlwaysGenerateContext.Size = new System.Drawing.Size(263, 22);
            this.tsmAlwaysGenerateContext.Text = "Always Generate Context from Rom";
            this.tsmAlwaysGenerateContext.Click += new System.EventHandler(this.tsmAlwaysGenerateContext_Click);
            // 
            // editFieldDetails1
            // 
            this.editFieldDetails1.EditField = null;
            this.editFieldDetails1.Location = new System.Drawing.Point(824, 36);
            this.editFieldDetails1.Name = "editFieldDetails1";
            this.editFieldDetails1.Size = new System.Drawing.Size(493, 556);
            this.editFieldDetails1.TabIndex = 9;
            this.editFieldDetails1.UnchangedRom = null;
            this.editFieldDetails1.Visible = false;
            // 
            // editFieldList1
            // 
            this.editFieldList1.AutoScroll = true;
            this.editFieldList1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editFieldList1.Dirty = false;
            this.editFieldList1.Folder = null;
            this.editFieldList1.Group = null;
            this.editFieldList1.Items = null;
            this.editFieldList1.Location = new System.Drawing.Point(256, 36);
            this.editFieldList1.Multiselect = false;
            this.editFieldList1.Name = "editFieldList1";
            this.editFieldList1.Rom = null;
            this.editFieldList1.Size = new System.Drawing.Size(550, 556);
            this.editFieldList1.TabIndex = 8;
            this.editFieldList1.UnchangedRom = null;
            this.editFieldList1.Valid = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 608);
            this.Controls.Add(this.editFieldDetails1);
            this.Controls.Add(this.editFieldList1);
            this.Controls.Add(this.treeFolders);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormMain";
            this.Text = "SF2 Hex Utility";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem tsmFileExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem tsmRomOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem tsmRomSave;
        private System.Windows.Forms.ToolStripMenuItem tsmRomSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem tsmRomReload;
        private System.Windows.Forms.ToolStripLabel lblRom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.TreeView treeFolders;
        private EditFieldList editFieldList1;
        private EditFieldDetails editFieldDetails1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem tsmLocationsNewLocation;
        private System.Windows.Forms.ToolStripMenuItem tsmLocationsImportList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem tsmLocationsSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem tsmLocationsEditList;
        private System.Windows.Forms.ToolStripMenuItem tsmLocationsEditLocation;
        private System.Windows.Forms.ToolStripMenuItem tsmLocationsNew;
        private System.Windows.Forms.ToolStripMenuItem tsmLocationsSave;
        private System.Windows.Forms.OpenFileDialog openFileDialogRom;
        private System.Windows.Forms.SaveFileDialog saveFileDialogRom;
        private System.Windows.Forms.ImageList imageListTreeView;
        private System.Windows.Forms.OpenFileDialog openFileDialogHexList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem tsmHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmHelpTestScreen;
        private System.Windows.Forms.ToolStripMenuItem tsmToolsUnchangedRoms;
        private System.Windows.Forms.ToolStripMenuItem tsmShowToolTips;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmAlwaysGenerateContext;
    }
}

