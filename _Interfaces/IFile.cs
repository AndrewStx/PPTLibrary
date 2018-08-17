using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using PPT = Microsoft.Office.Interop.PowerPoint;


namespace ShapesLibrary
{
    public interface IFile
    {
        IGroup Group { get; }

        string FullPath { get; }

        string Name { get; }

        bool Hidden { get; }

        ReadOnlyCollection<IFileItem> Items { get; }

        void LoadItems(bool rebuildIndex);

        List<IFileItem> AppendSlides(List<PPT.Slide> srcSlides);

        List<IFileItem> AppendShapes(List<PPT.Shape> srcShapes);

        bool IndexNeedsUpdate { get; }

        bool FileExists();

        void Delete();

        IFileItem CreateItem(int id);
    }

}
