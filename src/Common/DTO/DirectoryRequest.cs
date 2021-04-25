using Disk.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Disk.Common.DTO
{
    public class DirectoryRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(64)]
        [DirectoryName]
        [NotMatches(@"^\.\.?$")]
        public string Name { get; set; } = null!;
    }
}
