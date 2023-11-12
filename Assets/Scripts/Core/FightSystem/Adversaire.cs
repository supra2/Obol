using Core;
using Core.FightSystem.AttackSystem;
using Core.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Core.FightSystem
{
    [CreateAssetMenu(fileName = "Adversaire", menuName = "Obol/Characters/Enemy", order = 1)]
    public class Adversaire : Character, ICharacteristic,ICard , ICloneable
    {

        #region Enum
        public enum EnnemyState
        {
            Neutral,
            Attacked,
            Attacking,
            Dead,
            Victorious
        }
        #endregion

        #region Members
        #region Hidden
        /// <summary>
        /// Current State
        /// </summary>
        protected EnnemyState _currentState;
        #endregion
        #region Visible
        [SerializeField]
        protected List<Attack> _attacks;
        [SerializeField]
        protected Sprite _illustrations;
        [SerializeField]
        protected List<int> _lootProba;
        [SerializeField]
        protected List<Item> _lootItem;
        [SerializeField]
        protected string _descriptionKey;
        [SerializeField]
        protected Exploration.ExplorationEvent.Rarity _rarity;
        [SerializeField]
        protected int _cardID;
        [SerializeField]
        protected string[] _tags;
        #region Hidden
        /// <summary>
        /// Individual instance for Attacks
        /// </summary>
        protected List<Attack> _instanciatedAttacks;
       
        #endregion
        #endregion
        #endregion

        #region Event
        public UnityCharacterEvent OnStartTurn;
        #endregion

        #region Getters

        public UnityAttackEvent Attacked;

        public Sprite Illustration
        {
            get => _illustrations;
        }

        public bool IsDead()
        {
            return _life == 0;
        }

        public List<Attack> AttackList
        {
            get => _instanciatedAttacks;

        }

        public Dictionary<int, Item> LootTable
        {
            get
            {
                Dictionary<int, Item> dico = new Dictionary<int, Item>();
                for(int i = 0 ; i < _lootProba.Count; i++ )
                {
                    dico.Add(_lootProba[i], _lootItem[i]);
                }
                return dico;
            }
        }
        public Exploration.ExplorationEvent.Rarity Rarity => _rarity;
        #endregion

        //--------------------------------------------------------------

        public void Attack(int degat)
        {
            Life = Mathf.Clamp(degat, 0, _maxLife);
        }

        //--------------------------------------------------------------

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
            Life = _maxLife;
            Stamina = 1;
            _instanciatedAttacks = new List<Attack>();
            foreach (Attack attack in _attacks)
            {
              Attack instance =  ScriptableObject.Instantiate(attack);
              instance.Init();
              instance.AttackLaunched.AddListener(OnAttackLaunched);
              _instanciatedAttacks.Add(instance);
            }
            if (Attacked == null)
            {
                Attacked = new UnityAttackEvent();
            }

        }

        //--------------------------------------------------------------

        public override string ToString()
        {
            return string.Format(" [ {0} PV: {1}/{2} ]",
                _characterNameKey, _life, _maxLife);
        }


        //--------------------------------------------------------------

        public void OnAttackLaunched( Attack attack )
        {
            Attacked?.Invoke(attack);
        }

        //--------------------------------------------------------------

        public Sprite GetIllustration()
        {
            return _illustrations;
        }

        //--------------------------------------------------------------

        public string DescriptionKey()
        {
            return _descriptionKey;
        }

        //--------------------------------------------------------------

        public string TitleKey()
        {
            return _characterNameKey;
        }

        //--------------------------------------------------------------

        public void Play()
        {
            throw new NotImplementedException();
        }
        //--------------------------------------------------------------

        public void Resolve()
        {
            throw new NotImplementedException();
        }

        //--------------------------------------------------------------

        public int GetCardId()
        {
            return _cardID;
        }

        //--------------------------------------------------------------

        public bool Equals(ICard other)
        {
            if (!(other is Adversaire))
                return false;

            Adversaire otherAdversaire = (Adversaire)other;
            return ((Adversaire)other)._cardID != _cardID;
        }
        //--------------------------------------------------------------
        public object Clone()
        {
            Adversaire adversaire = ScriptableObject.CreateInstance<Adversaire>();
            adversaire._characterNameKey = this._characterNameKey;
            adversaire._attacks = new List<Attack>();
            for ( int i = 0 ; i < _attacks.Count ; i++ )
            {
                adversaire._attacks.Add(
                    ScriptableObject.Instantiate(_attacks[i]));
            }
            adversaire._illustrations = this._illustrations;
            adversaire._lootProba = this._lootProba;
            adversaire._lootItem = new List<Item>(); 
            for (int i = 0; i < this._lootItem.Count; i++)
            {
                adversaire._lootItem.Add(
                    ScriptableObject.Instantiate(this._lootItem[i]));
            }
            adversaire._descriptionKey = this._descriptionKey;

            adversaire._rarity = this._rarity;
            adversaire._cardID = this._cardID;
            adversaire.Init();
            return adversaire;
        }

        //--------------------------------------------------------------

        public string[] GetTags()
        {
            return _tags;
        }

        //--------------------------------------------------------------

    }
}