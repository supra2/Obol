using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCombat : MonoBehaviour
{

    #region Members
    #region Visible
    [SerializeField]
    protected CombatManager _combatManager;

    [SerializeField]
    protected List<Core.FightSystem.PlayableCharacter> _heroParty;

    [SerializeField]
    protected List<Core.FightSystem.Adversaire> _adversaires;

    [SerializeField]
    protected CombatVar.Initiative _initState;
    #endregion
    #endregion

    #region Initialisation

    public void Awake()
    {

        SeedManager.GenerateRandomSeed();
        CombatVar combat = new CombatVar();
        combat.Adversaires = _adversaires;
        combat.Party = _heroParty;
        combat.FightInitiative = _initState;
        _combatManager.StartCombat(combat);

    }

    #endregion

}
