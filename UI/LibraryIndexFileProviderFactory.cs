namespace ShapesLibrary
{
    public class IndexFileProviderFactory : IIndexProviderFactory
    {
        public IIndexProvider Create(IFile libraryFile)
        {
            return new IndexFileAdapter(libraryFile);
        }
    }

}
