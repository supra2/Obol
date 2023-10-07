using Core.Exploration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{

    #region Members
    #region Visible
    [SerializeField]
    protected Grid _grid;
    [SerializeField]
    protected Camera _sceneCamera;
    [SerializeField]
    protected LayerMask _layerMask;
    [SerializeField]
    protected List<TileDisplayer> _tiles;
    #endregion
    #region Hidden
    public bool _dragged;
    public Vector3 _dragPosition;
    public Vector3 _initialdragPosition;
    #endregion
    #endregion

    #region Getters
    public List<TileDisplayer> Tiles => _tiles;
    #endregion

    #region Initialisation

    public void Awake()
    {
        _tiles = new List<TileDisplayer>( );
    }

    #endregion

    #region Public Methods
    //-----------------------------------------------------------

    /// <summary>
    /// Place a tile on the grid ( parent it , and set it inner place )
    /// </summary>
    /// <param name="tileToPlace"></param>
    /// <param name="position"></param>
    public void PlaceTile( TileDisplayer tileToPlace ,  Vector2 position  )
    {
        tileToPlace.transform.SetParent(transform);
        tileToPlace.transform.rotation = 
            Quaternion.AngleAxis(0f, new Vector3(1, 0, 0));
        tileToPlace.Position = position;
        Place( tileToPlace.transform, position, tileToPlace.Tile.RotationY );
        _tiles.Add( tileToPlace );
    }

    //-----------------------------------------------------------

    /// <summary>
    /// Placement operation for a transform on the grid 
    /// </summary>
    /// <param name="placetransform"></param>
    /// <param name="position"></param>
    /// <param name="rotationForward"></param>
    public void Place( Transform placetransform , Vector2 position,
        float rotationForward = 0f )
    {
        placetransform.position = _grid.CellToWorld(
        new Vector3Int((int)position.x, (int)position.y, 0));
        placetransform.rotation =
            Quaternion.AngleAxis(rotationForward, new Vector3(0, 0, 1));
    }

    //-----------------------------------------------------------

    public Vector3 ProjectedPosition(Vector3 clickPosition)
    {
        Vector3 hitPosition = Vector3.zero;
        clickPosition.z = _sceneCamera.nearClipPlane;
        Ray ray = _sceneCamera.ScreenPointToRay(clickPosition);
        RaycastHit hit;
        if ( Physics.Raycast(ray, out hit , 10000 , _layerMask) )
        {
            hitPosition = hit.point;
        }
        return hitPosition;
    }

    //-----------------------------------------------------------

    public Tile GetTileAtWorldPosition(  Vector2  worldPosition  )
    {
       Vector3 pos =  ProjectedPosition( worldPosition );
           Vector2 tileposition =  _grid.CellToWorld(
           new Vector3Int( (int)pos.x , (int)pos.y , 0) );
        if ( GetTileDisplayer(pos)  == null)
        {
            Debug.Log( " Tile not found " + pos );
        }
         return GetTileDisplayer( pos ).Tile;
    }

    //-----------------------------------------------------------

    public TileDisplayer GetTileDisplayer( Vector2 gridPosition )
    {
          return _tiles.Find( (X) => X.Position == gridPosition );
    }

    //-----------------------------------------------------------
    protected void Update()
    {

#if UNITY_STANDALONE

        Vector3 mousePosition = Input.mousePosition;

        Vector3 hitPosition = ProjectedPosition(mousePosition);
      
        if (Input.GetMouseButtonDown(0))
        {
            _initialdragPosition = hitPosition;
            _dragPosition = hitPosition;
            _dragged = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if( Vector3.kEpsilon < ( _initialdragPosition - hitPosition).magnitude )
            {
                Clicked(hitPosition);
            }
            _dragged = false;
            
        }
#elif UNITY_IOS || UnityANDROID
        if ( Input.touchCount > 0 )
        {
            Vector3 touch = Input.touches[0].position;
            DetectSelectedMapPosition( touch );
        }
#endif
        if ( _dragged )
        {
            GetDragValue();
        }

    }

    //-----------------------------------------------------------

    public void GetDragValue()
    {

#if UNITY_STANDALONE
     
        Vector3 drag = _dragPosition - ProjectedPosition(Input.mousePosition);
        _dragPosition = ProjectedPosition(Input.mousePosition);

#elif UNITY_IOS || UnityANDROID


#endif
        transform.position += drag;
    }

    //-----------------------------------------------------------

    public void Clicked(Vector2 position)
    {
        
    }

    //-----------------------------------------------------------
    #endregion

}
