using UnityEngine;

namespace ADM.Core
{
    public class DropDownAttribute : PropertyAttribute
    {
        public readonly string[] Options;
        public DropDownAttribute(params string[] options)
            => Options = options;
    }
}
