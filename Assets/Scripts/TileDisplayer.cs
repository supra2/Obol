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
    /// <summary>
    /// Illustration
    /// </summary>
    protected  static Sprite _hiddenIllustration;

    protected VisibilityMode _visibilityMode;

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

    public Sprite hiddenIllustration => _hiddenIllustration;

    public ExplorationEvent Event { get => _event; set => _event = value; }

    public VisibilityMode Visibility => _visibilityMode;
    #endregion

    #region Enum

    public enum VisibilityMode
    {
        Hidden,
        Visible,
    }

    #endregion

    #region Initialisation

    protected void Awake()
    {
        if(_hiddenIllustration == null)
        {
            _hiddenIllustration = Sprite.Instantiate(Resources.Load<Sprite>("Textures/Illustration/FoggedTile"));
        }
        ShowSprite();
    }

    protected void ShowSprite()
    {
        _illustrationRenderer.sprite = _visibilityMode == VisibilityMode.Visible ? 
            _tile.Sprite : _hiddenIllustration;
    }

    #endregion

    #region Public Method

    public void SetMode(VisibilityMode mode)
    {
        _visibilityMode = mode;
    }

    #endregion
}
