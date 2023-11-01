using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Members
    #region Visible

    #endregion
    #region Hidden
    protected int _currentTime;

    #endregion
    #endregion

    #region Event
    public UnityIntEvent OnTimePassed;
    #endregion 

    public void Init()
    {
        _currentTime = 0;
    }

    public void PassTime(int timeElapsed)
    {
        _currentTime += timeElapsed;
        _currentTime %= 120;
    }

    public bool IsDay()
    {
        return _currentTime < 60;
    }
}
