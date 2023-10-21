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
    /// <summary>
    /// position
    /// </summary>
    [SerializeField]
    protected Vector2 _position;
    #endregion
    #region Hidden
    protected ExplorationEvent _event;
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

    public ExplorationEvent Event { get => _event; set => _event = value; }

    #endregion

    #region Initialisation

    protected void Awake()
    {
        _illustrationRenderer.sprite = _tile.Sprite;
    }

    #endregion
}
