using System.Collections.Generic;


namespace ShapesLibrary
{
    public interface ILibrary
    {
        IGroup System { get; }

        IGroup Personal { get; }

        IGroup Shared { get; }

        void Load();

        IEnumerable<IFileItem> GetAllItems();

        void ConfigureSharedGroup(string groupName);

        bool GetFilesListNeedsUpdateStatus();

        void UpdateFilesList();

    }


}
