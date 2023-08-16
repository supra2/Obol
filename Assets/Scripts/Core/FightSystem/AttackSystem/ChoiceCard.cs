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
        public List<PlayerCard> Cards => _cards;
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
                _cards.Add( ScriptableObject.Instantiate(Card));
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

        protected void OnChoicePlayed(ICard card)
        {
            CardPlayed?.Invoke(this);
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
            foreach(PlayerCard card in _cards )
            {
                card.Init();
            }
        }

        //-------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public override void Play()
        {
            UICombatController.Instance.DisplayChoice(this);
           
        }

        //-------------------------------------------------------------
        #endregion

    }
}
