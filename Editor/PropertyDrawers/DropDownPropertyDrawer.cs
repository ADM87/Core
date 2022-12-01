using UnityEditor;
using UnityEngine;

namespace ADM.Core.Editor
{
    [CustomPropertyDrawer(typeof(DropDownAttribute))]
    public class DropDownPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var options = (attribute as DropDownAttribute).Options;
            var index = EditorGUI.Popup(position, label.text, Mathf.Clamp(ArrayUtility.IndexOf(options, property.stringValue), 0, options.Length), options);
            property.stringValue = options[index];
        }
    }
}
