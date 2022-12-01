using UnityEngine;
using UnityEngine.EventSystems;

namespace ADM.Core
{
    public class TouchTarget : MonoBehaviour, 
        IPointerUpHandler,
        IPointerDownHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerMoveHandler
    {
        internal delegate void TouchHandler(ETouchInteractions interaction, Vector2 position);
        internal event TouchHandler TouchInteraction;

        public void OnPointerUp(PointerEventData data) 
            => TouchInteraction?.Invoke(ETouchInteractions.Up, data.position);
        public void OnPointerDown(PointerEventData data)
            => TouchInteraction?.Invoke(ETouchInteractions.Down, data.position);
        public void OnPointerEnter(PointerEventData data)
            => TouchInteraction?.Invoke(ETouchInteractions.Enter, data.position);
        public void OnPointerExit(PointerEventData data)
            => TouchInteraction?.Invoke(ETouchInteractions.Exit, data.position);
        public void OnPointerMove(PointerEventData data)
            => TouchInteraction?.Invoke(ETouchInteractions.Move, data.position);
    }
}
