using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

using PPT = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

namespace ShapesLibrary
{
    [System.Diagnostics.DebuggerDisplay("{Name} {FullPath}; Files:{Files.Count}")]
    public class LibraryGroup : IGroup
    {
        public const string SHAPES_FILE_NAME = "__Shapes.pptx";

        protected IIndexProviderFactory indexFileFactory;

        /// <summary>
        /// Logical name of the Group
        /// </summary>
        public string Name { get; protected set; }

        public string FullName { get => Parent == null? Name: Path.Combine(Parent.Name, Name); }

        /// <summary>
        /// Path to Folder where group's files are reside
        /// </summary>
        public string FullPath { get => Parent == null? basePath: Path.Combine(Parent.FullPath, Name); }

        public IFile ShapesFile { get; protected set; }

        /// <summary>
        /// List of  Group's Library Files 
        /// </summary>
        public ReadOnlyCollection<IFile> Files { get { return files.AsReadOnly(); } }

        public ReadOnlyCollection<IFile> AllFiles { get { return allFiles.AsReadOnly(); } }

        protected List<IFile> files = new List<IFile>();

        protected List<IFile> allFiles = new List<IFile>();

        public ReadOnlyCollection<IGroup> SubGroups { get { return subGroups.AsReadOnly(); } }

        protected List<IGroup> subGroups = new List<IGroup>();

        public IGroup Parent { get; protected set; } = null;

        string basePath = null;

        public LibraryGroup(string name, string baseFolder, IIndexProviderFactory indexFileFactory)
        {
            Name = name;
            basePath = baseFolder;

            this.indexFileFactory = indexFileFactory;
        }

        public LibraryGroup(IGroup parent, string name, IIndexProviderFactory indexFileFactory)
        {
            Parent = parent;
            Name = name;

            this.indexFileFactory = indexFileFactory;
        }

        /// <summary>
        /// Return list of pptx file names in group folder
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> GetFileNamesInTheFolder()
        {
            string[] filePathsinTheFolder = Directory.GetFiles(FullPath, "*.pptx");
            return filePathsinTheFolder
                        .Where(path => !Path.GetFileName(path).StartsWith("~$"))
                        .OrderBy(path => path)
                        .Select(path => Path.GetFileName(path))
                    ;
        }

        /// <summary>
        /// Loads (or Reloads) list of LibraryFiles getting information from files located in the group folder.
        /// List of LibraryFiles is accessible vis Files property
        /// </summary>
        /// <param name="forceToRebuildIndex"></param>
        public void LoadFiles(bool forceToRebuildIndex)
        {
            files.Clear();
            allFiles.Clear();

            if (!string.IsNullOrEmpty(FullPath) && Directory.Exists(FullPath))
            {
                foreach (string filePath in GetFileNamesInTheFolder())
                {
                    try
                    {
                        LoadFile(filePath, forceToRebuildIndex);
                    }
                    catch
                    {
                        //TODO:
                    }
                }
                allFiles.AddRange(files);

                foreach (string path in Directory.GetDirectories(FullPath))
                {
                    string name = Path.GetFileName(path);

                    LibraryGroup group = new LibraryGroup(this, name, indexFileFactory);
                    group.LoadFiles(forceToRebuildIndex);

                    subGroups.Add(group);
                    allFiles.AddRange(group.AllFiles);
                }
            }
        }

        public void Rename(string newName)
        {
            string oldPath = FullPath;
            string oldName = FullName;
            Name = newName;

            Directory.Move(oldPath, FullPath);
            indexFileFactory.RenameGroup(oldName, FullName);
        }

        protected void LoadFile(string filePath, bool forceToRebuildIndex)
        {
            LibraryFile file = new LibraryFile(this, filePath, indexFileFactory);
            file.LoadItems(forceToRebuildIndex);
            files.Add(file);
            if (Path.GetFileName(filePath) == SHAPES_FILE_NAME)
            {
                file.Hidden = true;
                ShapesFile = file;
            }
        }

        /// <summary>
        /// Returns true if Group Folder contain file with specified name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool ContainsFile(string fileName)
        {
            return File.Exists(Path.Combine(FullPath, Path.GetFileName(fileName)));
        }

        /// <summary>
        /// Returns true if Group Folder contain file with specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsFolder(string name)
        {
            return Directory.Exists(Path.Combine(FullPath, Path.GetFileName(name)));
        }

