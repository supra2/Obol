using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represent a grid of cell to display 
/// + a depth representing the rendering order
/// </summary>
public class Layer : ScriptableObject
{

    #region Members
    [SerializeField]
    protected MapCells[,] _mcells;
    [SerializeField]
    protected int _depth;
    #endregion

    #region Getters
    public MapCells this[int key , int key2 ]
    {
        get => _mcells[key, key2];
        set => _mcells[key, key2] = value;
    }

    public int Width => _mcells.GetLength(0);
    public int Height => _mcells.GetLength(1);

    public int Depth=>_depth;
    #endregion
}
