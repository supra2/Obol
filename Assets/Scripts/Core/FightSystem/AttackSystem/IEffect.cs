using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FightSystem.AttackSystem
{
    public interface IEffect
    {
        public  void CreateFromLine(string[] words);

        public void Apply(ITargetable itargetable);

    }
}
