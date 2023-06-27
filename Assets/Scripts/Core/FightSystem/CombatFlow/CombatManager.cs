using Core.FightSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatManager : Singleton<CombatManager>
{

    #region InnerClass

    public class InitiativeSorter : IComparer<ICharacteristic>
    {
        public int Compare(ICharacteristic x, ICharacteristic y)
        {
            return y.GetCharacteristicsByName("Speed")  - x.GetCharacteristicsByName("Speed");
        }
    }

    #endregion

    #region Enum
    public enum CombatPhase
    {
        NotStarted,
        Initialisation,
        Combat,
        End
    }
    #endregion

    #region Members
    #region Visible
    [SerializeField]
    protected List<FightingCharacter> _fightingCharacter;
    [SerializeField]
    protected FightStack _fightStack;
    [Header("Developper")]
    [SerializeField]
    protected DebugUICombatController _uiCombatController;
    #endregion
    #region Hidden
    /// <summary>
    /// nb turn done
    /// </summary>
    protected int nbTurn;
    /// <summary>
    /// Var relative to fight
    /// </summary>
    protected CombatVar _vars;
    /// <summary>
    /// current phase
    /// </summary>
    protected CombatPhase _combatPhase;

    #endregion
    #endregion

    #region public Method


    /// <summary>
    /// Start Combat
    /// </summary>
    /// <param name="vars"></param>
    public void StartCombat( CombatVar vars )
    {
        _vars = vars;

        nbTurn = 0;

        _combatPhase = CombatPhase.Initialisation;

        for (int j = 0;  j < _fightingCharacter.Count; j++ )
        {
            _fightingCharacter[j].Active = j < vars.Party.Count;

            _fightingCharacter[j].Setup( vars.Party[j] );
        }

        InitializeUI( _vars );

        UpdateOrder( );

        InitiativePhase(0 );

    }

    /// <summary>
    ///  Initialize UI 
    /// </summary>
    /// <param name="vars"></param>
    protected void InitializeUI(CombatVar vars)
    {
        _uiCombatController.Init(vars);
    }

    /// <summary>
    ///  Update Orders 
    /// </summary>
    protected void UpdateOrder(  )
    {
        _vars.Party.Sort(new InitiativeSorter( ));
        _vars.Adversaires.Sort(new InitiativeSorter());
        int i = 0;
        foreach ( FightingCharacter character in _fightingCharacter)
        {
            if(character.Active == true)
            {
                while ( character.Character.GetCharacteristicsByName("Speed") <=
                    _vars.Adversaires[i].GetCharacteristicsByName("Speed") 
                    && i < _vars.Adversaires.Count)
                {
                    Core.FightSystem.CombatFlow.AdversaireTurn adversaireTurn =
                       new Core.FightSystem.CombatFlow.AdversaireTurn(_vars.Adversaires[i], nbTurn);
                    i++;
                }
                Core.FightSystem.CombatFlow.CharacterTurn characterTurn =
                       new Core.FightSystem.CombatFlow.CharacterTurn(character, nbTurn);
                _fightStack.Pile(characterTurn);
               
            }
        }
    }

    protected void InitiativePhase(int CharacterID  )
    {

       switch(_vars.FightInitiative)
        {
            case CombatVar.Initiative.Normal:

                break;
            case CombatVar.Initiative.Opportunity:
                _fightingCharacter[CharacterID].Draw(1);
                break;
            case CombatVar.Initiative.Surprised:
                //_listcharacterSubject[CharacterID].ChangeState("Discarding");
                break;
        }
    }

    
    #endregion

}
