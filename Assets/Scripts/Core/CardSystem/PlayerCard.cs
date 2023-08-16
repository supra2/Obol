using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using Core.FightSystem.CombatFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.CardSystem
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Obol/Character/PlayerCard", order = 3)]
    public  class PlayerCard : ScriptableObject, ICard , ICloneable
    {

        #region Enum
        public enum Type
        { Action, Affliction};
        public enum Nature
        { Physique, Mentale };
        #endregion

        #region Members
        [Header("Main")]
        [SerializeField]
        protected string _cardName;
        /// <summary>
        ///Description Key
        /// </summary>
        [SerializeField]
        protected string _description;
        /// <summary>
        /// Type of the card
        /// </summary>
        [SerializeField]
        protected Type _type;
        /// <summary>
        /// Nature of the card
        /// </summary>
        [SerializeField]
        protected Nature _nature;
        /// <summary>
        /// Sprite : Illustration
        /// </summary>
        [SerializeField]
        protected Sprite _illustration;
        [Header("TargetingOptions")]
        [SerializeField]
        /// <summary>
        /// Target Monster Card
        /// </summary>
        protected bool _targetMonster;
        [SerializeField]
        /// <summary>
        /// Target self Card
        /// </summary>
        protected bool _targetSelf;
        [Header("Play Effect Configuration")]
        [TextArea(5, 10)]
        /// <summary>
        /// Basic effect text
        /// </summary>
        [SerializeField]
        protected string _effect;
        /// <summary>
        /// Effect list
        /// </summary>
        [SerializeField]
        protected List<IEffect> _effects;
        /// <summary>
        /// int Stamina Cost
        /// </summary>
        [SerializeField]
        protected int _staminaCost ;
        /// <summary>
        /// dynamic Stamina Costs
        /// </summary>
        [SerializeField]
        protected bool _dynamicStaminaCost;
        #region Hidden 
        public UnityCardEvent _playCardEvent;
        protected int _instanceID;
        protected static int _lastID=0;
        #endregion
        #endregion

        #region Getters
        public Nature CardNature => _nature;
        public Type CardType => _type;
        public string CardName => _cardName;
        public int InstanceID => _instanceID;
        #endregion

        #region Initialisation

        public PlayerCard(string cardName, Type type,Nature nature, Sprite illustration,string effect,
            List<IEffect> effectList, bool targetMonster,string description)
        {
            _cardName = cardName;
            _type = type;
            _nature = nature;
            _illustration = illustration;
            _effect = effect;
            if(effectList != null)
                _effects = new List<IEffect>(effectList);
            _targetMonster = targetMonster;
            _description = description;
            _instanceID = _lastID++;
            CardPlayed = new UnityCardEvent();
        }

        public virtual void Init()
        {
            _effects = EffectFactory.ParseEffect(_effect);
        }

        #endregion

        #region Methods

        public bool CanPayStaminaCost()
        {
            int staminaAvailable = ((PlayableCharacter)CombatManager.Instance.GetCurrentCharacter()).
                GetCharacteristicsByName("Stamina");

            return ( !_dynamicStaminaCost && staminaAvailable >= _staminaCost)
                || (_dynamicStaminaCost && staminaAvailable >= 1);
        }

        public virtual void Play()
        {
            if( CanPayStaminaCost() )
            {
                if ( !_dynamicStaminaCost )
                {
                    int Stamina =  ((PlayableCharacter)CombatManager.
                                    Instance.GetCurrentCharacter()).
                                    GetCharacteristicsByName("Stamina");
                    Stamina -= _staminaCost;
                    ((PlayableCharacter)CombatManager.
                        Instance.GetCurrentCharacter()).
                    SetCharacteristicsByName("Stamina", Stamina);
                }
                if (_targetMonster )
                {
                    UICombatController.Instance.SelectAdversaire(
                    Resolve);
                }
                else
                {
                   Resolve ();
                }
            }
            else
            {
                Debug.Log("CantPayStaminaCost");
            }
        }

        public void Resolve()
        {
            foreach (IEffect effect in _effects)
            {
                if (effect.SelfTarget())
                {
                    effect.Apply(
                    (ITargetable)CombatManager.Instance.
                    GetCurrentCharacter() );
                }
                else
                {
                    effect.Apply(null);
                }
            }
            CardPlayed?.Invoke(this);
        }

        public void Resolve(ITargetable target)
        {
            foreach (IEffect effect in _effects)
            {
                if(effect.SelfTarget())
                {
                    effect.Apply(
                        (ITargetable)CombatManager.Instance.GetCurrentCharacter() );
                }
                else
                {
                    effect.Apply(target);
                }
            }
            CardPlayed?.Invoke(this);
        }
      
        public Sprite GetIllustration()
        {
            return _illustration;
        }

        public string DescriptionKey()
        {
            return _description;
        }

        public string TitleKey()
        {
            return CardName;
        }

        public virtual object Clone()
        {
            PlayerCard clone = ScriptableObject.Instantiate(this);
            clone.Init();
            return clone;
        }

        #endregion

        #region Events
        public UnityCardEvent CardPlayed;
        #endregion

        #region Equals implementation

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (!(other is PlayerCard))
                return false;


            return ((PlayerCard)other).InstanceID == _instanceID;
        }

        #endregion 
    }

}