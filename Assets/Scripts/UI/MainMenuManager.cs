using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{

    #region Members
    #region Visible
    [SerializeField]
    protected LocalizedString _startButtonLocalisation;
    [SerializeField]
    protected LocalizedString _optionsButtonLocalisation;
    [SerializeField]
    protected LocalizedString _resumeButtonLocalisation;
    [SerializeField]
    protected LocalizedString _quitButtonLocalisation;
    [SerializeField]
    protected Animator _animatorBalance;
    #endregion
    #region Hidden
    protected Button startGame;
    protected Button optionsButton;
    protected Button optionsQuit;
    #endregion
    #endregion

    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Initialize the character list controller
        var characterListController = new CharacterListController();
        Initialize(uiDocument.rootVisualElement);
    }

    void OnDisable()
    {

    }

    public void Initialize(VisualElement root)
    {
        startGame = root.Q<Button>("ButtonStartGame");
        optionsButton = root.Q<Button>("ButtonOptions");
        optionsQuit = root.Q<Button>("ButtonQuitter");
        startGame.clicked += StartGame;
        optionsButton.clicked += OpenOptions;
        optionsQuit.clicked += Quit;
        startGame.RegisterCallback<MouseOverEvent>((e) => { ResetTriggers();  _animatorBalance.SetTrigger("pos3"); });
        optionsButton.RegisterCallback<MouseOverEvent>((e) => { ResetTriggers();  _animatorBalance.SetTrigger("pos2"); });
        optionsQuit.RegisterCallback<MouseOverEvent>((e) => { ResetTriggers(); _animatorBalance.SetTrigger("pos1"); });
        _startButtonLocalisation.StringChanged += SetLocStartGame;
        _optionsButtonLocalisation.StringChanged += SetLocOptions;
        _quitButtonLocalisation.StringChanged += SetLocQuit;
    }


    #region UI Callback

    protected void StartGame()
    {
        GameManager.Instance.StartNewGame();
    }
    protected void OpenOptions()
    {
    }
    protected void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region Localization

    public void SetLocStartGame(string newValue)
    {
        startGame.text = newValue;
    }

    public void SetLocOptions(string newValue)
    {
        optionsButton.text = newValue;
    }

    public void SetLocQuit(string newValue)
    {
        optionsQuit.text = newValue;
    }

    #endregion

    #region Animation

    protected void ResetTriggers()
    {
        for (int i = 1; i <= 3; i++)
        {
            _animatorBalance.ResetTrigger($"pos{i}");
        }
    }

    #endregion
}
