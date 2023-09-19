using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Display Manager for Adeersaire Animator overlay dispatching Animation Events.
/// </summary>
[RequireComponent(typeof(Animator))]
public class OverlayAnimatorManager : MonoBehaviour
{

    #region Members
    public UnityEvent _DiedAnimationCompleted;
    #endregion

    #region PublicMethods

    public void DiedAnimationCompleted()
    {
        _DiedAnimationCompleted?.Invoke();
    }

    #endregion

}
