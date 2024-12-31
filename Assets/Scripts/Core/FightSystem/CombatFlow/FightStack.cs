using Core.FightSystem.CombatFlow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

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
        Run();
    }

    #endregion

    #region Public Methods
    //__________________________________________________________________

    /// <summary>
    /// Pile a command on top 
    /// </summary>
    /// <param name="command"></param>
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

    public async UniTask DepileAction( )
    {
        _command = _commandStack[0];
        _commandStack.RemoveAt(0);
        await _command.Execute();
    }

    //__________________________________________________________________

    public async UniTaskVoid Run()
    {
        while(true)
        {
            await UniTask.NextFrame();
            if (_command == null && _commandStack.Count> 0 )
            {
               await DepileAction();
            }
        }
    }

    //__________________________________________________________________
    #endregion

}
