using System.Drawing;

namespace ShapesLibrary
{

    [System.Diagnostics.DebuggerDisplay("{File.Group.Name}:{File.Group.FullPath} - {File.Name}:{Index} - {Title}")]
    public class LibraryFileItem : IFileItem
    {
        public IFile File { get; protected set; }

        public int Index { get; protected set; }

        public ItemType Type { get; set; }

        public Bitmap Image { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public string ShapeType { get; set; }

        public int ShapesCount { get; set; }


        public LibraryFileItem(IFile file, int id)
        {
            File = file;
            Index = id;
        }

        public LibraryFileItem(IFile file, int id, Bitmap image) : this(file, id)
        {
            Image = image;
        }

        public override string ToString() => $"{File.Name}:{Index} '{ShapeType}'";

    }

}
