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
    public class Adversaire : Character, ICharacteristic, ITargetable
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
        /// <summary>
        /// Current Life
        /// </summary>
        protected int _currentlife;
        #endregion
        #region Visible
        [SerializeField]
        protected List<Attack> _attacks;
        [SerializeField]
        protected Sprite _illustrations;
        [SerializeField]
        protected string _name;
        #endregion
        #endregion

        #region Getters

        public int Life
        {
            get => _currentlife;
        }

        public Sprite Illustration
        {
            get => _illustrations;
        }

        public bool IsDead()
        {
            return _currentlife == 0;
        }

        #endregion

        //--------------------------------------------------------------

        public void Attack(int degat)
        {
            _currentlife = Mathf.Clamp(_lifeMax, 0, _lifeMax);

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

        }

        //--------------------------------------------------------------
        public override string ToString()
        {
            return string.Format(" [ {0} PV: {1}/{2} ]", _name, _currentlife, _lifeMax);
        }

        //--------------------------------------------------------------

     


    }
}