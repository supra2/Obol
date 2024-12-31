﻿using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FightSystem.CombatFlow
{
    public class StartNewRound : ICommand
    {
        UniTask ICommand.Execute()
        {
            return CombatManager.Instance.StartNewRound();
        }

    }
}

