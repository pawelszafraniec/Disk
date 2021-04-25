using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Disk.Api.Data
{
    [Table("file")]
    public class File
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("size")]
        public int Size { get; set; }

        [Column("directory_id")]
        public Guid DirectoryId { get; set; }

    }
}
