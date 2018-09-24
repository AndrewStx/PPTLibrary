using System;
using ShapesLibrary;

namespace Gallery
{
    public partial class SelectFileForm : DevExpress.XtraEditors.XtraForm
    {
        public object File { get => uxSelectFile.SelectedFile; }

        public bool OnlyFolders { get => uxSelectFile.OnlyFolders; set => uxSelectFile.OnlyFolders = value; }

        public SelectFileForm()
        {
            InitializeComponent();
        }

        public void SetDataSource(dsTree.TreeDataTable table)
        {
            uxSelectFile.SetDataSource(table);
        }

        private void uxSelectFile_FileSelected(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void uxSelectFile_SelectionChanged(object sender, EventArgs e)
        {
            uxOK.Enabled = File != null;
        }

        private void uxOK_Click(object sender, EventArgs e)
        {
        }

    }
}