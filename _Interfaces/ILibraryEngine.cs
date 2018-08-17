using PPT = Microsoft.Office.Interop.PowerPoint;

namespace ShapesLibrary
{
    public interface ILibraryEngine
    {
        void Add(PPT.Slide slide);
        void Add(PPT.Shape shape);
        void Add(PPT.Presentation presenatation);
    }
}
