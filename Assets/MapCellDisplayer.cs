using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapCellDisplayer : MonoBehaviour,IDisplayable
{
    #region Members
    /// <summary>
    /// map cells
    /// </summary>
    protected MapCells _mapCells;
    /// <summary>
    /// Sprite Renderer
    /// </summary>
    protected SpriteRenderer _spriteRenderer;
    #endregion

    #region Interface Implementation
    public void Display()
    {
        if(_spriteRenderer == null)
        _spriteRenderer = GetComponent<SpriteRenderer>()

        spriteRenderer = _mapCells.CellID
    }
    #endregion
}
