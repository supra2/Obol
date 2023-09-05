using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    #region Enum

    public enum InputMode
    {
        KeyboardMouse,
        GameController,
        Tactile
    }

    #endregion

    #region Members



    #endregion

    #region Initialisation
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
}
