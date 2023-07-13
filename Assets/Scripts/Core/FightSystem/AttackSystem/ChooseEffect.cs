using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FightSystem.AttackSystem
{
    public class ChooseEffect : IEffect
    {

        #region Members
        #region Hidden
        /// <summary>
        /// Choices 1 effect
        /// </summary>
        protected List<IEffect> _choices1;
        /// <summary>
        /// Choices 2 effect
        /// </summary>
        protected List<IEffect> _choices2;
        /// <summary>
        /// Description Key
        /// </summary>
        protected string _locDescriptionKey;
        /// <summary>
        /// Choice name for ever
        /// </summary>
        protected List<string> _choicekey;

        protected int choose;
        #endregion
        #endregion

        #region Getter

        public List<IEffect> Choices1
        {
            get => _choices1;
            set => _choices1 = value;
        }
        public List<IEffect> Choices2
        {
            get => _choices2;
            set => _choices2 = value;
        }

        public string TextKey => _locDescriptionKey;

        public string ChoiceKey1 => _choicekey[0];

        public string ChoiceKey2 => _choicekey[1];
        #endregion

        #region Initialisation

        public ChooseEffect(string locDescriptionKey)
        {
            _choicekey = new List<string>();
            _choices1 = new List<IEffect>();
            _choices2 = new List<IEffect>();

        }

        #endregion

        #region Methods
        //-------------------------------------------------------------

        /// <summary>
        /// Add a choice to the effect. 
        /// </summary>
        /// <param name="choicekey"> </param>
        /// <param name="resolutionEffect"> </param>
        public void AddChoice(string choicekey, IEffect resolutionEffect)
        {

        }

        //-------------------------------------------------------------

        public void Apply(ITargetable itargetable)
        {
            UICombatController.Instance.DisplayChoice(
                (IEffect)this, () => ExecuteChoice1(itargetable),
                () => ExecuteChoice2(itargetable));
                  }

        //-------------------------------------------------------------

        /// <summary>
        /// Create choice effect form a description line
        /// </summary>
        /// <param name="words"> words array  </param>
        public void CreateFromLine( string[] words )
        {
            if( words.Length >= 2 )
            {
                _locDescriptionKey = words[1];
            }

            for( int i = 0 ; i < words.Length ; i++ )
            {
                _choicekey.Add( words[i] );
            }
        }


        //-------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------

        protected void ExecuteChoice1(ITargetable targetable)
        {
            foreach (IEffect choices in _choices1)
            {
                choices.Apply(targetable);
            }
        }

        //-------------------------------------------------------------
        protected void ExecuteChoice2(ITargetable targetable)
        {
            foreach (IEffect choices in _choices2)
            {
                choices.Apply(targetable);
            }
        }
        //-------------------------------------------------------------
    }
}
