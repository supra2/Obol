using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{

    public delegate void LifeChanged(int life);

    public interface IMortal
    {


        public void Attack(int degat);

        public bool IsDead();

    }
}
