using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.FightSystem.AttackSystem
{
    
    public enum DamageType
    {
        Health,
        San,
    }

    public enum DamageComputation
    {
        Static,
        XValue,
        StatsValue
    }

    public class InflictEffect : IEffect
    {

        #region members
        int _damageValue;
        string _competence;
        DamageType _damagetype;
        DamageComputation _methodofComputation;
        #endregion

        #region Initialisation

        public void CreateFromLine( string[] words )
        {
            _damageValue = 0;
            for ( int i = 1; i < words.Length ; i++ )
            {
                  string w = words[i].Trim().ToLower();
                  if (!string.IsNullOrEmpty(w))
                  {
                        if( w[0] == '[')
                        {
                            _methodofComputation= DamageComputation.StatsValue;
                            _competence = w.Substring(1, w.Length - 2);
                        }
                        else if ( w[0] == 'X' )
                        {
                            _methodofComputation = DamageComputation.XValue;
                        }
                        else if ( w[ w.Length - 1 ] == '%' )
                        {
                            _damageValue = System.Int32.Parse(w.Substring(0, w.Length - 1));
                        }
                  }
            }
        }

        public InflictEffect(int Value,DamageType dt)
        {
            _damageValue = Value;

            _damagetype = dt;
        }

        #endregion

        public void Apply(ITargetable itargetable)
        {
            switch (_methodofComputation)
            {
                case DamageComputation.Static:
                itargetable.Inflict(_damagetype, _damageValue);
                    break;
                case DamageComputation.XValue:
                    itargetable.Inflict(_damagetype, Mathf.CeilToInt(_damageValue / 100f) * CombatManager.Instance.X );
                    break;

                case DamageComputation.StatsValue:
                    int currentStatsValue = CombatManager.Instance.GetCurrentCharacter().GetCharacteristicsByName(_competence);
                    itargetable.Inflict(_damagetype, Mathf.CeilToInt(_damageValue / 100f) * currentStatsValue);
                    break;
            }
        }

        public bool SelfTarget()
        {
            return false;
        }
    }

}
