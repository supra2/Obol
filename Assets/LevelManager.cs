using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    #region Members 
    /// <summary>
    /// Grid For Level
    /// </summary>
    [SerializeField]
    protected Grid _prefabGrid;
    #region Hidden
    protected List<Map> _listMap;
    protected uint loadedLevel;
    #endregion
    #endregion


    public void Awake()
    {
        _listMap = new List<Map>(Resources.LoadAll<Map>("Level"));
    }


    /// <summary>
    /// Method allowing to load a level ingame 
    /// </summary>
    /// <param name="id"> int id </param>
    public void LoadLevel( uint id )
    {
        UnloadLevel();

        Map map = _listMap.Find( (x)=> x.ID  == id);

        if( map == null )
        {
            throw new System.Exception(" No map loaded match the id : " + id);
        }
        foreach (Layer layer in map)
        {
            GameObject layerObject = GameObject.Instantiate(_prefabGrid.gameObject,this.transform);
            layerObject.name = string.Format("[Layer{0}]", layer.Depth);
            layerObject.SetActive(true);
            for (int i = 0; i < layer.Width; i++)
            {
                for (int j = 0; j < layer.Height; j++)
                {
                 Vector3 position = layerObject.GetComponent<Grid>().
                        GetCellCenterWorld(new Vector3Int(i, j, layer.Depth));
                }
            }

        }
        
    }

    /// <summary>
    /// Unload the current level
    /// </summary>
    public void UnloadLevel()
    {
       if (loadedLevel != 0 )
       {
            loadedLevel = 0;
       }
    }
}
