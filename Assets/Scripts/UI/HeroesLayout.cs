using Core.FightSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Heroes  Layout: Organise the UI 
/// displaying order Heroes + Action Panel for the current heroess
/// </summary>
public class HeroesLayout : MonoBehaviour
{

    #region Member
    /// <summary>
    /// Base Prefab class for dynamic instantiation
    /// </summary>
    [SerializeField]
    protected CharacterDisplayer _characterDisplayerPrefab;
    //Instances List
    protected List<CharacterDisplayer> _characterDisplayerList;
    #endregion

    #region Getter
    /// <summary>
    /// Accessor on the Displayer gameobject list
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public GameObject this[int i] { get => _characterDisplayerList[i].gameObject; }
    #endregion

    #region Initialisation


    /// <summary>
    /// Add A new Character to display in the UI 
    /// Plug every event needed in order to display the different state of 
    /// the a caracter/ Dispatch interaction
    /// </summary>
    /// <param name="character"> Playable Character to display</param>
    /// <param name="StartTurnCallBack"> Start Turn Callback</param>
    /// <param name="EndTurnCallBack"> End Turn Callback</param>
    public void Add(PlayableCharacter character, UnityAction<Character> StartTurnCallBack=null,
        UnityAction<Character> EndTurnCallBack = null)
    {
        if(_characterDisplayerList == null )
        {
            _characterDisplayerList = new List<CharacterDisplayer>();
        }
            
       CharacterDisplayer characterDisplayer = 
            GameObject.Instantiate(_characterDisplayerPrefab, transform);

       
        _characterDisplayerList.Add(characterDisplayer);
        FightingCharacter fightingCharacter = characterDisplayer.GetComponent<FightingCharacter>();
      
        fightingCharacter.Setup(character);
        characterDisplayer.Init(fightingCharacter.Deck, fightingCharacter.DiscardPile, fightingCharacter.Hand);

        if (StartTurnCallBack != null)
        fightingCharacter.OnTurnStarted.AddListener(StartTurnCallBack);
        if (EndTurnCallBack != null)
            fightingCharacter.OnTurnEnded.AddListener(EndTurnCallBack);
    }


    #endregion

}
