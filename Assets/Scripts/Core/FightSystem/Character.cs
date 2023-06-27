using Core;
using Core.CardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

//TODO MOVE THEM TO A PROPER FILE
public class UnityCardEvent : UnityEvent<ICard>
{

}

public class UnityCharacterEvent : UnityEvent<Character>
{

}

[CreateAssetMenu(fileName = "PlayerCard", menuName = "Obol/Character/PlayableCharacter", order = 0)]
public class Character : ScriptableObject, IMortal,ICharacteristic,ITargetable
{

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
    protected int _life;
    /// <summary>
    /// Mental health value
    /// </summary>
    [SerializeField]
    protected int _san;
    /// <summary>
    /// Mental health MaximumValue
    /// </summary>
    [SerializeField]
    protected int _maxSan;
    [SerializeField]
    protected List<PlayerCard> _cardList;
    [SerializeField]
    protected string _characterNameKey;
    #endregion
    #region Hidden
    protected List<Tuple<string, int>> _permModifiers;
    protected List<Tuple<string, int>> _tempModifiers;
    #endregion
    #endregion

    #region Getter
    public List<PlayerCard> CardList => _cardList;
    #endregion

    #region Initialisation
    public Character()
    {
        _tempModifiers = new List<Tuple<string, int>>();
        _permModifiers = new List<Tuple<string, int>>();
    
    }
    #endregion

    #region Initialisation

    /// <summary>
    /// Init Fight
    /// </summary>
    public void Init ( )
    {
        if(_tempModifiers == null)
        { 
            _tempModifiers = new List<Tuple<string, int>>();
        }
        else
        {
            _tempModifiers.Clear();
        }
        
    }
    #endregion

    #region  Characteristics Interface
    //--------------------------------------------------------------

    /// <summary>
    /// Characteristics by NAME
    /// </summary>
    /// <param name="characName"> charac Name </param>
    /// <returns></returns>
    public int GetCharacteristicsByName(string characName)
    {   
        int carac = 0;
        switch( characName.ToUpper())
        {
            case "INTELLIGENCE":
                carac = _intelligence;
                break;
            case "STRENGTH":
                carac = _strength;
                break;
            case "CONSTITUTION":
                carac = _constitution;
                break;
            case "SPEED":
                carac = _speed;
                break;
        }
        return carac;
    }

    //--------------------------------------------------------------

    public int GetCompetenceModifierByName( string compName  )
    {
         int sum =  _tempModifiers.SkipWhile(x => x.Item1 == compName)          
        .TakeWhile(x => x.Item1 != compName) 
        .Sum(x => x.Item2);

        int sum2 = _permModifiers.SkipWhile(x => x.Item1 == compName)
        .TakeWhile(x => x.Item1 != compName)
        .Sum(x => x.Item2);
        return sum + sum2;
    }

    //--------------------------------------------------------------
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

