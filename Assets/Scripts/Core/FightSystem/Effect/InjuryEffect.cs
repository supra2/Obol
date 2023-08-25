using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core.FightSystem.AttackSystem
{ 
    public class InjuryEffect : IEffect
    {
        public void Apply(ITargetable itargetable)
        {
            if( itargetable is PlayableCharacter)
            {
                ((PlayableCharacter) itargetable).Exchange(CardManager.Instance.Instantiate( 0) ,
                    CardManager.Instance.Instantiate(1) );
            }
            else
            {
                Debug.LogWarning(" ITargetabe");
            }
        }

        public void CreateFromLine(string[] words)
        {
            
        }

        public bool SelfTarget()
        {
            return false;
        }

    }
}
