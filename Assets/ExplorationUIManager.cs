using Core.Exploration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplorationUIManager : MonoBehaviour
{

    #region Members
    #region Visible
    [SerializeField]
    protected GridView _gridview;
    [SerializeField]
    protected ExplorationManager _explorationManager;
    #endregion
    #region Hidden
    protected Button _upArrow;
    protected Button _downArrow;
    protected Button _rightArrow;
    protected Button _leftArrow;
    protected Label _foodLabel;
    protected Label _hour;
    protected UICardDisplayer _cardDisplayer;
    #endregion
    #endregion

    #region Event
    public UnityIntEvent _directionButtonClicked;
    #endregion

    #region Initialization

    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Initialize the character list controller
        var characterListController = new CharacterListController();
       Initialize(uiDocument.rootVisualElement);
    }

    void OnDisable()
    {

    }

    public void Initialize(VisualElement root)
    {
        _upArrow = root.Q<Button>("UpArrow");
        _downArrow = root.Q<Button>("DownArrow");
        _rightArrow = root.Q<Button>("RightArrow");
        _leftArrow = root.Q<Button>("LeftArrow");
        _foodLabel = root.Q<Label>("foodlabel");
        _cardDisplayer = new UICardDisplayer( root.Q<VisualElement>("CardUI"));
         _hour = root.Q<Label>("hour");
        _upArrow.clicked +=() => _directionButtonClicked?.Invoke(0);
         _downArrow.clicked += () => _directionButtonClicked?.Invoke(1);
        _leftArrow.clicked += () => _directionButtonClicked?.Invoke(2);
        _rightArrow.clicked += () => _directionButtonClicked?.Invoke(3);
        _explorationManager.OnPlayerMoved.AddListener( OnPlayerMoved );
        _explorationManager.GridView.OnTileDisplayerPicked.AddListener(TileDisplayerPicked);
        PartyManager.Instance.FoodChanged.AddListener(FoodChanged);
        _cardDisplayer.Hide();
    }

    #endregion

    #region UI Callback
    
    protected void OnPlayerMoved( Vector2 newPosition )
    {
        Tile tile = _gridview.GetTileDisplayer( newPosition ).Tile;
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    _upArrow.visible =
                    tile.DirectionAvailable((Tile.Direction)( 1 << i ) );
                break;
                case 1:
                    _downArrow.visible =
                    tile.DirectionAvailable((Tile.Direction)( 1 << i ));
                 break;
                case 2:
                    _leftArrow.visible =
                    tile.DirectionAvailable((Tile.Direction)( 1 << i ));
                    break;
                case 3:
                    _rightArrow.visible =
                    tile.DirectionAvailable((Tile.Direction)( 1 << i ));
                    break;
            }
        }
    }

    protected void ShowEvent(ExplorationEvent EEvent)
    {

    }

    protected void HourChanged( float newhour)
    {
        _hour.text = string.Format("{0:hhmm}",newhour);
    }

    protected void FoodChanged(int foodchanged)
    {
        Debug.Log("food changed");
        _foodLabel.text = string.Format("Food : {0} ", foodchanged);
    }

    protected void TileDisplayerPicked(TileDisplayer tiledisplayer)
    {
        _cardDisplayer.SetCard( tiledisplayer.Event );
        _cardDisplayer.Show();
    }
    #endregion

}
