using System.Collections.Generic;

namespace Gear.Localizer
{
    public class LanguageEqualityComparer : IEqualityComparer<Language>
    {
        public bool Equals(Language x, Language y) =>
            x != null && y != null && x.Identifier.Equals(y.Identifier);

        public int GetHashCode(Language obj) =>
            obj.Identifier.GetHashCode();
    }
}