        public IGroup CreateGroup(string name)
        {
            Directory.CreateDirectory(Path.Combine(FullPath, name));
            LibraryGroup group = new LibraryGroup(this, name, indexFileFactory);
            group.LoadFiles(false);

            subGroups.Add(group);
            allFiles.AddRange(group.AllFiles);

            return group;
        }

        /// <summary>
        /// Copies presentation file to group folder and saves it with specified name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public IFile AddFile(string fileName, PPT.Presentation source)
        {
            string newFilePath = Path.Combine(FullPath, Path.GetFileName(fileName));

            IFile file = files.Where(l => l.FullPath == newFilePath).FirstOrDefault();

            if (file != null)
            {
                file.Delete();
                files.Remove(file);
            }

            file = null;
            try
            {
                source.SaveCopyAs(newFilePath);

                file = new LibraryFile(this, fileName, indexFileFactory);
                file.LoadItems(true);

                files.Add(file);

                return file;
            }
            catch (Exception ex)
            {
                if (file != null)
                {
                    files.Remove(file);
                    file.Delete();
                }
                throw ex;
            }
        }

        public IFile AddFile(string fileName)
        {
            string newFilePath = Path.Combine(FullPath, Path.GetFileName(fileName));

            var file = new LibraryFile(this, fileName, indexFileFactory);
            file.LoadItems(true);

            files.Add(file);

            return file;

        }


        public void CreateShapesFile()
        {
            if (ContainsFile(SHAPES_FILE_NAME))
            {
                return;
            }

            string path = Path.Combine(FullPath, Path.GetFileName(SHAPES_FILE_NAME));
            PPT.Application ppt = null;
            PPT.Presentations pres = null;
            PPT.Presentation pptPresentation = null;
            try
            {
                ppt = new PPT.Application();
                pres = ppt.Presentations;
                pptPresentation = pres.Add(MsoTriState.msoFalse);
                pptPresentation.SaveAs(path);

                LoadFile(path, false);

            }
            finally
            {
                if (pptPresentation != null)
                {
                    pptPresentation.Saved = MsoTriState.msoTrue;
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

        /// <summary>
        /// Returns true if 
        /// - file were added to group folder
        /// - file was removeed from group folder
        /// - file already in th group was updated and file index is not uptodate
        /// </summary>
        /// <returns></returns>
        public bool GetNeedsIndexUpdateStatus()
        {
            if (string.IsNullOrEmpty(FullPath) || !Directory.Exists(FullPath))
            {
                return true;
            }

            foreach (IFile file in files)
            {
                if (!file.FileExists())
                {
                    return true;
                }
            }

            foreach (string fileName in GetFileNamesInTheFolder())
            {
                IFile lFile = files.Where(f => f.Name == fileName).FirstOrDefault();
                if (lFile == null)
                {
                    return true;
                }
                else
                {
                    if (lFile.IndexNeedsUpdate)
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        /// <summary>
        /// Updates list of files and file indexes that need update
        /// </summary>
        public void UpdateFilesList()
        {
            List<IFile> toBeDeleted = new List<IFile>();

            toBeDeleted.AddRange(files.Where(file => !file.FileExists()));
            toBeDeleted.ForEach(file =>
            {
                try
                {
                    file.Delete();
                }
                catch
                {

                }
                files.Remove(file);
            });

            foreach (string fileName in GetFileNamesInTheFolder())
            {
                try
                {
                    UpdateFileRecord(fileName);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Adds, Removes of Updates LibraryFile record for file in the group specified by name 
        /// </summary>
        /// <param name="fileName"></param>
        protected void UpdateFileRecord(string fileName)
        {
            IFile lFile = files.Where(f => f.Name == fileName).FirstOrDefault();
            if (lFile == null)
            {
                LibraryFile file = new LibraryFile(this, fileName, indexFileFactory);
                file.LoadItems(true);

                files.Add(file);
            }
            else
            {
                if (lFile.IndexNeedsUpdate)
                {
                    lFile.LoadItems(true);
                }
            }
        }


        public IEnumerable<IFileItem> GetAllItems()
        {
            foreach (IFile file in AllFiles)
            {
                foreach (IFileItem item in file.Items)
                {
                    yield return item;
                }
            }

        }


        public void Delete(IFile file)
        {
            file.Delete();

            allFiles.Remove(file);
            files.Remove(file);
        }


        public void Delete(IGroup group)
        {
            if (Directory.Exists(group.FullPath))
            {
                Directory.Delete(group.FullPath, true);
            }

            indexFileFactory.DeleteGroup(group.FullName);

            subGroups.Remove(group);
            group.AllFiles.ForEach(file => allFiles.Remove(file));

        }
    }

}
