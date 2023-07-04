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

    #region Initialisation
    public Attack()
    {
        _listEffect = new List<IEffect>();
    }

    #endregion

    #region Attack Resolution

    public void ParseEffect()
    {
        string[] lines = _effect.Split( "\n");
        bool AddCardToChoice = false;
        int CardToAddtoChoice = 0;
        foreach (string line in lines)
        {
            string[] words = line.Split();
            IEffect effect = null;
            ChooseEffect chooseEffect=null;
            if ( words.Length >= 0 ) 
            {
                switch( words[0] )
                {
                    case "Inflict" :
                        effect = new InflictEffect( 0 , DamageType.Health );
                        effect.CreateFromLine(words); 
                    break;
                    case "Choose":
                        chooseEffect = new ChooseEffect("empty_key");
                        chooseEffect.CreateFromLine(words);
                        CardToAddtoChoice = 2;
                    break;
                    case "(":
                        AddCardToChoice = true;
                    break;
                    case ")":
                        CardToAddtoChoice--;
                        AddCardToChoice = false;
                    break;
                }
            }
            if ( AddCardToChoice && CardToAddtoChoice == 2 )
            {
                chooseEffect.Choices1.Add( effect );
            }
            else if ( AddCardToChoice && CardToAddtoChoice == 1 ) 
            {
                chooseEffect.Choices2.Add( effect );
            }
        }
    }


    public void PlayAttack ( ITargetable targetable )
    {
        foreach( IEffect ieffect in _listEffect )
        {
            ieffect.Apply(targetable);
        }
    }


    #endregion

   
}
