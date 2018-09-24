using System.Drawing;


namespace ShapesLibrary
{
    public interface IFileItem
    {
        IFile File { get; }

        int Index { get; set; } //TODO: AST: use             item.File.Items.IndexOf(item)+1;

        string Title { get; set; }

        string Description { get; set; }

        string Keywords { get; set; }

        Bitmap Image { get; set; }

        ItemType Type { get; set; }

        int ShapesCount { get; set; }
    }

}
