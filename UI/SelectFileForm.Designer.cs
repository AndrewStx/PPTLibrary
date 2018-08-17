namespace Gallery
{
    partial class SelectFileForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.uxCancel = new DevExpress.XtraEditors.SimpleButton();
            this.uxOK = new DevExpress.XtraEditors.SimpleButton();
            this.uxSelectFile = new ucSelectFile();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.uxCancel);
            this.panel1.Controls.Add(this.uxOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 304);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(313, 27);
            this.panel1.TabIndex = 1;
            // 
            // uxCancel
            // 
            this.uxCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uxCancel.Location = new System.Drawing.Point(216, 2);
            this.uxCancel.Name = "uxCancel";
            this.uxCancel.Size = new System.Drawing.Size(94, 23);
            this.uxCancel.TabIndex = 1;
            this.uxCancel.Text = "Cancel";
            // 
            // uxOK
            // 
            this.uxOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.uxOK.Location = new System.Drawing.Point(116, 2);
            this.uxOK.Name = "uxOK";
            this.uxOK.Size = new System.Drawing.Size(94, 23);
            this.uxOK.TabIndex = 0;
            this.uxOK.Text = "OK";
            this.uxOK.Click += new System.EventHandler(this.uxOK_Click);
            // 
            // uxSelectFile
            // 
            this.uxSelectFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxSelectFile.Location = new System.Drawing.Point(0, 0);
            this.uxSelectFile.Name = "uxSelectFile";
            this.uxSelectFile.Size = new System.Drawing.Size(313, 304);
            this.uxSelectFile.TabIndex = 0;
            this.uxSelectFile.FileSelected += new System.EventHandler<ucSelectFile.FileSelectedEventArgs>(this.uxSelectFile_FileSelected);
            this.uxSelectFile.SelectionChanged += new System.EventHandler<ucSelectFile.FileSelectedEventArgs>(this.uxSelectFile_SelectionChanged);
            // 
            // SelectFileForm
            // 
            this.AcceptButton = this.uxOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.uxCancel;
            this.ClientSize = new System.Drawing.Size(313, 331);
            this.Controls.Add(this.uxSelectFile);
            this.Controls.Add(this.panel1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectFileForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Selelect Library File";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton uxOK;
        private DevExpress.XtraEditors.SimpleButton uxCancel;
        private ucSelectFile uxSelectFile;
    }
}