using UnityEngine;

namespace ADM.Core
{
    public class TouchController : MonoBehaviour
    {
        [SerializeField]
        private bool m_DebugDragVector;

        [SerializeField]
        private TouchTarget m_Target;

        [SerializeField]
        private LayerMask m_DragContactLayer;

        [SerializeField]
        private float m_DragThreshold;

        private bool m_IsDown;
        private bool m_IsDragging;

        private Vector2 m_TouchPosition;
        private Vector3 m_WorldPosition;
        private Vector3 m_WorldDownPosition;
        private Vector3 m_DragVector;

        private Ray CastRay
            => Camera.main.ScreenPointToRay(m_TouchPosition);

        private void Awake()
            => Find.Required(out m_Target);

        private void OnEnable()
        {
            ResetControls(true);
            m_Target.TouchInteraction += OnTouchInteraction;
        }

        private void OnDisable()
            => m_Target.TouchInteraction -= OnTouchInteraction;

        private void Update()
        {
            if (!m_IsDown)
                return;

            if (Raycaster.GetWorldPoint(CastRay, m_DragContactLayer, out m_WorldPosition))
            {
                m_DragVector = m_WorldDownPosition - m_WorldPosition;

                if (!m_IsDragging && m_DragVector.magnitude >= m_DragThreshold)
                {
                    m_IsDragging = true;
                    SendTouchEvent(TouchEvent.DRAG_START);
                }

                if (m_IsDragging)
                    SendTouchEvent(TouchEvent.DRAG);

                if (m_DebugDragVector)
                    Debug.DrawRay(m_WorldDownPosition + Vector3.up * 0.01f, m_DragVector + Vector3.up * 0.01f, Color.red);
            }
        }

        private void OnTouchInteraction(ETouchInteractions interaction, Vector2 position)
        {
            m_TouchPosition = position;
            switch (interaction)
            {
                case ETouchInteractions.Up:
                    HandleUp();
                    return;

                case ETouchInteractions.Down:
                    HandleDown();
                    return;

                case ETouchInteractions.Move:
                    // Nothing required
                    return;

                case ETouchInteractions.Enter:
                case ETouchInteractions.Exit:
                    ResetControls(true);
                    return;
            }
        }

        private void HandleDown()
        {
            m_IsDown = Raycaster.GetWorldPoint(CastRay, m_DragContactLayer, out m_WorldPosition);
            if (m_IsDown)
            {
                m_WorldDownPosition = m_WorldPosition;
                SendTouchEvent(TouchEvent.DOWN);
            }
        }

        private void HandleUp()
        {
            if (m_IsDown)
            {
                if (m_IsDragging)
                    SendTouchEvent(TouchEvent.DRAG_END);

                SendTouchEvent(TouchEvent.UP);
            }
            ResetControls(false);
        }

        private void ResetControls(bool cancelTouch)
        {
            if (cancelTouch)
                SendTouchEvent(TouchEvent.CANCEL);

            m_IsDown = false;
            m_IsDragging = false;
            m_WorldPosition = Vector3.zero;
            m_WorldDownPosition = Vector3.zero;
            m_DragVector = Vector3.zero;
        }

        private void SendTouchEvent(string name)
            => EventDispatcher.Dispatch(new TouchEvent(name, 
                m_TouchPosition, 
                m_WorldPosition, 
                m_WorldDownPosition, 
                m_DragVector, 
                m_IsDragging));
    }
}
