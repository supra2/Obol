using Core.CardSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Splines;
using Cysharp.Threading.Tasks;


public class HandDisplayer : MonoBehaviour
{

    #region members
    #region Visible
    [SerializeField]
    protected Core.FightSystem.PlayableCharacter character;
    protected UniTaskCompletionSource<List<PlayerCard>> taskCompletionSource;
    [SerializeField]
    protected List<CardDisplayer> _cardDisplayers;
    [SerializeField]
    protected LayoutElement _layoutElement;
    [SerializeField]
    protected GameObject _cardDisplayerPrefab;
    [SerializeField]
    /// <summary>
    /// Character Displayer
    /// </summary>
    protected CharacterDisplayer _characterDisplayer;
    [SerializeField]
    protected SplineContainer _spline;
    #endregion
    #region hidden
    /// <summary>
    /// Current  Selection
    /// </summary>
    protected CardDisplayer[] _selection;
    /// <summary>
    /// Selection maximum size 
    /// </summary>
    protected int _selectionSize;
    /// <summary>
    /// Card Selected
    /// </summary>
    protected int _cardSelected;
    protected int _cardName;
    /// <summary>
    /// Hand 
    /// </summary>
    protected Hand<PlayerCard> _hand;
    /// <summary>
    /// Cartes a ajouter
    /// </summary>
    protected Queue<ICard> _card_ToAdd;
    /// <summary>
    /// Is currently Handling a car drawing
    /// </summary>
    protected bool _drawingCard;
    /// <summary>
    /// Displayer: Initialised
    /// </summary>
    protected bool _initialised;
    /// <summary>
    /// Force canvas Update
    /// </summary>
    protected bool _forceCanvasUpdate;
    /// <summary>
    /// general card mode to apply on newly drawn card
    /// </summary>
    protected CardDisplayer.CardMode _handMode;
    #endregion
    #endregion

    #region Getters
    public bool Drawing => _card_ToAdd.Count > 0 || _drawingCard;
    #endregion

    #region Event
    public UnityCardEvent OnCardPlayed;
    #endregion

    #region Methods
    //_______________________________________________________

    /// <summary>
    /// Initialise the Hand Displayer
    /// </summary>
    public void Init(CharacterDisplayer displayer, Hand<PlayerCard> hand)
    {
        _characterDisplayer = displayer;
        if (_cardDisplayers == null)
        {
            _cardDisplayers = new List<CardDisplayer>();
        }
        else
        {
            _cardDisplayers.Clear();
        }
        _hand = hand;
        _handMode = CardDisplayer.CardMode.Display_Hand;
        _card_ToAdd = new Queue<ICard>();
        _initialised = true;
    }

    //_______________________________________________________

    public void Update()
    {
        if (_initialised && _drawingCard != true 
            && _initialised &&
           _card_ToAdd.Count > 0)
        {
            _drawingCard = true;
            DrawCard(_card_ToAdd.Dequeue());
        }
        if (_hand != null && _hand.IsDirty)
        {
            RefreshHandDisplay();
        }
        if (_forceCanvasUpdate)
        {
            Canvas.ForceUpdateCanvases();
            _forceCanvasUpdate = false;
        }

    }

    //_______________________________________________________

    /// <summary>
    /// Add Card display in hand
    /// </summary>
    /// <param name="cardDisplayer"></param>
    public async void RefreshHandDisplay()
    {
        if (_cardDisplayers.Count > 0)
        {
            RectTransform rt = transform as RectTransform;
            int nbCardInHand = _cardDisplayers.Count;
            List<Task> tasks = new List<Task>();
            float size = 1.0f/( nbCardInHand + 1 );
            for (int i = 0; i < nbCardInHand; i++)
            {
                float bezierPosition = size * (i + 1);
                tasks.Add( _cardDisplayers[i].
                    Replace( (Vector3)_spline.EvaluatePosition(bezierPosition),
                    Quaternion.AngleAxis( Vector3.SignedAngle( Vector3.right ,
                     _spline.EvaluateTangent( bezierPosition ) , -Vector3.up ),
                     Vector3.forward), 0.1f) );
            }
            await Task.WhenAll(tasks.ToArray()).
            ContinueWith((X) =>
            {
                _forceCanvasUpdate = true;
                _drawingCard = false;
                _hand.IsDirty = false;
            });
        }
    }

    //_______________________________________________________

    /// <summary>
    /// Discard i card from hand
    /// </summary>
    /// <param name="nbcard">number of card to select </param>
    public async UniTask DiscardCard(int nbcard)

