using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionList : MonoBehaviour
{

    #region Members
    protected List<Option> _options;
    #region Visible
    [SerializeField]
    protected Option _basePrefab;
    #endregion
    #endregion

    //---------------------------------------------------

    /// <summary>
    /// Add Options
    /// </summary>
    public void AddOptions(  Action _action , string key_prefab )
    {

        Option instance =  GameObject.Instantiate(_basePrefab,transform) as Option;
        instance.Init(_action, key_prefab);
        _options.Add(instance);
    }

    //---------------------------------------------------

  

    /// <summary>
    /// Add Options
    /// </summary>
    public void RemoveOption(Action _action)
    {
        _options.RemoveAll(x => x.Action == _action);
    }

    //---------------------------------------------------


    /// <summary>
    /// toglle display
    /// </summary>
    /// <param name="show"> boolan for showing option list or hiding it</param>
    public void Show(bool show)
    {
        Graphic[] graphics = GetComponentsInChildren<Graphic>();
        foreach (Graphic g in graphics)
        {
            g.enabled = show;
        }

    }

    //---------------------------------------------------

    /// <summary>
    /// Hide Option List
    /// </summary>
    public void Hide()
    {
        Show(false);

    }
    //---------------------------------------------------
}
