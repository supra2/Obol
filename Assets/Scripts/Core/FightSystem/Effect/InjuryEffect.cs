using Core.CardSystem;
using Core.FightSystem;
using Core.FightSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// effect removing an Physique PlayerCard and adding an injury if such a card is found
/// if the player fail at flip coin between injury value and consitution
/// </summary>
public class InjuryEffect : IEffect
{
  
    #region Members
    #region private Mambers
    /// <summary>
    /// factor impacting probability of injury beeing applied to target
    /// </summary>
    protected int injury_value;
    /// <summary>
    /// Id of the injury card
    /// </summary>
    protected int injury_card_id;
    /// <summary>
    /// Targetable 
    /// </summary>
    protected ITargetable _target;
    #endregion
    #endregion

    #region Public Method
    public void Apply(ITargetable itargetable)
    {
        if(itargetable is PlayableCharacter)
        {

            PlayableCharacter pc = (PlayableCharacter)itargetable;
            _target = itargetable;
            CoinFlipManager.Instance.Flip(injury_value, pc.GetCharacteristicsByName("Constitution"), ResolveFlip,false);
        }
    }

    public void ResolveFlip(bool success)
    {
        PlayableCharacter pc = (PlayableCharacter)_target;
        List<PlayerCard> listcards = pc.CardList.FindAll((x) =>
           x.CardNature == PlayerCard.Nature.Physique &&
           x.CardType == PlayerCard.Type.Action);
        int index = SeedManager.NextInt(0, listcards.Count - 1);
        pc.Exchange(index, injury_card_id);
    }

    public void CreateFromLine(string[] words)
    {
        if (!System.Int32.TryParse(words[1],out injury_card_id))
        {
            Debug.LogError("Unable to parse  card ID ( arg 1 ) ");
        }
        if (!System.Int32.TryParse(words[2], out injury_value))
        {
            Debug.LogError("Unable to parse injury_value ( arg 2 ) ");
        }
    }

    public bool SelfTarget()
    {
        return false;
    }
    #endregion

}

