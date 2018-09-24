using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using PPT = Microsoft.Office.Interop.PowerPoint;


namespace ShapesLibrary
{
    public interface IIndexProviderFactory
    {
        /// <summary>
        /// Creates Index provider for specified file
        /// </summary>
        /// <param name="libraryFile"></param>
        /// <returns></returns>
        IIndexProvider Create(IFile libraryFile);

        void RenameGroup(string oldName, string newName);

        void DeleteGroup(string groupName);

    }

    public interface IIndexProvider
    {
        /// <summary>
        /// Library File this index created for
        /// </summary>
        IFile LibraryFile { get; }

        /// <summary>
        /// Collection of index entries from Library File
        /// </summary>
        ReadOnlyCollection<IFileItem> Items { get; }

        void DeleteItem(IFileItem item);

        void LoadIndex();

        void CreateIndex();

        List<IFileItem> UpdateIndex(PPT.Presentation pptPresentation, int startIndex);

        /// <summary>
        /// Gets the latest time index was upodated
        /// </summary>
        DateTime LatestUpdate { get; }

        void DeleteIndexFile();

        void Rename();

    }

}
