using Core.FightSystem.AttackSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeywordManager : MonoBehaviour
{

    #region Members
    #region Visible
    [SerializeField]
    protected List<string> _keyword;
    #endregion
    #region Hidden
    protected List<IWordBuilder> _declaredBuilders;
    #endregion
    #endregion

    #region Initialisation

    public void Awake()
    {
        _declaredBuilders = new List<IWordBuilder>();
        foreach (string typename in  _keyword )
        {
            IWordBuilder wb = (IWordBuilder)Activator.CreateInstance(
              EffectFactory.GetEffectTypeByName(typename));
            _declaredBuilders.Add(wb);
            EffectFactory.Register(wb);
        }
    }

    #endregion

}
