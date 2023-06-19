using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Adversaire",menuName = "Obol/Characters/Enemy",order = 1)]
public class Adversaire : ScriptableObject,IMortal
{

    #region Members
    #region Hidden
    protected int _currentlife;
    #endregion
    #region Visible
    [SerializeField]
    protected int _lifeMax;
    [SerializeField]
    protected List<Attack> _attacks;
    [SerializeField]
    protected Sprite _illustrations;
    #endregion
    #endregion

    #region Getters
    public int Life
    {
        get => _currentlife;
    }

    public Sprite Illustration
    {
        get => _illustrations;
    }


  

    public bool IsDead()
    {
        return _currentlife == 0;
    }



    #endregion

    public void Attack(int degat)
    {
        _currentlife = Mathf.Clamp(_lifeMax, 0, _lifeMax);
       
    }
}
