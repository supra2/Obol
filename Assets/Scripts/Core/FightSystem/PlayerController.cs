using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region members
    protected List<Character> _characters;
    #endregion

    #region Debug
    public void Generate_StartTeam()
    {
        _characters = new List<Character>();
        Character character = new Character();
        
        _characters.Add(character);
    }

    #endregion

}
