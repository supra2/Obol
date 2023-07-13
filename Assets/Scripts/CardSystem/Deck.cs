using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Deck<T>  where T : ICard , ICloneable
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

    /// <summary>
    /// getter on Cards cound
    /// </summary>
    public int Count => _cards.Count;

    public Deck()
    {
        _cards = new List<T>( );
    }

    public Deck( List<T> cards )
    {
        _cards = new List<T>( cards);
    }

    #region Methods

    public void Shuffle()
    {
        if(_cards.Count >0)
        { 
            for (int i = 0; i < nbShuffle; i++)
            { 
                int j = SeedManager.NextInt(0, _cards.Count-1);
                int k = SeedManager.NextInt(0, _cards.Count-1);
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
        return x;

    }

    public void AddTop(T Card)
    {
        _cards.Add(Card);
    }

    public void AddTop( List<T> Cards )
   {
        foreach(T card in Cards)
        {
            AddTop(card);
        }
   }

    #endregion

}
