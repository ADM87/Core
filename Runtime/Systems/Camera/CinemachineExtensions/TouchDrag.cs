using Cinemachine;
using System;
using UnityEngine;

namespace ADM.Core
{
    [AddComponentMenu("")]
    public class TouchDrag : BaseCinemachineExtension,
        IEventReceiver<TouchEvent>
    {
        private const float MIN_DAMPENING = 0f;
        private const float MAX_DAMPENING = 10f;

        [SerializeField]
        private CinemachineCore.Stage m_ApplyOn;

        [SerializeField, Range(MIN_DAMPENING, MAX_DAMPENING)]
        private float m_Dampening = 1f;

        private Vector3 m_Target;

        public override void Activate()
        {
            m_Target = VirtualCamera.transform.position;
            EventDispatcher.AddReceiver(this, TouchEvent.DRAG_START, TouchEvent.DRAG, TouchEvent.CANCEL);
        }

        public override void Deactivate()
            => EventDispatcher.RemoveReceiver(this);

        public void HandleEvent(in TouchEvent eventData)
        {
            switch (eventData.Name)
            {
                case TouchEvent.DRAG_START:
                case TouchEvent.DRAG:
                    m_Target = VirtualCamera.transform.position + eventData.DragVector;
                    return;

                case TouchEvent.CANCEL:
                    m_Target = VirtualCamera.transform.position;
                    return;
            }
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage != m_ApplyOn)
                return;

            var currentPosition = VirtualCamera.transform.position;
            if (currentPosition == m_Target)
                return;

            if (m_Dampening == MIN_DAMPENING)
            {
                vcam.transform.position = m_Target;
                return;
            }

            var appliedDampening = Mathf.Max(MAX_DAMPENING - m_Dampening, 1) * deltaTime;
            currentPosition.x = Mathf.Lerp(currentPosition.x, m_Target.x, appliedDampening);
            currentPosition.z = Mathf.Lerp(currentPosition.z, m_Target.z, appliedDampening);

            if (Vector3.Distance(currentPosition, m_Target) <= 0.001f)
                currentPosition = m_Target;

            VirtualCamera.transform.position = currentPosition;
        }
    }
}
