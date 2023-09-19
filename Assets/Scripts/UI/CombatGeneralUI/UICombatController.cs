using Core.CardSystem;
using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    protected AdversaireLayout _adversaireLayout;
    [SerializeField]
    protected Button _fleeButton;
    [SerializeField]
    protected FleeWindow _fleeWindow;
    [SerializeField]
    protected HeroesLayout _heroesLayout;
    #endregion

    #region Event
    public UnityIntEvent onChoiceSelected;
    #endregion

    public void Init( CombatVar vars )
    {
        _fleeButton.onClick.AddListener( ( )=> _fleeWindow.RollFlee( vars ) );
    }

    #region PlayerSpecific UI 

    public void DisplayChoice( Core.FightSystem.AttackSystem.ChoiceCard Choicecards )
    {
        _choiceUI.InitChoice( Choicecards.DescriptionKey() );
        foreach ( PlayerCard playerCard in Choicecards.Cards )
        {
            _choiceUI.AddChoice( playerCard.TitleKey(), playerCard.Play, 
                playerCard.GetIllustration(),playerCard.CanPayStaminaCost() );
        }
        _choiceUI.Show(true);
    }

    public void DisplayChoice(string ChoicePromptKey,string keychoice1,string keychoice2,
        Sprite Illustration1,Sprite Illustration2,Action action1,Action action2)
    {
        _choiceUI.InitChoice(ChoicePromptKey);
        for(int i = 0 ; i<2 ; i++ )
        {
            if(i ==1)
            {
                _choiceUI.AddChoice(keychoice1, action1, Illustration1,true);
            }
            else
            {
                _choiceUI.AddChoice(keychoice2, action2, Illustration2, true);
            }
        }

        _choiceUI.Show(true);
    }
    public void DisplayChoice()
    {


    }

    public void SelectAdversaire( Action<ITargetable> callback )
    {
        _adversaireLayout.SelectAdversaireMode( callback   );
    }

    public void CancelAttack()
    {
        
    }

    #endregion
}
