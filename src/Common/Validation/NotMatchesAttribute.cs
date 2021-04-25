using System.ComponentModel.DataAnnotations;

namespace Disk.Common.Validation
{
    public class NotMatchesAttribute : RegularExpressionAttribute
    {
        public NotMatchesAttribute(string pattern) : base(pattern)
        {
        }

        public override bool IsValid(object value)
        {
            return !base.IsValid(value);
        }
    }
}
