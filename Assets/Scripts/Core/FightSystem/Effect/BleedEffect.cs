using Core.FightSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BleedEffect : IEffect
{

    #region Inner Class

    public class BleedBuilder : IWordBuilder
    {
        public IEffect BuildEffect(string[] words)
        {
            IEffect effect = new BleedEffect();
            effect.CreateFromLine(words);
            return effect;
        }

        public string GetKeyWord()
        {
            return "Bleed";
        }

        public bool NestedKeyword()
        {
            return false;
        }

    }
    #endregion

    #region Members
    protected int _bleedValue;
    #endregion

    /// <summary>
    /// Apply bleed effect on Target
    /// </summary>
    /// <param name="itargetable"> target for effect</param>
    public void Apply(ITargetable itargetable)
    {
        Bleed bleed = new Bleed(_bleedValue);
        itargetable.AddAlteration(AlterationType.Bleeding, bleed);
    }

    /// <summary>
    /// Create effect from text line 
    /// </summary>
    /// <param name="words"></param>
    public void CreateFromLine(string[] words)
    {
        System.Int32.TryParse(words[1], out _bleedValue);
    }

    /// <summary>
    /// Self Target
    /// </summary>
    /// <returns></returns>
    public bool SelfTarget()
    {
       return  false;
    }

 
}
