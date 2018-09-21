using System;
using ShapesLibrary;

namespace Gallery
{
    public partial class SelectFileForm : DevExpress.XtraEditors.XtraForm
    {
        public LibraryFile File { get; protected set; }
        public SelectFileForm()
        {
            InitializeComponent();
        }

        public void SetDataSource(dsTree.TreeDataTable table)
        {
            uxSelectFile.SetDataSource(table);
        }

        private void uxSelectFile_FileSelected(object sender, ucSelectFile.FileSelectedEventArgs e)
        {
            File = e.File;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void uxSelectFile_SelectionChanged(object sender, ucSelectFile.FileSelectedEventArgs e)
        {
            File = uxSelectFile.SelectedFile;
            uxOK.Enabled = e.File != null;
        }

        private void uxOK_Click(object sender, EventArgs e)
        {
            File = uxSelectFile.SelectedFile;
        }

    }
}