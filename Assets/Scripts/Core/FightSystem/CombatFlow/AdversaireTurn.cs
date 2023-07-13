using System;
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

        #region Interface

        public void Execute()
        {
            //TODO : Implement Adversaire Turn
            Debug.Log(" Start Adversaire turn ");
        }

        public bool IsCommandEnded()
        {
            //TODO : Over
            throw new NotImplementedException();
        }

        #endregion

    }
}