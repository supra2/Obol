using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{

    #region Members
    #region Hidden
    protected List<ICombatState> _states;
    protected int nbTurn;
    protected CombatVar _vars;
    protected ICombatState currentState;
    #endregion
    #endregion

    #region public Method

    protected void Update()
    {

    }

    protected void StartCombat( CombatVar vars )
    {
         _states[0].Start(vars);
         nbTurn = 0;
         currentState = _states[0];
    }

    #endregion

}
