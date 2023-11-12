using Core.FightSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartyManager 
{

    #region Members
    #region Hidden
    [SerializeField]
    protected Party _party;
    [SerializeField]
    protected int _visionRange ;
    #endregion
    #endregion

    #region Getter

    public List<Core.Items.Item> Inventory => _party.Inventory;

    public int InventorySize => _party.InventorySize;

    public Party Party => _party;

    public int VisionRange => _visionRange;
    #endregion

    #region Initialisation

    public PartyManager()
    {
        _party = new Party();
        _visionRange = 1;
    }

    #endregion

    #region Method

    public void UpdateGroup(List<PlayableCharacter> playable_character)
    {
        _party.CharacterParty = playable_character;
    }

    public void Debug_Init( List<PlayableCharacter> _character )
    {
        _party = new Party();
        _party.CharacterParty = _character;
        _party.OnFoodChanged+= (X) => FoodChanged?.Invoke(X);
        _visionRange = 1;
    }

    public Character GetMainCharacter()
    {
        return _party.CharacterParty.Find( (X) => X.MainCharacter == true );
    }

    public void SetParty(Party party)
    {
        _party = party;
    }

    #endregion

    #region Event

    public UnityIntEvent FoodChanged = new UnityIntEvent();


    #endregion

}
