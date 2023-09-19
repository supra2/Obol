using Core.FightSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class UnityAttackEvent: UnityEvent<Attack>
{

}

[CreateAssetMenu(fileName = "Attack", menuName = "Obol/Characters/Attack", order = 2)]
public class Attack : ScriptableObject
{

    #region Members
    #region Visible
    [SerializeField]
    protected LocalizedString _nameKey;
    [SerializeField]
    protected LocalizedString _descriptionKey;
    [TextArea(3, 10)]
    [SerializeField]
    protected string _effect;
    [SerializeField]
    protected Sprite _illustration;
    [SerializeField]
    protected int _stamina;
    #endregion
    #region hidden
    protected List<IEffect> _listEffect;
    #endregion
    #endregion

    #region Getters
    public int Stamina => _stamina;

    public LocalizedString DescriptionKey => _descriptionKey;

    public LocalizedString NameKey => _nameKey;

    #endregion

    public UnityAttackEvent AttackLaunched;

    #region Initialisation

    public Attack()
    {
        _listEffect = new List<IEffect>();
    }

    /// <summary>
    /// Initialise
    /// </summary>
    public void Init()
    {
        _listEffect = EffectFactory.ParseEffect(_effect);
        if (AttackLaunched == null)
            AttackLaunched = new UnityAttackEvent();
    }

    #endregion

    #region Attack Resolution


    /// <summary>
    /// Play the effect of the attack on the targetable
    /// </summary>
    /// <param name="targetable"> targetable </param>
    public void PlayAttack ( ITargetable targetable )
    {
        
        foreach( IEffect ieffect in _listEffect )
        {
            ieffect.Apply(targetable);
        }


        AttackLaunched?.Invoke(this);
    }

    #endregion

   
}
