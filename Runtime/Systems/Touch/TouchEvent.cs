using UnityEngine;

namespace ADM.Core
{
    public class TouchEvent : EventDispatcher.EventData
    {
        public const string DOWN = "TouchEvent.Down";
        public const string UP = "TouchEvent.Up";
        public const string DRAG_START = "TouchEvent.DragStart";
        public const string DRAG = "TouchEvent.Drag";
        public const string DRAG_END = "TouchEvent.DragEnd";
        public const string CANCEL = "TouchEvent.Cancel";

        public readonly Vector2 ScreenPosition;
        public readonly Vector3 WorldPosition;
        public readonly Vector3 WorldDownPosition;
        public readonly Vector3 DragVector;
        public readonly bool IsDragging;

        public TouchEvent(string name) 
            : base(name) { }

        public TouchEvent(
            string name,
            Vector2 screenPosition,
            Vector3 worldPosition,
            Vector3 worldDownPosition,
            Vector3 dragVector,
            bool isDragging) : 
            base(name) 
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
            WorldDownPosition = worldDownPosition;
            DragVector = dragVector;
            IsDragging = isDragging;
        }
    }
}
