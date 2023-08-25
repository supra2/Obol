using Core;
using Core.FightSystem.AttackSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Core.FightSystem
{
    [CreateAssetMenu(fileName = "Adversaire", menuName = "Obol/Characters/Enemy", order = 1)]
    public class Adversaire : Character, ICharacteristic
    {

        #region Enum
        public enum EnnemyState
        {
            Neutral,
            Attacked,
            Attacking,
            Dead,
            Victorious
        }
        #endregion

        #region Members
        #region Hidden
        /// <summary>
        /// Current State
        /// </summary>
        protected EnnemyState _currentState;
        #endregion
        #region Visible
        [SerializeField]
        protected List<Attack> _attacks;
        [SerializeField]
        protected Sprite _illustrations;
        #region Hidden
        /// <summary>
        /// Individual instance for Attacks
        /// </summary>
        protected List<Attack> _instanciatedAttacks;
        #endregion
        #endregion
        #endregion

        #region Event
        public UnityCharacterEvent OnStartTurn;
        #endregion

        #region Getters
        public UnityAttackEvent Attacked;
        public Sprite Illustration
        {
            get => _illustrations;
        }

        public bool IsDead()
        {
            return _life == 0;
        }

        public List<Attack> AttackList
        {
            get => _instanciatedAttacks;

        }

        #endregion

        //--------------------------------------------------------------

        public void Attack(int degat)
        {
            _life = Mathf.Clamp(degat, 0, _maxlife);
        }

        //--------------------------------------------------------------

        /// <summary>
        /// Init Fight
        /// </summary>
        public void Init()
        {
            if (_tempModifiers == null)
            {
                _tempModifiers = new List<Tuple<string, int>>();
            }
            else
            {
                _tempModifiers.Clear();
            }
            Life = _maxlife;
            Stamina = 1;
            _instanciatedAttacks = new List<Attack>();
            foreach (Attack attack in _attacks)
            {
              Attack instance =  ScriptableObject.Instantiate(attack);
               instance.Init();
                instance.AttackLaunched.AddListener(OnAttackLaunched);
              _instanciatedAttacks.Add(instance);
            }
            if (Attacked == null)
            {
                Attacked = new UnityAttackEvent();
            }

        }

        //--------------------------------------------------------------

        public override string ToString()
        {
            return string.Format(" [ {0} PV: {1}/{2} ]",
                _characterNameKey, _life, _maxlife);
        }


        //--------------------------------------------------------------

        public void OnAttackLaunched( Attack attack )
        {
            Attacked?.Invoke(attack);
        }

        //--------------------------------------------------------------

    }
}