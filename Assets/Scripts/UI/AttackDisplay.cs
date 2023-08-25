using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class AttackDisplay : MonoBehaviour
{

    #region Members
    /// <summary>
    /// Animator : Display Animator
    /// </summary>
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected UnityEngine.Localization.Components.LocalizeStringEvent _localizedStringDescription;
    [SerializeField]
    protected UnityEngine.Localization.Components.LocalizeStringEvent _localizedTitle;
    #endregion

    public void ShowAttackDescription(Attack attack)
    {
        ResetAllTriggers();
        _animator.SetFloat("Speed", 1);
        _animator.SetTrigger("ShowAttackDescription");
        _localizedStringDescription.StringReference = attack.DescriptionKey;
        _localizedTitle.StringReference = attack.NameKey;
    }

    public void HideAttackDescription()
    {

        ResetAllTriggers();
        _animator.SetFloat("Speed", 1);
        _animator.SetTrigger("HideAttackDescription");
    }

    public void SkipAnimation()
    {
        _animator.SetFloat("Speed", 10000);
    }

    public void SkipButtonPressed( )
    {
        if ( _animator.GetCurrentAnimatorStateInfo(0).IsName("Show Attack"))
        {
            SkipAnimation();
        }
        else if ( _animator.GetCurrentAnimatorStateInfo(0).IsName("Shown") )
        {
            Debug.Log(" Hide");
            HideAttackDescription();
        }
    }

    public void ResetAllTriggers()
    {
        _animator.ResetTrigger("ShowAttackDescription");
        _animator.ResetTrigger("HideAttackDescription");
    }

}
