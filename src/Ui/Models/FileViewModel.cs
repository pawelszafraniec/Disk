using Disk.Common.DTO;

namespace Disk.Ui.Models
{
    public class FileViewModel
    {
        public FileResponse File { get; }

        public FileType FileType { get; set; }

        public FileViewModel(FileResponse file, FileType fileType)
        {
            this.File = file;
            this.FileType = fileType;
        }
    }
}
