using System.IO;

namespace ShapesLibrary
{
    public class IndexFileProviderFactory : IIndexProviderFactory
    {
        public int ThumbnailWidth { get; protected set; }
        public string Folder { get; protected set; }

        private IndexFileProviderFactory()
        {

        }

        public IndexFileProviderFactory(int thumbnailWidth, string folder)
        {
            ThumbnailWidth = thumbnailWidth;
            Folder = folder;
        }


        public IIndexProvider Create(IFile libraryFile)
        {
            return new IndexFileAdapter(libraryFile, Folder)
            {
                ThumbnailWidth = this.ThumbnailWidth
            };
        }

        public void RenameGroup(string oldName, string newName)
        {
            string src = Path.Combine(Folder, oldName);
            string dst = Path.Combine(Folder, newName);
            if (Directory.Exists(src))
            {
                Directory.Move(src, dst);
            }
        }

        public void DeleteGroup(string groupName)
        {
            string src = Path.Combine(Folder, groupName);
            if (Directory.Exists(src))
            {
                Directory.Delete(src, true);
            }
        }

    }

}
