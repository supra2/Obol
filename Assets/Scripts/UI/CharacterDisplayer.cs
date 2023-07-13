using Core.CardSystem;
using Core.FightSystem;
using Core.FightSystem.CombatFlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    protected Button _fleeFight;
    #endregion

    #region Event
    public UnityCardEvent OnCardDiscarded;
    #endregion

    #region Initialisation

    public void Init(Deck<PlayerCard> deck, Deck<PlayerCard> discard,Hand<PlayerCard> hand)
    {
        _handDisplayer.Init(this, hand);
        _deckDisplayer.Init(deck);
        _discardDisplayer.Init(discard);
    }

    public void OnEnable()
    {
        GetComponent<FightingCharacter>().OnCardDrawn.AddListener(OnCardDrawn);
        GetComponent<FightingCharacter>().OnTurnStarted.AddListener(OnStartTurn);
        GetComponent<FightingCharacter>().OnTurnEnded.AddListener(OnEndTurn);
        GetComponent<FightingCharacter>().OnCardDiscard.AddListener(OnDiscard);
    }

    public void OnDisable()
    {
        GetComponent<FightingCharacter>().OnCardDrawn.RemoveListener(OnCardDrawn);
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
        // _handDisplayer.DiscardCard(nbcard, Discarded);
        DiscardCommand discardCommand = new DiscardCommand( _handDisplayer , nbcard );

        CombatManager.Instance.CommandStack.Pile(discardCommand, true) ;
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
