using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class FleeWindow : MonoBehaviour
{

    #region Members
    /// <summary>
    /// localize String for flee success
    /// </summary>
    [SerializeField]
    protected LocalizedString _fleeSucces;
    /// <summary>
    /// Localize String for flee failure
    /// </summary>
    [SerializeField]
    protected LocalizedString _fleeFailure;
    /// <summary>
    /// Localize String for flee Partial Failure
    [SerializeField]
    protected LocalizedString _fleePartialFailure;
    /// <summary>
    /// Localize String for flee Partial Failure
    [SerializeField]
    protected LocalizeStringEvent _message;
    /// <summary>
    /// Close Popup Button
    /// </summary>
    [SerializeField]
    protected Button _buttonflee;
    /// <summary>
    /// Close Popup Button
    /// </summary>
    [SerializeField]
    protected Button _buttonStay;
    /// <summary>
    /// Close Popup Button
    /// </summary>
    [SerializeField]
    protected RectTransform _successScreenRoot;
    #endregion

    #region  Public Methods

    /// <summary>
    ///  Roll Flee for the group
    /// </summary>
    /// <param name="var"></param>
    public void RollFlee( CombatVar var)
    {
        _buttonflee.onClick.RemoveAllListeners();
        _buttonStay.onClick.RemoveAllListeners();
        _buttonStay.gameObject.SetActive(false);
        ShowSuccessScreenRoot(true);
        int maxSpeed=0;
        foreach(Core.FightSystem.Adversaire adversaire in var.Adversaires)
        {
            int advSpeed =  adversaire.GetCharacteristicsByName("Speed");
            if(advSpeed > maxSpeed)
            {
                maxSpeed = advSpeed;
            }

        }
        bool[] fleed = new bool[var.Party.Count];
        bool AllFled = false;
        int i = 0;
        foreach (Core.FightSystem.Character character in var.Party)
        {
            fleed[i]  = 
            CoinFlipManager.Instance.Flip( 
                character.GetCharacteristicsByName("Speed") + 
                character.GetCompetenceModifierByName("Distance"),
                maxSpeed);
            AllFled = AllFled && fleed[i];
              i++;
        }

        if(AllFled)
        {
            _message.StringReference = _fleeSucces;
            _buttonflee.onClick.AddListener(CombatManager.Instance.EndFight);
            _buttonflee.onClick.AddListener(()=>ShowSuccessScreenRoot(false));
        }
        else
        {
            if( !fleed[0])
            {
                _message.StringReference = _fleePartialFailure;
                _buttonflee.gameObject.SetActive(false);
                _buttonStay.gameObject.SetActive(true);
                _buttonStay.onClick.AddListener(CombatManager.Instance.EndTurn);
                _buttonStay.onClick.AddListener(() => ShowSuccessScreenRoot(false));
            }
            else
            {
                _buttonStay.gameObject.SetActive(true);
               
                // remove left behind party members
                for (int j = fleed.Length; j>0 ; j--)
                {
                    var.Party.RemoveAt(j);
                 }
                _buttonflee.onClick.AddListener(CombatManager.Instance.EndFight);
                _buttonflee.onClick.AddListener(() => ShowSuccessScreenRoot(false));
                _buttonStay.onClick.AddListener(CombatManager.Instance.EndTurn);
                _buttonStay.onClick.AddListener(() => ShowSuccessScreenRoot(false));
            }
        }
    }

    public void ShowSuccessScreenRoot(bool show)
    {
        _successScreenRoot.gameObject.SetActive(show);
    }
    #endregion
}
