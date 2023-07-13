using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.FightSystem.AttackSystem
{
    public class GainEffect : IEffect
    {

        #region Members
        protected string _competence;
        protected int _modifier;
        #endregion

        public GainEffect(string comp,int modifier)
        {
            _competence = comp;
            _modifier = modifier;
        }

        public void Apply(ITargetable itargetable)
        {
            if(  itargetable is Character )
            {
               Core.FightSystem.Character charac = (Core.FightSystem.Character)itargetable;
                charac.AddTempCompetenceModifier(_competence, _modifier);
            }
        }

        public void CreateFromLine(string[] words)
        {
            int val = 0;
            if( words.Length == 3 && int.TryParse( words[2] , out val )  )
            {
                _competence = words[1];
                _modifier = val;
            }
            else
            {
                Debug.LogError("Incorrectyl formated command");
            }

        }

      
    }
}
