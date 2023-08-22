using Core.FightSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Obol/Characters/Attack", order = 2)]
public class Attack : ScriptableObject
{

    #region Members
    #region Visible
    [SerializeField]
    protected string _nameKey;
    [SerializeField]
    protected string _descriptionKey;
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
    #endregion

    #region Initialisation
    public Attack()
    {
        _listEffect = new List<IEffect>();
    }

    public void Init()
    {


    }

    #endregion

    #region Attack Resolution

    public void PlayAttack ( ITargetable targetable )
    {
        foreach( IEffect ieffect in _listEffect )
        {
            ieffect.Apply(targetable);
        }
        
    }

    public void Init()
    {

    }
    #endregion

   
}
