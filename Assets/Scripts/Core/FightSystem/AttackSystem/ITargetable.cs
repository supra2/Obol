
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.FightSystem.AttackSystem 
{ 
public interface ITargetable
{
        public void Inflict(DamageType damagetype, int value);

        public void AddAlteration(AlterationType type, IAlteration value);
    }
}