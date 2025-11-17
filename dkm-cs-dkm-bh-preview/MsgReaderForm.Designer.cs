
namespace dkm_cs_dkm_bh_preview
{
    partial class MsgReaderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgReaderForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.edtLog = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.edtJahr = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.edtMonat = new System.Windows.Forms.ToolStripTextBox();
            this.edtBhRootFolder = new System.Windows.Forms.ToolStripComboBox();
            this.btnReadBhFolder = new System.Windows.Forms.ToolStripButton();
            this.pnlInputData = new System.Windows.Forms.Panel();
            this.btnExecute = new System.Windows.Forms.Button();
            this.lblBetrag = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.edtInfo = new System.Windows.Forms.TextBox();
            this.lblBelegDate = new System.Windows.Forms.Label();
            this.errProvYear = new System.Windows.Forms.ErrorProvider(this.components);
            this.errProvMon = new System.Windows.Forms.ErrorProvider(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.errProvBhRootFolder = new System.Windows.Forms.ErrorProvider(this.components);
            this.errProvInfo = new System.Windows.Forms.ErrorProvider(this.components);
            this.errProvBetrag = new System.Windows.Forms.ErrorProvider(this.components);
            this.errProvBelegDate = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlInputData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errProvYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvMon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvBhRootFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvBetrag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvBelegDate)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 75);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 380);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 354);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.edtLog);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 354);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // edtLog
            // 
            this.edtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.edtLog.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edtLog.Location = new System.Drawing.Point(8, 6);
            this.edtLog.Multiline = true;
            this.edtLog.Name = "edtLog";
            this.edtLog.Size = new System.Drawing.Size(776, 335);
            this.edtLog.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrev,
            this.btnNext,
            this.toolStripLabel2,
            this.edtJahr,
            this.toolStripLabel3,
            this.edtMonat,
            this.edtBhRootFolder,
            this.btnReadBhFolder});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPrev
            // 
            this.btnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("btnPrev.Image")));
            this.btnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(27, 22);
            this.btnPrev.Text = "<<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(27, 22);
            this.btnNext.Text = ">>";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(28, 22);
            this.toolStripLabel2.Text = "Jahr";
            // 
            // edtJahr
            // 
            this.edtJahr.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.edtJahr.Name = "edtJahr";
            this.edtJahr.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel3.Text = "Monat";
            // 
            // edtMonat
            // 
            this.edtMonat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.edtMonat.Name = "edtMonat";
            this.edtMonat.Size = new System.Drawing.Size(100, 25);
            // 
            // edtBhRootFolder
            // 
            this.edtBhRootFolder.Name = "edtBhRootFolder";
            this.edtBhRootFolder.Size = new System.Drawing.Size(300, 25);
            // 
            // btnReadBhFolder
            // 
            this.btnReadBhFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReadBhFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnReadBhFolder.Image")));
            this.btnReadBhFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReadBhFolder.Name = "btnReadBhFolder";
            this.btnReadBhFolder.Size = new System.Drawing.Size(56, 22);
            this.btnReadBhFolder.Text = "auslesen";
            this.btnReadBhFolder.Click += new System.EventHandler(this.btnReadBhFolder_Click);
            // 
            // pnlInputData
            // 
            this.pnlInputData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInputData.Controls.Add(this.btnExecute);
            this.pnlInputData.Controls.Add(this.lblBetrag);
            this.pnlInputData.Controls.Add(this.label2);
            this.pnlInputData.Controls.Add(this.edtInfo);
            this.pnlInputData.Controls.Add(this.lblBelegDate);
            this.pnlInputData.Location = new System.Drawing.Point(0, 28);
            this.pnlInputData.Name = "pnlInputData";
            this.pnlInputData.Size = new System.Drawing.Size(800, 51);
            this.pnlInputData.TabIndex = 2;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(661, 18);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 5;
            this.btnExecute.Text = "ausführen";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // lblBetrag
            // 
            this.lblBetrag.AutoSize = true;
            this.lblBetrag.Location = new System.Drawing.Point(403, 2);
            this.lblBetrag.Name = "lblBetrag";
            this.lblBetrag.Size = new System.Drawing.Size(38, 13);
            this.lblBetrag.TabIndex = 4;
            this.lblBetrag.Text = "Betrag";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(229, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Info";
            // 
            // edtInfo
            // 
            this.edtInfo.Location = new System.Drawing.Point(232, 18);
            this.edtInfo.Name = "edtInfo";
            this.edtInfo.Size = new System.Drawing.Size(155, 20);
            this.edtInfo.TabIndex = 2;
            // 
            // lblBelegDate
            // 
            this.lblBelegDate.AutoSize = true;
            this.lblBelegDate.Location = new System.Drawing.Point(12, 2);
            this.lblBelegDate.Name = "lblBelegDate";
            this.lblBelegDate.Size = new System.Drawing.Size(63, 13);
            this.lblBelegDate.TabIndex = 0;
            this.lblBelegDate.Text = "Belegdatum";
            // 
            // errProvYear
            // 
            this.errProvYear.ContainerControl = this;
            // 
            // errProvMon
            // 
            this.errProvMon.ContainerControl = this;
            // 
            // errProvBhRootFolder
            // 
            this.errProvBhRootFolder.ContainerControl = this;
            // 
            // errProvInfo
            // 
            this.errProvInfo.ContainerControl = this;
            // 
            // errProvBetrag
            // 
            this.errProvBetrag.ContainerControl = this;
            // 
            // errProvBelegDate
            // 
            this.errProvBelegDate.ContainerControl = this;
            // 
            // MsgReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlInputData);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "MsgReaderForm";
            this.Text = "MsgReaderForm";
            this.Load += new System.EventHandler(this.MsgReaderForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlInputData.ResumeLayout(false);
            this.pnlInputData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errProvYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvMon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvBhRootFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvBetrag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvBelegDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel pnlInputData;
        private System.Windows.Forms.Label lblBelegDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edtInfo;
        private System.Windows.Forms.Label lblBetrag;
        private System.Windows.Forms.ToolStripButton btnPrev;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.ToolStripTextBox edtJahr;
        private System.Windows.Forms.ToolStripTextBox edtMonat;
        private System.Windows.Forms.ErrorProvider errProvYear;
        private System.Windows.Forms.ErrorProvider errProvMon;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripButton btnReadBhFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox edtLog;
        private System.Windows.Forms.ToolStripComboBox edtBhRootFolder;
        private System.Windows.Forms.ErrorProvider errProvBhRootFolder;
        private System.Windows.Forms.ErrorProvider errProvInfo;
        private System.Windows.Forms.ErrorProvider errProvBetrag;
        private System.Windows.Forms.ErrorProvider errProvBelegDate;
    }
}