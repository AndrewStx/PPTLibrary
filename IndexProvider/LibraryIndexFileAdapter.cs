using System;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Text;

using PPT = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

namespace ShapesLibrary
{
    //TODO: Move storage-independant (interaction with PPT) functionality to separate calss (base or porovider)
    /// <summary>
    /// Library File Index provider storing index data in the file on the disk
    /// </summary>
    public class IndexFileAdapter : IIndexProvider
    {
        const string ZIPEntry_IndexFileName = @"Index.txt";
        const string ZIPEntry_ImagesFolder = @"Images/";

        public int ThumbnailWidth { get; set; }


        protected string indexFileFullName => Path.Combine(indexFolder, LibraryFile.Group.FullName, filename);

        public IFile LibraryFile { get; protected set; }

        public ReadOnlyCollection<IFileItem> Items { get { return items.AsReadOnly(); } }

        private List<IFileItem> items = new List<IFileItem>();

        string indexFolder;
        string filename;

        public IndexFileAdapter(IFile libraryFile, string indexFolder)
        {
            this.LibraryFile = libraryFile;
            this.indexFolder = indexFolder;
            filename = GetIndexFileName();
        }

        public void Rename()
        {
            string oldPath = indexFileFullName;
            filename = GetIndexFileName();
            File.Move(oldPath, indexFileFullName);
        }


        private string GetIndexFileName() => Path.ChangeExtension(Path.GetFileName(LibraryFile.FullPath), ".zip");

        public void LoadIndex()
        {
            items.Clear();

            if (File.Exists(indexFileFullName) && !IndexNeedsUpdate)
            {
                if (!ReadIndex())
                {
                    CreateIndex();
                }
            }
            else
            {
                 CreateIndex();
            }
        }

        public DateTime LatestUpdate { 
            get { return File.GetLastWriteTimeUtc(indexFileFullName); } 
        }

        public bool IndexNeedsUpdate
        {
            get
            {
                return File.GetLastWriteTimeUtc(indexFileFullName) < File.GetLastWriteTimeUtc(LibraryFile.FullPath);
            }
        }


