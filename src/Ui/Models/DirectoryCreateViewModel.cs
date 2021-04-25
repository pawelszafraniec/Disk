using Disk.Common.Validation;

using System.ComponentModel.DataAnnotations;

namespace Disk.Ui.Models
{
    public class DirectoryCreateViewModel
    {
        [Display(Name = "Directory name")]
        [Required(ErrorMessage = "{0} is required")]
        [MinLength(1, ErrorMessage = "{0} is required")]
        [MaxLength(64, ErrorMessage = "{0} cannot be longer than 64 characters")]
        [DirectoryName]
        [NotMatches(@"^\.\.?$", ErrorMessage = "{0} is invalid directory name")]
        public string? Name { get; set; }
    }
}
