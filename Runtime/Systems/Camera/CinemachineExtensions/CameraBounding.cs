using Cinemachine;
using UnityEngine;

namespace ADM.Core
{
    public class CameraBounding : BaseCinemachineExtension
    {
        [SerializeField]
        private BoxCollider m_Bounds;

        public override void Activate()
            => Assert.NotNull(m_Bounds, "Must assign a box collider reference for camera bounding");

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage != CinemachineCore.Stage.Finalize || m_Bounds == null)
                return;

            var min = m_Bounds.bounds.min;
            var max = m_Bounds.bounds.max;

            var currentPosition = vcam.transform.position;
            currentPosition.x = Mathf.Clamp(currentPosition.x, min.x, max.x);
            currentPosition.z = Mathf.Clamp(currentPosition.z, min.z, max.z);
            vcam.transform.localPosition = currentPosition;
        }
    }
}
