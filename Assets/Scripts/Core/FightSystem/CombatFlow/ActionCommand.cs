using Core.FightSystem.CombatFlow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCommand : ICommand
{

    #region Members
    Action _action;
    bool executed = false;
    #endregion

    public ActionCommand(Action action)
    {
        _action = action;
    }

    public void Execute()
    {
        _action?.Invoke();
        executed = true;
    }

    public bool IsCommandEnded()
    {
        return executed;
    }
}
