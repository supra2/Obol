using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FightSystem
{
    public class FightingAdversaire
    {

        #region Members
        #region Hidden
        protected Adversaire _adversaire;
        #endregion
        #endregion

        #region Members
        public void Setup(Adversaire adversaire)
        {
            _adversaire = adversaire;
            
            
        }
        #endregion

    }
}
