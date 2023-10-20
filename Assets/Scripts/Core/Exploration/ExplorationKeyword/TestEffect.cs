using Core.FightSystem.AttackSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exploration.ExplorationKeyword
{
    public class TestEffect : NestedEffect
    {

        #region Members

        protected Test _test;

        protected int _value;

        protected List<IEffect> _nestedeffect;

        protected bool effectOnSuccess;

        protected ITargetable _target;
        #endregion

        #region class

        public class TestBuilder : IWordBuilder
        {

            public IEffect BuildEffect(string[] words)
            {
                IEffect TestEffect = new TestEffect();
                TestEffect.CreateFromLine(words);
                return TestEffect;
            }

            public string GetKeyWord()
            {
                return "Test";
            }

            public bool NestedKeyword()
            {
                return true;
            }

        }

        #endregion

        #region Interface Implementation

        public void Apply(ITargetable itargetable)
        {
            _test.RunTest(itargetable, _value, Resolve);
        }

        public void CreateFromLine(string[] words)
        {
            if (System.Int32.TryParse(words[2], out _value))
            {
                _test = new Test(words[1], Test.TestType.Coinflip, true, _value);
            }
            effectOnSuccess = string.Compare(words[3], "Success") == 0;
        }

        public bool SelfTarget()
        {
            return true;
        }

        public void SetNestedEffect(List<IEffect> listeffect)
        {
            _nestedeffect = listeffect;
        }

        public void Resolve( bool success )
        {
            if (success == effectOnSuccess)
            {
                foreach (IEffect ieffect in _nestedeffect)
                {
                    ieffect.Apply( _target );
                }
            }
        }

        #endregion
    }
}
