using Core;
using Core.CardSystem;
using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

//TODO MOVE THEM TO A PROPER FILE
[Serializable]
public class UnityCardEvent : UnityEvent<ICard>
{

}

[Serializable]
public class UnityCharacterEvent : UnityEvent<Character>
{

}

namespace Core.FightSystem
{
    [CreateAssetMenu(fileName = "PlayerCard",
        menuName = "Obol/Character/PlayableCharacter", 
        order = 0)]
    public class PlayableCharacter : Character , ICharacteristic , ITargetable
    {

        #region Members
        #region Visible
        [SerializeField]
        protected List<PlayerCard> _cardList;
        /// <summary>
        /// Mental health Maximum Value
        [SerializeField]
        protected int _maxSan;
        #endregion
        #region Hidden
        /// <summary>
        /// current mental health value
        /// </summary>
        protected int _san;
        #endregion
        #endregion

        #region Getter
        public List<PlayerCard> CardList => _cardList;
        #endregion

        #region Initialisation

        public PlayableCharacter()
        {
            _tempModifiers = new List<Tuple<string, int>>();
            _permModifiers = new List<Tuple<string, int>>();
            _life = _maxlife;
            _san = _maxSan;

        }

        /// <summary>
        /// Init Fight
        /// </summary>
        public void Init ( )
        {
            if(_tempModifiers == null)
            { 
                _tempModifiers = new List<Tuple<string, int>>();
            }
            else
            {
                _tempModifiers.Clear();
            }
        
        }

        #endregion

        public bool IsDead()
        {
            throw new NotImplementedException();
            return false;
        }

        public override void Inflict(DamageType damagetype, int value)
        {
            switch(damagetype)
            {
                case DamageType.San:
                    _san -= ComputeValue(value, _intelligence, "Resilience");
                    break;
            }
            base.Inflict(damagetype, value);
        }

    }

}

