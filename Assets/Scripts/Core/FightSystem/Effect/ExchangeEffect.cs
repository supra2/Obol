using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core.FightSystem.AttackSystem
{ 
    /// <summary>
    /// Exchange two car
    /// </summary>
    public class ExchangeEffect : IEffect
    {

        #region Members
        protected int idCard1;
        protected int idCard2;
        #endregion

        public ExchangeEffect(int card1,int card2)
        {
            idCard1 = card1;
            idCard2 = card2;
        }

        public void Apply(ITargetable itargetable)
        {
            if( itargetable is PlayableCharacter)
            {
                ((PlayableCharacter) itargetable).Exchange( idCard1 , idCard2 );
            }
            else
            {
                Debug.LogWarning(" ITargetabe");
            }
        }

        public void CreateFromLine(string[] words)
        {
            System.Int32.TryParse(words[1] , out idCard1);
            System.Int32.TryParse(words[2] , out idCard2);
        }

        public bool SelfTarget()
        {
            return true;
        }

    }
}
