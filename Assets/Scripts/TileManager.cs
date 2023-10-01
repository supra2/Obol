using Core.Exploration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Core.Exploration.Tile;

public class TileManager : MonoBehaviour
{

    #region Members
    #region Visible
    [SerializeField]
    protected List<Tile> _tileDisplayer;
    #endregion
    #endregion

    #region  TileList
    //------------------------------------------------------

    public void Init()
    {
        foreach(Tile  t in _tileDisplayer)
        {
            t.Init();
        }
    }

    //------------------------------------------------------

    /// <summary>
    /// Return the list of Tile  Matching the direction flag
    /// </summary>
    /// <param name="Direction"> dir </param>
    /// <returns> List Tile </returns>
    public List<Tile> GetListOfTiles( List<Tuple<Direction,bool>> directionPassableList )
    {
        return _tileDisplayer.FindAll((X) =>
        {
            bool match = true;
           foreach ( Tuple<Direction, bool> tuple in directionPassableList )
           {
                match = match & X.DirectionAvailable( tuple.Item1 ) == tuple.Item2;
           }
            return match;
       });

    }

    //------------------------------------------------------
    #endregion
}
