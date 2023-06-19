using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cells : ScriptableObject
{

    #region Inner Type

    #endregion

    #region Members
    [Flags]
    public enum CelluleType
    {
        Simple=0x00,
        Animated = 0x01,
        Border=0x02,
        BorderAnimated= Animated & Border
    }
    /// <summary>
    /// cellule unique ID referencing it in the factory for instanciation purpose
    /// </summary>
    protected int id;
    /// <summary>
    /// inner list of sprite of a given Cell for the first frame
    /// </summary>
    [SerializeField]
    protected Sprite[] sprites_frame1;
    /// <summary>
    /// inner list of sprite of a given Cell for the second frame
    /// </summary>
    [SerializeField]
    protected Sprite[] sprites_frame2;
    /// <summary>
    /// inner list of sprite of a given Cell for the third frame
    /// </summary>
    [SerializeField]
    protected Sprite[] sprites_frame3;
    [SerializeField]
    protected CelluleType celluleType;
    #endregion

    #region Getter
    //----------------------------------------------------------

    /// <summary>
    /// get sprite 
    /// </summary>
    /// <returns></returns>
    protected Sprite GetSprite()
    {

        //Time.deltaTime 
        //return;
        throw new NotImplementedException();
    }

    //----------------------------------------------------------
    #endregion

}
