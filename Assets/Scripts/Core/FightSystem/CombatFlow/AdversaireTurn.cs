using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.FightSystem.CombatFlow
{

    public class AdversaireTurn : ICommand
    {

        #region Members
        /// <summary>
        /// Adversaire 
        /// </summary>
        protected Adversaire _adversaire;
        /// <summary>
        /// Nbturn
        /// </summary>
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
            List<Attack> AvailableAttack = new List<Attack>();
            _adversaire.ApplyAlteration(true);
            _adversaire.OnStartTurn?.Invoke(_adversaire);
            foreach ( Attack attack in _adversaire.AttackList )
            {
                if( attack.Stamina <= _adversaire.Stamina )
                {
                    AvailableAttack.Add(attack);
                }
            }

            if( AvailableAttack.Count > 0 )
            {
                int attackLaunched = SeedManager.NextInt( 0 , AvailableAttack.Count-1 );
                // Select Target
                int totalrange = 0;
                foreach(  PlayableCharacter character in CombatManager.Instance.Var.Party )
                {
                    int prob = Mathf.FloorToInt( ( 1 / 
                        character.GetCharacteristicsByName( "Speed") + 
                        character.GetCompetenceModifierByName("Distance")  ) * 100 );
                    totalrange += prob;
                }
                int Adversairepick = SeedManager.NextInt( 0 , totalrange );
                int pointeur = 0;
                PlayableCharacter pickedCharacter = new PlayableCharacter();
                foreach (PlayableCharacter character in 
                    CombatManager.Instance.Var.Party)
                {
                    pointeur += Mathf.FloorToInt((1 /
                    character.GetCharacteristicsByName("Speed") +
                        character.GetCompetenceModifierByName("Distance")) * 100);
                    if ( pointeur > Adversairepick )
                    {
                        pickedCharacter = character;
                    }
                    break;
                }
                AvailableAttack[attackLaunched].PlayAttack(pickedCharacter);
                 _adversaire.Stamina -= AvailableAttack[attackLaunched].Stamina;
            }
            else
            {
                Debug.Log( "Skipping Adversaries turn ");
            }
            _adversaire.ApplyAlteration(false);
            _adversaire.Recover();
        }

        public bool IsCommandEnded()
        {
            return true;
        }

        #endregion

    }
}