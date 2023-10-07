using Core.FightSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Party
{

    #region Hidden
    #region Visible
    [SerializeField]
    protected List<PlayableCharacter> _characterParty;
    [SerializeField]
    protected int _obolNumber;
    [SerializeField]
    protected List<Core.Items.Item> _inventory;
    [SerializeField]
    protected int _inventorySize = 5;
    [SerializeField]
    protected int _foodlevel = 100;
    #endregion
    #endregion

    #region Getter

    public List<PlayableCharacter> CharacterParty
    {
        get => _characterParty;
        set => _characterParty = value;
    }

    public int ObolNumber => _obolNumber;

    public List<Core.Items.Item> Inventory => _inventory;

    public int InventorySize => _inventorySize;

    public int FoodLevel => _foodlevel;

    #endregion

    #region Members

    public  void ChangeFoodLevel( )
    {
        _foodlevel = (int) Mathf.Clamp(_foodlevel - _characterParty.Count , 0f , 100f );
    }

    #endregion
}

