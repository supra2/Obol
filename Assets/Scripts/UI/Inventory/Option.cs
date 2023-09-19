using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(Button))]
public class Option : MonoBehaviour,IEqualityComparer<Option>
{

    #region Members
    [SerializeField]
    protected LocalizeStringEvent _text;
    [SerializeField]
    protected Button _button;
    [SerializeField]
    protected Action _action;
    #endregion

    #region Getter
    public Action Action =>  _action;
    #endregion

    #region Initialisation

    public void Init( Action callback , string promptKey )
    {
        _button.onClick.AddListener( () => callback?.Invoke() );
        _text.SetEntry(promptKey);
        gameObject.SetActive(true);
    }

    public bool Equals(Option x, Option y)
    {
      return  x.Action == y.Action;
    }

    public int GetHashCode(Option obj)
    {
        return obj._action.GetHashCode();
    }

    #endregion


    #region Cleaning


    #endregion

}
