using System.Linq;

namespace ShapesLibrary
{
    public class TreeTableBuilder
    {
        public dsTree.TreeDataTable TreeTable { get { return dsTree.Tree; } }

        protected ILibrary libraryData;

        protected dsTree dsTree = new dsTree();

        protected dsTree.TreeRow rowRoot;
        protected dsTree.TreeRow rowSystem;
        protected dsTree.TreeRow rowPersonal;
        public dsTree.TreeRow RowShared { get; protected set; }

        public TreeTableBuilder(ILibrary libraryData)
        {
            this.libraryData = libraryData;
            LoadDataTableFromLibrary();
        }

        protected void LoadDataTableFromLibrary()
        {
            dsTree = new dsTree();
            rowRoot = dsTree.Tree.AddTreeRow(-1, 0, "All Libraries", null, false); //TODO: Make consts
            rowSystem = dsTree.Tree.AddTreeRow(rowRoot.ID, 1, "System Library", libraryData.System, true);
            rowPersonal = dsTree.Tree.AddTreeRow(rowRoot.ID, 2, "Personal Library", libraryData.Personal, false);
            RowShared = dsTree.Tree.AddTreeRow(rowRoot.ID, 3, "Shared Library", libraryData.Shared, false);

            foreach (LibraryFile f in libraryData.System.Files)
            {
                dsTree.Tree.AddTreeRow(rowSystem.ID, 4, f.Name, f, true);
            }

            FillNode(libraryData.Personal, rowPersonal.ID);

            //libraryData.Personal?.Files.Where(f => !f.Hidden).Select(f =>
            //    dsTree.Tree.AddTreeRow(rowPersonal.ID, 4, f.Name, f, false));

            //TODO: For now only System and Personal folders
            //if (libraryData.Shared != null)
            //{
            //    foreach (LibraryFile f in libraryData.Shared.Files)
            //    {
            //        dsTree.Tree.AddTreeRow(RowShared.ID, 4, f.Name, f, false);
            //    }
            //}
            //else
            //{
            //    RowShared.Name += " (Not Configured).";
            //}
            dsTree.Tree.AcceptChanges();
        }

        protected void FillNode(IGroup group, int parentID)
        {
            group.Folders.ForEach(subFolder =>
            {
                dsTree.TreeRow row = dsTree.Tree.AddTreeRow(parentID, 0, subFolder.Name, subFolder, false);
                FillNode(subFolder, row.ID);
            });

            group.Files.Where(f => !f.Hidden).ForEach(file =>
            {
                dsTree.Tree.AddTreeRow(parentID, 4, file.Name, file, false);
            });
        }

        public void ReloadGroupData(IGroup group)
        {
            dsTree.TreeRow rw
                = dsTree.Tree.Where(row => row.Data.Equals(group)).FirstOrDefault();

            var list = dsTree.Tree.Where(row => row.ParentID == rw.ID).ToList();
            foreach (var row in list)
            {
                row.Delete();
            }

            foreach (LibraryFile f in group.Files)
            {
                dsTree.Tree.AddTreeRow(rw.ID, 4, f.Name, f, false);
            }

            dsTree.Tree.AcceptChanges();
        }

        public void UpdateShared()
        {
            if (RowShared == null)
            {
                RowShared = dsTree.Tree.AddTreeRow(dsTree.Tree[0].ID, 3, "Shared Library", libraryData.Shared, false);
            }
            else
            {
                RowShared.Name = "Shared Library";
                RowShared.Data = libraryData.Shared;
                libraryData.Shared.LoadFiles(false);
                ReloadGroupData(libraryData.Shared);
            }
        }

        public void AddFile(IFile file)
        {
            dsTree.TreeRow rowGroup = TreeTable.Where(row =>
                    row.Data is LibraryGroup && (row.Data as LibraryGroup).Name == file.Group.Name
                ).FirstOrDefault();

            if (rowGroup != null)
            {
                TreeTable.AddTreeRow(rowGroup.ID, 4, file.Name, file, false);
            }

        }
    }
}
