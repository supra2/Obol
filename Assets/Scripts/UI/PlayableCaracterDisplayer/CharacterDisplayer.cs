using Core.CardSystem;
using Core.FightSystem;
using Core.FightSystem.CombatFlow;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

[RequireComponent(typeof(FightingCharacter))]
public class CharacterDisplayer : MonoBehaviour
{

    #region Members
    [Header("UIElements")]
    [SerializeField]
    protected HandDisplayer _handDisplayer;
    [SerializeField]
    protected DeckDisplayer _deckDisplayer;
    [SerializeField]
    protected DeckDisplayer _discardDisplayer;
    [SerializeField]
    protected Button _skipTurn;
    [SerializeField]
    protected LocalizeStringEvent _staminaDisplay;
    [SerializeField]
    protected TextMeshProUGUI _lifeDisplayer;
    [SerializeField]
    protected TextMeshProUGUI _sanDisplayer;
    [SerializeField]
    protected Image _portraitDisplayer;
    #endregion

    #region Event

    public UnityCardEvent OnCardDiscarded;

    public UnityCardEvent OnCardPlayed;

    #endregion

    #region Initialisation

    public void Init( Deck<PlayerCard> deck, Deck<PlayerCard> discard,Hand<PlayerCard> hand)
    {
        _handDisplayer.Init( this, hand );
        _handDisplayer.OnCardPlayed.AddListener( CardPlayed );
        _deckDisplayer.Init( deck );
        _discardDisplayer.Init( discard );
    }

    public void OnEnable()
    {
        GetComponent<FightingCharacter>().OnCardDrawn.AddListener(OnCardDrawn);
        GetComponent<FightingCharacter>().OnTurnStarted.AddListener(OnStartTurn);
        GetComponent<FightingCharacter>().OnTurnEnded.AddListener(OnEndTurn);
        GetComponent<FightingCharacter>().OnCardDiscard.AddListener(OnDiscard);
        GetComponent<FightingCharacter>().Initialized.AddListener(CharacterSetup);

    }

    public void OnDisable()
    {
        GetComponent<FightingCharacter>().OnCardDrawn.RemoveListener(OnCardDrawn);
        GetComponent<FightingCharacter>().OnTurnStarted.RemoveListener(OnStartTurn);
        GetComponent<FightingCharacter>().OnTurnEnded.RemoveListener(OnEndTurn);
        GetComponent<FightingCharacter>().OnCardDiscard.RemoveListener(OnDiscard);
        GetComponent<FightingCharacter>().Initialized.RemoveListener(CharacterSetup);
    }


    #endregion

    #region Members

    /// <summary>
    /// Card Drawn handler:
    /// Handle every UI update linked to 
    /// the card draw event
    /// </summary>
    /// <param name="card">Card Drawn</param>
    protected void OnCardDrawn( ICard card )
    {
        _handDisplayer.OnCardDrawn( card );
    }

    protected void OnStartTurn( Character character )
    {
        _handDisplayer.PlayCardMode();
        _skipTurn.interactable = true;
    }

    protected void OnEndTurn( Character character )
    {
        _handDisplayer.StopPlayCardMode();
        _skipTurn.interactable = false;
    }

    protected void OnDiscard( int nbcard )
    {
        DiscardCommand discardCommand = new DiscardCommand( _handDisplayer , nbcard );
        CombatManager.Instance.CommandStack.PileOnTop( discardCommand );
    }

    protected void CardPlayed( ICard card )
    {
        _discardDisplayer.Deck.AddTop( (PlayerCard) card );
    }

    protected void OnLifeChanged( int newValue)
    {
        _lifeDisplayer.text =
           newValue.ToString();
    }

    protected void OnSanChanged(int newValue)
    {
        _sanDisplayer.text = newValue.ToString();
    }

    protected void OnStaminaChanged( int stamina )
    {
        IntVariable intvariable = new IntVariable();
        intvariable.Value = stamina;
        _staminaDisplay.StringReference["stamina"] = intvariable;
        _staminaDisplay.StringReference.RefreshString();
    }

    protected void CharacterSetup(Character character)
    {
        character.LifeChangeEvent.AddListener(OnLifeChanged);
        character.SanChangeEvent.AddListener(OnSanChanged);
        _portraitDisplayer.sprite =((PlayableCharacter)character).Portrait;
        character.StaminaChangeEvent.AddListener(OnStaminaChanged);
        OnLifeChanged(character.Life);
        OnSanChanged(character.San);
    }

    #endregion

    #region Public Method

    public void Discarded(List<PlayerCard> discardedCards)
    {
        GetComponent<FightingCharacter>().Discarded(discardedCards);
        _discardDisplayer.Deck.AddTop(discardedCards);
    }

    #endregion


}
