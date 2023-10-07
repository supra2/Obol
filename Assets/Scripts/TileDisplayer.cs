using Core.Exploration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDisplayer : MonoBehaviour
{

    #region Members
    #region Visible
    [Header("Data")]
    [SerializeField]
    protected Tile _tile;
    [Header("Display Element")]
    [SerializeField]
    protected SpriteRenderer _illustrationRenderer;
    #endregion
    #region Hidden
    /// <summary>
    /// position
    /// </summary>
    protected Vector2 _position;
    /// <summary>
    /// Illustration
    /// </summary>
    protected Sprite _illustration;
    #endregion
    #endregion

    #region Getter

    public Tile Tile
    {
        get
        {
            return _tile;
        }
        set
        {
            _illustrationRenderer.sprite = value.Sprite;
            _tile = value;
        }

    }

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public Sprite illustration => _illustration;

    #endregion

}