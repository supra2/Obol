using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitalState : ICombatState
{
    
    public void Start(CombatVar var)
    {
        switch ( var.FightInitiative)
        {
            case CombatVar.Initiative.Normal:
                break;
            case CombatVar.Initiative.Opportunity:
                foreach (Characters characters in var.Party)
                {
                    characters.Draw(1);
                }
                break;
            case CombatVar.Initiative.Surprised:
                foreach (Characters characters in var.Party)
                {
                    characters.Discard(1);
                }
                break;
        }
    }

    public void Exec(CombatVar vars)
    {
        throw new System.NotImplementedException();
    }

    public void Stop(CombatVar vars)
    {
        throw new System.NotImplementedException();
    }

    public ICombatState GetNextState()
    {
        throw new System.NotImplementedException();
    }

}
