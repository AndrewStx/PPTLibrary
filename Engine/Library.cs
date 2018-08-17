using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ShapesLibrary
{
    public partial class Library : ILibrary
    {
       
        protected IIndexProviderFactory indexAdapterFactory;

        public IGroup System { get; protected set; }

        public IGroup Personal { get; protected set; }

        public IGroup Shared { get; protected set; }

        public Library(string systemFolder, string personalFolder, string sharedFolder, IIndexProviderFactory indexAdapterFactory)
        {
            this.indexAdapterFactory = indexAdapterFactory;

            System = CreateGroup("System", systemFolder);
            Personal = CreateGroup("Personal", personalFolder);
            Shared = CreateGroup("Shared", sharedFolder);
        }

        public void ConfigureSharedGroup(string sharedFolder)
        {
            Shared = CreateGroup("Shared", sharedFolder);
        }

        protected IGroup CreateGroup(string groupName, string folder)
        {
            IGroup group = null;
            try
            {
                if (!string.IsNullOrEmpty(folder))
                {
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    if (Directory.Exists(folder))
                    {
                        group = new LibraryGroup(groupName, folder, indexAdapterFactory);
                    }
                }
            }
            catch
            {
                group = null;
            }

            return group;
        }

        /// <summary>
        /// Loads Files in all Groups
        /// </summary>
        public void Load()
        {
            LoadGroupIfConfigured(System);
            LoadGroupIfConfigured(Personal);
            LoadGroupIfConfigured(Shared);
        }

        protected void LoadGroupIfConfigured(IGroup group)
        {
            try
            {
                group?.LoadFiles(false);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Returns all LibraryFileItems in all configured groups
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFileItem> GetAllItems()
        {
            var c1 = System?.GetAllItems()?? Enumerable.Empty<IFileItem>();
            var c2 = Personal?.GetAllItems()?? Enumerable.Empty<IFileItem>();
            var c3 = Shared?.GetAllItems()?? Enumerable.Empty<IFileItem>();

            return c1.Concat(c2).Concat(c3);
        }

        public bool GetFilesListNeedsUpdateStatus()
        {
            return
                System.GetNeedsIndexUpdateStatus() ||
                Personal.GetNeedsIndexUpdateStatus() ||
                (Shared != null && Shared.GetNeedsIndexUpdateStatus());
        }

        public void UpdateFilesList()
        {
            if (System.GetNeedsIndexUpdateStatus())
            {
                System.UpdateFilesList();
            }

            if (Personal.GetNeedsIndexUpdateStatus())
            {
                Personal.UpdateFilesList();
            }

            if (Shared != null && Shared.GetNeedsIndexUpdateStatus())
            {
                Shared.UpdateFilesList();
            }

        }

    }
}
