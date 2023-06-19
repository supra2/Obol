using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedInput : MonoBehaviour
{
    #region members 
    [SerializeField]
    protected TMP_InputField _inputField;
    #endregion
    #region UI event Handler
    public void SeedInputed(string input)
    {
        if (input.Length == 8)
        {
            _inputField.targetGraphic.color = Color.green;
            SeedManager.SetSeed(input);
        }
        else
        {
            _inputField.targetGraphic.color = Color.red;
        }
    }
    #endregion
}
