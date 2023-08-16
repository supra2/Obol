using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BaseItem",
      menuName = "Obol/Items/BaseItem",
      order = 0)]
[Serializable]
public class Item : ScriptableObject
{

    #region Members
    #region Item
    [SerializeField]
    protected int _value;
    [SerializeField]
    protected string[] _tags;
    [SerializeField]
    protected Sprite[] _illustration;
    [SerializeField]
    protected string _nameKey;
    #endregion
    #endregion
}
