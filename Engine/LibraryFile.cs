using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using PPT = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

namespace ShapesLibrary
{
    [System.Diagnostics.DebuggerDisplay("{Group.Name}:{Group.FullPath} - {Name} - {Items.Count}")]
    public class LibraryFile : IFile
    {
        protected IIndexProvider fileIndex;

        public IGroup Group { get; protected set; }

        public string FullPath { get { return Path.Combine(Group.FullPath, Name); } }

        /// <summary>
        /// File Name+Extension without path
        /// </summary>
        public string Name { get; protected set; }

        public bool Hidden { get; set; }

        /// <summary>
        /// All LibraryFileItems in the file
        /// </summary>
        public ReadOnlyCollection<IFileItem> Items { get { return fileIndex.Items; } }
        
        public LibraryFile(IGroup group, string path, IIndexProviderFactory factory)
        {
            Group = group;
            Name = Path.GetFileName(path);

            fileIndex = factory.Create(this);
        }

        public IFileItem CreateItem(int id) => new LibraryFileItem(this, id);

        /// <summary>
        /// Loads file items from index.
        /// Recreates index if index needs to be updated or forced to be rebuild by rebuildIndex parameter.
        /// </summary>
        /// <param name="rebuildIndex"></param>
        public void LoadItems(bool rebuildIndex)
        {
            if (rebuildIndex)
            {
                fileIndex.CreateIndex();
            }
            else
            {
                fileIndex.LoadIndex();
            }
        }

        public void Delete()
        {
            try
            {
                if (File.Exists(FullPath))
                    File.Delete(FullPath);
            }
            catch
            {

            }
            
            try
            {
                fileIndex.DeleteIndexFile();
            }
            catch
            {

            }
        }

        public void Rename(string newName)
        {
            
            File.Move( Path.Combine(Group.FullPath, Name), Path.Combine(Group.FullPath, newName));
            Name = newName;
            fileIndex.Rename();
        }


        public List<IFileItem> AppendSlides(List<PPT.Slide> srcSlides)
        {
            PPT.Application ppt = null;
            PPT.Presentations pres = null;
            PPT.Presentation pptPresentation = null;
            try
            {
                ppt = new PPT.Application();
                pres = ppt.Presentations;

                pptPresentation = pres.Open(FullPath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoTrue);

                PPT.DocumentWindow wnd = pptPresentation.Windows[1];
                //wnd.Height = 200;
                //wnd.Top = -200;
                wnd.WindowState = PPT.PpWindowState.ppWindowMinimized;
                wnd = null;

                #region Alt. slower approach by copy ppt and insert slides from file
                //Copy 
                //string tempFile = @"c:\WRK\Temp.pptx";
                //ppt.ActivePresentation.SaveCopyAs(tempFile, PPT.PpSaveAsFileType.ppSaveAsDefault, Microsoft.Office.Core.MsoTriState.msoTrue);
                //PPT.Presentation tempPPT = ppt.Presentations.Open(tempFile,MsoTriState.msoFalse,MsoTriState.msoFalse, MsoTriState.msoFalse);

                //List<string> slideNames = slides.Select(slide => slide.Name).ToList();
                //var v = from PPT.Slide sl in tempPPT.Slides where !slideNames.Contains(sl.Name) select sl;
                //foreach(PPT.Slide s in v)
                //{
                //    s.Delete();
                //}
                //tempPPT.Save();
                //tempPPT.Close();
                //gallery.Slides.InsertFromFile(tempFile, gallery.Slides.Count);
                #endregion

                PPT.Slides gSlides = pptPresentation.Slides;
                int startIndex = gSlides.Count + 1;
                foreach (PPT.Slide slide in srcSlides)
                {
                    slide.Copy();
                    gSlides.Paste();
                }
                List<IFileItem> newItems = new List<IFileItem>();
                newItems = fileIndex.UpdateIndex(pptPresentation, startIndex);

                return newItems;
            }
            finally
            {
                if (pptPresentation != null)
                {
                    pptPresentation.Save();
                    pptPresentation.Close();
                }
                pptPresentation.ReleaseCOM();
                pptPresentation = null;

                pres.ReleaseCOM();
                pres = null;

                ppt.ReleaseCOM();
                ppt = null;
            }
        }

        public List<IFileItem> AppendShapes(List<PPT.Shape> srcShapes)
        {
            PPT.Application ppt = null;
            PPT.Presentations pres = null;
            PPT.Presentation pptPresentation = null;
            try
            {
                ppt = new PPT.Application();
                pres = ppt.Presentations;

                pptPresentation = pres.Open(FullPath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoTrue);

                PPT.DocumentWindow wnd = pptPresentation.Windows[1];
                //wnd.Height = 200;
                //wnd.Top = -200;
                wnd.WindowState = PPT.PpWindowState.ppWindowMinimized;
                wnd = null;

                PPT.Slides gSlides = pptPresentation.Slides;
                int startIndex = gSlides.Count + 1;
                int count = 0;
                foreach (PPT.Shape shape in srcShapes)
                {
                    PPT.Slide slide = gSlides.Add(startIndex + count++, PPT.PpSlideLayout.ppLayoutBlank);
                    shape.Copy();
                    slide.Shapes.Paste();
                    slide.Tags.Add(ShapeTag.Tag, ShapeTag.Value);
                    
                    slide.ReleaseCOM();
                    slide = null;
                }
                List<IFileItem> newItems = new List<IFileItem>();
                newItems = fileIndex.UpdateIndex(pptPresentation, startIndex);

                return newItems;
            }
            finally
            {
                if (pptPresentation != null)
                {
                    pptPresentation.Save();
                    pptPresentation.Close();
                }
                pptPresentation.ReleaseCOM();
                pptPresentation = null;

                pres.ReleaseCOM();
                pres = null;

                ppt.ReleaseCOM();
                ppt = null;
            }
        }

        public bool IndexNeedsUpdate {
            get {
                return fileIndex.LatestUpdate < File.GetLastWriteTimeUtc(FullPath);
            }
        }

        public bool FileExists()
        {
            try
            {
                return File.Exists(FullPath);
            }
            catch
            {
                return false;
            }
        }

        public void DeleteItem(IFileItem item)
        {
            fileIndex.DeleteItem(item);
        }
    }

 }
