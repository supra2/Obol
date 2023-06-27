using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Deck<T>  where T : ICard
{

    #region members
    /// <summary>
    /// cards list
    /// </summary>
    protected List<T> _cards;
    protected int nbShuffle = 1000;
    #endregion
    public Deck()
    {
        _cards = new List<T>();
    }

    public Deck( List<T> cards)
    {
        _cards = cards;
    }

    #region methods
    public void Shuffle()
    {

        for (int i = 0; i < nbShuffle; i++)
        {

            int j = SeedManager.NextInt(i, _cards.Count); // select a random j such that i <= j < instance.Length

            // swap instance[i] and instance[j]
            T x = _cards[j];
            _cards[j] = _cards[i];
            _cards[i] = x;

        }
        
    }

    public T Draw()
    {
        T x = _cards[0];
        _cards.RemoveAt(0);
        return _cards[0];

    }
    #endregion
}
