using Core.FightSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BleedEffect : IEffect
{

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
