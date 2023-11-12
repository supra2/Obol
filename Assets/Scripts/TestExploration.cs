using Core.Exploration;
using Core.FightSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExploration : MonoBehaviour
{

    #region Members

    #endregion

    #region Members

    public void Awake( )
    {
        GameManager.Instance.StartNewGame();
    }

    #endregion

}
