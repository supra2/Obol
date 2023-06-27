using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUICombatController : MonoBehaviour
{
    public void Init(CombatVar vars)
    {

        Debug.Log(" Encountering an ennemy encounter");
        Debug.LogFormat(" Enemy composition :  ", string.Join(" ", vars.Adversaires));
        Debug.Log(" Your party is ");
        Debug.LogFormat(" Enemy composition :  ", string.Join(" ", vars.Party));
    }
}

