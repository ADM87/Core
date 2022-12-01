using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADM.Core
{
    public class WhenAll : CustomYieldInstruction
    {
        private IEnumerable<CustomYieldInstruction> m_YieldInstructions;
        public override bool keepWaiting => m_YieldInstructions.Any(instruction => instruction.keepWaiting);
        public WhenAll(IEnumerable<CustomYieldInstruction> yieldInstructions)
            => m_YieldInstructions = yieldInstructions;
    }
}
