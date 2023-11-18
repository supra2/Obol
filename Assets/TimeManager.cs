using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Members
    #region Visible
    #endregion
    #region Hidden
    protected int _currentTimeMinute;

    protected int _currentTimeHour = -1;
    #endregion
    #endregion

    #region Getter
    public int Hour
    {
        get => _currentTimeHour;
        set
        {
            if ( _currentTimeHour != value)
                OnHourChanged?.Invoke(value);
           _currentTimeHour = value;
        }
    }
    #endregion
    #region Event

    public UnityIntEvent OnHourChanged;


    #endregion 

    public void Init()
    {
        _currentTimeHour = 0;
    }


    /// <summary>
    /// Pass time 
    /// 
    /// </summary>
    /// <param name="timeElapsed"> time elapsed 1 = 5 minutes </param>
    public void PassTime(int timeElapsed)
    {
        _currentTimeMinute += timeElapsed;
        _currentTimeMinute %= 288;
        Hour = _currentTimeMinute / 12;
    }

    public void SetTime(int newHour)
    {
        Hour = newHour;
    }

    public bool IsDay()
    {
        return (_currentTimeMinute -144 )>-96 && (_currentTimeMinute - 144) < 96;
    }
}
