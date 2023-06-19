using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatVar : MonoBehaviour
{

    public enum Initiative
    {
        Normal,
        Opportunity,
        Surprised
    }

    #region Members
    [SerializeField]
    protected int _lightValue;
    [SerializeField]
    protected Initiative _initiative;
    [SerializeField]
    protected List<Adversaire> _adversaires;
    [SerializeField]
    protected List<Characters> _party;
    #endregion

    #region Getters
    /// <summary>
    /// Getter: illumination Value for the fight 
    /// </summary>
    public int LightValue => _lightValue;
    /// <summary>
    /// Getter for Initiative type
    /// </summary>
    public Initiative  FightInitiative => _initiative;
    /// <summary>
    /// Adversaires
    /// </summary>
    public List<Adversaire> Adversaires => _adversaires;
    /// <summary>
    /// Heroes party
    /// </summary>
    public List<Characters> Party => _party;   
    #endregion

}
