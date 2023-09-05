using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFlipManager : Singleton<CoinFlipManager>
{

    #region Hidden
    protected Animator _CoinAnimator;
    #endregion
    public void Awake()
    {
        //Todo Animate coin
        //_CoinAnimator = (GameObject.Instantiate(Resources.Load("Prefabs/Coin")) as GameObject).GetComponent<Animator>();
    }
    public  bool Flip( int val_comp1 , int val_comp2,bool Silent = true )
    {
        int roll = SeedManager.NextInt(0, 100);
        int treshold = Mathf.Clamp(50 -(val_comp1 - val_comp2)*5, 5, 95);
        return roll > treshold;
    }
  
}
