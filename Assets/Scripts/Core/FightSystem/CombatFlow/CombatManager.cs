using Core.FightSystem;
using System.Collections;
using System.Collections.Generic;
using UI.ItemSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatManager : Singleton<CombatManager>
{

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
    /// Fighting Character currentlyPlaying
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

    public CombatVar Var => _vars;


    #endregion

    #region Public Method
    //---------------------------------------------------

    /// <summary>
    /// Start Combat
    /// </summary>
    /// <param name="vars"></param>
    public void StartCombat(CombatVar vars)
    {
        _vars = vars;
        AsyncOperation handler = SceneManager.LoadSceneAsync("CombatScene");
        handler.completed += DelayedStart;

    }

    //---------------------------------------------------

    public void DelayedStart( AsyncOperation handler )
    {

        _vars.NbRound = 0;
        _fightingCharacter = new List<FightingCharacter>();
        _fightingAdversaires = new List<FightingAdversaire>();
         _uiCombatController = 
            GameObject.FindGameObjectWithTag("RootUI").
            GetComponent<UICombatController>();
        _fightStack = GameObject.FindObjectOfType<FightStack>();
        _adversaireLayout = GameObject.
            FindObjectOfType<AdversaireLayout>();
        _heroesLayout = GameObject.
            FindObjectOfType<HeroesLayout>();

        CurrentCombatPhase = CombatPhase.Initialisation;

    }

    //---------------------------------------------------

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

    //---------------------------------------------------

    /// <summary>
    /// End Fight 
    /// </summary>
    public void EndFight()
    {
        PartyManager.Instance.UpdateGroup(_vars.Party );
    }

    //---------------------------------------------------

    public void WinFight()
    {

        PartyManager.Instance.UpdateGroup(_vars.Party);
        LootWindow._adversaireFought = _vars.Adversaires;
        SceneManager.LoadScene("LootScene");
    }

    //---------------------------------------------------

    public void LoseFight()
    {
        SceneManager.LoadScene("GameOver");
    }

    //---------------------------------------------------

    /// <summary>
    /// Get Current Character
    /// </summary>
    /// <returns></returns>
    public Character GetCurrentCharacter()
    {
        return _currentCharacter;

    }

    //---------------------------------------------------

    /// <summary>
    ///  character : 
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public FightingCharacter GetFightingCharacter(Character character)
    {
        return _fightingCharacter.Find( x => x.Character == character );

    }

    //---------------------------------------------------

    /// <summary>
    /// End the turn of the Current Character
    /// </summary>
    public void EndTurn()
    {
       FightingCharacter character = 
            _fightingCharacter.Find((x) => x.Character == _currentCharacter);
       character.EndTurn();
    }

    //---------------------------------------------------

    public void StartNewRound()
    {
        _vars.NbRound++;
        UpdateOrder();
    }

    //---------------------------------------------------

    public void SetOpponentDeck(List<Adversaire> deckContent)
    {

    }

    //---------------------------------------------------
    #endregion

    #region Private Method

    protected void InitiativePhase()
    {
        InitializeHero(_vars);

        InitializeAdversairesUI(_vars);

        InitializeUI(_vars);

        InitiativePhase(0);
    }

    private void InitializeAdversairesUI(CombatVar vars)
    {
        foreach ( Core.FightSystem.Adversaire adversary in vars.Adversaires )
        {
            Core.FightSystem.Adversaire adversaire = ScriptableObject.Instantiate(adversary);
            adversaire.Init();
            adversaire.OnStartTurn.AddListener( CharacterStartTurn );
            _adversaireLayout.AddAdversaire(adversaire);
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
            vars.Party[j].Init();
            _heroesLayout.Add( vars.Party[j] ,
             ( X )=> CharacterStartTurn( X ) );
            _fightingCharacter.Add(
                _heroesLayout[j].
                GetComponent<FightingCharacter>() );
            _fightingCharacter[j].Active = true;
        }
    }

    /// <summary>
    ///  Initialize UI 
    /// </summary>
    /// <param name="vars"></param>
    protected void InitializeUI(CombatVar vars)
    {
        //_uiDebugCombatController.Init(vars);
        _uiCombatController.Init(vars);
    }

    /// <summary>
    ///  Update Orders 
    /// </summary>
    protected void UpdateOrder()
    { 
        int i = 0;
        for (int k = 5 ; k >= 0 ; k--)
        {
            foreach ( Core.FightSystem.Adversaire advers
                in _adversaireLayout )
            { 
                if( advers.GetCharacteristicsByName("Speed") == k )
                {
                    Core.FightSystem.CombatFlow.AdversaireTurn adversaireTurn =
                   new Core.FightSystem.CombatFlow.AdversaireTurn(_adversaireLayout[i], _vars.NbRound );
                    _fightStack.PileBottom( adversaireTurn );
                }
            }
            foreach (FightingCharacter character in _fightingCharacter)
            { 
                if ( _fightingCharacter[i].Active == true && 
                    character.Character.GetCharacteristicsByName("Speed") == k )
                {
                    Core.FightSystem.CombatFlow.CharacterTurn characterTurn =
                    new Core.FightSystem.CombatFlow.CharacterTurn( character , _vars.NbRound);
                    _fightStack.PileBottom(characterTurn);
                }
            }
        }
        _fightStack.PileBottom(new Core.FightSystem.CombatFlow.StartNewRound());
    }

    /// <summary>
    /// Resolve Initiative Phase ( Draw phase 
    /// for the Party before any turn)
    /// </summary>
    /// <param name="CharacterID"> Character Id beeing initialised</param>
    protected void InitiativePhase(int CharacterID)
    {
        if( _fightingCharacter.Count > CharacterID)
        {
            switch (_vars.FightInitiative)
            {
                case CombatVar.Initiative.Normal:
                    _fightingCharacter[CharacterID].Draw( 2 ,
                        () => InitiativePhase( CharacterID + 1 ) );
                    break;
                case CombatVar.Initiative.Opportunity:
                    _fightingCharacter[CharacterID].Draw(3, 
                        () =>  InitiativePhase( CharacterID + 1 ) );
                    break;
                case CombatVar.Initiative.Surprised:
                    _fightingCharacter[CharacterID].Draw(2, () =>
                    _fightingCharacter[CharacterID].Discard(1,
                    ()=> InitiativePhase(CharacterID + 1)));
                    break;
            }
        }
        else
        {
            CurrentCombatPhase = CombatPhase.Combat;
        }

    }

    protected void CharacterStartTurn(Character character)
    {
        _currentCharacter = character;
    }

    protected void MainPhase()
    {
        UpdateOrder();
    }

    protected void LootScreen()
    { 

    }

  
    #endregion

}
