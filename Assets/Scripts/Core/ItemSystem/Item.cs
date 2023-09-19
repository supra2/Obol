using Core.FightSystem.AttackSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Items
{ 
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
    protected Sprite _illustration;
    [SerializeField]
    protected string _nameKey;
    [SerializeField]
    protected string _descriptionKey;
    [TextArea(5, 10)]
    [SerializeField]
    protected string _stringeffect;
    #endregion
    #region Inner
    protected  List<IEffect> _effects ;
    #endregion
    #endregion

    #region Getters
    public Sprite Illustration => _illustration;
    public string TitleKey => _nameKey;
    public string DescriptionKey => _descriptionKey;
    #endregion

    #region Initialisation

    protected void Init()
    {
        _effects = EffectFactory.ParseEffect(_stringeffect);
    }

    #endregion

}

}
