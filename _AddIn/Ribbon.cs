using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools;
using ShapesLibrary;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace PPTShapesLibraryAddIn
{
    [ComVisible(true)]
    public class Ribbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public Ribbon()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("PPTShapesLibraryAddIn.Ribbon.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit http://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public Bitmap GetItemImage(string imageName)
        {
            Bitmap icon = null;
            try
            {
                icon = (Bitmap)Properties.Resources.ResourceManager.GetObject(imageName);
            }
            catch { }
            return icon;
        }
        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        public string SystemFolder { get; set; }

        public string PersonalFolder { get; set; }

        public string SharedFolder { get; set; }

        protected ShapesLibrary.Library library = null;

        //protected CustomTaskPane taskPaneLibrary = null;
        //protected Gallery.ucLibraryView uxLibraryView = null;

        protected UserControl CreateControl()
        {
            try
            {
                if (library == null)
                {
                    SystemFolder = @"C:\WRK\GalleryAddIn\z_Files";
                    PersonalFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"_ShapesLibrary");
                    SharedFolder = Properties.Settings.Default.SharedFolder;

                    library = new Library(SystemFolder, PersonalFolder, SharedFolder, new IndexFileProviderFactory());
                }

                Gallery.ucLibraryView uxLibraryView = new Gallery.ucLibraryView() { LibraryData = library };
                uxLibraryView.LoadLibraryData();

                uxLibraryView.ConfigChanged += libraryControl_ConfigChanged;

                return uxLibraryView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); //TODO: Replace with logger
                return null;
            }
        }

        void libraryControl_ConfigChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SharedFolder = library != null && library.Shared != null ? library.Shared.FullPath : (string)null;
                //C:\Users\astartsev\AppData\Local\Microsoft_Corporation\PPTShapesLibraryAddIn.vsto_vstoloc_Path_mbm4olrz2ri053gttzcaxysot1ldjeao
                Properties.Settings.Default.Save();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString()); //TODO: Replace with logger
            }
        }

        Dictionary<int, CustomTaskPane> taskPanes = new Dictionary<int, CustomTaskPane>();

        public void OnOpenGalleryView(Office.IRibbonControl control)
        {
            CustomTaskPane pane = GetLibraryTaskPane();
            if (pane != null)
            {
                pane.Visible = true;
            }
        }

        protected CustomTaskPane GetLibraryTaskPane()
        {
            int id = Globals.ThisAddIn.Application.ActiveWindow.HWND;

            CustomTaskPane pane = null;
            if (!taskPanes.Keys.Contains(id))
            {
                UserControl userControl = CreateControl();
                pane = Globals.ThisAddIn.CustomTaskPanes.Add(userControl, "+LIBRARY+");

                pane.VisibleChanged += paneGallery_VisibleChanged;
                pane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
                pane.Width = 430;
            
                taskPanes[id] = pane;
            }
            else
            {
                pane = taskPanes[id];
            }

            return pane;
        }

        protected Gallery.ucLibraryView LibraryControl
        {
            get => GetLibraryTaskPane()?.Control as Gallery.ucLibraryView;
        }

        public void CloseTaskPane()
        {
            int id = Globals.ThisAddIn.Application.ActiveWindow.HWND;

            CustomTaskPane pane = null;
            if (taskPanes.Keys.Contains(id))
            {
                pane = taskPanes[id];
//                pane.Visible = false;
                Globals.ThisAddIn.CustomTaskPanes.Remove(pane);
            }

        }

        void paneGallery_VisibleChanged(object sender, EventArgs e)
        {
            
        }


        public void OnManageGallery(Office.IRibbonControl control)
        {
        }

        public void OnAddToPersonal(Office.IRibbonControl control)
        {
            LibraryControl?.PublishFileToPersonal();
        }

        public void OnAddToShared(Office.IRibbonControl control)
        {
            LibraryControl?.PublishFileToShared();
        }

        public void OnAddSlide(Office.IRibbonControl control)
        {
            LibraryControl?.AddSelectedSlides();
        }

        public void OnAddShape(Office.IRibbonControl control)
        {
            LibraryControl?.AddSelectedShapes();
        }


    }
}
