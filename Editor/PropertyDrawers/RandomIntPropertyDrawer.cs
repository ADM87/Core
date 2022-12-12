using UnityEditor;
using UnityEngine;

namespace ADM.Core.Editor
{
    [CustomPropertyDrawer(typeof(RandomIntAttribute))]
    public class RandomIntPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyRect = new Rect(position.x, position.y, position.width * 0.75f, position.height);
            var btnRect = new Rect(propertyRect.x + propertyRect.width, position.y, position.width * 0.25f, position.height);

            if (GUI.Button(btnRect, "Randomize"))
            {
                var min = (attribute as RandomIntAttribute).Min;
                var max = (attribute as RandomIntAttribute).Max;
                property.intValue = Random.Range(min, max);
            }
            EditorGUI.PropertyField(propertyRect, property, label);
        }
    }
}
