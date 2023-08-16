using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using System;
using UnityEngine.Events;

public class Choice : MonoBehaviour
{

    #region Members
    /// <summary>
    /// Image : illustration for choice
    /// </summary>
    [SerializeField]
    protected Image _image;
    /// <summary>
    /// String Event
    /// </summary>
    [SerializeField]
    protected LocalizeStringEvent _stringEvent;
    /// <summary>
    /// String Event
    /// </summary>
    [SerializeField]
    protected Button _button;

    protected UnityAction callback;
    protected bool _active;

    #endregion

    #region Getter

    public Sprite Illustration
    {
        get
        {
            return _image.sprite;
        }
        set
        {
            _image.sprite = value;
            _image.enabled = value != null;
        }
    }
    public string StringLocalization {
        get
        {
            return _stringEvent.StringReference.TableEntryReference;
        }
        set
        {
            _stringEvent.SetEntry(value);
        }
    }

    public UnityAction Callback
    {
        set
        {
            if (callback != null)
                _button.onClick.RemoveListener(callback);

            _button.onClick.AddListener(value);
            callback = value;
        }
    }

    public void SetActive(bool active)
    {
        _active = active;
        _image.color = active ? Color.white : new Color(0.45f, 0.45f, 0.45f, 0.75f);
        _button.interactable = active;
    }

    #endregion
}
