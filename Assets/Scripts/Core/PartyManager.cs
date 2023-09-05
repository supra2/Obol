using Core.FightSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PartyManager : Singleton<PartyManager>
{

    #region Members
    #region Hidden
    [SerializeField]
    protected List<PlayableCharacter> _characterParty;
    [SerializeField]
    protected int _obolNumber;
    [SerializeField]
    protected List<Item> _inventory;
    #endregion
    #endregion

    #region Initialisation
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    #region Method

    public void UpdateGroup(List<PlayableCharacter> playable_character)
    {
        _characterParty = playable_character;
    }

    public void Save ( string filename )
    {
    }

    public void Load(string filename)
    {
    
    }

    #endregion
}
