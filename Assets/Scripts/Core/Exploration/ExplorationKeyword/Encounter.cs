using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Exploration.ExplorationKeyword
{
    public class Encounter : IEffect
    {

        #region Inner Class
        public class EncounterBuilder : IWordBuilder
        {
            public IEffect BuildEffect(string[] words)
            {
                return new Encounter();
            }

            public string GetKeyWord()
            {
                return "Encounter";
            }

            public bool NestedKeyword()
            {
                return false;
            }
        }

        #endregion

        #region Member
        #region Members
        /// <summary>
        /// List Effect 
        /// </summary>
        public List<IEffect> _listEffect;
        #endregion
        #region hidden
        protected int _nbOpponent;
        protected bool setOpponent;
        protected string _tutor;
        /// <summary>
        /// List rules 
        /// </summary>
        protected List<Rule> _rules;
        #endregion
        #endregion

        #region Innerclass
        public struct Rule
        {
            public enum RuleType 
            {
                Random,
                Tutor,
                Id
            }

            public RuleType ruleType;
            public int  nbOpponent;
            public string Value;

        }
        #endregion

        #region Init

        public void Init()
        {

        }

        #endregion

        #region Methods
        //--------------------------------------------------------------

        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="itargetable"> targetable </param>
        public void Apply(ITargetable itargetable)
        {
            CombatVar vars = new CombatVar();
            vars.Party = PartyManager.Instance.Party.CharacterParty;
            vars.Adversaires = GetAdversaires();
            vars.FightInitiative = CombatVar.Initiative.Normal;
            CombatManager.Instance.StartCombat( vars );
        }

        //--------------------------------------------------------------

        /// <summary>
        /// Create From string line the encounter effect
        /// </summary>
        /// <param name="words"></param>
        public void CreateFromLine( string[] words )
        {
            System.Int32.TryParse( words[1] , out _nbOpponent);
            int i = 2;
            while ( i < words.Length )
            {
                Rule rule = new Rule();
                if (words[i] == "Tutor"  )
                {
                    if ( i+2 < words.Length )
                    {
                        i++;
                        rule.nbOpponent = System.Int32.Parse(words[i]);
                        rule.ruleType = Rule.RuleType.Tutor;
                        i++;
                        rule.Value = words[i];
                        _rules.Add(rule);
                    }
                    else
                    {
                        break;
                    }
                }
                else if (words[i] == "Fixed")
                {
                    if (i + 2 < words.Length)
                    {
                        i++;
                        rule.nbOpponent = System.Int32.Parse(words[i]);
                        rule.ruleType = Rule.RuleType.Id;
                        i++;
                        rule.Value = words[i];
                        _rules.Add(rule);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        //--------------------------------------------------------------

        public bool SelfTarget()
        {
            return true;
        }

        //--------------------------------------------------------------
        #endregion

        #region Inner Method
        //--------------------------------------------------------------

        /// <summary>
        /// Get Adversaire list for the encounter
        /// </summary>
        /// <returns> a list of Adversaires </returns>
        protected List<Adversaire> GetAdversaires()
        {
            List<Adversaire> adversaires = new List<Adversaire>();
            int i = 0;
            int j = 0;
            while (  i < _nbOpponent )
            {
                Adversaire adversaireDrawn = null;
                Predicate<Adversaire> predicate = null;
                switch ( _rules[j].ruleType )
                {
                    case Rule.RuleType.Tutor:
                        predicate = (X) =>
                           {
                               string[] listTags = X.GetTags();
                               bool match = false;
                               for (int i = 0; i < listTags.Length; i++)
                               {

                                   match = match || listTags[i] == _tutor;
                               }
                               return match;
                           };
                        break;
                    case Rule.RuleType.Id:
                        predicate = (X) =>
                        {
                            System.Int32.TryParse(_tutor,out int id);
                            return X.GetCardId() == id;
                        };
                       
                        break;
                }
                adversaireDrawn = ExplorationManager.Instance.CurrentLevel.
                          EncounterDeck.GetFirstCard(predicate);
                adversaires.Add(adversaireDrawn);
                i++;
                if( i>= _rules[j].nbOpponent)
                {
                     i=0;
                     j++;
                }

            }
            return adversaires;
        }

        //--------------------------------------------------------------
        #endregion
    }
}