using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossEffect : IEffect
{ 

     #region Members
        protected string _competence;
        protected int _modifier;
     #endregion

    public void Apply(ITargetable itargetable)
    {
        if (itargetable is Character)
        {
            Core.FightSystem.Character charac = (Core.FightSystem.Character)itargetable;
            charac.AddTempCompetenceModifier(_competence, - _modifier);
        }
    }

    public void CreateFromLine(string[] words)
    {
        int val = 0;
        if (words.Length == 3 && int.TryParse(words[1], out val))
        {
            _competence = words[2];
            _modifier = val;
        }
        else
        {
            Debug.LogError("Incorrectly formated command");
        }
    }

    public bool SelfTarget()
    {
        return false;
    }
}
