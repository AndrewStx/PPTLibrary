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
            this.treeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsTree = new dsTree();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.uxTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
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
            this.uxTree.OptionsBehavior.Editable = false;
            this.uxTree.OptionsBehavior.ImmediateEditor = false;
            this.uxTree.OptionsBehavior.ReadOnly = true;
            this.uxTree.OptionsBehavior.ResizeNodes = false;
            this.uxTree.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.RowFullFocus;
            this.uxTree.OptionsView.ShowColumns = false;
            this.uxTree.OptionsView.ShowFilterPanelMode = DevExpress.XtraTreeList.ShowFilterPanelMode.Never;
            this.uxTree.OptionsView.ShowHorzLines = false;
            this.uxTree.OptionsView.ShowIndicator = false;
            this.uxTree.OptionsView.ShowRoot = false;
            this.uxTree.OptionsView.ShowVertLines = false;
            this.uxTree.SelectImageList = this.imageCollection1;
            this.uxTree.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.Default;
            this.uxTree.Size = new System.Drawing.Size(304, 249);
            this.uxTree.TabIndex = 1;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "Name";
            this.treeListColumn1.FieldName = "Name";
            this.treeListColumn1.MinWidth = 88;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 199;
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
            // ucSelectFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uxTree);
            this.Name = "ucSelectFile";
            this.Size = new System.Drawing.Size(304, 249);
            ((System.ComponentModel.ISupportInitialize)(this.uxTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList uxTree;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private System.Windows.Forms.BindingSource treeBindingSource;
        private dsTree dsTree;
    }
}
