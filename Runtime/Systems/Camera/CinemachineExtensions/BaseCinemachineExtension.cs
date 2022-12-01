using Cinemachine;

namespace ADM.Core
{
    public abstract class BaseCinemachineExtension : CinemachineExtension
    {
        public virtual void Activate() { }
        public virtual void Deactivate() { }
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime) { }
    }
}
