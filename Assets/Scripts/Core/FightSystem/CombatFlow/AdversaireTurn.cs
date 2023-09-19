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

        protected bool _commandEnded;

        protected PlayableCharacter _pickedCharacter;

        protected Attack _Attack;
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
                int attackLaunched = SeedManager.NextInt( 0 , AvailableAttack.Count );
                Debug.Log("AttackLaunched "+ attackLaunched);
                // Select Target
                int totalrange = 0;
                foreach(  PlayableCharacter character in CombatManager.Instance.Var.Party )
                {
                    int prob = Mathf.FloorToInt( (1.0f / 
                        character.GetCharacteristicsByName( "Speed") + 
                        character.GetCompetenceModifierByName("Distance")  ) * 100 );
                    totalrange += prob;
                }
                int Adversairepick = SeedManager.NextInt( 0 , totalrange );
                int pointeur = 0;
                _pickedCharacter = null;
                foreach ( PlayableCharacter character in 
                    CombatManager.Instance.Var.Party )
                {
                    pointeur += Mathf.FloorToInt((1.0f /
                    character.GetCharacteristicsByName("Speed") +
                        character.GetCompetenceModifierByName("Distance")) * 100);
                    if ( pointeur >= Adversairepick )
                    {
                        _pickedCharacter = character;
                    }
                    break;
                }
                _Attack = AvailableAttack[attackLaunched];
                // Todo  only dodge for physical Attack
                CoinFlipManager.Instance.Flip(_pickedCharacter.GetCharacteristicsByName("Speed") +
                       _pickedCharacter.GetCompetenceModifierByName("Distance"), _adversaire.GetCharacteristicsByName("Speed"),
                       ResolveAttack, true);
                //Remove distance to target
                _adversaire.Stamina -= AvailableAttack[attackLaunched].Stamina;
            }
            else
            {
                Debug.Log( "Skipping Adversaries turn ");
                _commandEnded = true;
            }
          
        }

        public void ResolveAttack(bool dodged)
        {
            if( !dodged )
            {
                _Attack.PlayAttack(_pickedCharacter);
            }
            else
            {
                _pickedCharacter.Dodged();
            }
            _adversaire.ApplyAlteration(false);
            _adversaire.Recover();

            _commandEnded = true;
        }

        public bool IsCommandEnded()
        {
            return _commandEnded;
        }

        #endregion

    }
}