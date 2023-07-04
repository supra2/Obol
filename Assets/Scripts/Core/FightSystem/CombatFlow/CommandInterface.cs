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
        /// <summary>
        /// hand displayer
        /// </summary>
        protected HandDisplayer _displayer;
        /// <summary>
        /// 
        /// </summary>
        protected int _nbcard;
        protected bool _discarded;
        #endregion
        #endregion

        #region Initialisation
        public DiscardCommand(HandDisplayer displayer,int nbcard)
        {
            _nbcard = nbcard;
            _displayer.DiscardCard(_nbcard, Discarded);
        }
        #endregion

        #region Interface implementation

        public  void Execute()
        {
            _displayer.DiscardCard(_nbcard, Discarded);
        }

        public bool IsCommandEnded()
        {
            return _discarded;
        }

        #endregion

        protected void Discarded()
        {
            _discarded = true;
        }
    }
}
