using Core.Exploration;
using Core.FightSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[Serializable]
public class GameData
{
    #region Members
    [SerializeField]
    Party _party;

    [SerializeField]
    Level _currentLevel;

    [SerializeField]
    Vector2 _playerPosition;

    [SerializeField]
    bool _inFight;


    [SerializeField]
    string _seed;
    #endregion

    public Party Party{get => _party; set=> _party = value; }

    public Level CurrentLevel { get => _currentLevel; set => _currentLevel = value; }

    public Vector2 PlayerPosition { get => _playerPosition; set => _playerPosition = value; }

    public bool InFight { get => _inFight; set => _inFight = value; }

    public string Seed { get => _seed; set => _seed = value; }
}
[Serializable]
public class ZoneInfo
{
    public GameManager.Zones zone;
    public Level level;
}

/// <summary>
/// Manager a game session from loading data to saving the data and comming back to the 
/// Main Menu
/// </summary>
public class GameManager
    : Singleton<GameManager>
{
    public enum Zones
    {
        Tutoriel,
        FishMarket,
        BasseVille,
        Port,
        CrescentBeach,
        QuietHill,
        OldInsdutrialBlock
    }

    #region Members
    #region Visible
    /// <summary>
    /// Exploration manager
    /// </summary>
    [SerializeField]
    protected ExplorationManager _manager;
    /// <summary>
    /// level 1 : Tutoriel
    /// </summary>
    [SerializeField]
    protected ZoneInfo[] _levels;
    /// <summary>
    /// Seed Manager
    /// </summary>
    [SerializeField]
    protected SeedManager _seedManager;
    /// <summary>
    /// Exploration manager
    /// </summary>
    [SerializeField]
    protected TileManager _tileManager;
    /// <summary>
    /// list 
    /// </summary>
    [SerializeField]
    protected List<PlayableCharacter> _character;
    #endregion
    #region Hidden
    protected CombatVar _vars;
    /// <summary>
    /// reference to the Party manager
    /// </summary>
    [SerializeField]
    protected PartyManager _partyManager;
    //Exploration manager
    protected ExplorationManager _explorationManager;
    /// <summary>
    /// game Data
    /// </summary>
    protected GameData _gameData;
    #endregion
    #endregion

    #region Getters 
    /// <summary>
    /// reference to the Party manager
    /// </summary>
    public PartyManager PartyManager => _partyManager;
    //Exploration manager
    public ExplorationManager ExplorationManager => _explorationManager;
    #endregion

    #region Initialisation
    //--------------------------------------------------------

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        _partyManager = new PartyManager();
        _partyManager.Party.CharacterParty = _character;
    }

    //--------------------------------------------------------
    #endregion

    #region MainMenuOptions
    //--------------------------------------------------------

    public void StartNewGame()
    {
        SceneManager.LoadSceneAsync("ExplorationScene");
        SceneManager.sceneLoaded += LaunchTutorielLevel;
        _gameData = new GameData();
        _gameData.Seed =  SeedManager.GenerateRandomSeed();
    }

    //--------------------------------------------------------

    public void ResumeGame(string filename)
    {
        LoadDatas(filename);
        SceneManager.LoadSceneAsync("ExplorationScene");
        SceneManager.sceneLoaded += LaunchLoadedLevel;
    }

    //--------------------------------------------------------

    public void LoadDatas(string filename)
    {
        _gameData = Load(filename);
        _partyManager.SetParty(_gameData.Party);

    }

    //--------------------------------------------------------

    public GameData Load(string filename)
    {
        string persistantDatapath = System.IO.Path.
            Combine(Application.persistentDataPath, "Save", "save.json");
        string jsonContent = null;
#if UNITY_ANDROID && !UNITY_EDITOR
        WWW www = new WWW (persistantDatapath);
        while (!www.isDone) {}
        if (string.IsNullOrEmpty(www.error))
        {
            jsonContent = www.text;
        }
        else
        {
            Debug.Log ("No such file");
        }
#else
        if (File.Exists(persistantDatapath))
        {
            jsonContent = File.ReadAllText(persistantDatapath);
        }
        else
        {
            Debug.Log("No such file");
        }
#endif
        return JsonUtility.FromJson<GameData>(jsonContent);
    }

    //--------------------------------------------------------

    public void Save(string filename)
    {
        string persistantDatapath = System.IO.Path.Combine(
            Application.persistentDataPath, "Save",
            "save.json");

        GameData gamedata = new GameData();
        gamedata.Party = _partyManager.Party;
        gamedata.PlayerPosition = _explorationManager.PlayerPosition;
        gamedata.CurrentLevel = _explorationManager.CurrentLevel;
        File.WriteAllText(persistantDatapath,
            JsonUtility.ToJson(gamedata));
    }

    //--------------------------------------------------------

    /// <summary>
    /// Launch Tutoriel Level
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void LaunchTutorielLevel(Scene scene, LoadSceneMode mode)
    {
        if (_explorationManager == null)
        {
            _explorationManager = GameObject.
                FindFirstObjectByType<ExplorationManager>();
            _explorationManager.Init(_levels[0].level);
        }
    }

    //--------------------------------------------------------

    /// <summary>
    /// Launch Tutoriel Level
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void LaunchLoadedLevel(Scene scene, LoadSceneMode mode)
    {
        if (_explorationManager == null)
        {
            _explorationManager = GameObject.
                FindFirstObjectByType<ExplorationManager>();
            _explorationManager.Load(_gameData.CurrentLevel, _gameData.PlayerPosition);
        }
    }

    //--------------------------------------------------------
    #endregion

    #region CombatManagemenet 
    //--------------------------------------------------------

    public void StartCombat(CombatVar vars)
    {
        _vars = vars;
        _gameData.InFight = true;
        SceneManager.LoadScene("CombatScene");
        SceneManager.sceneLoaded += DelayedCombatInit;
    }

    //--------------------------------------------------------

    public void DelayedCombatInit( Scene newScene , LoadSceneMode mode )
    {
       GameObject[] go = newScene.GetRootGameObjects();
       foreach( GameObject g in go)
       {
            if( g.transform.name == "CombatManager" )
            {
                g.GetComponent<CombatManager>().Var = _vars;
            }
       }
    }

    //--------------------------------------------------------
    #endregion
}
