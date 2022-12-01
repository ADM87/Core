using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ADM.Core
{
    [Serializable]
    public class CameraConfiguration
    {
        public enum EKeys
        {
            None,
            FreeRoam,
            BuildModeRoam
        }

        [Flags]
        public enum ESettings
        {
            None = 0,
            MaintainPositionOnConfigurationChange = 1 << 0,
            RequiresAFollowTarget = 1 << 1
        }

        [SerializeField]
        private EKeys m_Key;
        public EKeys Key => m_Key;

        [SerializeField]
        private ESettings m_Settings;
        public ESettings Settings => m_Settings;

        [SerializeField]
        private CinemachineVirtualCamera m_VCam;
        public CinemachineVirtualCamera VCam => m_VCam;

        private IEnumerable<BaseCinemachineExtension> m_CustomCinemachineExtensions;
        public IEnumerable<BaseCinemachineExtension> CustomCinemachineExtensions
        {
            get
            {
                if (m_CustomCinemachineExtensions == null)
                    m_CustomCinemachineExtensions = m_VCam.GetComponents<BaseCinemachineExtension>();
                return m_CustomCinemachineExtensions;
            }
        }
    }
}
