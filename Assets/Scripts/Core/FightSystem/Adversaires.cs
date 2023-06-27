using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Adversaire",menuName = "Obol/Characters/Enemy",order = 1)]
public class Adversaire : ScriptableObject, IMortal , ICharacteristic, ITargetable
{
    #region Enum
    public enum EnnemyState
    {
        Neutral,
        Attacked,
        Attacking,
        Dead,
        Victorious
    }
    #endregion

    #region Members
    #region Hidden
    /// <summary>
    /// Current State
    /// </summary>
    protected EnnemyState _currentState;
    /// <summary>
    /// Current Life
    /// </summary>
    protected int _currentlife;
    protected List<Tuple<string, int>> _tempModifiers;
    #endregion
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
    protected int _lifeMax;
    [SerializeField]
    protected List<Attack> _attacks;
    [SerializeField]
    protected Sprite _illustrations;
    [SerializeField]
    protected string _name;
    #endregion
    #endregion

    #region Getters

    public int Life
    {
        get => _currentlife;
    }

    public Sprite Illustration
    {
        get => _illustrations;
    }

    public bool IsDead()
    {
        return _currentlife == 0;
    }

    #endregion

    //--------------------------------------------------------------

    public void Attack(int degat)
    {
        _currentlife = Mathf.Clamp(_lifeMax, 0, _lifeMax);
       
    }

    //--------------------------------------------------------------

    /// <summary>
    /// Init Fight
    /// </summary>
    public void Init()
    {
        if (_tempModifiers == null)
        {
            _tempModifiers = new List<Tuple<string, int>>();
        }
        else
        {
            _tempModifiers.Clear();
        }

    }

    //--------------------------------------------------------------
    public override string ToString()
    {
        return string.Format(" [ {0} PV: {1}/{2} ]", _name ,  _currentlife, _lifeMax);
    }

    //--------------------------------------------------------------

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
        switch (characName.ToUpper())
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

    public int GetCompetenceModifierByName(string compName)
    {
        return _tempModifiers.SkipWhile(x => x.Item1 == compName)
       .TakeWhile(x => x.Item1 != compName)
       .Sum(x => x.Item2);
    }

    //--------------------------------------------------------------
    #endregion

    #region  Characteristics Interface
    //--------------------------------------------------------------

    /// <summary>
    /// state name 
    /// </summary>
    /// <param name="stateName"></param>
    public void ChangeState(string stateName)
    {
        string[] eName = System.Enum.GetNames(typeof(EnnemyState)) ;
        int i = 0;
        foreach(string n  in eName)
        {
            if(n == stateName)
            {
                _currentState = (EnnemyState)i;
                break;
            }
            i++;
        }
    }

    //--------------------------------------------------------------
    #endregion
}
