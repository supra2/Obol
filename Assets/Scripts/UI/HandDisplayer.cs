using Core.CardSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HandDisplayer : MonoBehaviour
{

    #region members
    #region Visible
    [SerializeField]
    protected Core.FightSystem.PlayableCharacter character;
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
    /// <summary>
    /// Hand 
    /// </summary>
    protected Hand<PlayerCard> _hand;
    /// <summary>
    /// 
    /// </summary>
    protected Queue<ICard> card_ToAdd;
    /// <summary>
    /// Is currently Handling a car drawing
    /// </summary>
    protected bool drawingCard;
    /// <summary>
    /// 
    /// </summary>
    protected bool initialised;
    #endregion
    #endregion

    #region Methods
    //_______________________________________________________

    /// <summary>
    /// Initialise the Hand Displayer
    /// </summary>
    public void Init( CharacterDisplayer displayer, Hand<PlayerCard> hand )
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
        card_ToAdd = new Queue<ICard>();
        initialised = true;
    }

    //_______________________________________________________

    public void Update()
    {
        if( drawingCard != true  && initialised && 
            card_ToAdd.Count > 0 )
        {
            DrawCard( card_ToAdd.Dequeue()  );

        }
        if(_hand !=null && _hand.IsDirty)
        {
            RefreshHandDisplay();
        }
    }

    //_______________________________________________________
    /// <summary>
    /// Add Card display in hand
    /// </summary>
    /// <param name="cardDisplayer"></param>
    public async void RefreshHandDisplay(  )
    {
        if(_cardDisplayers.Count >0 )
        { 
            RectTransform rt = transform as RectTransform;

            int nbCardInHand = _cardDisplayers.Count;
            _layoutElement.minWidth = (nbCardInHand) *
            (31 + 5) - 5;

            Canvas.ForceUpdateCanvases();

            List<Task> tasks = new List<Task>();

            Vector3 position = 
                -new Vector3(0f, 0f, 0f) / 2f;
            for (int i = 0; i < nbCardInHand; i++)
            {
               tasks.Add( _cardDisplayers[i].
                   Replace( position  , Quaternion.identity , 0.1f ) );
                position += 
                    new Vector3( 31 + 5 , 0f , 0f );
            }
            await Task.WhenAll(tasks.ToArray()).
            ContinueWith((X)=>
            {
                    
                drawingCard = true;
                _hand.IsDirty = false;
            });

        }
    }

    //_______________________________________________________

    /// <summary>
    /// Discard i card from hand
    /// </summary>
    /// <param name="nbcard">number of card to select </param>
    public void DiscardCard(int nbcard, Action<List<ICard>> Discarded   )
    {
        //first of all check if the nbcard not superior to the total of 
        //card in hands; elsewhere the discard autoresolve by emptying hand automtically
        // Todo: UI INSTRUCTION DISCLAIMING THE REMAINING CARD TO PICK
        _selection = new CardDisplayer[nbcard];
        _selectionSize = nbcard;
        foreach ( CardDisplayer cd in _cardDisplayers )
        {
            cd.ChangeMode( CardDisplayer.CardMode.Pickable );
            cd.CardPicked.AddListener( SelectCard );
            cd.CardUnpicked.AddListener( DeselectCard );
        }
    }

    //_______________________________________________________


    /// <summary>
    /// Call Back when Picking a card from hand
    /// </summary>
    /// <param name="card"></param>
    public void SelectCard(ICard card)
    {
        int index = _cardDisplayers.FindIndex( x => x.Card == card );
        if (index != -1 && _cardSelected < _selectionSize)
        {
            _selection[_selectionSize] = _cardDisplayers[index];
            _cardSelected++;
            if( _cardSelected >= _selectionSize )
            {
                List<PlayerCard> cards = new List<PlayerCard>();
                foreach( CardDisplayer displayer in _cardDisplayers)
                {
                    cards.Add((PlayerCard)displayer.Card);
                    displayer.CardPicked.RemoveListener(SelectCard);
                    displayer.CardUnpicked.RemoveListener(DeselectCard);
                }
                _characterDisplayer.Discarded(cards);
            }
        }
    }

    //_______________________________________________________

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

    public void PlayCardMode()
    {
        foreach (CardDisplayer cd in _cardDisplayers)
        {
            cd.ChangeMode( CardDisplayer.CardMode.Playable );
        }
    }

    //_______________________________________________________

    public void StopPlayCardMode( )
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
        card_ToAdd.Enqueue(card);
    }

    //_______________________________________________________

    public void DrawCard(ICard card)
    {
        CardDisplayer cardInstance = GameObject.Instantiate(
            _cardDisplayerPrefab , transform )
            .GetComponent<CardDisplayer>();
        cardInstance.Card = card;
        cardInstance.PlayDrawAnimation(AddCard);
    }

    //_______________________________________________________

    public void AddCard( CardDisplayer cardDrawnDisplayer )
    {
        _cardDisplayers.Add(cardDrawnDisplayer);
        cardDrawnDisplayer.ChangeMode(
            CardDisplayer.CardMode.Display_Hand);
        _hand.Add((PlayerCard)cardDrawnDisplayer.Card);
        drawingCard = false;
    }

    //_______________________________________________________
    #endregion

}
