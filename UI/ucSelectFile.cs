using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using DevExpress.XtraTreeList;
using ShapesLibrary;

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

        public LibraryFile File { get; protected set; }

        DataView dv = new DataView();
        
        public ucSelectFile()
        {
            InitializeComponent();
            
            uxTree.DoubleClick += uxTree_DoubleClick;
            uxTree.FocusedNodeChanged += uxTree_FocusedNodeChanged;
        }

        void uxTree_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            dsTree.TreeRow row = (uxTree.GetDataRecordByNode(uxTree.FocusedNode) as DataRowView).Row as dsTree.TreeRow;
            File = row.Data as LibraryFile;
            SelectionChanged?.Invoke(this, new FileSelectedEventArgs(row.Data as LibraryFile));
        }

        public void ResetFocus()
        {
            uxTree.Nodes[0].Selected = true;
            uxTree.ExpandAll();
        }

        public void SetDataSource(dsTree.TreeDataTable table)
        {
            uxTree.KeyFieldName = "ID";
            uxTree.ParentFieldName = "ParentID";
            uxTree.ImageIndexFieldName = "ImageID";
//            uxWhere.RootValue = 1;
            dv.Table = table;
            dv.RowFilter = "ReadOnly=false";
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

    }
}
