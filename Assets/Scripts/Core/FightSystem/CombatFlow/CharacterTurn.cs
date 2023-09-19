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
        /// <summary>
        /// Turn is Ended
        /// </summary>
        protected bool _turnEnded;
        #endregion

        #region Initialisation 

        public CharacterTurn( FightingCharacter character , int nbTurn )
        {
            _character = character;
            _nbTurn = nbTurn;
            _character.OnTurnEnded.AddListener( TurnEnded );
        }

        #endregion

        #region ICommand Interface Implementation

        public void Execute()
        {
            if ( _nbTurn != 0 )
            {
                _character.Draw( 1 , ContinueTurn );
            }
            else
            {
                ContinueTurn();
            }
        }

        protected void ContinueTurn()
        {   
            _character.StartTurn();
         
        }

        protected void TurnEnded(Character character)
        {
            _character.Character.Recover();
            _character.OnTurnEnded.RemoveListener(TurnEnded);
            _turnEnded = true;
        }

        public bool IsCommandEnded()
        {
            return _turnEnded;
        }
        
        #endregion

    }

}
