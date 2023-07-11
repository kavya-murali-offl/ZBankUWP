using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ZBank.Utilities.Helpers
{
    internal class FilesHelper
    {

        public async static Task<IReadOnlyList<StorageFile>> GetFiles()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add("*");
            return await openPicker.PickMultipleFilesAsync();
        }

        public async static Task<string> GetFileNames()
        {
            IReadOnlyList<StorageFile> files = await GetFiles();
            if (files.Count > 0)
            {
                StringBuilder output = new StringBuilder("Uploaded Files: ");
                foreach (StorageFile file in files)
                {
                    output.Append(file.Name + "\n");
                }
                return output.ToString();
            }
            return string.Empty;
        }
    }
}
