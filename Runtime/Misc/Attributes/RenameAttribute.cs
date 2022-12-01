using UnityEngine;

namespace ADM.Core
{
    public class RenameAttribute : PropertyAttribute
    {
        public readonly string Name;
        public RenameAttribute(string name)
            => Name = name;
    }
}
