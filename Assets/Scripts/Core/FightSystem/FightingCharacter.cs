using Core.CardSystem;
using Core.FightSystem;
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
    protected Deck<PlayerCard> _deck;
    // Deck Card not drawed
    protected Deck<PlayerCard> _discard;
    // Stamina meter
    protected int _stamina;
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
        get => _stamina;
        set => _stamina = value;
    }

    public Deck<PlayerCard> DiscardPile  => _discard;

    public Deck<PlayerCard> Deck => _deck;

    public Hand<PlayerCard> Hand => _hand;

    #endregion

    #region Initialisation


    /// <summary>
    /// Setup
    /// </summary>
    /// <param name="character"></param>
    public void Setup(PlayableCharacter character)
    {
        _character = character;
        _deck = new Deck<PlayerCard>(_character.CardList);
        _deck.Shuffle();
        if ( _hand == null )
        {
            _hand = new Hand<PlayerCard>();
        }
        else 
        { 
            _hand.Clear();
        }
    }


    #endregion

    #region Turn Management

    public void StartTurn()
    {
        OnTurnStarted?.Invoke(Character);
    }

    public void EndTurn()
    {
        OnTurnEnded?.Invoke(Character);
    }

    #endregion

    #region Action Implementation

    public void Draw( int nbcard  )
    {
        List<PlayerCard> drawnCard = new List<PlayerCard>();
        for (int i = 0; i < nbcard; i++)
        {
            PlayerCard drawncard = _deck.Draw();
            _hand.Add(drawncard);
            OnCardDrawn?.Invoke(drawncard);
        }

    }

    public void Discard( int nbcard , UnityAction callback )
    {
        OnCardDiscard?.Invoke(nbcard);
        OnCardDiscarded.RemoveListener(_discardCallback);
        _discardCallback = callback;  
        OnCardDiscarded.AddListener(_discardCallback);
    }

    public void Flee()
    {
        //TODO implement
    }

    public void Discarded ( List<PlayerCard> discardedCards)
    {
        OnCardDiscarded?.Invoke();
    }

    #endregion

}
