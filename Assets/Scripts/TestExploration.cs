using Core.Exploration;
using Core.FightSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExploration : MonoBehaviour
{
    #region Members
    /// <summary>
    /// Exploration manager
    /// </summary>
    [SerializeField]
    protected ExplorationManager _manager;
    /// <summary>
    /// level to explore
    /// </summary>
    [SerializeField]
    protected Level _level;
    /// <summary>
    /// Seed Manager
    /// </summary>
    [SerializeField]
    protected SeedManager _seedManager;
    /// <summary>
    /// Exploration manager
    /// </summary>
    [SerializeField]
    protected TileManager _tileManager;
    /// <summary>
    /// list 
    /// </summary>
    [SerializeField]
    protected List<PlayableCharacter> _character;
    #endregion

    #region Members

    public void Awake( )
    {
        SeedManager.GenerateRandomSeed();
        _tileManager.Init();
        PartyManager.Instance.Debug_Init(_character); 
        _manager.Init(_level);
    }

    #endregion
}
