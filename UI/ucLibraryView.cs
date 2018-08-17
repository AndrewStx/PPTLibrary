using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList.Nodes;

using PPT = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using ShapesLibrary;

namespace Gallery
{
    public partial class ucLibraryView : UserControl
    {
        /// <summary>
        /// Fired when Shared Folder is changed
        /// </summary>
        public event EventHandler<EventArgs> ConfigChanged;

        public ILibrary LibraryData { get; set; }

//        protected dsTree dsTree = new dsTree();

        protected dsTree.TreeDataTable tree { get => treeBuilder.TreeTable; }

        protected TreeTableBuilder treeBuilder = null;


        public ucLibraryView(ILibrary libraryData)
        {
            LibraryData = libraryData;

            InitializeComponent();

            //uxSearchGroup.Visible = uxCommandsGroup.Visible = false;
            SetEmptyTextForWhat();
            WhatButtonsEnable();

            toolTipController.AutoPopDelay = Int32.MaxValue;
            toolTipController.CalcSize += toolTipController_CalcSize;
        }

        public ucLibraryView()
        {
            InitializeComponent();
            
            //uxSearchGroup.Visible = uxCommandsGroup.Visible = false;
            SetEmptyTextForWhat();
            WhatButtonsEnable();

            toolTipController.AutoPopDelay = Int32.MaxValue;
            toolTipController.CalcSize += toolTipController_CalcSize;

            timerCheckForUpdates.Enabled = true;

            //uxGalleryTable.SetData(GetAllItems());
            
            #region Test Generate slides

            /*
            //for (int i = 0; i < 10; i++)
            //{
            //    Bitmap img = new Bitmap(300, 150);
            //    Graphics g = Graphics.FromImage(img);
            //    Font font = new Font("Arial", 10f);

            //    Rectangle r = new Rectangle(new Point(0, 0), img.Size);
            //    g.FillRectangle(Brushes.Cornsilk, r);
            //    StringFormat fmt = new StringFormat(StringFormatFlags.NoClip);
            //    fmt.Alignment = StringAlignment.Center;
            //    fmt.LineAlignment = StringAlignment.Center;
            //    g.DrawString(i.ToString(), font, Brushes.RoyalBlue, r, fmt);

            //    gallery.Add(img);
            //}

            //PPT.Application ap = new PPT.Application();
            //PPT.Presentations pres = ap.Presentations;
            //PPT.Presentation pr = pres.Add(Microsoft.Office.Core.MsoTriState.msoFalse);

            //PPT.Slides slides = pr.Slides;
            //for (int i = 1; i < 10; i++)
            //{
            //    PPT.Slide slide = slides.Add(i, PPT.PpSlideLayout.ppLayoutTitleOnly);
            //    PPT.Shapes shapes = slide.Shapes;
            //    PPT.Shape shape = slide.Shapes[1];

            //    PPT.TextRange txt = shape.TextFrame.TextRange;
            //    txt.Text = "Slide: " + i;
            //    txt.Font.Name = "Comic Sans MS";
            //    txt.Font.Size = 48;
            //    //PPT.Shape shape = shapes.AddTextbox(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal, 0, 0, 100, 20);

            //    shape.Copy();

            //    PPT.ShapeRange rng = shapes.Paste();
            //    shape.Top = 100;
            //    txt.Font.Name = "Arial";

            //    slide.Export(@"c:\WRK\_Gallery\" + i + ".png", "png", 320, 240);
            //}

            //pr.SaveAs(@"C:\WRK\_Gallery\Gallery.pptx");
            //pr.Close();
            */
            #endregion Generate slides


            //for (int i = 0; i < imageCollection1.Images.Count; i++)
            //{
            //    imageCollection1.Images[i].Save(@"C:\WRK\IM1\" + i + ".png", ImageFormat.Png);
            //}
            //for (int i = 0; i < imageCollection2.Images.Count; i++)
            //{
            //    imageCollection2.Images[i].Save(@"C:\WRK\IM2\" + i + ".png", ImageFormat.Png);
            //}
        }

        public void LoadLibraryAsync()
        {
            BackgroundWorker bkw = new BackgroundWorker();
            bkw.RunWorkerCompleted += bkw_RunWorkerCompleted;
            bkw.DoWork += bkw_DoWork;
            bkw.RunWorkerAsync();
        }
        
