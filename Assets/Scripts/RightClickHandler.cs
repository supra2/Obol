using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RightClickHandler : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    #region Members
    [SerializeField]
    protected float _timeRightClickMobile=2.0F;

    protected float timeTouched;

    protected bool touching;
    #endregion

    #region Event
    public UnityEvent RightClicked;
    #endregion


    #region Public Method
    public void OnPointerDown(PointerEventData eventData)
    {

#if UNITY_IOS || UNITY_ANDROID
        touching = true;
        timeTouched = Time.time;
#endif
    }

    public void OnPointerUp(PointerEventData eventData)
    {
#if UNITY_IOS || UNITY_ANDROID

        if (Time.time - timeTouched >_timeRightClickMobile )
            RightClicked?.Invoke();
        touching = false;
#endif
    }

    public void Update()
    {

#if !UNITY_IOS && !UNITY_ANDROID
        if (Input.GetMouseButtonDown(1))
        {
            RightClicked?.Invoke();
        }
#endif
    }

#endregion
}
