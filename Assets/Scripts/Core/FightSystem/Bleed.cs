using Core.FightSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlterationEvent : UnityEvent<IAlteration>
{

}

public interface IAlteration
{
    public void Apply(ITargetable targetable);
    public void Merge(IAlteration alteration);
}

public class Bleed : IAlteration
{

    #region Members
    
    protected int _bleed;
    #endregion

    #region Getter
    public int BleedValue
    {
        set
        {
            _bleed = value;
        }
        get
        {
            return _bleed ;
        }
    }
    #endregion

    #region Public Methods

    public void AddBleeding(int additionalBleeding)
    {
        _bleed = Mathf.Max(additionalBleeding, _bleed);
    }

    public void Apply( ITargetable targetable )
    {
        targetable.Inflict( DamageType.Health , _bleed );
        _bleed--;
    }

    public void Merge(IAlteration alteration)
    {
        if( alteration is Bleed )
        {
            Bleed otherbleed = (Bleed)alteration;
            AddBleeding(otherbleed.BleedValue);
        }
    }

    #endregion

}

public enum AlterationType
{
    Bleeding
}