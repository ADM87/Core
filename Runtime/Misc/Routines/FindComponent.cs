using System;
using UnityEngine;

namespace ADM.Core
{
    public class FindComponent<TComponent> : CustomYieldInstruction
        where TComponent : Component
    {
        private TComponent m_Component;
        private Action<TComponent> m_OnFound;
        private float m_SearchIntervals;
        private float m_Elapsed;

        public override bool keepWaiting
        {
            get
            {
                m_Elapsed += Time.deltaTime;
                if (m_Elapsed < m_SearchIntervals)
                    return true;

                if ((m_Component = UnityEngine.Object.FindObjectOfType<TComponent>(true)) != null)
                    m_OnFound(m_Component);

                m_Elapsed = 0f;
                return m_Component == null;
            }
        }

        public FindComponent(Action<TComponent> onFound, float searchIntervals = 025f)
        {
            m_OnFound = onFound;
            m_SearchIntervals = searchIntervals;
            m_Elapsed = m_SearchIntervals;
        }
    }
}
