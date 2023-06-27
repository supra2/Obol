using Core.FightSystem.CombatFlow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class FightStack : MonoBehaviour
{

    #region Members
    /// <summary>
    /// Stack: command
    /// </summary>
    protected Stack<ICommand> _commandStack;
    /// <summary>
    /// Command Execution Thread
    /// </summary>
    protected Thread _commandThread ;
    #endregion

    #region Initialisation
    public void Start()
    {
        _commandStack = new Stack<ICommand>();
        _commandThread = new Thread(new ThreadStart(Run)); ;
    }

    #endregion

    #region Public Methods

    public void Pile(ICommand command)
    {
        _commandStack.Push(command);
        if(_commandStack.Count==1)
        {
            DepileAction();
        }
    }

    public void DepileAction( )
    {
       ICommand command = _commandStack.Pop();
        command.Execute();
    }


    public void Run()
    {
        while (_commandStack.Count == 0)
        {
            Thread.Sleep(10);
        };
        DepileAction();
    }
    #endregion

}
