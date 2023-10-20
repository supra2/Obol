using Core.CardSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.FightSystem.AttackSystem
{
    [CreateAssetMenu(fileName = "ChoiceCard", menuName = "Obol/Character/ChoiceCard", order = 4)]
    public class ChoiceCard : PlayerCard
    {

        #region Members
        #region Visible
        [SerializeField]
        /// <summary>
        /// List of cards matching everyChoices
        /// </summary>
        protected List<PlayerCard> _cards;
        #endregion
        #region Hidden
        protected int choose;
        #endregion
        #endregion

        #region Getters

        /// <summary>
        /// Cards 
        /// </summary>
        public List<PlayerCard> Cards
        {

            get => _cards;
            set => _cards = value;
        }

        #endregion

        #region Init
        //-------------------------------------------------------------

        public ChoiceCard(string cardName, Type type, Nature nature, Sprite illustration, string effect,
            List<IEffect> effectList, bool targetMonster, string description, List<PlayerCard> cards) :
            base(cardName, type, nature, illustration, effect, effectList, targetMonster, description)
        {
            _cards = new List<PlayerCard>();
            foreach( PlayerCard Card  in cards)
            {
                PlayerCard pc = ScriptableObject.Instantiate(Card);
                pc.Init();
                _cards.Add(pc);
            }
        }

        //-------------------------------------------------------------
        #endregion

        #region protected Methods
        //-------------------------------------------------------------

        /// <summary>
        /// Execute Choice 1
        /// </summary>
        /// <param name="targetable"> ITargetable: targetable </param>
        protected void ExecuteChoice1(ITargetable targetable)
        {
            _cards[0].Play();
        }

        //-------------------------------------------------------------

        /// <summary>
        /// Execute Choice 2
        /// </summary>
        /// <param name="targetable"> ITargetable: targetable </param>
        protected void ExecuteChoice2(ITargetable targetable)
        {
            _cards[1].Play();
        }

        //-------------------------------------------------------------

        /// <summary>
        /// Enable Component callback
        /// </summary>
        protected void OnEnable( )
        {
            foreach (PlayerCard playerCard in _cards)
            {
                playerCard.CardPlayed.AddListener(
                    OnChoicePlayed);
            }
        }

        //-------------------------------------------------------------

        /// <summary>
        /// Disable Component callback
        /// </summary>
        protected void OnDisable()
        {
            foreach (PlayerCard playerCard in _cards)
            {
                playerCard.CardPlayed.RemoveListener(
                    OnChoicePlayed);
            }
        }

      
        //-------------------------------------------------------------

       /// <summary>
       /// Choice Played
       /// </summary>
       /// <param name="card"></param>
        protected void OnChoicePlayed(ICard card)
        {
            CardPlayed?.Invoke(this);
        }

        //-------------------------------------------------------------

        public override object Clone()
        {
            ChoiceCard choiceCardClone=(ChoiceCard)  base.Clone();
            List<PlayerCard> instanciatedCards = new List<PlayerCard>();
            foreach (PlayerCard Card in _cards)
            {
                PlayerCard playerCard = ScriptableObject.Instantiate(Card);
                playerCard.Init();
                instanciatedCards.Add(playerCard);
            }
            choiceCardClone.Cards = instanciatedCards; 
            choiceCardClone.OnEnable();
            return choiceCardClone;
        }

        //-------------------------------------------------------------
        #endregion

        #region Public Methods
        //-------------------------------------------------------------

        public bool SelfTarget()
        {
            return true;
        }

        //-------------------------------------------------------------

        /// <summary>
        /// Initialise Underlying Card
        /// </summary>
        public override void Init()
        {
            _instanceID = _lastID++;
            foreach (PlayerCard card in _cards )
            {
                card.Init();
            }
        }

        //-------------------------------------------------------------

        /// <summary>
        /// Play 
        /// </summary>
        public override void Play()
        {
            UICombatController.Instance.DisplayChoice(this);
        }

        //-------------------------------------------------------------
        #endregion

    }
}
