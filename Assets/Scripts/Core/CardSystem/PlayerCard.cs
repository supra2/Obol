using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.CardSystem
{
    
    public abstract class PlayerCard: ScriptableObject,ICard
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
        /// <summary>
        /// Sprite : Portrait
        /// </summary>
        [SerializeField]
        protected Sprite _portrait;
        /// <summary>
        /// Sprite : Combat Sprite
        /// </summary>
        [SerializeField]
        protected Sprite _CombatSprite;
        #endregion

        #region Getters
        public Nature CardNature => _nature;
        public Type CardType => _type;
        public string CardName => _cardName;
        #endregion

        #region Methods
        public abstract void Play( );
        #endregion

    }

}