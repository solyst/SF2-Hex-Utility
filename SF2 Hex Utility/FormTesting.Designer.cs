namespace SF2_Hex_Utility
{
    partial class FormTesting
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
            this.picTest = new System.Windows.Forms.PictureBox();
            this.btnDraw = new System.Windows.Forms.Button();
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.numIndex = new System.Windows.Forms.NumericUpDown();
            this.txtHex = new System.Windows.Forms.TextBox();
            this.lblSearchResult = new System.Windows.Forms.Label();
            this.txtSearchTerm = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // picTest
            // 
            this.picTest.Location = new System.Drawing.Point(31, 26);
            this.picTest.Name = "picTest";
            this.picTest.Size = new System.Drawing.Size(471, 157);
            this.picTest.TabIndex = 8;
            this.picTest.TabStop = false;
            this.picTest.Paint += new System.Windows.Forms.PaintEventHandler(this.picTest_Paint);
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(427, 298);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(75, 46);
            this.btnDraw.TabIndex = 7;
            this.btnDraw.Text = "Draw Test";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // numLength
            // 
            this.numLength.Location = new System.Drawing.Point(247, 324);
            this.numLength.Name = "numLength";
            this.numLength.Size = new System.Drawing.Size(161, 20);
            this.numLength.TabIndex = 5;
            // 
            // numIndex
            // 
            this.numIndex.Location = new System.Drawing.Point(247, 298);
            this.numIndex.Name = "numIndex";
            this.numIndex.Size = new System.Drawing.Size(161, 20);
            this.numIndex.TabIndex = 6;
            // 
            // txtHex
            // 
            this.txtHex.Location = new System.Drawing.Point(125, 189);
            this.txtHex.Multiline = true;
            this.txtHex.Name = "txtHex";
            this.txtHex.Size = new System.Drawing.Size(377, 100);
            this.txtHex.TabIndex = 4;
            this.txtHex.Text = "123456789012345678901234567890123456789012345678901234567890123456789012345678901" +
    "234567890";
            // 
            // lblSearchResult
            // 
            this.lblSearchResult.AutoSize = true;
            this.lblSearchResult.Location = new System.Drawing.Point(594, 223);
            this.lblSearchResult.Name = "lblSearchResult";
            this.lblSearchResult.Size = new System.Drawing.Size(35, 13);
            this.lblSearchResult.TabIndex = 9;
            this.lblSearchResult.Text = "label1";
            // 
            // txtSearchTerm
            // 
            this.txtSearchTerm.Location = new System.Drawing.Point(586, 189);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(181, 20);
            this.txtSearchTerm.TabIndex = 10;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(586, 255);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(583, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Search (wildcards use double underscore)";
            // 
            // FormTesting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearchTerm);
            this.Controls.Add(this.lblSearchResult);
            this.Controls.Add(this.picTest);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.numLength);
            this.Controls.Add(this.numIndex);
            this.Controls.Add(this.txtHex);
            this.Name = "FormTesting";
            this.Text = "Testing";
            ((System.ComponentModel.ISupportInitialize)(this.picTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picTest;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.NumericUpDown numIndex;
        private System.Windows.Forms.TextBox txtHex;
        private System.Windows.Forms.Label lblSearchResult;
        private System.Windows.Forms.TextBox txtSearchTerm;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label1;
    }
}