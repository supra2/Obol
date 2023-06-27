using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.FightSystem.CombatFlow
{
    public class CharacterTurn : ICommand
    {
        #region members
         protected bool turnEnded;
        /// <summary>
        /// turn Number
        /// </summary>
        protected int _nbTurn;
        /// <summary>
        /// character turn
        /// </summary>
        protected FightingCharacter _character;
        #endregion

        #region Initialisation 

        public CharacterTurn(FightingCharacter character,int nbTurn)
        {
            _character = character;
            _nbTurn = nbTurn;
        }

        #endregion
        
        #region ICommand Interface Implementation
        public void Execute()
        {
            if (_nbTurn != 0)
                _character.Draw(1);

            _character.StartTurn();
            _character.OnTurnEnded.AddListener(OnEndTurn);
            turnEnded = false;
            while (!turnEnded) ;
            RecoveryPhase();
        }

        private void RecoveryPhase()
        {
            _character.Stamina +=1;
        }

        public void OnEndTurn(Character character)
        {
            if(_character == character)
            { 
            _character.OnTurnEnded.RemoveListener(OnEndTurn);
            turnEnded = true;
            }
        }
        #endregion
    }
}