    {
        //first of all check if the nbcard not superior to the total of 
        //card in hands; elsewhere the discard autoresolve by emptying hand automtically
        // Todo: UI INSTRUCTION DISCLAIMING THE REMAINING CARD TO PICK

        _selection = new CardDisplayer[nbcard];
        _selectionSize = nbcard;
        
        foreach (CardDisplayer cd in _cardDisplayers)
        {
            cd.ChangeMode(CardDisplayer.CardMode.Pickable,
                SelectCard, DeselectCard);
        }
       var cardListDiscarded = await taskCompletionSource.Task;


    }

    //_______________________________________________________

    /// <summary>
    /// Call Back when Picking a card from hand
    /// </summary>
    /// <param name="card"></param>
    public void SelectCard( ICard card )
    {
        int index = _cardDisplayers.FindIndex(x => x.Card == card);
        if (index != -1 && _cardSelected < _selectionSize)
        {
            _selection[_selectionSize] = _cardDisplayers[index];
            _cardSelected++;
            if ( _cardSelected >= _selectionSize )
            {
                List<PlayerCard> cards = new List<PlayerCard>();
                foreach (CardDisplayer displayer in _cardDisplayers)
                {
                    cards.Add((PlayerCard)displayer.Card);
                    displayer.CardPicked.RemoveListener(SelectCard);
                    displayer.CardUnpicked.RemoveListener(DeselectCard);
                }
                taskCompletionSource.TrySetResult(cards);
            }
        }
    }

    //_______________________________________________________

    /// <summary>
    /// Deselect CARD
    /// </summary>
    /// <param name="card"> deselected CARD</param>
    public void DeselectCard(ICard card)
    {
        int indextoReplace = -1;
        for (int i = 0; i < _selectionSize; i++)
        {
            if (indextoReplace != -1)
            {
                _selection[indextoReplace] = _selection[i];
                indextoReplace++;
            }
            if (_selection[i].Card == card)
            {
                indextoReplace = i;
            }
        }
        _cardSelected--;
    }

    //_______________________________________________________

    /// <summary>
    /// Pass Hand in play Card mode
    /// </summary>
    public void PlayCardMode()
    {
        ChangeMode(CardDisplayer.CardMode.Playable);
    }

    //_______________________________________________________

    /// <summary>
    /// Change card in hand plus future cards in a state 
    /// pass as parameter
    /// </summary>
    /// <param name="cardmode"> mode </param>
    public void ChangeMode( CardDisplayer.CardMode cardmode )
    {
        foreach ( CardDisplayer cd in _cardDisplayers )
        {
            cd.ChangeMode(cardmode);
        }
        _handMode = cardmode;
    }

    //_______________________________________________________

    public void StopPlayCardMode()
    {
        foreach (CardDisplayer cd in _cardDisplayers)
        {
            cd.ChangeMode(CardDisplayer.CardMode.Display_Hand);
        }
    }

    //_______________________________________________________

    /// <summary>
    /// Event on card Drawn
    /// </summary>
    /// <param name="card"></param>
    public void OnCardDrawn(ICard card)
    {
        _card_ToAdd.Enqueue(card);
    }

    //_______________________________________________________

    /// <summary>
    /// Draw Card 
    /// </summary>
    /// <param name="card"> card </param>
    public void DrawCard( ICard card )
    {
        CardDisplayer cardInstance = GameObject.Instantiate(
            _cardDisplayerPrefab, transform).GetComponent<CardDisplayer>();
        cardInstance.name = "card" + _cardName++;
        cardInstance.Card = card;
        cardInstance.PlayDrawAnimation( AddCard );
    }

    //_______________________________________________________

    /// <summary>
    /// Add Card in Hand
    /// </summary>
    /// <param name="cardDrawnDisplayer"></param>
    public void AddCard( CardDisplayer cardDrawnDisplayer )
    {
        cardDrawnDisplayer.ChangeMode(_handMode);
        cardDrawnDisplayer.CardPlayed.AddListener(CardPlayed);
        _hand.Add((PlayerCard)cardDrawnDisplayer.Card);
        _cardDisplayers.Add(cardDrawnDisplayer);
    }

    //_______________________________________________________

    /// <summary>
    /// Card Played
    /// </summary>
    /// <param name="card">  </param>
    public void CardPlayed( ICard card )
    {
       CardDisplayer displayer =
            _cardDisplayers.Find( (x) => (
            (PlayerCard)x.Card).GetHashCode() == 
            ((PlayerCard)card).GetHashCode());
        _cardDisplayers.Remove( displayer );

        if( card is PlayerCard )
        { 
            _hand.Play( (PlayerCard)card );
        }

        _hand.Remove( (PlayerCard)card );
        displayer.transform.SetParent(null);
        GameObject.DestroyImmediate( displayer.gameObject );

        OnCardPlayed?.Invoke( card );
    }

    //_______________________________________________________
    #endregion

}
