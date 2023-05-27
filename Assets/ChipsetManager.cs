using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manage all the chipset in the Level 
/// </summary>
public class ChipsetManager : Singleton<ChipsetManager>
{

    #region Members

    /// <summary>
    /// Chipset: 
    /// </summary>
    protected Chipset[] _chipsets;
    /// <summary>
    /// Prefab
    /// </summary>
    protected GameObject prefab;
    #endregion

    #region Initialisation

    public void Awake()
    {
        _chipsets = Resources.LoadAll<Chipset>("Level");
        prefab = Resources.Load("Prefabs/MapCell") as GameObject;
    }

    #endregion


    #region Display methods

    public GameObject CreateCell( int chipset_id , int cell_id )
    {
        if ( _chipsets[chipset_id][cell_id] )
        {
            _chipsets.
        }
    }

    #endregion
}
