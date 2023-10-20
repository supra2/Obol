using Core;
using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEffect : NestedEffect
{

    #region Inner Class

    public class SelectBuilder : IWordBuilder
    {

        public IEffect BuildEffect(string[] words)
        {
            IEffect effect = new SelectEffect();
            effect.CreateFromLine(words);
            return effect;
        }

        public string GetKeyWord()
        {
            return "Select";
        }

        public bool NestedKeyword()
        {
            return true;
        }

    }

    #endregion
    
    #region Members

    protected List<IEffect> _nestedEffect;
    protected Method _methodSelection;
    protected Test test;
    protected int _value;
    #endregion

    #region Enum 
    protected enum Method
    {
        Random,
        StatsCriteria
    }
    #endregion

    public void Apply(ITargetable attacker)
    {

        List<ITargetable> listTargetable = new List<ITargetable>();

        switch(_methodSelection)
        {
            case Method.Random:
                listTargetable = SelectRandom( );
                    break;
            case Method.StatsCriteria:
                listTargetable = SelectByStatsCriteria(attacker);
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
        string _comparisonStat;
        Test.TestType testType = Test.TestType.None;
        switch (words[2]){
            case "Random":
                System.Int32.TryParse(words[1], out _value);
                break;
            case "<":
                testType = Test.TestType.LessThan;
                _methodSelection = Method.StatsCriteria;
                break;
            case ">":
                testType = Test.TestType.MoreThan;
                _methodSelection = Method.StatsCriteria;
                break;
            case "=":
                testType = Test.TestType.Equal;
                _methodSelection = Method.StatsCriteria;
                break;
        }

        if (_methodSelection == Method.StatsCriteria)
        {
            _comparisonStat = words[1].Substring(1, words.Length - 2);
            if( words[3] == "this"  )
            {
                test = new Test(_comparisonStat, testType, false);
            }else
            {
                int value = 0;
                 int.TryParse(words[2], out value);
                new Test(_comparisonStat, testType, true, value);
            }
           
        }
     
        
    }

    public bool SelfTarget()
    {
        return true;
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
            while (itargetable.Count> _value )
            {
                 roll = SeedManager.NextInt(0, characters.Count - 1);
                if (!itargetable.Contains(characters[roll]))
                {
                    itargetable.Add(characters[roll]);
                }
            } 
        }
        return itargetable;
    }

    public List<ITargetable> SelectByStatsCriteria(ITargetable attacker)
    {
        CombatVar var = CombatManager.Instance.Var;
        List<PlayableCharacter> characters = var.Party;
        List<PlayableCharacter> targetList = null;

        List<ITargetable> itargetable = new List<ITargetable>();
        targetList = characters.FindAll( (X) => test.RunTest( X, test.StaticValue ? _value: 
            ((Character) attacker).GetCharacteristicsByName(test.Ability)));
        foreach (PlayableCharacter character in targetList)
        {
            itargetable.Add(character);
        }
        return itargetable;
    }
}


