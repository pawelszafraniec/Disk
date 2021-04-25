using System;

namespace Disk.Common.DTO
{
    public class FileResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public long Size { get; set; }

        public FileResponse()
        {

        }

        public FileResponse(Guid id, string name, long size)
        {
            this.Id = id;
            this.Name = name;
            this.Size = size;
        }
    }
}