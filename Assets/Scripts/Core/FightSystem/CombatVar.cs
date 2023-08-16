using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatVar 
{


    #region InnerClass

    public class InitiativeSorter : IComparer<ICharacteristic>
    {
        public int Compare(ICharacteristic x, ICharacteristic y)
        {
            return y.GetCharacteristicsByName("Speed") - x.GetCharacteristicsByName("Speed");
        }
    }

    #endregion

    #region Initiative
    public enum Initiative
    {
        Normal,
        Opportunity,
        Surprised
    }
    #endregion

    #region Members
    [SerializeField]
    protected int _lightValue;
    [SerializeField]
    protected Initiative _initiative;
    [SerializeField]
    protected List<Core.FightSystem.Adversaire> _adversaires;
    [SerializeField]
    protected List<Core.FightSystem.PlayableCharacter> _party;
    [SerializeField]
    protected int  _nbturn;
    #endregion

    #region Getters
    /// <summary>
    /// Getter: illumination Value for the fight 
    /// </summary>
    public int LightValue
    {
        get => _lightValue;
        set => _lightValue = value;
    }

    /// <summary>
    /// Getter for Initiative type
    /// </summary>
    public Initiative FightInitiative
    {
        get => _initiative;
        set => _initiative = value;
    }
    /// <summary>
    /// Adversaires
    /// </summary>
    public List<Core.FightSystem.Adversaire> Adversaires {
        get => _adversaires;
        set => _adversaires = value;
    }

    /// <summary>
    /// Heroes party
    /// </summary>
    public List<Core.FightSystem.PlayableCharacter> Party
    {
        get => _party;
        set => _party = value;
    }

    /// <summary>
    /// Sorted Hero list ( by Initiative)
    /// </summary>
    public List<Core.FightSystem.PlayableCharacter> SortedParty
    {
        get
        {
            List<Core.FightSystem.PlayableCharacter> sortedlist = new List<Core.FightSystem.PlayableCharacter>(Party);
            sortedlist.Sort(new InitiativeSorter());
            return sortedlist;
        }
      
    }

    /// <summary>
    ///number of turn already spent in fight
    /// </summary>
    public int NbRound
    {
        get => _nbturn;
        set => _nbturn = value;
    }
    #endregion

    }
