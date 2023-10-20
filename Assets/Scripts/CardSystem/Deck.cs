using Core.CardSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Deck<T> where T : ICard, ICloneable
{

    #region members
    /// <summary>
    /// cards list
    /// </summary>
    protected List<T> _cards;
    /// <summary>
    /// Number of shuffleing permutations
    /// </summary>
    protected int nbShuffle = 200;
    #endregion

    #region Events
    public delegate void DeckEvent(Deck<T> deck);
    public DeckEvent OnDeckIsEmpty;
    #endregion

    #region Getter 

    /// <summary>
    /// getter on Cards cound
    /// </summary>
    public int Count => _cards.Count;

    #endregion

    #region Constructors
    public Deck()
    {
        _cards = new List<T>();
    }

    public Deck(List<T> cards)
    {
        _cards = new List<T>(cards);
    }
    #endregion

    #region Public Methods

    public void Shuffle()
    {
        if (_cards.Count > 0)
        {
            for (int i = 0; i < nbShuffle; i++)
            {
                int j = SeedManager.NextInt(0, _cards.Count - 1);
                int k = SeedManager.NextInt(0, _cards.Count - 1);
                // select a random j such that 0 <= j < instance.Length
                // swap instance[i] and instance[j]
                T x = _cards[j];
                _cards[j] = _cards[k];
                _cards[k] = x;

            }
        }
    }

    public T Draw()
    {
        T x = _cards[0];
        _cards.RemoveAt(0);
        if (_cards.Count == 0)
        {
            OnDeckIsEmpty?.Invoke(this);
        }
        return x;

    }

    public void AddTop(T Card)
    {
        _cards.Add(Card);
    }

    public void AddTop(List<T> Cards)
    {
        foreach (T card in Cards)
        {
            AddTop(card);
        }
    }

    public List<T> Filter( Predicate<T> filterDelegate )
    {
        return _cards.FindAll(filterDelegate);
    }

    public T GetFirstCard(Predicate<T> filterDelegate)
    {
        int index = _cards.FindIndex(filterDelegate);
        T card = _cards[index];
        Remove(card);
        return card;
    }


    
    public bool Contains(T Card)
    {
        return _cards.Contains(Card);
    }

    public int FindIndex( T Card ) 
    {
        return _cards.FindIndex((x)=> x.Equals(Card));
    }

    public void Remove(T Card)
    {
        _cards.Remove(Card);
    }

    public void InsertAt(int index ,T Card)
    {
         _cards.Insert(index,Card);
    }
    #endregion

}


public class PlayerCardDeck<T>: Deck<T> where T:PlayerCard
{
   

    #region Members

    public List<T> FilterbyNature(PlayerCard.Nature nature , PlayerCard.Type type )
    {
       return  _cards.FindAll(x => x.CardNature == nature && x.CardType == type);
    }

    #endregion

}