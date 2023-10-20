using Core.FightSystem.AttackSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exploration.ExplorationKeyword
{
    public class May : NestedEffect
    {
        #region Inner Class
        public class MayBuilder : IWordBuilder
        {
            public IEffect BuildEffect( string[] words )
            {

                May  may = new May();
                may.CreateFromLine(words);
                return may;
            }

            public string GetKeyWord( )
            {
                return "May";
            }

            public bool NestedKeyword()
            {
                return true;
            }
        }

        #endregion

        #region Member
        #region Members
        /// <summary>
        /// listeffect 
        /// </summary>
        public List<IEffect> _listEffect;

        #endregion
        #region hidden
        public ITargetable _itargetable;
        /// <summary>
        /// localisation Key for prompt
        /// </summary>
        public static string _locKey;
        /// <summary>
        /// localisation Key for prompt
        /// </summary>
        public static string _Acceptkey;
        /// <summary>
        /// localisation Key for prompt
        /// </summary>
        public static string _refuseKey;
        #endregion
        #endregion

        #region Methods

        public void Apply( ITargetable itargetable )
        {
            UICombatController.Instance.DisplayChoice(_locKey, _Acceptkey, _refuseKey,
                null, null, MayEffectApply, MayEffectDontApply);
            _itargetable = itargetable;
        }

        public void CreateFromLine(string[] words)
        {
            
        }

        public bool SelfTarget()
        {
            return true;
        }

        public void SetNestedEffect(List<IEffect> listeffect)
        {
            _listEffect = listeffect;
        }

        public void MayEffectApply()
        { 
            foreach ( IEffect effect in _listEffect )
            {
                    Apply(_itargetable);
            }
        }

        public void MayEffectDontApply()
        {
        }

        #endregion

    }
}