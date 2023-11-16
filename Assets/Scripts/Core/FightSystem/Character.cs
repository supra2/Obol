using Core.FightSystem.AttackSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Core.FightSystem
{
    public abstract class Character : ScriptableObject, ITargetable
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
        protected int _maxLife;
        [SerializeField]
        protected int _maxSan;
        /// </summary>
        [SerializeField]
        protected string _characterNameKey;
        [Header("Event")]
        public UnityIntEvent LifeChangeEvent;

        public UnityIntEvent SanChangeEvent;

        public UnityIntEvent StaminaChangeEvent;

        public AlterationEvent AlterationAppliedEvent;
        #endregion
        #region Hidden
        /// <summary>
        /// Perm Modifier
        /// </summary>
        protected List<Tuple<string, int>> _permModifiers;
        /// <summary>
        /// temp Modifier
        /// </summary>
        protected List<Tuple<string, int>> _tempModifiers;
        /// <summary>
        /// Current Life 
        /// </summary>
        protected int _life;
        /// <summary>
        /// San
        /// </summary>
        protected int _san;
        protected Dictionary<AlterationType, IAlteration> _alterations;
        protected int _stamina;
        #endregion
        #endregion

        #region Event
        public UnityEvent _dodged;
        public UnityEvent _died;
        #endregion

        #region Getter

        public int Life
        {
            set
            {
                _life = Mathf.Clamp( value, 0, _maxLife);
                LifeChangeEvent?.Invoke(_life);
            }
            get => _life;
        }

        public int MaxLife
        {
            set
            {
                _maxLife = value;
                if (Life > _maxLife)
                {
                    int damages = Life - _maxLife;
                    Life = _maxLife;
                    LifeChangeEvent?.Invoke(damages);
                }
            }
            get => _maxLife;
        }

        public int San
        {
            set
            {
                _san = Mathf.Clamp( value,0,_maxSan);
                SanChangeEvent?.Invoke(_san);
            }
            get => _san;
        }

        public int Stamina
        {
            get => _stamina;
            set
            {
                _stamina = value;
                StaminaChangeEvent?.Invoke(_stamina);
            }
        }

        public string CharacterNameKey => _characterNameKey;
        #endregion

        #region  Initialisation

        protected Character()
        {
            _permModifiers = new List<Tuple<string, int>>();
            _alterations = new Dictionary<AlterationType, IAlteration>();
            
        }

        public void Create()
        {
            San = _maxSan;
            Life = _maxLife;
        }

        #endregion

        #region Inner Methods
        //--------------------------------------------------------------

        protected int ComputeValue(int damage, int defbasevalue, string competence)
        {

            Tuple<string, int> t =
                _permModifiers.Find( X => X.Item1 == competence );
            float defense = 0;
            if (t == null)
            {
                defense = _constitution;
            }
            else
            {
                defense = _constitution + t.Item2;
            }
            Debug.Log(" Inflict " + (int)Mathf.Clamp(damage - defense, 0f, 9999f) + " Damage");
            return (int)Mathf.Clamp(damage - defense, 0f, 9999f);
        }

        //--------------------------------------------------------------

        /// <summary>
        /// Inflict Damage to a Jauge characteristique
        /// </summary>
        /// <param name="damagetype"> jauge characteristique affected </param>
        /// <param name="value"> </param>
        public virtual void Inflict( DamageType damagetype , int value )
        {
            switch (damagetype)
            {
                case DamageType.Health:
                   int damages = ComputeValue( value, _constitution, "Resistance" ); 
                    Life = Mathf.Clamp( Life - damages , 0 , _maxLife );
                    if( Life == 0)
                    {
                        _died?.Invoke();
                    }
                    break;
            }
        }

        //--------------------------------------------------------------
        #endregion

        #region  Characteristics Interface
        //--------------------------------------------------------------

        /// <summary>
        /// Characteristics by NAME
        /// </summary>
        /// <param name="characName"> charac Name </param>
        /// <returns></returns>
        public virtual int GetCharacteristicsByName(string characName)
        {
            int carac = 0;
            switch (characName.ToUpper())
            {
                case "STAMINA":
                    carac  = Stamina;
                    break;
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

        /// <summary>
        /// Characteristics by NAME
        /// </summary>
        /// <param name="characName"> charac Name </param>
        /// <param name="newValue"> new  Value </param>
        /// <returns></returns>
        public virtual void SetCharacteristicsByName(string characName,int newValue)
        {
            switch (characName.ToUpper())
            {
                case "INTELLIGENCE":
                    _intelligence = newValue;
                    break;
                case "STRENGTH":
                    _strength = newValue;
                    break;
                case "CONSTITUTION":
                     _constitution = newValue;
                    break;
                case "SPEED":
                     _speed = newValue;
                    break;

            }
        }

        //--------------------------------------------------------------

        public int GetCompetenceModifierByName( string compName )
        {

            if( _tempModifiers.Find((x) => x.Item1 == compName )  == null)
                return 0;
            
            return _tempModifiers.SkipWhile(x => x.Item1 == compName)
           .TakeWhile(x => x.Item1 != compName)
           .Sum(x => x.Item2);

        }

        //--------------------------------------------------------------

        public void AddTempCompetenceModifier(string comp, int modifier  )
        {
            _tempModifiers.Add(new Tuple<string,int>(comp, modifier));
        }

        //--------------------------------------------------------------
        #endregion

        #region Members
        //--------------------------------------------------------------

        /// <summary>
        /// Character Recovery of stamina after turn End
        /// </summary>
        public void Recover()
        {
            Stamina++;
        }

        //--------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void AddAlteration(AlterationType type, IAlteration value)
        {
            if (!_alterations.ContainsKey(type))
            {
                _alterations.Add(type, value);
                AlterationAppliedEvent?.Invoke(value);
            }
            else
            {
                _alterations[type].Merge(value);
                AlterationAppliedEvent?.Invoke(_alterations[type]);
            }
        }

        //--------------------------------------------------------------

        /// <summary>
        /// Apply Alteration
        /// </summary>
        /// <param name="turnBegin"> turn Begin</param>
        public void ApplyAlteration(bool turnBegin)
        {
            List<AlterationType> toRemove = new List<AlterationType>();
            foreach(KeyValuePair<AlterationType,IAlteration> alteration in _alterations)
            {
                if( alteration.Value.StartTurnAlteration() == turnBegin )
                {
                    alteration.Value.Apply(this);
                    AlterationAppliedEvent?.Invoke( alteration.Value );
                    if (!alteration.Value.StillGoingOn())
                    {
                        toRemove.Add(alteration.Key);
                    }
                }
            }
            foreach( AlterationType type in toRemove)
            {
                _alterations.Remove(type);
            }
        }

        //-------------------------------------------------------------

        public void Dodged()
        {
            _dodged?.Invoke();
        }

        //--------------------------------------------------------------
        #endregion
    }

}
