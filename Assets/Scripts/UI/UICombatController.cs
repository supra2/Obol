using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityIntEvent:UnityEvent<int>
{

}

public class UICombatController : Singleton<UICombatController>
{

    #region Members
    [Header("PlayerSubUIScreen")]
    [SerializeField]
    protected RectTransform ChoiceUI;
    #endregion


    #region Event
    public UnityIntEvent onChoiceSelected;
    #endregion

    public void Init(CombatVar vars)
    {

    }

    #region Player Specific UI 

    public void DisplayChoice( Core.FightSystem.AttackSystem.IEffect Choice ,
        Action action1 , Action action2   )
    {


    }

    #endregion
}
