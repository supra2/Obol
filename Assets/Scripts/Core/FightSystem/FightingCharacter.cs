using Core.CardSystem;
using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FightingCharacter : MonoBehaviour
{

    #region Members
    #region Visible
    #endregion
    #region Hidden
    /// <summary>
    /// Associated characters
    /// </summary>
    protected PlayableCharacter _character;
    // Hand of Cards in combat
    protected Hand<PlayerCard> _hand;
    // Deck Card not drawed
    protected PlayerCardDeck<PlayerCard> _deck;
    // Deck Card not drawed
    protected PlayerCardDeck<PlayerCard> _discard;
    /// <summary>
    /// is the fighting character currently active in fight
    /// </summary>
    protected bool _active;
    /// <summary>
    /// Callback 
    /// </summary>
    protected UnityAction _discardCallback;
    #endregion
    #endregion

    #region Event
    [Header("Events")]
    public UnityCardEvent OnCardDrawn;

    public UnityEvent OnCardDiscarded;

    public UnityIntEvent OnCardDiscard;

    public UnityCharacterEvent OnTurnStarted;

    public UnityCharacterEvent OnTurnEnded;

    public UnityCharacterEvent Initialized;
    #endregion

    #region Getter

    public PlayableCharacter Character => _character;

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    public int Stamina
    {
        get => _character.GetCharacteristicsByName("STAMINA");
        set => _character.SetCharacteristicsByName("STAMINA", value);
    }

    public PlayerCardDeck<PlayerCard> DiscardPile => _discard;

    public PlayerCardDeck<PlayerCard> Deck => _deck;

    public Hand<PlayerCard> Hand => _hand;

    #endregion

    #region Initialisation

    public void Setup( PlayableCharacter character )
    {
        _character = character;
        _deck = new PlayerCardDeck<PlayerCard>();

        foreach (PlayerCard card in _character.CardList)
        { 
           _deck.AddTop((PlayerCard)card.Clone());
        }

        _discard = new PlayerCardDeck<PlayerCard>();
        _deck.Shuffle();
        _deck.OnDeckIsEmpty += RefillDrawpile;

        if ( _character._cardExchanged == null )
            _character._cardExchanged = new ExchangeEvent();

        _character._cardExchanged.AddListener( OnCardExchanged );

        if (_hand == null)
        {
            _hand = new Hand<PlayerCard>();
            _hand.CardPlayed.AddListener(CardPlayed);
        }
        else
        {
            _hand.Clear();
        }
        Initialized?.Invoke(_character);
    }

    #endregion

    #region Turn Management

    public void OnCardExchanged(int cardIdRemoved,int cardIdAdded)
    {
        // Gather all card matching id in hand/deck and discard
        List<PlayerCard> matchingCard = 
            Deck.Filter((x) => x.GetCardId() == cardIdRemoved);
        matchingCard.AddRange(DiscardPile.Filter((x) => x.GetCardId() == cardIdRemoved));
        matchingCard.AddRange(Hand.Filter( (x) => x.GetCardId() == cardIdRemoved) );
        PlayerCard toExchange = matchingCard[SeedManager.NextInt(0, matchingCard.Count - 1)];
        if( Hand.Contains(toExchange))
        {
            Hand.Remove(toExchange);
            Hand.Add(CardManager.Instance.Instantiate(cardIdAdded) as PlayerCard);
        }
        else if ( Deck.Contains(toExchange))
        {
           int index =  Deck.FindIndex(toExchange);
            Deck.Remove(toExchange);
            Deck.InsertAt(index, CardManager.Instance.Instantiate(cardIdAdded) as PlayerCard);
        }
        else if (DiscardPile.Contains(toExchange))
        {
            int index = DiscardPile.FindIndex(toExchange);
            DiscardPile.Remove(toExchange);
            DiscardPile.InsertAt(index, CardManager.Instance.Instantiate(cardIdAdded) as PlayerCard);
        }
    }

    public void StartTurn()
    {
        OnTurnStarted?.Invoke(Character);
        Character.ApplyAlteration(true);
    }

    public void EndTurn()
    {
        OnTurnEnded?.Invoke(Character);
        Character.ApplyAlteration(false);
    }

    #endregion

    #region Action Implementation

    public void Draw(int nbcard, Action callBack)
    {
        List<PlayerCard> drawnCard = new List<PlayerCard>();
        for (int i = 0; i < nbcard; i++)
        {
            PlayerCard drawncard = _deck.Draw();
            //_hand.Add(drawncard);
            OnCardDrawn?.Invoke(drawncard);
        }
        StartCoroutine(WaitForStartDrawing(callBack));
    }

    public IEnumerator WaitForStartDrawing(Action callBack)
    {
        HandDisplayer handDisplayer = GetComponentInChildren<HandDisplayer>();
        while (handDisplayer.Drawing)
        {
            yield return new WaitForEndOfFrame();
        }
        callBack?.Invoke();
    }

    public void Discard(int nbcard, UnityAction callback)
    {
        OnCardDiscard?.Invoke(nbcard);
        OnCardDiscarded.RemoveListener(_discardCallback);
        _discardCallback = callback;
        OnCardDiscarded.AddListener(_discardCallback);
    }

    public void Discarded(List<PlayerCard> discardedCards)
    {
        OnCardDiscarded?.Invoke();
    }

    public void RefillDrawpile(Deck<PlayerCard> deck)
    {
        int length = _discard.Count;
        for (int i = 0; i < length; i++)
        {
            PlayerCard card = _discard.Draw();
            _deck.AddTop(card);
        }
        _deck.Shuffle();
    }

    public void CardPlayed( ICard card )
    {
        OnTurnEnded?.Invoke(Character);
    }

    public void Skip()
    {
        _character.Recover();
        CombatManager.Instance.EndTurn();
    }

    #endregion

}
