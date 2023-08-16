using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FightSystem.CombatFlow
{
    public class StartNewRound : ICommand
    {
        void ICommand.Execute()
        {
            CombatManager.Instance.StartNewRound();
        }

        bool ICommand.IsCommandEnded()
        {
            return true;
        }
    }
}

