namespace SF2_Hex_Utility
{
    partial class EditHexLocation
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cboFolder = new System.Windows.Forms.ComboBox();
            this.cboGroup = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtBytes = new System.Windows.Forms.TextBox();
            this.txtContext = new System.Windows.Forms.TextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.cboRefType = new System.Windows.Forms.ComboBox();
            this.cboFormat = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.numOriginalValueIndex = new System.Windows.Forms.NumericUpDown();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnFindOriginalValueIndex = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.picHex = new System.Windows.Forms.PictureBox();
            this.btnAutoContext = new System.Windows.Forms.Button();
            this.txtSearchBytes = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOriginalValueIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHex)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Group";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(506, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Original Hex";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(506, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Context";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Location";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 125);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Length";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Reference Type";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(506, 152);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Format";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 178);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Description";
            // 
            // cboFolder
            // 
            this.cboFolder.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFolder.FormattingEnabled = true;
            this.cboFolder.Location = new System.Drawing.Point(106, 17);
            this.cboFolder.MaxLength = 30;
            this.cboFolder.Name = "cboFolder";
            this.cboFolder.Size = new System.Drawing.Size(384, 21);
            this.cboFolder.TabIndex = 1;
            // 
            // cboGroup
            // 
            this.cboGroup.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboGroup.FormattingEnabled = true;
            this.cboGroup.Location = new System.Drawing.Point(106, 44);
            this.cboGroup.MaxLength = 30;
            this.cboGroup.Name = "cboGroup";
            this.cboGroup.Size = new System.Drawing.Size(384, 21);
            this.cboGroup.TabIndex = 2;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(106, 71);
            this.txtName.MaxLength = 60;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(384, 20);
            this.txtName.TabIndex = 3;
            // 
            // txtBytes
            // 
            this.txtBytes.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBytes.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBytes.Location = new System.Drawing.Point(586, 19);
            this.txtBytes.MaxLength = 1000;
            this.txtBytes.Name = "txtBytes";
            this.txtBytes.Size = new System.Drawing.Size(384, 20);
            this.txtBytes.TabIndex = 8;
            this.toolTip1.SetToolTip(this.txtBytes, "The hex value in an unaltered rom.");
            // 
            // txtContext
            // 
            this.txtContext.AcceptsReturn = true;
            this.txtContext.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtContext.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContext.Location = new System.Drawing.Point(586, 45);
            this.txtContext.Multiline = true;
            this.txtContext.Name = "txtContext";
            this.txtContext.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContext.Size = new System.Drawing.Size(384, 72);
            this.txtContext.TabIndex = 9;
            this.toolTip1.SetToolTip(this.txtContext, "Bytes around the edit location in the rom.\r\nUsers can use the context to ensure t" +
        "hey are editing the correct information and that the rom they are editing has no" +
        "t moved the bytes to be edited.");
            // 
            // txtLocation
            // 
            this.txtLocation.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLocation.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocation.Location = new System.Drawing.Point(106, 97);
            this.txtLocation.MaxLength = 12;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(384, 20);
            this.txtLocation.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtLocation, "Use depends upon Reference Type.\r\nAbsolute = the specific location to edit.\r\nWild" +
        "card Search = the first byte to begin the search.");
            // 
            // numLength
            // 
            this.numLength.Location = new System.Drawing.Point(106, 123);
            this.numLength.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLength.Name = "numLength";
            this.numLength.Size = new System.Drawing.Size(120, 20);
            this.numLength.TabIndex = 5;
            this.toolTip1.SetToolTip(this.numLength, "Number of bytes to edit.");
            this.numLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cboRefType
            // 
            this.cboRefType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRefType.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRefType.FormattingEnabled = true;
            this.cboRefType.Location = new System.Drawing.Point(106, 149);
            this.cboRefType.Name = "cboRefType";
            this.cboRefType.Size = new System.Drawing.Size(120, 21);
            this.cboRefType.TabIndex = 6;
            this.toolTip1.SetToolTip(this.cboRefType, "How the location to edit in the rom is determined.\r\nAbsolute = specify the locati" +
        "on exactly.\r\nWildcard Match = search for the first location in the rom matching " +
        "the provided bytes.");
            this.cboRefType.SelectedIndexChanged += new System.EventHandler(this.cboRefType_SelectedIndexChanged);
            // 
            // cboFormat
            // 
            this.cboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormat.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFormat.FormattingEnabled = true;
            this.cboFormat.Location = new System.Drawing.Point(586, 149);
            this.cboFormat.Name = "cboFormat";
            this.cboFormat.Size = new System.Drawing.Size(384, 21);
            this.cboFormat.TabIndex = 11;
            this.toolTip1.SetToolTip(this.cboFormat, "How the bytes appear to the user for editing.");
            // 
            // txtDescription
            // 
            this.txtDescription.AcceptsReturn = true;
            this.txtDescription.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(106, 176);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(384, 249);
            this.txtDescription.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(506, 178);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Context Visual";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(506, 125);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(98, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Location in Context";
            // 
            // numOriginalValueIndex
            // 
            this.numOriginalValueIndex.Location = new System.Drawing.Point(606, 123);
            this.numOriginalValueIndex.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numOriginalValueIndex.Name = "numOriginalValueIndex";
            this.numOriginalValueIndex.Size = new System.Drawing.Size(53, 20);
            this.numOriginalValueIndex.TabIndex = 10;
            this.toolTip1.SetToolTip(this.numOriginalValueIndex, "Denotes where the Original Hex resides within the Context. Highlighted in yellow " +
        "in the Hex Visual.");
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // btnFindOriginalValueIndex
            // 
            this.btnFindOriginalValueIndex.Location = new System.Drawing.Point(665, 123);
            this.btnFindOriginalValueIndex.Name = "btnFindOriginalValueIndex";
            this.btnFindOriginalValueIndex.Size = new System.Drawing.Size(160, 20);
            this.btnFindOriginalValueIndex.TabIndex = 12;
            this.btnFindOriginalValueIndex.Text = "Auto Find Location in Context";
            this.toolTip1.SetToolTip(this.btnFindOriginalValueIndex, "Click to automatically populate the Location in Context.\r\nIf the wrong location i" +
        "s chosen because there are duplicate matches, click again to cycle through all p" +
        "otential matches.");
            this.btnFindOriginalValueIndex.UseVisualStyleBackColor = true;
            this.btnFindOriginalValueIndex.Click += new System.EventHandler(this.btnFindOriginalValueIndex_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(863, 446);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 37);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(744, 446);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(96, 37);
            this.btnReset.TabIndex = 14;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(633, 446);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 37);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // picHex
            // 
            this.picHex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picHex.Location = new System.Drawing.Point(586, 176);
            this.picHex.Name = "picHex";
            this.picHex.Size = new System.Drawing.Size(384, 249);
            this.picHex.TabIndex = 5;
            this.picHex.TabStop = false;
            this.picHex.Paint += new System.Windows.Forms.PaintEventHandler(this.picHex_Paint);
            // 
            // btnAutoContext
            // 
            this.btnAutoContext.Location = new System.Drawing.Point(831, 123);
            this.btnAutoContext.Name = "btnAutoContext";
            this.btnAutoContext.Size = new System.Drawing.Size(139, 20);
            this.btnAutoContext.TabIndex = 16;
            this.btnAutoContext.Text = "Build Context from Rom";
            this.toolTip1.SetToolTip(this.btnAutoContext, "If an unused rom is selected, this automatically builds appropriate context surro" +
        "unding the location.\r\nBest if using an absolute Reference Type.");
            this.btnAutoContext.UseVisualStyleBackColor = true;
            this.btnAutoContext.Click += new System.EventHandler(this.btnAutoContext_Click);
            // 
            // txtSearchBytes
            // 
            this.txtSearchBytes.Location = new System.Drawing.Point(232, 150);
            this.txtSearchBytes.Name = "txtSearchBytes";
            this.txtSearchBytes.Size = new System.Drawing.Size(258, 20);
            this.txtSearchBytes.TabIndex = 17;
            this.toolTip1.SetToolTip(this.txtSearchBytes, "Enter the search term in hex.\r\nUse a double underline \"__\" to denote a wildcard s" +
        "pace that matches on any value.");
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // EditHexLocation
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(997, 508);
            this.Controls.Add(this.txtSearchBytes);
            this.Controls.Add(this.btnAutoContext);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnFindOriginalValueIndex);
            this.Controls.Add(this.picHex);
            this.Controls.Add(this.numOriginalValueIndex);
            this.Controls.Add(this.numLength);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtContext);
            this.Controls.Add(this.txtBytes);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cboFormat);
            this.Controls.Add(this.cboRefType);
            this.Controls.Add(this.cboGroup);
            this.Controls.Add(this.cboFolder);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EditHexLocation";
            this.Text = "Edit Hex Location";
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOriginalValueIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboFolder;
        private System.Windows.Forms.ComboBox cboGroup;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtBytes;
        private System.Windows.Forms.TextBox txtContext;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.ComboBox cboRefType;
        private System.Windows.Forms.ComboBox cboFormat;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.PictureBox picHex;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numOriginalValueIndex;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnFindOriginalValueIndex;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAutoContext;
        private System.Windows.Forms.TextBox txtSearchBytes;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}