        /// <summary>
        /// Scans pptx file and creates Index
        /// </summary>
        public void CreateIndex()
        {
            items.Clear();

            DeleteIndexFile();
            CreateIndexFile();
            
            PPT.Application ppt = null;
            PPT.Presentations pres = null;
            PPT.Presentation presentation = null;
            try
            {
                ppt = new PPT.Application();
                pres = ppt.Presentations;
                presentation = pres.Open(LibraryFile.FullPath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                PPT.Slides gSlides = presentation.Slides;
                
                UpdateIndex(presentation, 1);
            }
            catch (Exception ex)
            {
                DeleteIndexFile();
                throw ex;
            }
            finally
            {
                presentation?.Close();

                //                gallery.ReleaseCOM();
                presentation = null;

                //                pres.ReleaseCOM();
                pres = null;

                ppt.ReleaseCOM();
                ppt = null;
            }
        }

        /// <summary>
        /// Creates Index Zip File with Index File Entry.
        /// <para>Can throw exceptions from inner calls.</para>
        /// </summary>
        protected void CreateIndexFile()
        {
            string directory = Path.GetDirectoryName(indexFileFullName);
            Directory.CreateDirectory(directory);

            using (FileStream zipFile = new FileStream(indexFileFullName, FileMode.Create))
            {
                using (ZipArchive zip = new ZipArchive(zipFile, ZipArchiveMode.Create))
                {
                    ZipArchiveEntry entry = zip.CreateEntry(ZIPEntry_IndexFileName);
                }
            }
        }

        /// <summary>
        /// Deletes Index file.
        /// <para>Can throw exceptions from inner calls.</para>
        /// </summary>
        public void DeleteIndexFile()
        {
            if (File.Exists(indexFileFullName))
            {
                File.Delete(indexFileFullName);
            }
        }

        /// <summary>
        /// Scans pptx file starting slide with specified index
        /// <para>Can throw exceptions from inner calls.</para>
        /// </summary>
        /// <param name="pptPresentation"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public List<IFileItem> UpdateIndex(PPT.Presentation pptPresentation, int startIndex)
        {
            PPT.Slide slide = null;
            dynamic addin = null;
            string tempIndexFile = null;
            List<string> tempFilesToDelete = new List<string>();

            try
            {
                List<IFileItem> newItems = new List<IFileItem>();

                string tempImgFile = null;

                addin = GetAddin();

                tempIndexFile = Path.GetTempFileName();

                using (FileStream zipFile = new FileStream(indexFileFullName, FileMode.Open))
                {
                    using (ZipArchive zip = new ZipArchive(zipFile, ZipArchiveMode.Update))
                    {
                        ZipArchiveEntry indexFileEntry = zip.GetEntry(ZIPEntry_IndexFileName);
                        indexFileEntry.ExtractToFile(tempIndexFile, true);

                        PPT.Slides gSlides = pptPresentation.Slides;
                        for (int idx = startIndex; idx <= gSlides.Count; idx++)
                        {
                            slide = gSlides[idx];
                            bool isShapeItem = slide.Tags[ShapeTag.Tag] != "";
                            #region Collect Item Data

                            string title = "";

                            StringBuilder sbKeyWords = new StringBuilder();
                            StringBuilder sbChartType = new StringBuilder();
                            StringBuilder sbDescr = new StringBuilder();

                            int chartsCount = 0;

                            if (isShapeItem)
                            {
                                try
                                {
                                    PPT.Shape shape = slide.Shapes[1]; //If it is the Shape we expect it to be the only shape on this slide
                                    if (addin?.IsExcelChart(shape)??false)
                                    {
                                        chartsCount++;
                                        #region Get Chart Description
                                        string[] str = addin.GetChartDescription(shape);
                                    }
                                    #endregion Get MG Chart Description
                                    else
                                    {
                                        //TODO: Get something from non chart shape
                                        title = shape.Title.Replace("\n", " ").Replace("  ", " ");
                                        sbKeyWords.Append(shape.Name);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else
                            { //Slide
                                foreach (PPT.Shape shape in slide.Shapes)
                                {
                                    try
                                    {
                                        if (shape.Name == "Title 1" && title == "")
                                        {
                                            PPT.TextFrame frame = shape.TextFrame;
                                            PPT.TextRange txt = frame.TextRange;
                                            title = txt.Text.Replace("\n", " ").Replace("  ", " ");
                                            txt = null;
                                            frame = null;
                                        }
                                        if (addin?.IsExceloChart(shape)??false)
                                        {
                                            chartsCount++;
                                        }
                                    }
                                    catch { }
                                }
                            }
                            sbKeyWords.Append(title);
                            sbKeyWords.Append(" ");

                            tempImgFile = Path.GetTempFileName();
                            float ratio = pptPresentation.PageSetup.SlideHeight / pptPresentation.PageSetup.SlideWidth;
                            slide.Export(tempImgFile, "png", ThumbnailWidth, (int)(ThumbnailWidth * ratio));
                            zip.CreateEntryFromFile(tempImgFile, ZIPEntry_ImagesFolder + idx + ".png");
                            Bitmap img = null;

                            using (var fs = new System.IO.FileStream(tempImgFile, System.IO.FileMode.Open))
                            {
                                img = new Bitmap(fs);
                            }

                            sbChartType.Replace("\n", " ");

                            #endregion

                            IFileItem item = LibraryFile.CreateItem(idx);
                            {
                                item.Image = img;
                                item.Type = (
                                    (Func<ItemType>)(() => {
                                        if (isShapeItem)
                                        {
                                            return chartsCount == 0 ? ItemType.Shape : ItemType.Chart;
                                        }
                                        else
                                        {
                                            //                                            if (mgChartsCount == 0)
                                            {
                                                return ItemType.Slide;
                                            }
                                            //    else
                                            //    {
                                            //        return mgChartsCount > 1 ? ItemType.SlideWithMultipleCharts : ItemType.SlideWithChart;
                                            //    }
                                        }
                                    })
                                )();
                                item.ShapesCount = chartsCount;
                                item.Title = title;
                                item.Description = sbDescr.ToString();
                                item.Keywords = sbKeyWords.ToString();
//TODO:                                item.ChartType = sbChartType.ToString();
                            }
                            newItems.Add(item);

                            slide = null;
                        }
                        items.AddRange(newItems);

                        Save(newItems, tempIndexFile);

                        indexFileEntry.Delete();
                        zip.CreateEntryFromFile(tempIndexFile, ZIPEntry_IndexFileName);
                    } //using (ZipArchive zip = new ZipArchive(zipFile, ZipArchiveMode.Update))
                } //using (FileStream zipFile = new FileStream(indexFilePath, FileMode.Open))

                return newItems;
            }
            finally
            {
                addin = null;

                slide.ReleaseCOM();
                slide = null;

                pptPresentation = null;

                try
                {
                    if (tempIndexFile != null)
                    {
                        File.Delete(tempIndexFile);
                    }

                    foreach (string path in tempFilesToDelete)
                    {
                        File.Delete(path);
                    }
                }
                catch
                {

                }
            }
        }

        dynamic GetAddin()
        {
            PPT.Application ppt = new PPT.Application();

            COMAddIn mg = null;
            foreach (COMAddIn addin in ppt.COMAddIns)
            {
                if (addin.ProgId == "AddInID")
                {
                    mg = addin;
                    break;
                }
            }

            dynamic mgAddin = mg?.Object;

            mg.ReleaseCOM();
            mg = null;

            ppt.ReleaseCOM();
            ppt = null;

            return mgAddin;
        }

        /*
        public List<IFileItem> Append(List<PPT.Slide> srcSlides)
        {
            PPT.Application ppt = null;
            PPT.Presentations pres = null;
            PPT.Presentation pptPresentation = null;
            try
            {
                ppt = new PPT.Application();
                pres = ppt.Presentations;

                pptPresentation = pres.Open(LibraryFile.FullPath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoTrue); //Copy Slide does not work if there is no window

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
                newItems = UpdateIndex(pptPresentation, startIndex);
                return newItems;
            }
            finally
            {
                if (pptPresentation != null)
                {
                    pptPresentation.Save();
                    pptPresentation.Close();
                    pptPresentation.ReleaseCOM();
                }
                pptPresentation = null;

                pres.ReleaseCOM();
                pres = null;

                ppt.ReleaseCOM();
                ppt = null;
            }
        }
        */

        #region File I/O operations
        protected bool ReadIndex()
        {
            try
            {
                using (FileStream zipFile = new FileStream(indexFileFullName, FileMode.Open))
                {
                    using (ZipArchive zip = new ZipArchive(zipFile, ZipArchiveMode.Read))
                    {
                        ZipArchiveEntry e = zip.GetEntry(ZIPEntry_IndexFileName);  //TODO: If not found?
                        Stream es = e.Open();
                        using (StreamReader file = new System.IO.StreamReader(es))
                        {
                            int i = 0;
                            IFileItem item;
                            while ((item = ReadItem(file, ++i)) != null)
                            {
                                ZipArchiveEntry imageFile = zip.GetEntry(ZIPEntry_ImagesFolder + i + ".png"); //TODO: If not found?
                                if (imageFile == null)
                                {
                                    return false;
                                }
                                Bitmap img = (Bitmap)Image.FromStream(imageFile.Open());
                                item.Image = img;
                                items.Add(item);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Reads one item form current position of the stream assosiated with index file. 
        /// index parameter if used for verification
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected IFileItem ReadItem(StreamReader stream, int index)
        {
            IFileItem row = LibraryFile.CreateItem(index);

            string str;
            str = stream.ReadLine();    // 1
            if (str == null)
            {
                return null;
            }

            int idx = -1;
            if (!int.TryParse(str, out idx) || idx != index)    //Broken structure
            {
                // return null;
            }

            str = stream.ReadLine();    // 2
            if (str == null)
            {
                return null;
            }
            str = XmlConvert.DecodeName(str);

            ItemType type = ItemType.Slide;
            Enum.TryParse<ItemType>(str, out type);
            {
                row.Type = type;
            }

            str = stream.ReadLine();    // 3
            if (str == null)
            {
                return null;
            }
            int.TryParse(str, out int count);
            {
                row.ShapesCount = count;
            }

            str = stream.ReadLine();    // 4
            if (str == null)
            {
                return null;
            }
            str = XmlConvert.DecodeName(str);
            row.Title = str;

            str = stream.ReadLine();    // 5
            if (str == null)
            {
                return null;
            }
            str = XmlConvert.DecodeName(str);
            row.Description = str;

            str = stream.ReadLine();    // 6
            if (str == null)
            {
                return null;
            }
            str = XmlConvert.DecodeName(str);
            row.Keywords = str;

            str = stream.ReadLine();     // 7
            if (str != "### " + index)
            {
                //Broken structure
                return null;
            }
            return row;
        }

        protected void Save(IEnumerable<IFileItem> items, string indexFile)
        {
            TextWriter wr = File.AppendText(indexFile);//TODO: Exception is possible here
            foreach (IFileItem item in items)
            {
                WriteItem(item, wr);
            }
            wr.Close();
        }

        protected void WriteItem(IFileItem item, TextWriter wr)
        {
            wr.WriteLine(XmlConvert.ToString(item.Index));              //1
            wr.WriteLine(XmlConvert.EncodeName(item.Type.ToString()));  //2
            wr.WriteLine(XmlConvert.ToString(item.ShapesCount));        //3
            wr.WriteLine(XmlConvert.EncodeName(item.Title));            //4
            wr.WriteLine(XmlConvert.EncodeName(item.Description));      //5
            wr.WriteLine(XmlConvert.EncodeName(item.Keywords));         //6
            wr.WriteLine("### " + item.Index.ToString());               //7
        }


        #endregion



    }
}
