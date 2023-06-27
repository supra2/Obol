using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatState 
{
    public void Start(CombatVar vars);

    public void Exec(CombatVar vars);

    public void Stop(CombatVar vars);


}
