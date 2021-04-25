using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Disk.Api.Data
{
    [Table("directory")]
    public class Directory
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("parent_id")]
        public Guid? ParentID { get; set; }

        [Column("is_root")]
        public bool IsRoot { get; set; }

    }
}
