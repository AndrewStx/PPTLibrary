using System;
using System.Linq;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using DevExpress.XtraTreeList;
using ShapesLibrary;

using PPT = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

namespace Gallery
{
    [DefaultEvent("SelectionChanged")]
    public partial class ucSelectFile : UserControl
    {
        public class FileSelectedEventArgs : EventArgs
        {
            public LibraryFile File { get; protected set; }
            
            public FileSelectedEventArgs(LibraryFile file)
            {
                File = file;
            }
        }

        public event EventHandler<FileSelectedEventArgs> FileSelected;

        public event EventHandler<FileSelectedEventArgs> SelectionChanged;

        public LibraryFile SelectedFile { get; protected set; }

        private dsTree.TreeDataTable dtTree;

        public ucSelectFile()
        {
            InitializeComponent();

            this.barManager1.SetPopupContextMenu(uxTree, this.pmTree);

            this.cmdRename.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.cmdRename_ItemClick);
            this.cmdDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.cmdDelete_ItemClick);
            this.cmdNewFolder.ItemClick += cmdNewFolder_ItemClick;
            this.cmdNewFile.ItemClick += cmdNewFile_ItemClick;

            uxTree.DoubleClick += uxTree_DoubleClick;
            uxTree.FocusedNodeChanged += uxTree_FocusedNodeChanged;
        }

        void uxTree_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            dsTree.TreeRow row = (uxTree.GetDataRecordByNode(uxTree.FocusedNode) as DataRowView).Row as dsTree.TreeRow;
            SelectedFile = row.Data as LibraryFile;
            SelectionChanged?.Invoke(this, new FileSelectedEventArgs(row.Data as LibraryFile));
        }

        public void ResetFocus()
        {
            uxTree.Nodes[0].Selected = true;
            uxTree.ExpandAll();
        }

        public void SetDataSource(dsTree.TreeDataTable table)
        {
            dtTree = table;

            uxTree.KeyFieldName = "ID";
            uxTree.ParentFieldName = "ParentID";
            uxTree.ImageIndexFieldName = "ImageID";
            //            uxWhere.RootValue = 1;
            DataView dv = new DataView();
            dv.Table = dtTree;
            dv.RowFilter = "ReadOnly=false AND ParentID <> -1";
            uxTree.DataSource = dv;
            
            uxTree.ExpandAll();
        }


        private void uxWhere_BeforeFocusNode(object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e)
        {
            //e.CanFocus = e.Node.Level > 1;
        }

        void uxTree_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraTreeList.TreeListHitInfo hi = uxTree.CalcHitInfo(uxTree.PointToClient(Control.MousePosition));
            if (hi.Node != null)
            {
                if (hi.Node.Level == 2)
                {
                    dsTree.TreeRow row = (uxTree.GetDataRecordByNode(hi.Node) as DataRowView).Row as dsTree.TreeRow;
                    if (FileSelected != null)
                        FileSelected(this, new FileSelectedEventArgs(row.Data as LibraryFile));
                }
            }
        }

        private void uxTree_ShowingEditor(object sender, CancelEventArgs e)
        {
            dsTree.TreeRow row = (uxTree.GetDataRecordByNode(uxTree.FocusedNode) as DataRowView).Row as dsTree.TreeRow;

            if (row.ParentID == -1)
            {
                e.Cancel = true;
            }
        }

        dsTree.TreeRow FocusedRow => (uxTree.GetDataRecordByNode(uxTree.FocusedNode) as DataRowView).Row as dsTree.TreeRow;
        private void uxTree_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (FocusedRow.Data is IFile file)
            {
                try
                {
                    string newName = Path.ChangeExtension(e.Value as string, "pptx");
                    string path = Path.Combine(file.Group.FullPath, newName);
                    if (string.IsNullOrEmpty(newName) || newName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                    {
                        throw new Exception();
                    }
                    Path.GetInvalidFileNameChars();
                    if (System.IO.File.Exists(path))
                    {
                        MessageBox.Show("File exists");
                        e.Valid = false;
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid File Name");
                    e.Valid = false;
                }
            }

            else if (FocusedRow.Data is IGroup group)
            {
                try
                {
                    string newName = e.Value as string;
                    string path = Path.Combine(group.Parent.FullPath, newName);

                    if (string.IsNullOrEmpty(newName) || newName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                    {
                        throw new Exception();
                    }
                    if (Directory.Exists(path))
                    {
                        MessageBox.Show("Folder exists");
                        e.Valid = false;
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid File Name");
                    e.Valid = false;
                }
            }
        }

        private void uxTree_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {

        }

        private void uxTree_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            dsTree.TreeRow row = (uxTree.GetDataRecordByNode(uxTree.FocusedNode) as DataRowView).Row as dsTree.TreeRow;
            if (row.Data is IFile file)
            {
                string newName = Path.ChangeExtension(e.Value as string, "pptx");
                file.Rename(newName);
            }

            else if (row.Data is IGroup group)
            {
                string newName = e.Value as string;
                group.Rename(newName);
            }


        }

        private void uxTree_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {

        }

        private void cmdRename_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            uxTree.ShowEditor();
        }

        private void cmdDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FocusedRow.Data is IFile file)
            {
                file.Group.Delete(file);
                dtTree.RemoveTreeRow(FocusedRow);
            }

            else if (FocusedRow.Data is IGroup group)
            {
                group.Parent.Delete(group);
                RemoveRow(FocusedRow);
            }
        }


        void RemoveRow(dsTree.TreeRow row)
        {
            var rows = dtTree.Where(r => r.ParentID == row.ID).Select(r=>r).ToArray();
            rows.ForEach(r => RemoveRow(r) );
            dtTree.RemoveTreeRow(row);
        }

        private void cmdNewFolder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FocusedRow.Data is IGroup group)
            {
                var baseName = "New folder";
                var name = baseName;
                int cnt = 2;
                while (group.ContainsFolder(name)) //TODO: AST: Limit number of iterations
                {
                    name = baseName + $" ({cnt++})";
                }

                var gr = group.CreateGroup(name);
                var row = FocusedRow;

                var newRow = dtTree.AddTreeRow(row.ID, 0, gr.Name, gr, false); //TODO: AST: Move it to builder

                var node = uxTree.FindNodeByKeyID(newRow.ID);
                if (node != null)
                {
                    node.Selected = true;
                    uxTree.ShowEditor();
                }
            }
        }

        private void cmdNewFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (FocusedRow.Data is IGroup group)
            {
                var baseName = "New file";
                var name = baseName + ".pptx";
                int cnt = 2;
                while (group.ContainsFile(name)) //TODO: AST: Limit number of iterations
                {
                    name = Path.Combine(baseName + $" ({cnt++})")+ ".pptx";
                }
                var fullPath = Path.Combine(group.FullPath, name);


                PPT.Application app = null;
                PPT.Presentations pres = null;
                PPT.Presentation ppt = null;

                try
                {
                    app = new PPT.Application();
                    pres = app.Presentations;

                    ppt = pres.Add(MsoTriState.msoFalse);
                    ppt.SaveAs(fullPath);
                }
                finally
                {
                    ppt?.Close();
                    ppt.ReleaseCOM();
                    ppt = null;

                    pres.ReleaseCOM();
                    pres = null;

                    app = null;
                    //TODO: AST: app is not released
                }


                var file = group.AddFile(name);
                var row = FocusedRow;

                var newRow = dtTree.AddTreeRow(row.ID, 4, Path.GetFileNameWithoutExtension(file.Name), file, false); //TODO: AST: Move it to builder

                var node = uxTree.FindNodeByKeyID(newRow.ID);
                if (node != null)
                {
                    node.Selected = true;
                    uxTree.ShowEditor();
                }
            }

        }


    }
}
