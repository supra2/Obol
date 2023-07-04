using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.FightSystem;
public class PlayerController : MonoBehaviour
{

    #region Members
    /// <summary>
    /// list characters playable
    /// </summary>
    protected List<PlayableCharacter> _characters;
    #endregion

    #region Debug
    public void Generate_StartTeam()
    {
        _characters = new List<PlayableCharacter>();
        PlayableCharacter character = new PlayableCharacter();
        
        _characters.Add(character);
    }

    #endregion

}
