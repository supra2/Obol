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

    public bool StartTurnAlteration();

    public bool StillGoingOn();

    public AlterationEvent AlterationTriggeredEvent();

    public string GetIconPath();

    public AlterationType AlterationType();

}

public class Bleed : IAlteration
{

    #region Members
    
    protected int _bleed;

    protected AlterationEvent _alterationTriggered;

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

    public Bleed( int bleed)
    {
        _alterationTriggered = new AlterationEvent();
        _bleed = bleed;
    }

    #region Public Methods

    public void AddBleeding(int additionalBleeding)
    {
        _bleed = Mathf.Max(additionalBleeding, _bleed);
    }

    public void Apply( ITargetable targetable )
    {
        Debug.Log(" Inflict " + _bleed + " damages ");
        targetable.Inflict( DamageType.Health , _bleed );
        _bleed--;
        _alterationTriggered?.Invoke(this);
    }

    public void Merge(IAlteration alteration)
    {
        if( alteration is Bleed )
        {
            Bleed otherbleed = (Bleed)alteration;
            AddBleeding(otherbleed.BleedValue);
        }
    }

    public bool StartTurnAlteration()
    {
        return false;
    }

    public bool StillGoingOn()
    {
        return BleedValue > 0;
    }

    public AlterationEvent AlterationTriggeredEvent()
    {
        return _alterationTriggered;
    }

    public string GetIconPath()
    {
        return "UI/Icon/Beta_BleedIcon";
    }

    public AlterationType AlterationType()
    {
        return global::AlterationType.Bleeding;
    }



    #endregion

}

public enum AlterationType
{
    Bleeding
}