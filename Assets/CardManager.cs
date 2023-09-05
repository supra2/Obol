using Core.CardSystem;
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
    protected List<PlayerCard> _cardList;
    #endregion

    #region Public Methods
    public ICard Instantiate(int cardId)
    {
        PlayerCard card = _cardList.Find( (x) => x.GetCardId() == cardId);
        if( card == null)
        { throw new System.Exception("CARD ID NOT CONTAINED IN CARD MANAGER, INSTANCIATION FAILED"); }
        PlayerCard _instanceCard =  ScriptableObject.Instantiate(card);
        _instanceCard.Init();
        return _instanceCard;
    }
    #endregion

}
