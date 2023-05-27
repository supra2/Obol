using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:UnityEngine.Object
{

    #region Members
    #region Hidden
    protected T _instance;
    #endregion
    #endregion

    #region Getter
    public T Instance
    {
        get
        {

            if (_instance ==null)
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }
    #endregion

}
