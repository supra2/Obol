using Core.FightSystem.AttackSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Test
    {

        #region members
        protected string _ability;
        protected int _value;
        protected bool _staticValue;
        protected TestType _testType;

        #endregion

        #region Enum

        public enum TestType
        {
            None,
            LessThan,
            Equal,
            MoreThan,
            Coinflip
        }

        #endregion

        #region Initialisation

        public Test(string ability, TestType Operator, bool staticvalue, int testvalue =0)
        {
            _ability = ability;
            _testType = Operator;
            _value = testvalue;
            _staticValue = staticvalue;
        }

        #endregion

        #region Getter 

        public bool StaticValue => _staticValue;

        public string Ability => _ability;

        #endregion

        #region Members

        public bool RunTest( ITargetable targetable1 , int valueComp , Action<bool> Callback = null) 
        {
            int comparisonValue = _staticValue ? _value :
                CombatManager.Instance.GetCurrentCharacter().GetCharacteristicsByName(_ability);
            bool testValue = false;
            switch ( _testType )
            {
                case TestType.Equal:
                    testValue = valueComp == comparisonValue;
                    break;
                case TestType.LessThan:
                    testValue = valueComp < comparisonValue;
                    break;
                case TestType.MoreThan:
                    testValue = valueComp > comparisonValue;
                    break;
                case TestType.Coinflip:
                    CoinFlipManager.Instance.Flip(comparisonValue, valueComp, Callback, false);
                     
                    break;
            }
            return testValue;
        }


        #endregion

    }
}
