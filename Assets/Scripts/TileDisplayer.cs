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
    protected static Sprite _hiddenIllustration;
    /// <summary>
    /// Visiblity Mode
    /// </summary>
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

    public VisibilityMode Visibility
    {
        get => _visibilityMode;
        set
        {
            _visibilityMode = value;
            ShowSprite();
        }
    }

    #endregion

    #region Event
    /// <summary>
    /// TileEvent
    /// </summary>
    /// <param name="displayer"></param>
    public delegate void TileEvent(TileDisplayer displayer);

    /// <summary>
    /// Called when the tile becomeVisible
    /// </summary>
    public TileEvent TileShownEvent;

    /// <summary>
    /// Called when the tile is hidden after being visible
    /// </summary>
    public TileEvent TileHiddenEvent;
    #endregion

    #region Enum

    public enum VisibilityMode
    {
        Hidden,
        ShownUnlit,
        ShownLit,
    }

    #endregion

    #region Initialisation

    protected void Awake()
    {
        if (_hiddenIllustration == null)
        {
            _hiddenIllustration = Sprite.Instantiate(Resources.Load<Sprite>("Textures/Illustration/FoggedTile"));
        }
        ShowSprite();
    }

    #endregion

    #region Protected Method

    protected void ShowSprite()
    {
        _illustrationRenderer.sprite = _visibilityMode == VisibilityMode.ShownLit ?
            _tile.Sprite : _hiddenIllustration;
        _illustrationRenderer.gameObject.SetActive(_visibilityMode != VisibilityMode.Hidden);
    }

    protected bool Connected(TileDisplayer t)
    {
        bool common_direction = (t.Tile.DirectionFlags | Tile.DirectionFlags) !=0;
        return CloseEnough(t, 1) && common_direction;
    }

    protected bool CloseEnough(TileDisplayer t, int distance)
    {
        return ((int)(_position - t.Position).magnitude) <= distance;
    }

    #endregion

    #region Public Method

    public void SetMode(VisibilityMode mode)
    {
        _visibilityMode = mode;
    }

    public bool VisibleFrom(TileDisplayer t, int lightingdistance)
    {
        return Connected(t) && CloseEnough(t , lightingdistance);
    }

    #endregion

}
