using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// reference card for dynamic generation
/// </summary>
public class CardManager : Singleton<CardManager>
{

    #region Members
    [SerializeField]
    protected List<ICard> _cardList;
    #endregion

    public ICard Instantiate(int cardId)
    {
        return _cardList[cardId];
    }

}
