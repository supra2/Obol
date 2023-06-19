using Core;
using Core.CardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerCard", menuName = "Obol/Character/PlayableCharacter", order = 0)]
public class Characters : ScriptableObject, IMortal
{
    public class UnityCardEvent:UnityEvent<ICard>
    {

    }

    #region Members
    #region Visible
    [SerializeField]
    protected int _intelligence;
    [SerializeField]
    protected int _strength;
    [SerializeField]
    protected int _constitution;
    [SerializeField]
    protected int _speed;
    [SerializeField]
    protected List<PlayerCard> _cardList;
    [SerializeField]
    protected string _characterName;
    //[SerializeField]
    //protected EvolutionTree evolutionTree;
    [SerializeField]
    protected UnityCardEvent OnCardDrawn;
    #endregion
    #region Hidden
    protected List<Tuple<int, string>> _modifiers;
    // Hand of cards in combat
    protected List<PlayerCard> _hand;
    protected Deck<PlayerCard> _deck;
    #endregion
    #endregion

    #region Initialisation

    /// <summary>
    /// Init Fight
    /// </summary>
    public void Init ( InitalState state)
    {
        if(_modifiers == null)
        { 
            _modifiers = new List<Tuple<int, string>>();
        }
        else
        {
            _modifiers.Clear();
        }
        
    }
    #endregion

    #region  member 

    public void StartTurn()
    {

    }

    public void EndTurn()
    {

    }

    public void Draw( int nbcard )
    {
        for (int i = 0; i <= nbcard; i++)
        {
            PlayerCard drawncard = _deck.Draw();
            _hand.Add(drawncard);
            OnCardDrawn?.Invoke(drawncard);
        }
    }

    #endregion

    public void Attack(int degat)
    {
        
    }

    public bool IsDead()
    {
        throw new NotImplementedException();
        return false;
    }

   
}

