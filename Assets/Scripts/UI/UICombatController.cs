using Core.FightSystem.AttackSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class UnityIntEvent:UnityEvent<int>
{

}

public class UICombatController : Singleton<UICombatController>
{

    #region Members
    [Header("PlayerSubUIScreen")]
    [SerializeField]
    protected ChoiceDisplayer _choiceUI;
    [SerializeField]
    protected AdversaireLayout _adversaireLayout ;

    [SerializeField]
    protected HeroesLayout _heroesLayout;
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

        ChooseEffect chooseEffect = (ChooseEffect)Choice;
        _choiceUI.InitChoice(chooseEffect.TextKey, chooseEffect.ChoiceKey1, chooseEffect.ChoiceKey2, action1, action2);
        _choiceUI.gameObject.SetActive(true);
    }

    public void SelectAdversaire(Action<ITargetable> callback)
    {
        _adversaireLayout.SelectAdversaireMode(callback);
    }

    #endregion
}
