using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using System;

/// <summary>
/// Choice Displayer
/// </summary>
public class ChoiceDisplayer : MonoBehaviour
{

    #region members
    #region Visible
    [SerializeField]
    protected RectTransform _layout;
    [SerializeField]
    protected LocalizeStringEvent _textKey;
    [SerializeField]
    protected RectTransform _prefab;
    #endregion
    #region Hidden
    protected List<Choice> _choices;
    #endregion
    #endregion

    #region Public Method

    public void InitChoice(string textKey)
    {
        _textKey.SetEntry(textKey);
        if(_choices !=null)
        {
            foreach(Choice choice in _choices)
            {
                GameObject.DestroyImmediate(choice.gameObject);
            }
        }
        _choices = new List<Choice>();
    }
    
    public void AddChoice(string key , Action action , Sprite illustrationSprite , bool active)
    {
        RectTransform rt = GameObject.Instantiate(_prefab, _layout);
        Choice choice = rt.GetComponent<Choice>();
        choice.Illustration = illustrationSprite;
        choice.StringLocalization = key;
        choice.Callback = () => action();
        choice.SetActive(active);
        choice.gameObject.SetActive(true);
        _choices.Add(choice);
    }

    public void Show( bool show )
    {
        gameObject.SetActive(show);
    }
   
    #endregion


}
