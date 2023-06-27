using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FightSystem.CombatFlow
{
    public class DiscardCommand:ICommand
    {
        #region Members
        #region Hidden
        protected HandDisplayer _displayer;
        protected int _nbcard;
        #endregion
        #endregion

        #region Initialisation
        public DiscardCommand(HandDisplayer displayer,int nbcard)
        {
            _nbcard = nbcard;
            _displayer.DiscardCard(_nbcard);
        }
        #endregion

        #region Interface implementation

        public async void Execute()
        {

        }

        #endregion
    }
}
