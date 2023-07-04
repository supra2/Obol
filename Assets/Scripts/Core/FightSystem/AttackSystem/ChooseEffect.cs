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
        /// Choices 
        /// </summary>
        protected List<IEffect> _choices1;

        /// <summary>
        /// Choices 
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
                (IEffect)this , ExecuteChoice1 , ExecuteChoice2 );
            if ( choose == 0 )
            {
           
            }
            else
            {
                foreach (IEffect choices in _choices2 )
                {

                }
                    
            }
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

        protected void ExecuteChoice1()
        {
            foreach (IEffect choices in _choices1)
            {

            }
        }

        //-------------------------------------------------------------
        protected void ExecuteChoice2()
        {
            foreach (IEffect choices in _choices2)
            {

            }
        }
        //-------------------------------------------------------------
    }
}
