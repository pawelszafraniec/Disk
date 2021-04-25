using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Disk.Common.Validation
{
    public class DirectoryNameAttribute : ValidationAttribute
    {


        public DirectoryNameAttribute() : base()
        {

        }

        public override bool IsValid(object value)
        {
            if (value is string textValue)
            {
                IEnumerable<char> forbiddenCharacters = GetForbiddenCharacters();

                bool result = !forbiddenCharacters.Any(character => textValue.Contains(character));
                return result;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            IEnumerable<char> characters = GetForbiddenCharacters()
                .Where(character => !char.IsControl(character));

            return $"{name} cannot contain any of the following characters: {string.Join(' ', characters)}";
        }

        private static IEnumerable<char> GetForbiddenCharacters()
        {
            return Path.GetInvalidPathChars()
                .Concat(new char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' })
                .Distinct();
        }
    }
}
