using ShapesLibrary;
namespace Gallery
{
    partial class ucSelectFile
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSelectFile));
            this.uxTree = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.treeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsTree = new ShapesLibrary.dsTree();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.pmTree = new DevExpress.XtraBars.PopupMenu(this.components);
            this.cmdRename = new DevExpress.XtraBars.BarButtonItem();
            this.cmdDelete = new DevExpress.XtraBars.BarButtonItem();
            this.cmdNewFolder = new DevExpress.XtraBars.BarButtonItem();
            this.cmdNewFile = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.uxTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // uxTree
            // 
            this.uxTree.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
            this.uxTree.Appearance.FocusedCell.Options.UseBackColor = true;
            this.uxTree.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.uxTree.Appearance.SelectedRow.Options.UseBackColor = true;
            this.uxTree.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.uxTree.Cursor = System.Windows.Forms.Cursors.Default;
            this.uxTree.DataSource = this.treeBindingSource;
            this.uxTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTree.Location = new System.Drawing.Point(0, 0);
            this.uxTree.Name = "uxTree";
            this.uxTree.OptionsBehavior.EditorShowMode = DevExpress.XtraTreeList.TreeListEditorShowMode.MouseDownFocused;
            this.uxTree.OptionsBehavior.ResizeNodes = false;
            this.uxTree.OptionsSelection.SelectNodesOnRightClick = true;
            this.uxTree.OptionsView.ShowColumns = false;
            this.uxTree.OptionsView.ShowFilterPanelMode = DevExpress.XtraTreeList.ShowFilterPanelMode.Never;
            this.uxTree.OptionsView.ShowHorzLines = false;
            this.uxTree.OptionsView.ShowIndicator = false;
            this.uxTree.OptionsView.ShowRoot = false;
            this.uxTree.OptionsView.ShowVertLines = false;
            this.uxTree.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.uxTree.SelectImageList = this.imageCollection1;
            this.uxTree.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.Default;
            this.uxTree.Size = new System.Drawing.Size(304, 249);
            this.uxTree.TabIndex = 1;
            this.uxTree.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.uxTree_ValidatingEditor);
            this.uxTree.PopupMenuShowing += new DevExpress.XtraTreeList.PopupMenuShowingEventHandler(this.uxTree_PopupMenuShowing);
            this.uxTree.CellValueChanging += new DevExpress.XtraTreeList.CellValueChangedEventHandler(this.uxTree_CellValueChanging);
            this.uxTree.CellValueChanged += new DevExpress.XtraTreeList.CellValueChangedEventHandler(this.uxTree_CellValueChanged);
            this.uxTree.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.uxTree_ShowingEditor);
            this.uxTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxTree_KeyDown);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "Name";
            this.treeListColumn1.ColumnEdit = this.repositoryItemTextEdit1;
            this.treeListColumn1.FieldName = "Name";
            this.treeListColumn1.MinWidth = 88;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 199;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // treeBindingSource
            // 
            this.treeBindingSource.DataMember = "Tree";
            this.treeBindingSource.DataSource = this.dsTree;
            // 
            // dsTree
            // 
            this.dsTree.DataSetName = "dsTree";
            this.dsTree.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.InsertGalleryImage("packageproduct_16x16.png", "office2013/support/packageproduct_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/support/packageproduct_16x16.png"), 0);
            this.imageCollection1.Images.SetKeyName(0, "packageproduct_16x16.png");
            this.imageCollection1.InsertGalleryImage("version_16x16.png", "office2013/support/version_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/support/version_16x16.png"), 1);
            this.imageCollection1.Images.SetKeyName(1, "version_16x16.png");
            this.imageCollection1.InsertGalleryImage("employee_16x16.png", "office2013/people/employee_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/people/employee_16x16.png"), 2);
            this.imageCollection1.Images.SetKeyName(2, "employee_16x16.png");
            this.imageCollection1.InsertGalleryImage("usergroup_16x16.png", "office2013/people/usergroup_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/people/usergroup_16x16.png"), 3);
            this.imageCollection1.Images.SetKeyName(3, "usergroup_16x16.png");
            this.imageCollection1.Images.SetKeyName(4, "PPT2.png");
            // 
            // pmTree
            // 
            this.pmTree.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdNewFile),
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdRename),
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdDelete),
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdNewFolder),
            new DevExpress.XtraBars.LinkPersistInfo(this.cmdNewFile)});
            this.pmTree.Manager = this.barManager1;
            this.pmTree.Name = "pmTree";
            // 
            // cmdRename
            // 
            this.cmdRename.Caption = "Rename";
            this.cmdRename.Id = 0;
            this.cmdRename.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2);
            this.cmdRename.Name = "cmdRename";
            // 
            // cmdDelete
            // 
            this.cmdDelete.Caption = "Delete";
            this.cmdDelete.Id = 1;
            this.cmdDelete.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.Delete);
            this.cmdDelete.Name = "cmdDelete";
            // 
            // cmdNewFolder
            // 
            this.cmdNewFolder.Caption = "New Folder";
            this.cmdNewFolder.Id = 2;
            this.cmdNewFolder.Name = "cmdNewFolder";
            // 
            // cmdNewFile
            // 
            this.cmdNewFile.Caption = "New File";
            this.cmdNewFile.Id = 3;
            this.cmdNewFile.Name = "cmdNewFile";
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.cmdRename,
            this.cmdDelete,
            this.cmdNewFolder,
            this.cmdNewFile});
            this.barManager1.MaxItemId = 4;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(304, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 249);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(304, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 249);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(304, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 249);
            // 
            // ucSelectFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uxTree);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ucSelectFile";
            this.Size = new System.Drawing.Size(304, 249);
            ((System.ComponentModel.ISupportInitialize)(this.uxTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList uxTree;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private System.Windows.Forms.BindingSource treeBindingSource;
        private dsTree dsTree;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.PopupMenu pmTree;
        private DevExpress.XtraBars.BarButtonItem cmdRename;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem cmdDelete;
        private DevExpress.XtraBars.BarButtonItem cmdNewFolder;
        private DevExpress.XtraBars.BarButtonItem cmdNewFile;
    }
}
