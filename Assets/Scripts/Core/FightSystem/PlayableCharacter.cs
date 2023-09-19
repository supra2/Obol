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
    public class ExchangeEvent:UnityEvent<int,int>
    {

    }

    [CreateAssetMenu(fileName = "PlayerCard",
        menuName = "Obol/Character/PlayableCharacter",
        order = 0)]
    [Serializable]
    public class PlayableCharacter : Character, ICharacteristic, ITargetable
    {

        #region Members
        #region Visible
        [SerializeField]
        protected List<PlayerCard> _cardList;
        /// <summary>
        /// Mental health Maximum Value
        [SerializeField]
        protected int _maxSan;
        protected List<Tuple<int, int>> _exchangeMemory;
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

        public int San => _san;
        #endregion

        #region Event
        public ExchangeEvent CardExchanged;
        #endregion

        #region Initialisation

        public PlayableCharacter()
        {
            _tempModifiers = new List<Tuple<string, int>>();
            _permModifiers = new List<Tuple<string, int>>();
            _life = _maxlife;
            _san = _maxSan;
            _exchangeMemory = new List<Tuple<int, int>>();
        }

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

            if (_permModifiers == null)
            {
                _permModifiers = new List<Tuple<string, int>>();
            }
            foreach (PlayerCard card in _cardList)
            {
                if (card is ChoiceCard)
                {
                    ((ChoiceCard)card).Init();
                }
                else
                {
                    card.Init();
                }
            }
            Stamina = 1;
        }

        #endregion

        #region Public Methods

        public bool IsDead()
        {
            throw new NotImplementedException();
            return false;
        }

        public override void Inflict(DamageType damagetype, int value)
        {
            switch (damagetype)
            {
                case DamageType.San:
                    _san -= ComputeValue(value, _intelligence, "Resilience");
                    break;
            }
            base.Inflict(damagetype, value);
        }

        public override int GetCharacteristicsByName(string characName)
        {
            switch (characName.ToUpper())
            {


                default:
                    return base.GetCharacteristicsByName(characName);
            }
        }

        public override void SetCharacteristicsByName(string characName, int newValue)
        {
            switch (characName.ToUpper())
            {
                case "STAMINA":
                    Stamina = newValue;
                    break;
                default:
                    base.SetCharacteristicsByName(characName, newValue);
                    break;
            }
        }

        public void Exchange(int card1, int card2)
        {
            PlayerCard pc = _cardList.Find(x => x.GetCardId() == card1);
            if (pc != null)
            {
                _exchangeMemory.Add(new Tuple<int, int>(card1, card2));
                _cardList.Remove(pc);
                _cardList.Add(CardManager.Instance.Instantiate(card2) as PlayerCard);
                CardExchanged?.Invoke(card1, card2);
            }
            else
            {
                Debug.LogError("card not found in deck. Exchange chanceled");
            }
        }

        #endregion

    }

}