        public void LoadLibraryData()
        {
            try
            {
                LibraryData.Load();
            }
            catch
            {
                //
            }
            treeBuilder = new TreeTableBuilder(LibraryData);
            uxWhere.Properties.DataSource = tree.DefaultView;

            InitializeTreeControl();
            //uxSearchGroup.Visible = uxCommandsGroup.Visible = true;
            Filter();
        }

        void bkw_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadLibraryData();
        }

        void bkw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            InitializeTreeControl();
            //uxSearchGroup.Visible = uxCommandsGroup.Visible = true;
            Filter();
        }

        protected void InitializeTreeControl()
        {
            uxWhere.Properties.TreeList.KeyFieldName = "ID";
            uxWhere.Properties.TreeList.ParentFieldName = "ParentID";
            uxWhere.Properties.TreeList.ImageIndexFieldName = "ImageID";

//            uxWhere.Properties.DataSource = storage.TreeTable.DefaultView;
            //uxWhere.Properties.DisplayMember = "Name";
            uxWhere.Properties.ValueMember = "ID";

            dsTree.TreeRow rowRoot = tree[0];
            uxWhere.EditValue = rowRoot.ID;
            uxWhere.Properties.ContextImage = imageCollection1.Images[rowRoot.ImageID];

            int cnt = Math.Min(20, tree.DefaultView.Count);
            Size sz = uxWhere.Properties.PopupFormSize;
            sz.Height = cnt * 28;
            uxWhere.Properties.PopupFormSize = sz;

        }

        private void uxContainer_Resize(object sender, EventArgs e)
        {
            uxImagesView.Width = uxContainer.ClientRectangle.Width;
        }

        public void AddSlideOld()
        {
            if ((LibraryData.Personal == null || LibraryData.Personal.Files.Count == 0) &&
                (LibraryData.Shared == null || LibraryData.Shared.Files.Count == 0))
            {
                MessageBox.Show("The is no Customizable Libraries.");
            }
            else if (LibraryData.Personal.Files.Count == 1 && (LibraryData.Shared == null || LibraryData.Shared.Files.Count == 0))
            {
                AddSelectedSlides(LibraryData.Personal.Files[0]);
            }
            else
            {
                SelectFileForm dlg = new SelectFileForm();
                dlg.SetDataSource(tree);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    AddSelectedSlides(dlg.File);
                }
            }
        }

        public void AddSelectedSlides()
        {
            if (LibraryData.Personal == null)
            {
                MessageBox.Show("Personal Library is not configured.");
            }
            else 
            {
                if (LibraryData.Personal.ShapesFile == null)
                {
                    LibraryData.Personal.CreateShapesFile();
                }

                AddSelectedSlides(LibraryData.Personal.ShapesFile);
            }
        }

        public void AddSelectedShapes()
        {
            if (LibraryData.Personal == null)
            {
                MessageBox.Show("Personal Library is not configured.");
            }
            else
            {
                if (LibraryData.Personal.ShapesFile == null)
                {
                    LibraryData.Personal.CreateShapesFile();
                }

                AddSelectedShapes(LibraryData.Personal.ShapesFile);
            }
        }

        public void PublishFileToPersonal()
        {
            PublishFileIntoGroup(LibraryData.Personal);
        }

        public void PublishFileToShared()
        {
            if (LibraryData.Shared == null)
            {
                if (!ConfigureShared())
                    return;
            }

            PublishFileIntoGroup(LibraryData.Shared);
        }

        #region Tooltop

        void toolTipController_CalcSize(object sender, DevExpress.Utils.ToolTipControllerCalcSizeEventArgs e)
        {
            //e.Size = new Size(340, 340);
        }

        private void uxGalleryTable_HoveredItemChanged(object sender, GalleryItemEventArgs e)
        {
            if (e.Item == null)
            {
                toolTipController.HideHint();
            }
            else
            {
                DevExpress.Utils.ToolTipControllerShowEventArgs args;
                args = new DevExpress.Utils.ToolTipControllerShowEventArgs();
                args.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
                args.SuperTip = CreateToolTip(e.Item);
                args.ToolTipLocation = DevExpress.Utils.ToolTipLocation.Fixed;
                Point p = PointToScreen(this.uxContainer.Location);

                p.X -= 340;
//                toolTipController.ShowHint(args, new Point(this.Location.X - 340, this.Location.Y));
                if (!pmList.Visible)
                    toolTipController.ShowHint(args, p);
            }
        }

        private DevExpress.Utils.SuperToolTip CreateToolTip(IFileItem item)
        {
            DevExpress.Utils.SuperToolTip superToolTip = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitle = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipBody = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipItem toolTipFooter = new DevExpress.Utils.ToolTipItem();

            toolTipTitle.Text = item.Title;
            toolTipTitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            toolTipBody.ImageOptions.Image = item.Image;
//            toolTipBody.Appearance.Options.UseImage = true;

            toolTipFooter.Text = "Location: " + item.File.Group.Name + "\\" + item.File.Name + ". Slide " + item.Index;

            toolTipFooter.Appearance.Image = imageCollection1.Images[ item.Type == ItemType.Shape ? 0:1];
            toolTipFooter.Appearance.Options.UseImage = true;
            toolTipFooter.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            toolTipFooter.Appearance.Options.UseTextOptions = true;

            superToolTip.Items.Add(toolTipTitle);
            superToolTip.Items.Add(toolTipBody);
           superToolTip.Items.Add(toolTipFooter);

            superToolTip.FixedTooltipWidth = true;
            superToolTip.MaxWidth = 400;

            return superToolTip;
        }

        private void HideToolTip()
        {
            toolTipController.HideHint();
        }
        
        #endregion Tooltop

        private void InsertItem(IFileItem item)
        {
            if (item == null)
                return;

            PPT.Application ap = new PPT.Application();
            PPT.Presentations pres = ap.Presentations;

            PPT.Presentation gallery = pres.Open(item.File.FullPath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
            PPT.Slides gallerySlides = gallery.Slides;
            PPT.Slide gallerySlide = gallerySlides[item.Index];

            if (item.Type == ItemType.Slide)
            {
                gallerySlide.Copy();
            }
            else
            {
                gallerySlide.Shapes[1].Copy();
            }
            gallery.Close();

            PPT.Presentation pr = ap.ActivePresentation;
            PPT.Slides prSlides = pr.Slides;
            //TODO: Check if there is no selection (selection between slides)
            int ix = (ap.ActiveWindow.View.Slide as PPT.Slide).SlideIndex + 1; //TODO: 2 dots!
            if (item.Type == ItemType.Slide)
            {
                pr.Slides.Paste(ix);
            }
            else
            {
                ap.ActiveWindow.View.Paste();
            }
        }

        private void uxAddToPersonal_Click(object sender, EventArgs e)
        {
            PublishFileIntoGroup(LibraryData.Personal);
        }

        private void uxAddToShared_Click(object sender, EventArgs e)
        {
            PublishFileToShared();
        }

        private void uxAddSlide_Click(object sender, EventArgs e)
        {
            AddSelectedSlides();
        }

        protected bool ConfigureShared()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            dlg.Description = 
@"Select folder for Shared Libraries.
Make sure you have permission to create files at this location.";

            if (LibraryData.Shared != null)
            {
                dlg.SelectedPath = LibraryData.Shared.FullPath;
            }
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                uxWhere.Focus();
                return false;
            }

            //TODO: Check if possible to write


            LibraryData.ConfigureSharedGroup(dlg.SelectedPath);
            if (LibraryData.Shared != null)
            {
                LibraryData.Shared.LoadFiles(false);
                treeBuilder.UpdateShared();
            }

            uxWhere.Refresh();
            uxWhere.Properties.TreeList.ExpandAll();
            uxWhere.Focus();
            //Filter();  Do not filter here 
            OnConfigChanged();
            return true;
        }

        protected void OnConfigChanged()
        {
            ConfigChanged?.Invoke(this, new EventArgs());
        }

        protected void PublishFileIntoGroup(IGroup group)
        {

            try
            {
                timerCheckForUpdates.Enabled = false;

                Publish(group);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                timerCheckForUpdates.Enabled = !needsUpdate;
            }
        }

        private void Publish(IGroup group)
        {
            PPT.Application app = app = new PPT.Application();
            PPT.Presentation pres = app.ActivePresentation;

            string fileName = Path.GetFileName(pres.FullName);
            if (!Path.HasExtension(fileName))
            {
                fileName = Path.ChangeExtension(fileName, "pptx");
            }

            if (group.ContainsFile(fileName))
            {
                if (group.FullPath == Path.GetDirectoryName(pres.FullName))
                {
                    MessageBox.Show("You can't publish file in the folder where it is already located.");
                    return;
                }

                DialogResult res = MessageBox.Show("File with the same name exists. Replace?", "Slides Gallery",
                    MessageBoxButtons.OKCancel);
                if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                if (res == System.Windows.Forms.DialogResult.No)
                {
                    //TODO: Get new name?
                    return;
                }
                else
                {

                }
            }

            IFile file = group.AddFile(fileName, pres);
            if (file != null)
            {
                if (treeBuilder == null)
                {
                    LoadLibraryData();
                }
                treeBuilder.AddFile(file);
                Filter();
            }
        }

        protected void AddSelectedSlides(IFile libraryFile)    
        {
            PPT.Application ppt = null;
            PPT.Presentation pr = null;
            PPT.DocumentWindow aw = null;
            PPT.Selection sel = null;

            try
            {
                timerCheckForUpdates.Enabled = false;

                ppt = new PPT.Application();
                pr = ppt.ActivePresentation;

                if (pr.FullName == libraryFile.FullPath)
                {
                    MessageBox.Show("You can't add slides to the same presentation");
                    return;
                }

                aw = ppt.ActiveWindow;
                sel = aw.Selection;
                List<PPT.Slide> slidesToAdd = new List<PPT.Slide>();

                if (sel.Type == PPT.PpSelectionType.ppSelectionSlides)
                {
                    foreach (PPT.Slide slide in sel.SlideRange)
                    {
                        slidesToAdd.Add(slide);
                    }
                }
                else
                {
                    slidesToAdd.Add(aw.View.Slide as PPT.Slide);
                }

                if (slidesToAdd.Count > 0)
                {
                    libraryFile.AppendSlides(slidesToAdd);
                    uxImagesView.DataSource = LibraryData.GetAllItems().ToList();
                    Filter();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sel.ReleaseCOM();
                sel = null;

                pr.ReleaseCOM();
                pr = null;

                aw.ReleaseCOM();
                aw = null;
                
                ppt.ReleaseCOM();
                ppt = null;

                timerCheckForUpdates.Enabled = !needsUpdate;
            }
        }

        protected void AddSelectedShapes(IFile libraryFile)
        {
            PPT.Application ppt = null;
            PPT.Presentation pr = null;
            PPT.DocumentWindow aw = null;
            PPT.Selection sel = null;

            try
            {
                timerCheckForUpdates.Enabled = false;

                ppt = new PPT.Application();
                pr = ppt.ActivePresentation;

                if (pr.FullName == libraryFile.FullPath)
                {
                    MessageBox.Show("You can't add slides to the same presentation");
                    return;
                }

                aw = ppt.ActiveWindow;
                sel = aw.Selection;
                List<PPT.Shape> shapesToAdd = new List<PPT.Shape>();

                if (sel.Type == PPT.PpSelectionType.ppSelectionShapes)
                {
                    foreach (PPT.Shape slide in sel.ShapeRange)
                    {
                        shapesToAdd.Add(slide);
                    }
                }
                else
                {
                    shapesToAdd.Add(aw.View.Slide as PPT.Shape);
                }

                if (shapesToAdd.Count > 0)
                {
                    libraryFile.AppendShapes(shapesToAdd);
                    uxImagesView.DataSource = LibraryData.GetAllItems().ToList();
                    Filter();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sel.ReleaseCOM();
                sel = null;

                pr.ReleaseCOM();
                pr = null;

                aw.ReleaseCOM();
                aw = null;

                ppt.ReleaseCOM();
                ppt = null;

                timerCheckForUpdates.Enabled = !needsUpdate;
            }
        }
        
        #region Filtering

        private void uxWhere_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                dsTree.TreeRow row = tree.FindByID((int)uxWhere.EditValue);
                row = tree.FindByID(row.ParentID);
                if (row != null)
                {
                    uxWhere.EditValue = row.ID;
                }
            }
        }

        private void uxWhere_EditValueChanging(object sender, ChangingEventArgs e)
        {
            dsTree.TreeRow row = tree.FindByID((int)e.NewValue);

            if (row.ParentID != -1 && row.Data.Equals(System.DBNull.Value))
            {
                if (ConfigureShared())
                {
                    uxWhere.Refresh();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void uxWhere_EditValueChanged(object sender, EventArgs e)
        {
            dsTree.TreeRow row = tree.FindByID((int)uxWhere.EditValue);
            uxWhere.Properties.ContextImage = imageCollection1.Images[row.ImageID];
            uxWhere.Properties.Buttons[1].Enabled = row.ParentID != 0-1;

            Filter();
        }

        #region uxWhat

        private void uxWhat_Enter(object sender, EventArgs e)
        {
            if (uxWhat.Tag != null)
            {
                uxWhat.IsModified = false;
                uxWhat.Text = "";
                uxWhat.ForeColor = (Color)uxWhat.Tag;
                uxWhat.Tag = null;
            }
        }

        private void uxWhat_Leave(object sender, EventArgs e)
        {
            SetEmptyTextForWhat();
        }

        private void uxWhat_Validated(object sender, EventArgs e)
        {
//            if (uxWhat.Tag == null)
            {
                Filter();
            }

        }

        private void uxWhat_EditValueChanged(object sender, EventArgs e)
        {
            if (uxWhat.IsModified)
            {
                WhatButtonsEnable();
            }
        }

        private void WhatButtonsEnable()
        {
            uxWhat.Properties.Buttons[(int)WhatButtons.Search].Enabled = true;
            uxWhat.Properties.Buttons[(int)WhatButtons.Clear].Enabled = uxWhat.Text.Length > 0 && uxWhat.Tag == null;
        }

        private void SetEmptyTextForWhat()
        {
            if (uxWhat.Text.Length == 0)
            {
                uxWhat.Tag = uxWhat.ForeColor;
                uxWhat.ForeColor = Color.Silver;
                uxWhat.IsModified = false;
                uxWhat.Text = "Search in the library";
            }
            else
            {
                uxWhat.Tag = null;
            }
        }

        enum WhatButtons { Clear=0, Search=1 };

        private void uxWhat_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == (int)WhatButtons.Clear)
            {
                if (uxWhat.Text.Length == 0)
                {
                    return;
                }

                uxWhat.IsModified = false;
                uxWhat.Text = "";
                WhatButtonsEnable();
            }

            if (uxWhat.Tag == null)
            {
                Filter();
            }
        }
        
        #endregion uxWhat

        private void Filter()
        {
            uxWhat.Properties.Buttons[(int)WhatButtons.Search].Enabled = false;

            Regex regEx = new Regex(uxWhat.Tag == null ? uxWhat.Text : "", RegexOptions.IgnoreCase);

            dsTree.TreeRow row = tree.FindByID((int)uxWhere.EditValue);
            var list = LibraryData.GetAllItems();
            if (row.ParentID == -1)
            {
                list = list.Where(item =>
                    regEx.IsMatch(item.Keywords)
                    );
            }
            else if (row.Data is LibraryFile)
            {
                LibraryFile file = row.Data as LibraryFile;
                list = list.Where(item =>
                    item.File.Group.Name == file.Group.Name &&
                    item.File.Name == file.Name &&
                    regEx.IsMatch(item.Keywords)
                    );
            }
            else 
            {
                LibraryGroup group = row.Data as LibraryGroup;
                if (group != null)
                {
                    list = list.Where(item =>
                        item.File.Group.FullPath == group.FullPath &&
                        regEx.IsMatch(item.Keywords)
                        );
                }
                else
                {
                    list = Enumerable.Empty<IFileItem>();
                }
            }

            uxImagesView.DataSource = list.ToList();
        }

        #endregion Filtering

        /// <summary>
        /// Opens Folder in Windows Explorer
        /// </summary>
        /// <param name="path"></param>
        private void OpenFolder(string path)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
            }
            catch
            {

            }
        }
        
        /// <summary>
        /// Opens Presentation and selects slide
        /// </summary>
        /// <param name="path"></param>
        /// <param name="slideIndex"></param>
        private void OpenPresentation(string path, int slideIndex = 1)
        {
            PPT.Application ppt = null;
            PPT.Presentations pres = null;
            PPT.Presentation pptPresentation = null;

            try
            {
                ppt = new PPT.Application();
                pres = ppt.Presentations;

                //TODO: Check if file is already opned
                pptPresentation = pres.Open(path);
                pptPresentation.Slides[slideIndex].Select();

            }
            catch (Exception)
            {

            }
            finally
            {
                pptPresentation.ReleaseCOM();
                pptPresentation = null;

                pres.ReleaseCOM();
                pres = null;

                ppt.ReleaseCOM();
                ppt = null;
            }
        }

        private void barManager_QueryShowPopupMenu(object sender, DevExpress.XtraBars.QueryShowPopupMenuEventArgs e)
        {
            if (e.Menu == pmList)
            {
                e.Cancel = uxImagesView.SelectedItem == null;
            }
            else if (e.Menu == pmTree)
            {
                int treeLevel = 0;
                dsTree.TreeRow row = null;
                if (uxWhere.Properties.TreeList.Selection.Count > 0)
                {
                    TreeListNode node = uxWhere.Properties.TreeList.Selection[0];
                    treeLevel = node.Level;
                    row = (uxWhere.Properties.TreeList.GetDataRecordByNode(node) as DataRowView).Row as dsTree.TreeRow;
                }
                else
                {
                    row = tree.FindByID((int)uxWhere.EditValue);
                    if (row.ParentID == -1)
                        treeLevel = 0;
                    else if (row.ParentID == 1)
                        treeLevel = 1;
                    else
                        treeLevel = 2;

                }
                if (treeLevel == 0)
                {
                    cmdManage.Visibility = BarItemVisibility.Always;
                    cmdOpenFolder.Visibility = 
                    cmdOpenFile.Visibility = 
                    cmdChangeFolder.Visibility =
                        BarItemVisibility.Never;
                }
                else if (treeLevel == 1)
                {
                    bool shared = row == treeBuilder.RowShared;
                    bool configured = !row.Data.Equals(System.DBNull.Value);

                    cmdOpenFolder.Visibility = configured ? BarItemVisibility.Always : BarItemVisibility.Never;
                    cmdManage.Visibility =
                    cmdOpenFile.Visibility =
                        BarItemVisibility.Never;
                    cmdChangeFolder.Visibility = shared ? BarItemVisibility.Always : BarItemVisibility.Never; 
                }
                else if (treeLevel == 2)
                {
                    cmdOpenFolder.Visibility =
                    cmdOpenFile.Visibility = 
                        BarItemVisibility.Always;
                    cmdManage.Visibility =
                    cmdChangeFolder.Visibility =
                        BarItemVisibility.Never;
                }
            }
        }

        private void uxWhere_Popup(object sender, EventArgs e)
        {
            barManager.SetPopupContextMenu(uxWhere.GetPopupEditForm(), pmTree);
        }

        private void uxGalleryTable_Click(object sender, EventArgs e)
        {
            HideToolTip();
        }

        private void uxGalleryTable_DoubleClick(object sender, EventArgs e)
        {
            InsertItem(uxImagesView.SelectedItem);
        }

        private void cmdSetupFolder_ItemClick(object sender, ItemClickEventArgs e)
        {
            uxWhere.ClosePopup();
            ConfigureShared();
            Filter();
        }

        private void cmdOpenFolder_ItemClick(object sender, ItemClickEventArgs e)
        {
            dsTree.TreeRow row =
                (uxWhere.Properties.TreeList.GetDataRecordByNode(uxWhere.Properties.TreeList.FocusedNode) as DataRowView).Row as dsTree.TreeRow;
            
            LibraryFile file = row.Data as LibraryFile;
            if (file != null)
            {
                OpenFolder(file.FullPath);
            }
            else
            {
                LibraryGroup group = row.Data as LibraryGroup;
                if (group != null)
                {
                    OpenPresentation(group.FullPath);
                }
            }
        }

        private void cmdManage_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void cmdInsert_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InsertItem(uxImagesView.SelectedItem);
        }

        private void cmdOpenFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            dsTree.TreeRow row =
                (uxWhere.Properties.TreeList.GetDataRecordByNode(uxWhere.Properties.TreeList.FocusedNode) as DataRowView).Row as dsTree.TreeRow;
            LibraryGroup group = row.Data as LibraryGroup;
            LibraryFile file = row.Data as LibraryFile;
            if (file != null)
            {
                OpenPresentation(file.FullPath);
            }

        }

        private void cmdOpenContainingFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            IFileItem item = uxImagesView.SelectedItem;
            if (item != null)
            {
                OpenPresentation(item.File.FullPath, item.Index);
            }
        }

        protected bool needsUpdate;
        private void timerCheckForUpdates_Tick(object sender, EventArgs e)
        {
            timerCheckForUpdates.Enabled = false;
            if (!needsUpdate)
            {
                needsUpdate = LibraryData.GetFilesListNeedsUpdateStatus();
            }
            //uxRefresh.Enabled = needsUpdate;
            timerCheckForUpdates.Enabled = !needsUpdate;
        }

        private void uxRefresh_Click(object sender, EventArgs e)
        {
            LibraryData.UpdateFilesList();
            uxImagesView.DataSource = LibraryData.GetAllItems().ToList();


            treeBuilder = new TreeTableBuilder(LibraryData);
            uxWhere.Properties.DataSource = tree.DefaultView;

            Filter();
            needsUpdate = false;
            //uxRefresh.Enabled = false;

            timerCheckForUpdates.Enabled = true;
        }

        private void ucLibraryView_Leave(object sender, EventArgs e)
        {

        }
    }

    //using (ZipArchive zip = new ZipArchive(
    //    new FileStream(@"C:\WRK\_GallerySys\z.zip", FileMode.OpenOrCreate)
    //    , ZipArchiveMode.Create)
    //    )
    //{
    //    for (int i = 0; i < 100; i++ )
    //        zip.CreateEntryFromFile(@"C:\WRK\_GallerySys\1.png", "INDEX\\N"+i);

    //}

        //private static String WildCardToRegular(String value)
        //{
        //    return "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        //}
        ////    String test = "Some Data X";

        ////    Boolean endsWithEx = Regex.IsMatch(test, WildCardToRegular("*X"));
        ////    Boolean startsWithS = Regex.IsMatch(test, WildCardToRegular("S*"));
        ////    Boolean containsD = Regex.IsMatch(test, WildCardToRegular("*D*"));

        ////    // Starts with S, ends with X, contains "me" and "a" (in that order) 
        ////    Boolean complex = Regex.IsMatch(test, WildCardToRegular("S*me*a*X"));
        ////

        //public static Bitmap ResizeImage(Image image, int width, int height)
        //{
        //    var destRect = new Rectangle(0, 0, width, height);
        //    var destImage = new Bitmap(width, height);

        //    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    using (var graphics = Graphics.FromImage(destImage))
        //    {
        //        graphics.CompositingMode = CompositingMode.SourceCopy;
        //        graphics.CompositingQuality = CompositingQuality.HighQuality;
        //        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphics.SmoothingMode = SmoothingMode.HighQuality;
        //        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //        using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
        //        {
        //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        //            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //        }
        //    }

        //    return destImage;
        //}

    //var lBackup = new Dictionary<string, object>(); 
    //var lDataObject = Clipboard.GetDataObject();
    //var lFormats = lDataObject.GetFormats(false);
    //foreach (var lFormat in lFormats)
    //{
    //    var vv = lDataObject.GetData(lFormat, false);
    //    lBackup.Add(lFormat, vv);
    //}

    //Clipboard.SetText("asd");

    ////Would be interesting to check the contents of lDataObject here

    ////Restore data
    //foreach (var lFormat in lFormats)
    //{
    //    lDataObject.SetData(lBackup[lFormat]);
    //}
    ////This might be unnecessary
    //Clipboard.SetDataObject(lDataObject);
    //                popupControlContainer1.ShowPopup(this.PointToScreen(new Point(uxAddSlide.Left, uxAddSlide.Bottom)));

}
