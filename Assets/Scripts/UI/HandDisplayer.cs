using Core.CardSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HandDisplayer : MonoBehaviour
{

    #region members
    [SerializeField]
    protected Character character;

    [SerializeField]
    protected List<CardDisplayer> _cardDisplayers;

    [SerializeField]
    protected Vector3 offset = new Vector3(0, 0.3f, 0f);

    protected CardDisplayer[] selection;

    protected int selectionSize;
    #endregion

    #region Methods
    //_______________________________________________________

    /// <summary>
    /// Initialise the Hand Displayer
    /// </summary>
    public void Init()
    {
        if (_cardDisplayers == null)
        {
            _cardDisplayers = new List<CardDisplayer>();
        }
        else
        {
            _cardDisplayers.Clear();
        }
    }

    //_______________________________________________________

    /// <summary>
    /// Add Card display in hand
    /// </summary>
    /// <param name="cardDisplayer"></param>
    public async void AddCard(CardDisplayer cardDisplayer)
    {
        //first off determine destination position

        RectTransform rt = transform as RectTransform;
        Vector3 position = rt.position;
        int nbCardInHand = _cardDisplayers.Count;
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < nbCardInHand; i++)
        {
            float angleCard = 60 - i * (120 / nbCardInHand);

            Quaternion newRotation = Quaternion.AngleAxis(angleCard, Vector3.forward);
            if (i < nbCardInHand - 1)
            {
                tasks.Add(_cardDisplayers[i].Replace(
                (_cardDisplayers[i].transform as RectTransform).position
                    = position + newRotation * offset, newRotation, 0.1f));
            }
            else
            {
                tasks.Add(cardDisplayer.Replace(
                (cardDisplayer.transform as RectTransform).position
                    = position + newRotation * offset, newRotation, 0.3f));
            }
        }
        Task.WaitAll(tasks.ToArray());
        _cardDisplayers.Add(cardDisplayer);
        cardDisplayer.ChangeMode(CardDisplayer.CardMode.Display_Hand);
    }

    //_______________________________________________________

    /// <summary>
    /// Discard i card from hand
    /// </summary>
    /// <param name="nbcard">number of card to select </param>
    public void DiscardCard(int nbcard)
    {

        //first of all check if the nbcard not superior to the total of 
        //card in hands; elsewhere the discard autoresolve by emptying hand automtically
        // Todo: UI INSTRUCTION DISCLAIMING THE REMAINING CARD TO PICK
        selection = new CardDisplayer[nbcard];
        selectionSize = 0;
        foreach ( CardDisplayer cd in _cardDisplayers )
        {
            cd.ChangeMode( CardDisplayer.CardMode.Pickable );
            cd.CardPicked.AddListener( SelectCard);
            cd.CardUnpicked.AddListener(DeselectCard);
        }

    }

    //_______________________________________________________

    public void SelectCard(ICard card)
    {
        int index = _cardDisplayers.FindIndex(x => x == card);
        if (index != -1)
        {
            selection[selectionSize] = _cardDisplayers[index];
            selectionSize++;
        }
    }

    //_______________________________________________________

    public void PlayCardMode()
    {
        foreach (CardDisplayer cd in _cardDisplayers)
        {
            cd.ChangeMode(CardDisplayer.CardMode.Playable);
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

    public void DeselectCard(ICard card)
    {
        int indextoReplace = -1;
        for( int i =0; i <selectionSize;i++)
        {
            if(indextoReplace !=-1)
            {
                selection[indextoReplace] = selection[i];
                    indextoReplace++;
            }
            if( selection[i] == card)
            {
                indextoReplace = i;
            }
        }
        selectionSize--;
    }

    //_______________________________________________________
    #endregion

}
