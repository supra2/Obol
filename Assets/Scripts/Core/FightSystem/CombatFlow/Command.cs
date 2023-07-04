using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FightSystem.CombatFlow
{
    public interface ICommand
    {
        public void Execute();

        public bool IsCommandEnded();

    }
}
