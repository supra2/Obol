using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:UnityEngine.MonoBehaviour
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
                _instance = FindFirstObjectByType<T>();
            }
            if( _instance == null)
            {
                GameObject gameObject = new GameObject(typeof(T).Name);
                _instance = gameObject.AddComponent<T>();
            }
            return _instance;
        }
    }

    #endregion

}
