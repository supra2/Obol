using Cysharp.Threading.Tasks;
using System.Collections.Generic;

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
        /// nb card to discard 
        /// </summary>
        protected int _nbcard;
        #endregion
        #endregion

        #region Initialisation

        public DiscardCommand ( HandDisplayer displayer , int nbcard )
        {
            _nbcard = nbcard;
        }

        #endregion

        #region Interface implementation

        public async UniTask Execute()
        {
            await _displayer.DiscardCard( _nbcard );
        }


        #endregion


    }
}
