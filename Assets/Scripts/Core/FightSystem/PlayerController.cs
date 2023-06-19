using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region members
    protected List<Characters> _characters;
    #endregion

    #region Debug
    public void Generate_StartTeam()
    {
        _characters = new List<Characters>();
        Characters character = new Characters();
        
        _characters.Add(character);
    }

    #endregion

}
