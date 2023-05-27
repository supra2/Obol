using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map Cells 
/// </summary>
public class MapCells : MonoBehaviour
{
    #region Members
    [SerializeField]
    protected Passability passability;
    [SerializeField]
    protected int _cellID;
    #endregion

    #region Enum
    [Flags]
    public enum Passability
    {
        None = 0,
        Right = 1 << 0,
        Left = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3,
        All = Right| Left| Up | Down
    }
    #endregion

    #region Getters
    public int CellID => _cellID;
    #endregion
}
