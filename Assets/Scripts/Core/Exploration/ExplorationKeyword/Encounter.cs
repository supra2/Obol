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
                Encounter encounter = new Encounter();
                encounter.CreateFromLine(words);
                return encounter;
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
            vars.Party = GameManager.Instance.PartyManager.
                Party.CharacterParty;
            vars.Adversaires = GetAdversaires();
            vars.FightInitiative = CombatVar.Initiative.Normal;
            GameManager.Instance.SaveCurrentGame();

            GameManager.Instance.StartCombat( vars );
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
            _rules = new List<Rule>();
            while ( i < words.Length )
            {
                Rule rule = new Rule();
                if (words[i] == "Tutor"  )
                {
                    if ( i+2 < words.Length )
                    {
                        rule.ruleType = Rule.RuleType.Tutor;
                        i++;
                        if( !System.Int32.TryParse( words[i] ,
                            out rule.nbOpponent ) )
                        {
                             throw new ArgumentException("nb opponent value not parsable as an INT"); ;
                        }
                        rule.Value = words[i];
                        _rules.Add(rule);
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (words[i] == "Fixed")
                {
                    if ( i + 2 < words.Length )
                    {
                        rule.ruleType = Rule.RuleType.Id;
                        i++;
                        if (!System.Int32.TryParse(words[i],
                           out rule.nbOpponent))
                        {
                            throw new ArgumentException("nb opponents " +
                                "value not parsable as an INT"); ;
                        }
                        i++;
                        rule.Value = words[i];
                        _rules.Add(rule);
                        i++;
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
            int k = 0;
            while (k < _nbOpponent )
            {
                Adversaire adversaireDrawn = null;

                Predicate<Adversaire> predicate = null;

                string tutor = _rules[j].Value;

                switch ( _rules[j].ruleType )
                {
                    case Rule.RuleType.Tutor:
                        predicate = (X) =>
                        {
                            string[] listTags = X.GetTags();
                            bool match = false;
                            for ( int i = 0 ; i < listTags.Length ; i++ )
                            {
                                match = match || listTags[i] == tutor;
                            }
                            return match;
                        };
                        break;
                    case Rule.RuleType.Id:
                        predicate = (X) =>
                        {
                           if( !System.Int32.TryParse(tutor, out int id))
                           {
                                throw new ArgumentException("_tutor value" +
                                    " not parsable as an int");
                           }
                           return X.GetCardId() == id;
                        };
                        break;
                }

                adversaireDrawn = ExplorationManager.Instance.CurrentLevel.
                          EncounterDeck.GetFirstCard(predicate);
                adversaires.Add(adversaireDrawn);
                i++;
                k++;
                if ( i >= _rules[j].nbOpponent)
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