using Core.FightSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static AdversaireDisplayer;
using System;
using Core.FightSystem.AttackSystem;

public class Adversaire : UnityEvent<AdversaireDisplayer>
{ 

}

public class AdversaireLayout : MonoBehaviour
{

    #region Members
    #region Visible
    /// <summary>
    /// Prefab for Displaying Adversaries
    /// </summary>
    [SerializeField]
    protected AdversaireDisplayer _prefabDisplayers;
    /// <summary>
    /// Message displayed when picking an adversaries
    /// </summary>
    [SerializeField]
    protected RectTransform _messagePicking;
    /// <summary>
    /// Popup Screen for adversaire selection
    /// </summary>
    [SerializeField]
    protected RectTransform _adversaireScreen;
    #endregion
    #region Hidden
    protected List<AdversaireDisplayer> _displayers;
    protected Action<ITargetable> _callback;
    #endregion
    #endregion

    #region Initialisation
    
    public void Awake()
    {
        _displayers = new List<AdversaireDisplayer>();
        _adversaireScreen.gameObject.SetActive(false);
    }

    #endregion

    #region Public Methods
    //-------------------------------------------------------

    public void AddAdversaire(
        Core.FightSystem.Adversaire _adversaire)
    {
        AdversaireDisplayer displayer =
            GameObject.Instantiate(_prefabDisplayers, transform);
        displayer.Adversaire = _adversaire;
        _displayers.Add(displayer);
    }

    //-------------------------------------------------------

    public void SelectAdversaireMode( 
        Action<ITargetable> callback )
    {
        foreach (AdversaireDisplayer advDisplayer 
            in _displayers)
        {
            advDisplayer.SetMode(
                AdversaireDisplayerMode.Selection);
            _callback = callback;
            advDisplayer._onAdversairePicked.AddListener(
                AdversarySelected);
            advDisplayer._onAdversairePicked.AddListener(
                StopSelectingAdversaries);
        }
        _adversaireScreen.gameObject.SetActive(true);
    }

    //-------------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    public void AdversarySelected( Character character )
    {
        _callback?.Invoke((ITargetable)character);
    }

    //-------------------------------------------------------

    /// <summary>
    /// Stop Selecting Adversaries
    /// </summary>
    /// <param name="character"></param>
    public void StopSelectingAdversaries( Character character )
    {
        foreach (AdversaireDisplayer advDisplayer in _displayers)
        {
            advDisplayer.SetMode( AdversaireDisplayerMode.Neutral );
            advDisplayer._onAdversairePicked.RemoveListener(AdversarySelected);
            advDisplayer._onAdversairePicked.RemoveListener(
                StopSelectingAdversaries);
        }

        _adversaireScreen.gameObject.SetActive(false);
    }
   
    //-------------------------------------------------------
    #endregion

}
