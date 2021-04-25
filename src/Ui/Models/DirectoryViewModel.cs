using Disk.Common.DTO;

using System;
using System.Collections.Generic;

namespace Disk.Ui.Models
{
    public class DirectoryViewModel
    {
        public bool IncludeSubdirectories { get; }

        public string Query { get; }

        public bool InvalidQuery { get; }

        public Guid DirectoryId { get; }

        public List<DirectoryBaseResponse> Directories { get; }
        public List<FileResponse> Files { get; }

        public DirectoryViewModel(bool? includeSubdirectories,
                                  string? query,
                                  List<DirectoryBaseResponse> directories,
                                  List<FileResponse> files,
                                  bool invalidQuery,
                                  Guid directoryId)
        {
            this.IncludeSubdirectories = includeSubdirectories ?? false;
            this.Query = query ?? "";
            this.Directories = directories;
            this.Files = files;
            this.InvalidQuery = invalidQuery;
            this.DirectoryId = directoryId;
        }
    }
}