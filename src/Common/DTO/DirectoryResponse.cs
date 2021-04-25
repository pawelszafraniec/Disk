using System.Collections.Generic;

namespace Disk.Common.DTO
{
    public class DirectoryResponse : DirectoryBaseResponse
    {
        public List<DirectoryBaseResponse> Directories { get; set; } = null!;

        public List<FileResponse> Files { get; set; } = null!;

        public bool InvalidQuery { get; set; }
    }
}
