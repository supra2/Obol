using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:UnityEngine.Object
{

    #region Members
    #region Hidden
    protected static T _instance;
    #endregion
    #endregion

    #region Getter

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    #endregion

}
