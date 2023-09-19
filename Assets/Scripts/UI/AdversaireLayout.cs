using Core.FightSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static AdversaireDisplayer;
using System;
using Core.FightSystem.AttackSystem;

public class AdversaireEvent : UnityEvent<AdversaireDisplayer>
{ 

}

public class AdversaireLayout : MonoBehaviour,IEnumerable<Core.FightSystem.Adversaire>
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
    /// <summary>
    /// Attack Display : attack display
    /// </summary>
    [SerializeField]
    protected AttackDisplay _attackDisplay;
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

    #region Getters

    public Core.FightSystem.Adversaire this[int i]
    {
        get
        {
            return _displayers[i].Adversaire;
        }
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
        displayer._onDisplayDestroyed.AddListener(DisplayerDestroyed);
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

    public void ShowAttack(Attack attack)
    {
        _attackDisplay.ShowAttackDescription(attack);
    }

    //-------------------------------------------------------

    public IEnumerator<Core.FightSystem.Adversaire> GetEnumerator()
    {
        return (IEnumerator<Adversaire>)
        new AdversaireLayoutEnum(_displayers);

    }

    //-------------------------------------------------------

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (IEnumerator<AdversaireEvent>)
            new AdversaireLayoutEnum(_displayers);
    }

    //-------------------------------------------------------
    #endregion

    #region Protected method 


    protected void DisplayerDestroyed(Character c )
    {
        if( !(c is Adversaire ))
        {
            throw new Exception("Character not an adversaire");
        }

        Adversaire adv = c as Adversaire;
        AdversaireDisplayer display = _displayers.Find((x) => x.Adversaire == adv);
        _displayers.Remove(display);

        GameObject.Destroy(display.gameObject);

        if(_displayers.Count ==0)
        {
            CombatManager.Instance.WinFight();
        }
    }
    #endregion

}

public class AdversaireLayoutEnum : IEnumerator<Core.FightSystem.Adversaire>
{

    #region Members
    List<AdversaireDisplayer> _adversaireDisplayers;
    int i = -1;
    public Core.FightSystem.Adversaire Current =>  _adversaireDisplayers[i].Adversaire;
    #endregion



    object IEnumerator.Current => _adversaireDisplayers[i].Adversaire;

    public AdversaireLayoutEnum(List<AdversaireDisplayer> adversaireDisplayer)
    {
        _adversaireDisplayers = adversaireDisplayer;
    }

    public void Dispose()
    {
        _adversaireDisplayers = null;
    }

    public bool MoveNext()
    {
        bool not_end = i < _adversaireDisplayers.Count-1;
        if (not_end)
        {
            i++;
        }
        return not_end;
    }

    public void Reset()
    {
        i = -1;
    }


}
