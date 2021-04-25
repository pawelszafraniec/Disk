using System;

namespace Disk.Ui.Models
{
    public class FileUploadViewModel
    {
        public Guid DirectoryId { get; }

        public FileUploadViewModel(Guid directoryId)
        {
            this.DirectoryId = directoryId;
        }
    }
}
