using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCombat : MonoBehaviour
{

    [SerializeField]
    protected CombatManager _combatManager;

    [SerializeField]
    protected List<Character> _heroParty;

    [SerializeField]
    protected List<Adversaire> _adversaires;

    [SerializeField]
    protected CombatVar.Initiative _initState;

    public void Awake()
    {
        CombatVar combat = new CombatVar();
        combat.Adversaires = _adversaires;
        combat.Party = _heroParty;
        combat.FightInitiative = _initState;
        _combatManager.StartCombat(combat);
    }
}
