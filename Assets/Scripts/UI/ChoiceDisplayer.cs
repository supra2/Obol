using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using System;

public class ChoiceDisplayer : MonoBehaviour
{

    #region members
    #region Visible
    [SerializeField]
    protected LocalizeStringEvent _textKey;
    [SerializeField]
    protected LocalizeStringEvent _button1Key;
    [SerializeField]
    protected LocalizeStringEvent _button2Key;
    #endregion
    #region Hidden
    protected Action _action1;
    protected Action _action2;
    #endregion
    #endregion

    #region Public Method

    public void InitChoice(string textKey, string choice1, string choice2, Action action1, Action Action2)
    {
        _textKey.SetEntry(textKey);
        _button1Key.SetEntry( choice1 );
        _button2Key.SetEntry( choice2 );
        _action1 = action1;
        _action2 = Action2;
    }
    
    public void ChoicePicked(int choiceID)
    {
        if(choiceID ==0 )
        {
            _action1?.Invoke();
        }
        if (choiceID == 1)
        {
            _action2?.Invoke();
        }
        gameObject.SetActive(false);
    }
          
    #endregion

    #region protected Method


    #endregion
}
