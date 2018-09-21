using System.Linq;
using System.IO;


namespace ShapesLibrary
{
    public class TreeTableBuilder
    {
        protected ILibrary library;

        protected dsTree.TreeDataTable table;

        protected dsTree.TreeRow rowRoot;
        protected dsTree.TreeRow rowSystem;
        protected dsTree.TreeRow rowPersonal;
        public dsTree.TreeRow RowShared { get; protected set; }

        public TreeTableBuilder(ILibrary libraryData, dsTree.TreeDataTable treeTable)
        {
            library = libraryData;
            table = treeTable;

            Load();
        }

        public void Load()
        {
            table.Clear();

            rowRoot = table.AddTreeRow(-1, 0, "All Libraries", null, false); //TODO: Make consts
            rowSystem = table.AddTreeRow(rowRoot.ID, 1, "System Library", library.System, true);
            rowPersonal = table.AddTreeRow(rowRoot.ID, 2, "My Library", library.Personal, false);

            //            RowShared = table.AddTreeRow(rowRoot.ID, 3, "Shared Library", library.Shared, false);

            foreach (LibraryFile f in library.System.Files)
            {
                table.AddTreeRow(rowSystem.ID, 4, Path.GetFileNameWithoutExtension(f.Name), f, true);
            }

            FillNode(library.Personal, rowPersonal.ID);

            //libraryData.Personal?.Files.Where(f => !f.Hidden).Select(f =>
            //    Tree.AddTreeRow(rowPersonal.ID, 4, f.Name, f, false));

            //TODO: For now only System and Personal folders
            //if (libraryData.Shared != null)
            //{
            //    foreach (LibraryFile f in libraryData.Shared.Files)
            //    {
            //        Tree.AddTreeRow(RowShared.ID, 4, f.Name, f, false);
            //    }
            //}
            //else
            //{
            //    RowShared.Name += " (Not Configured).";
            //}
            table.AcceptChanges();
        }

        protected void FillNode(IGroup group, int parentID)
        {
            group.SubGroups.ForEach(subFolder =>
            {
                var row = table.AddTreeRow(parentID, 0, subFolder.Name, subFolder, false);
                FillNode(subFolder, row.ID);
            });

            group.Files.Where(f => !f.Hidden).ForEach(file =>
            {
                table.AddTreeRow(parentID, 4, Path.GetFileNameWithoutExtension(file.Name), file, false);
            });
        }

        public void ReloadGroupData(IGroup group)
        {
            var groupRow = table.Where(row => row.Data.Equals(group)).FirstOrDefault();

            var list = table.Where(row => row.ParentID == groupRow.ID).ToList();
            foreach (var row in list)
            {
                row.Delete();
            }

            foreach (LibraryFile f in group.Files)
            {
                table.AddTreeRow(groupRow.ID, 4, f.Name, f, false);
            }

            table.AcceptChanges();
        }

        public void UpdateShared()
        {
        //    if (RowShared == null)
        //    {
        //        RowShared = table.AddTreeRow(table[0].ID, 3, "Shared Library", library.Shared, false);
        //    }
        //    else
        //    {
        //        RowShared.Name = "Shared Library";
        //        RowShared.Data = library.Shared;
        //        library.Shared.LoadFiles(false);
        //        ReloadGroupData(library.Shared);
        //    }
        }

        public void AddFile(IFile file)
        {
            var groupRow = table.Where(row =>
                    row.Data is LibraryGroup && (row.Data as LibraryGroup).Name == file.Group.Name
                ).FirstOrDefault();

            if (groupRow != null)
            {
                table.AddTreeRow(groupRow.ID, 4, Path.GetFileNameWithoutExtension(file.Name), file, false);
            }

        }
    }
}
