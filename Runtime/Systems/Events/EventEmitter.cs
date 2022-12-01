using UnityEngine;

namespace ADM.Core
{
    public abstract class EventEmitter : MonoBehaviour
    {
        public abstract void Emit();
    }
}
