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
    protected List<ICommand> _commandStack;
    /// <summary>
    /// Command Execution Thread
    /// </summary>
    protected Thread _commandThread ;
    /// <summary>
    /// Command Execution Thread
    /// </summary>
    protected ICommand _command;
    #endregion

    #region Initialisation
    public void Awake()
    {
        _commandStack = new List<ICommand>();
        _commandThread = new Thread(new ThreadStart(Run)); ;
        _commandThread.Start();
    }

    #endregion

    #region Public Methods

    public void Pile(ICommand command, bool insertIntoTurn = false)
    {
        if (insertIntoTurn)
        {
            int insertbefore = _commandStack.FindIndex(
                (x) => x is CharacterTurn|| x is AdversaireTurn);
            _commandStack.Insert(insertbefore, command);
        }
        else
        {
            _commandStack.Add(command);
        }
    }

    public void Pile(Action action,bool insertIntoTurn = false)
    {
        ActionCommand cmd = new ActionCommand(action);
        if (insertIntoTurn)
        {
            int insertbefore = _commandStack.FindIndex(
                (x) => x is CharacterTurn || x is AdversaireTurn);
            _commandStack.Insert(insertbefore, cmd);
        }
        else
        {
            _commandStack.Add(cmd);
        }
    }

    public void DepileAction( )
    {
       ICommand command = _commandStack[0];
        _commandStack.RemoveAt(0);
       command.Execute();
    }

    public void Run()
    {
        while(true)
        { 
            while ( _commandStack.Count == 0 )
            {
                Thread.Sleep(10);
            };
            if( _command == null || _command.IsCommandEnded() )
            {
                DepileAction();
            }
        }
    }

    #endregion

}
