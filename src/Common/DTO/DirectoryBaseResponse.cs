using System;

namespace Disk.Common.DTO
{
    public class DirectoryBaseResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        public DirectoryBaseResponse()
        {
            this.Name = null!;
        }

        public DirectoryBaseResponse(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
