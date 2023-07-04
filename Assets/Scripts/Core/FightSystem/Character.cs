using Core.FightSystem.AttackSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.FightSystem
{
    public abstract class Character : ScriptableObject, ICharacteristic
    {

        #region Members
        #region Visible
        /// <summary>
        /// Intelligence
        /// </summary>
        [SerializeField]
        protected int _intelligence;
        /// <summary>
        /// Strength
        /// </summary>
        [SerializeField]
        protected int _strength;
        /// <summary>
        /// Constitution
        /// </summary>
        [SerializeField]
        protected int _constitution;
        /// <summary>
        /// Speed
        /// </summary>
        [SerializeField]
        protected int _speed;
        [SerializeField]
        protected int _maxlife;
        /// <summary>
        /// Mental health Maximum Value
        /// </summary>
        [SerializeField]
        protected int _maxSan;
        [SerializeField]
        protected int _lifeMax;
        [SerializeField]
        protected string _characterNameKey;
        #endregion
        #region Hidden
        protected List<Tuple<string, int>> _permModifiers;
        protected List<Tuple<string, int>> _tempModifiers;

        /// <summary>
        /// current life 
        /// </summary>
        protected int _life;
        #endregion
        #endregion

        #region 

        protected int ComputeValue(int damage, int defbasevalue, string competence)
        {
            Tuple<string, int> t =
                _permModifiers.Find(X => X.Item1 == competence);
            float defense = 0;
            if (t == null)
            {
                defense = _constitution;
            }
            else
            {
                defense = _constitution + t.Item2;
            }
            return (int)Mathf.Clamp(damage - defense, 0f, 9999f);
        }

        public virtual void Inflict(DamageType damagetype, int value)
        {
            switch (damagetype)
            {
                case DamageType.Health:
                    _life -= ComputeValue(value, _constitution, "Resistance");
                    break;
            }
        }

        #endregion

        #region  Characteristics Interface
        //--------------------------------------------------------------

        /// <summary>
        /// Characteristics by NAME
        /// </summary>
        /// <param name="characName"> charac Name </param>
        /// <returns></returns>
        public int GetCharacteristicsByName(string characName)
        {
            int carac = 0;
            switch (characName.ToUpper())
            {
                case "INTELLIGENCE":
                    carac = _intelligence;
                    break;
                case "STRENGTH":
                    carac = _strength;
                    break;
                case "CONSTITUTION":
                    carac = _constitution;
                    break;
                case "SPEED":
                    carac = _speed;
                    break;
            }
            return carac;
        }

        //--------------------------------------------------------------

        public int GetCompetenceModifierByName(string compName)
        {
            return _tempModifiers.SkipWhile(x => x.Item1 == compName)
           .TakeWhile(x => x.Item1 != compName)
           .Sum(x => x.Item2);
        }

        //--------------------------------------------------------------
        #endregion
    }
}
