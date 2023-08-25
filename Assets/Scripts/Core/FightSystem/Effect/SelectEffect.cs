using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEffect : NestedEffect
{
    #region Enum
    protected enum Method
    {
        Random,
        StatsCriteria
    }
    protected enum TestType
    {
        LessThan,
        Equal,
        MoreThan
    }
    #endregion

    #region Members
    protected List<IEffect> _nestedEffect;
    protected Method _methodSelection;

    protected string _comparisonStat;
    protected TestType _testType;

    protected int _value;
    #endregion

    public void Apply(ITargetable itargetable)
    {
        List<ITargetable> listTargetable = new List<ITargetable>(); 
        switch(_methodSelection)
        {
            case Method.Random:
                listTargetable = SelectRandom();
                    break;
            case Method.StatsCriteria:
                listTargetable = SelectByStatsCriteria();
                break;
        }

        foreach(ITargetable  target in listTargetable)
        {
            foreach(IEffect effect in _nestedEffect)
            {
                effect.Apply(target);
            }
        }

    }

    public void CreateFromLine(string[] words)
    {
        switch(words[2])
        {
            case "Random":
                System.Int32.TryParse(words[1], out _value);
                break;
            case "Less":
                _comparisonStat = words[1].Substring(1, words.Length - 2);
                _testType = TestType.LessThan;
                break;
            case "More":
                _testType = TestType.MoreThan;
                break;
            case "Equal":
                _testType = TestType.Equal;
                break;

        }
    }

    public bool SelfTarget()
    {
        return false;
    }

    public void SetNestedEffect(List<IEffect> listeffect)
    {
        _nestedEffect = listeffect;
    }

    public List<ITargetable> SelectRandom()
    {
        CombatVar var =  CombatManager.Instance.Var;
        List<PlayableCharacter> characters = var.Party;

        List<ITargetable> itargetable = new List<ITargetable>();
        if (characters.Count <= _value)
        {
            foreach (PlayableCharacter character in characters)
            {
                itargetable.Add(character);
            }

        }
        else
        { 
            int roll = 0;
            do
            {
                roll = SeedManager.NextInt(0, characters.Count - 1);
            } while (!itargetable.Contains(characters[roll]));
        }
        return itargetable;
    }

    public List<ITargetable> SelectByStatsCriteria()
    {
        CombatVar var = CombatManager.Instance.Var;
        List<PlayableCharacter> characters = var.Party;
        List<PlayableCharacter> targetList = null;

        List<ITargetable> itargetable = new List<ITargetable>();
        switch (_testType)
        {

            case TestType.Equal:
                targetList = characters.FindAll(x => x.GetCharacteristicsByName(_comparisonStat) ==
                CombatManager.Instance.GetCurrentCharacter().GetCharacteristicsByName(_comparisonStat));
                    break;
            case TestType.LessThan:
                targetList = characters.FindAll(x => x.GetCharacteristicsByName(_comparisonStat) <
                CombatManager.Instance.GetCurrentCharacter().GetCharacteristicsByName(_comparisonStat));
                break;
            case TestType.MoreThan:
                targetList = characters.FindAll(x => x.GetCharacteristicsByName(_comparisonStat) >
                CombatManager.Instance.GetCurrentCharacter().GetCharacteristicsByName(_comparisonStat));
                break;
        }
        foreach (PlayableCharacter character in targetList)
        {
            itargetable.Add(character);
        }
        return itargetable;
    }
}
