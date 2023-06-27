using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.FightSystem.CombatFlow
{

    public class AdversaireTurn : ICommand
    {
        #region Members
        protected Adversaire _adversaire;
        protected int _nbturn;
        #endregion

        public AdversaireTurn(Adversaire adversaire, int nbturn)
        {
            _adversaire = adversaire;
            _nbturn = nbturn;
        }

        public void Execute()
        {
            //TODO : Implement Adversaire Turn
        }
    }
}