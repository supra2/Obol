using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    protected Button startGame;
    protected Button optionsButton;
    protected Button optionsQuit;
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
}
