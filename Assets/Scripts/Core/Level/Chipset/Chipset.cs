using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelsData/Chipset")]
public class Chipset : ScriptableObject
{

    #region Members
    /// <summary>
    /// cell size
    /// </summary>
    [SerializeField]
    protected int _cellSize;
    /// <summary>
    /// Cells 
    /// </summary>
    [SerializeField]
    public List<Cells> _cells;
    #endregion

    #region Getter
    public Cells this[int i]
    {
        get => _cells [i];
    }
    #endregion

    #region Getters
    ///--------------------------------------------------------



    ///--------------------------------------------------------
    #endregion

}
