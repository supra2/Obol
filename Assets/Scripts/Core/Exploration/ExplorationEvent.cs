using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Core.Exploration.Tile;

namespace Core.Exploration
{

    [CreateAssetMenu(fileName = "Exploration Event",
      menuName = "Exploration/Event",
      order = 1)]
    [Serializable]
    public class ExplorationEvent:ScriptableObject,ICloneable,ICard
    {

        #region enum 

        public enum Rarity
        {
            Common,
            Uncommon,
            Rare,
            Unique
        }

        public enum EffectTrigger
        {
            OnReveal,
            OnEnter,
            OnLeave,
            OnFirstEnter
        }

        #endregion

        #region Members 
        #region Visible
        /// <summary>
        /// Key Name
        /// </summary>
        [SerializeField]
        protected string _keyName;
        /// <summary>
        ///  Description name 
        /// </summary>
        [SerializeField]
        protected string _descriptionKey;
        [SerializeField]
        protected Sprite _illustration;
        [SerializeField]
        protected int  _cardID;
        [SerializeField]
        protected Rarity _rarity;
        [SerializeField]
        protected bool _lit;
        /// <summary>
        ///  Effect : 
        /// </summary>
        [SerializeField]
        [TextArea(5, 10)]
        protected string _enterEffectsDescription;
        [SerializeField]
        [TextArea(5, 10)]
        protected string _firstEnterEffectsDescription;
        [SerializeField]
        [TextArea(5, 10)]
        protected string _leaveEffectsDescription;
        [SerializeField]
        [TextArea(5, 10)]
        protected string _revealEffectsDescription;
        [SerializeField]
        protected string[] _tags;
        #endregion
        #region Hidden
        protected int _instanceID;
        protected static int _lastID = 0;
        /// <summary>
        /// Dictionary effects
        /// </summary>
        protected Dictionary<EffectTrigger,List<IEffect>> _effectDictionary;
        protected bool _entered;
        #endregion
        #endregion

        #region  Initialisation

        public void Init()
        {
            _instanceID = _lastID++; 
            _entered = false;

             _effectDictionary = new Dictionary<EffectTrigger, List<IEffect>>();
            _effectDictionary.Add( EffectTrigger.OnEnter , EffectFactory.ParseEffect(_enterEffectsDescription) );
            _effectDictionary.Add(EffectTrigger.OnReveal, EffectFactory.ParseEffect(_revealEffectsDescription));
            _effectDictionary.Add(EffectTrigger.OnLeave, EffectFactory.ParseEffect(_leaveEffectsDescription));
            _effectDictionary.Add(EffectTrigger.OnFirstEnter, EffectFactory.ParseEffect(_firstEnterEffectsDescription));
        
        }

        #endregion

        #region Interface Implementation

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public string DescriptionKey()
        {
            return _descriptionKey;
        }

        public bool Equals(ICard other)
        {
            if (!(other is ExplorationEvent))
                return false;


            return ((ExplorationEvent)other).InstanceID == _instanceID;
        }


        #endregion

        #region Getter

        public Rarity EventRarity => _rarity;

        public int GetCardId()
        {
            return _cardID;
        }

        public Sprite GetIllustration()
        {
            return _illustration;
        }

        public string TitleKey()
        {
            return _keyName;
        }

        public int InstanceID => _instanceID;

        public bool Lit => _lit;

        #endregion

        #region Card Implementation

        public void Play()
        {
           
        }

        public void Resolve()
        {
       
        }

        public void Leave(Direction leavingfrom)
        {
            ApplyEffect( EffectTrigger.OnLeave );
        }

        public void Enter(Direction Entering)
        {
            if (!_entered)
            {
                ApplyEffect(EffectTrigger.OnFirstEnter);
                _entered = true;
            }
            ApplyEffect(EffectTrigger.OnEnter);
        }

        public void Reveal()
        {
            ApplyEffect(EffectTrigger.OnReveal);
        }

        #endregion

        #region InnerMethod

        protected void ApplyEffect( EffectTrigger effectTrigger )
        {
            List<IEffect> effectlist = _effectDictionary[effectTrigger];
            PlayableCharacter charac = null;
            
            foreach (PlayableCharacter character in GameManager.Instance.PartyManager.Party.CharacterParty )
            {
                if (character.MainCharacter)
                    charac = character;
            }

            if (effectlist != null)
            {
                foreach (IEffect effect in effectlist)
                {
                    effect.Apply(charac);
                }
            }

        }

        public string[] GetTags()
        {
            return _tags;
        }

        #endregion
    }
}
