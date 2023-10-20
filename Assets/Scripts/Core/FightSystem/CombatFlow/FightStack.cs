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
    protected ICommand _command;
    #endregion

    #region Initialisation

    public void Awake()
    {
        _commandStack = new List<ICommand>();
        StartCoroutine(Run());
    }

    #endregion

    #region Public Methods
    //__________________________________________________________________

    public void PileOnTop(ICommand command)
    {
        _commandStack.Add(command);
    }

    //__________________________________________________________________

    /// <summary>
    /// Pile On top
    /// </summary>
    /// <param name="action"></param>
    public void PileOnTop(Action action)
    {
        ActionCommand cmd = new ActionCommand(action);
        PileOnTop(cmd);
    }

    //__________________________________________________________________

    /// <summary>
    /// Pile a command on Bottom
    /// </summary>
    /// <param name="command"></param>
    public void PileBottom( ICommand command )
    {
        if(_commandStack.Count>0)
            _commandStack.Insert(_commandStack.Count, command );
        else
            _commandStack.Add( command);
    }

    //__________________________________________________________________

    public void DepileAction( )
    {
        _command = _commandStack[0];
        _commandStack.RemoveAt(0);
        _command.Execute();
    }

    //__________________________________________________________________

    public IEnumerator Run()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            if((_command == null || _command.IsCommandEnded()) && _commandStack.Count> 0 )
            {
                DepileAction();
            }
        }
    }

    //__________________________________________________________________
    #endregion

}
