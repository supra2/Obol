using Core.CardSystem;
using Core.FightSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingCharacter : MonoBehaviour
{

    #region Members
    #region Visible
    [SerializeField]
    protected HandDisplayer _displayer;
    [SerializeField]
    protected Button _skipTurn;
    [SerializeField]
    protected Button _fleeFight;
    #endregion
    #region Hidden
    protected PlayableCharacter _character;
    // Hand of cards in combat
    protected List<PlayerCard> _hand;
    // Deck ( card not drawed
    protected Deck<PlayerCard> _deck;
    // Stamina meter
    protected int _stamina;
    /// <summary>
    /// is the fighting character currently active in fight
    /// </summary>
    protected bool _active;
    #endregion
    #endregion

    #region Event
    [Header("Events")]
    public UnityCardEvent OnCardDrawn;
    public UnityCardEvent OnCardDiscard;
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
    #endregion

    #region Initialisation

    public void Awake()
    {
        _hand = new List<PlayerCard>();
        _deck = new Deck<PlayerCard>();
    }

    public void Setup(PlayableCharacter character)
    {
        _character = character;
        _deck = new Deck<PlayerCard>(_character.CardList);
        _deck.Shuffle();
        _hand.Clear();
    }

    #endregion

    #region Turn Management

    public void StartTurn()
    {
        _displayer.PlayCardMode();
        _skipTurn.interactable = true;
    }


    public void EndTurn()
    {
        _displayer.StopPlayCardMode();
        _skipTurn.interactable = false;
    }

    #endregion

    #region Action Implementation

    public void Draw(int nbcard)
    {
        for (int i = 0; i <= nbcard; i++)
        {
            PlayerCard drawncard = _deck.Draw();
            _hand.Add(drawncard);
            OnCardDrawn?.Invoke(drawncard);
        }

    }

    public void Flee()
    {
        //TODO implement
    }

    public void PlayTargetedAttack( )
    {
      //  monsters.
    }

    #endregion

}
