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
    public  class PlayerCard : ScriptableObject,ICard , ICloneable
    {

        #region Enum
        public enum Type
        { Action, Affliction};
        public enum Nature
        { Physique, Mentale };
        #endregion

        #region Members
        /// <summary>
        /// Name of the card
        /// </summary>
        [SerializeField]
        protected string _cardName;
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
        [SerializeField]
        /// <summary>
        /// Target Monster Card
        /// </summary>
        protected bool _targetMonster;
        [TextArea(5, 10)]
        /// <summary>
        /// Basic effect text
        /// </summary>
        [SerializeField]
        protected string _description;
        #endregion

        #region Getters
        public Nature CardNature => _nature;
        public Type CardType => _type;
        public string CardName => _cardName;
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
            _effects = new List<IEffect>(effectList);
            _targetMonster = targetMonster;
            _description = description;
        }

        #endregion

        #region Methods

        public virtual void Play()
        {
            if(_targetMonster )
            {
                CombatManager.Instance.CommandStack.Pile(new AttackCommand (ResolveAttack));
            }
            else
            { 
                CombatManager.Instance.CommandStack.Pile(Resolve);
            }
        }

        public virtual void Resolve()
        {
            foreach (IEffect effect in _effects)
            {
                effect.Apply(null);
            }
        }

        public virtual void ResolveAttack(ITargetable target)
        {
            foreach (IEffect effect in _effects)
            {
                effect.Apply(target);
            }
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

        public object Clone()
        {
            return new PlayerCard(_cardName, _type, _nature,_illustration,
                _effect,_effects,_targetMonster,_description);

        }


        #endregion

    }

}