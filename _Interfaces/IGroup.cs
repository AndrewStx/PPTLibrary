﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

using PPT = Microsoft.Office.Interop.PowerPoint;


namespace ShapesLibrary
{

    public interface IGroup
    {
        IGroup Parent
        {
            get;
        }

        /// <summary>
        /// Logical Name of the group
        /// </summary>
        string Name { get; }

        string FullName { get; }

        /// <summary>
        /// OS Path to the group folder
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// List of files in the group excluding subfolders
        /// </summary>
        ReadOnlyCollection<IFile> Files { get; }

        /// <summary>
        /// Returns all files in group including subfolders
        /// </summary>
        ReadOnlyCollection<IFile> GetAllFiles { get; }


        IEnumerable<IFileItem> GetAllItems();
       

        /// <summary>
        /// List of Subfolders inside of this group
        /// </summary>
        ReadOnlyCollection<IGroup> SubGroups { get; }

        IFile ShapesFile { get; }

        void CreateShapesFile();

        /// <summary>
        /// Loads (or Reloads) list of LibraryFiles getting information from files located in the group folder.
        /// List of LibraryFiles is accessible vis Files property
        /// </summary>
        /// <param name="forceToRebuildIndex"></param>
        void LoadFiles(bool forceToRebuildIndex);

        /// <summary>
        /// Copies presentation file to group folder and saves it with specified name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        IFile AddFile(string fileName, PPT.Presentation source);


        /// <summary>
        /// Return true is Group Folder contains file with specified name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool ContainsFile(string fileName);

        IGroup CreateGroup(string name);

        bool ContainsFolder(string name);

        IFile AddFile(string fileName);

        /// <summary>
        /// Returns true if 
        /// - file were added to group folder
        /// - file was removeed from group folder
        /// - file already in th group was updated and file index is not uptodate
        /// </summary>
        /// <returns></returns>
        bool GetNeedsIndexUpdateStatus();

        /// <summary>
        /// Updates list of files and file indexes that need update
        /// </summary>
        void UpdateFilesList();

        void Rename(string newName);

        void DeleteFile(string fileName);

        void Delete(IFile file);

        void Delete(IGroup group);


    }

}
