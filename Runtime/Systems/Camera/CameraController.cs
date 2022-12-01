using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ADM.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CameraConfiguration[] m_Configurations;
        private Dictionary<CameraConfiguration.EKeys, CameraConfiguration> m_ConfigurationMap;

        private CinemachineBrain m_Brain;
        private CameraConfiguration.EKeys m_Current;

        private void Awake()
        {
            Find.Required(out m_Brain);

            m_ConfigurationMap = new Dictionary<CameraConfiguration.EKeys, CameraConfiguration>();
            foreach (var config in m_Configurations)
            {
                DeactivateCustomCinemachineExtensions(config);
                config.VCam.Priority = 0;

                m_ConfigurationMap.Add(config.Key, config);
            }
        }

        private void ApplySettings(CameraConfiguration current, CameraConfiguration next, Transform followTarget = null)
        {
            if (current != null)
            {
                if (next.Settings.HasFlag(CameraConfiguration.ESettings.MaintainPositionOnConfigurationChange))
                    next.VCam.ForceCameraPosition(current.VCam.transform.position, next.VCam.transform.rotation);
            }

            if (next.Settings.HasFlag(CameraConfiguration.ESettings.RequiresAFollowTarget))
            {
                Assert.NotNull(followTarget, $"Camera configuration {next.Key} is set to require a follow target. Reference cannot be null.");
                next.VCam.Follow = followTarget;
            }
        }

        private void SwitchConfigurations(CameraConfiguration current, CameraConfiguration next)
        {
            if (current != null)
            {
                DeactivateCustomCinemachineExtensions(current);
                current.VCam.Priority = 0;
            }

            ActivateCustomCinemachineExtensions(next);
            next.VCam.Priority = 1;
        }

        private void ActivateCustomCinemachineExtensions(CameraConfiguration configuration)
            => configuration.CustomCinemachineExtensions.ForEach(ext => ext.Activate());

        private void DeactivateCustomCinemachineExtensions(CameraConfiguration configuration)
            => configuration.CustomCinemachineExtensions.ForEach(ext => ext.Deactivate());

        public void SetConfiguration(CameraConfiguration.EKeys key, Transform followTarget = null, Action transitionComplete = null)
        {
            if (m_Current == key)
                return;

            m_ConfigurationMap.TryGetValue(m_Current, out var current);
            m_ConfigurationMap.TryGetValue(key, out var next);

            Assert.NotNull(next, $"Cannot find camera configuration for {key}");
            Assert.NotNull(next.VCam, $"Camera configuration {key} is missing a virtual camera reference");

            ApplySettings(current, next, followTarget);
            SwitchConfigurations(current, next);

            m_Current = key;
        }
    }
}
