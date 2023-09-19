using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinAnimator : MonoBehaviour
{

    #region UnityEvent
    // Animation End
    public UnityEvent _endAnimation;

    public UnityEvent _hideAnimation;
    #endregion

    #region Public Methods

    public void OnEndAnimation()
    {
        _endAnimation?.Invoke();
    }
    public void OnEndHideAnimation()
    {
        _hideAnimation?.Invoke();
    }
    #endregion

}
