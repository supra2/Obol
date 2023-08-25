using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.FightSystem.AttackSystem
{
    public interface NestedEffect : IEffect
    {
        public void SetNestedEffect(List<IEffect> listeffect);
    }

}
