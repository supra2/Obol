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
            return y.GetCharacteristicsByName("Speed") - x.GetCharacteristicsByName("Speed");
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
    protected List<FightingAdversaire> _fightingAdversaires;
    [SerializeField]
    protected AdversaireLayout _adversaireLayout;
    [SerializeField]
    protected HeroesLayout _heroesLayout;
    [SerializeField]
    protected FightStack _fightStack;
    [SerializeField]
    protected UICombatController _uiCombatController;
    [Header("Developper")]
    [SerializeField]
    protected DebugUICombatController _uiDebugCombatController;
    #endregion
    #region Hidden
    /// <summary>
    /// nb turn done
    /// </summary>
    protected int nbTurn;
    /// <summary>
    /// character init id
    /// </summary>
    protected int _characterID;
    /// <summary>
    /// Var relative to fight
    /// </summary>
    protected CombatVar _vars;
    /// <summary>
    /// current phase
    /// </summary>
    protected CombatPhase _combatPhase;
    /// <summary>
    /// Character
    /// </summary>
    protected Character _currentCharacter;
    /// <summary>
    /// the X value
    /// </summary>
    public int X;
    #endregion
    #endregion

    #region Getter
    public FightStack CommandStack => _fightStack;

    public CombatPhase CurrentCombatPhase
    {
        get
        {
            return _combatPhase;
        }
        set
        {
            _combatPhase = value;
            ChangeState(_combatPhase);
        }
    }
    #endregion

    #region public Method


    /// <summary>
    /// Start Combat
    /// </summary>
    /// <param name="vars"></param>
    public void StartCombat(CombatVar vars)
    {
        _vars = vars;

        nbTurn = 0;

        CurrentCombatPhase = CombatPhase.Initialisation;

    }

    /// <summary>
    /// Handle Change State
    /// </summary>
    /// <param name="combatPhase"> Combat Phase </param>
    public void ChangeState(CombatPhase combatPhase)
    {
        switch (combatPhase)
        {
            case CombatPhase.Initialisation:
                InitiativePhase();
            break;
            case CombatPhase.Combat:
                UpdateOrder();
            break;
        }
    }

    /// <summary>
    /// from Vars initialise UI + data structurs
    /// then prepare the state of the game befor turns.
    /// </summary>
    public void InitiativePhase()
    {

        InitializeHero(_vars);

        InitializeAdversairesUI(_vars);

        InitializeUI(_vars);

        InitiativePhase(0);
    }

    private void InitializeAdversairesUI(CombatVar vars)
    {
        foreach (Core.FightSystem.Adversaire adersaire in vars.Adversaires)
        {
            adersaire.Init();
            _adversaireLayout.AddAdversaire(adersaire);
        }
    }

    /// <summary>
    /// Initialize Dynamic UI Element relative to the 
    /// Heroes party data
    /// </summary>
    /// <param name="vars"></param>
    private void InitializeHero(CombatVar vars)
    {
        for ( int j = 0 ; j < vars.Party.Count ; j++ )
        {
            _heroesLayout.Add( vars.Party[j],
             (X)=> CharacterStartTurn(X) );
            _fightingCharacter.Add(_heroesLayout[j].GetComponent<FightingCharacter>());
            _fightingCharacter[j].Active = true;
        }
    }

    /// <summary>
    ///  Initialize UI 
    /// </summary>
    /// <param name="vars"></param>
    protected void InitializeUI(CombatVar vars)
    {
        _uiDebugCombatController.Init(vars);
        _uiCombatController.Init(vars);
    }

    /// <summary>
    ///  Update Orders 
    /// </summary>
    protected void UpdateOrder()
    {
        _vars.Party.Sort(new InitiativeSorter());
        _vars.Adversaires.Sort(new InitiativeSorter());
        int i = 0;
        foreach (FightingCharacter character in _fightingCharacter)
        {
            if (character.Active == true)
            {
                while (character.Character.GetCharacteristicsByName("Speed") <=
                    _vars.Adversaires[i].GetCharacteristicsByName("Speed")
                    && i < _vars.Adversaires.Count)
                {

                    Core.FightSystem.CombatFlow.AdversaireTurn adversaireTurn =
                       new Core.FightSystem.CombatFlow.AdversaireTurn(_vars.Adversaires[i], nbTurn);
                    _fightStack.Pile(adversaireTurn);
                    i++;
                }
                Core.FightSystem.CombatFlow.CharacterTurn characterTurn =
                       new Core.FightSystem.CombatFlow.CharacterTurn(character, nbTurn);
                _fightStack.Pile(characterTurn);
            }
        }
    }

    protected void InitiativePhase(int CharacterID)
    {
        if( _fightingCharacter.Count > CharacterID)
        {
            Debug.Log("Initiative Phase " + CharacterID);
            switch (_vars.FightInitiative)
            {
                case CombatVar.Initiative.Normal:
                    _fightingCharacter[CharacterID].Draw( 2 );
                    InitiativePhase(CharacterID + 1);
                    break;
                case CombatVar.Initiative.Opportunity:
                    _fightingCharacter[CharacterID].Draw(3);
                    InitiativePhase(CharacterID + 1);
                    break;
                case CombatVar.Initiative.Surprised:

                    _fightingCharacter[CharacterID].Draw(2);
                    _fightingCharacter[CharacterID].Discard(1,
                    ()=> InitiativePhase(CharacterID + 1));
                    break;
            }
        }
        else
        {
            Debug.Log("Go to combat phase ");
            CurrentCombatPhase = CombatPhase.Combat;
        }

    }

    protected void CharacterStartTurn(Character character)
    {
        _currentCharacter = character;
    }

    public Character GetCurrentCharacter()
    {
        return _currentCharacter;

    }

    protected void MainPhase()
    {
        UpdateOrder();
    }

    #endregion

}
