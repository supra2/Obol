using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFlipManager : Singleton<CoinFlipManager>
{

    #region Hidden

    protected Animator _CoinAnimator;

    protected string _keyPrompt = "CoinFlipChoice_ChoicePrompt";

    protected string _keyChoice1 = "CoinFlipChoice_Choice1Prompt";

    protected string _keyChoice2 = "CoinFlipChoice_Choice2Prompt";

    protected Sprite IllustrationHead;

    protected Sprite IllustrationTails;

    protected int _stored_val_comp1;

    protected int _stored_val_comp2;

    protected Action<bool> _stored_callback;

    protected bool _flipsuccess;

    #endregion

    public void Awake()
    {
        _CoinAnimator = (GameObject.Instantiate(Resources.Load("Prefabs/Coin"),
            GameObject.FindGameObjectWithTag("RootUI").transform) as GameObject)
            .GetComponent<Animator>();
        Sprite[] all = Resources.LoadAll<Sprite>("Textures/Animsketch2");
        foreach (var s in all)
        {
            if (s.name == "Animsketch2_1")
            {
                IllustrationTails = GameObject.Instantiate(s);
            }
            if (s.name == "Animsketch2_8")
            {
                IllustrationHead = GameObject.Instantiate(s);
            }
        }
    }

    public void Flip(int val_comp1, int val_comp2, Action<bool> resolveCallback, bool Silent = true)
    {
        if (Silent)
        {
            resolveCallback(ResolveFlip(val_comp1, val_comp2));
        }
        else
        {
            _stored_callback = resolveCallback;
            _stored_val_comp1 = val_comp1;
            _stored_val_comp2 = val_comp2;
            UICombatController.Instance.DisplayChoice(
            "CoinFlipChoice_ChoicePrompt", "CoinFlipChoice_Choice1Prompt",
            "CoinFlipChoice_Choice2Prompt", IllustrationHead,
            IllustrationTails, () => ChooseHead(true), () => ChooseHead(false));
        }
    }

    protected void ChooseHead(bool headChosen)
    {
        _flipsuccess = ResolveFlip(_stored_val_comp1, _stored_val_comp2);
        _CoinAnimator.transform.GetComponent<CoinAnimator>()._endAnimation.AddListener(AnimationResolution);
        _CoinAnimator.transform.GetComponent<CoinAnimator>()._hideAnimation.AddListener(EndFlip);
        _CoinAnimator.SetTrigger(_flipsuccess && headChosen ?
            "ShowHead" : "ShowTail");
       
    }

    public bool ResolveFlip(int val_comp1, int val_comp2)
    {
        int roll = SeedManager.NextInt(0, 100);
        int treshold = Mathf.Clamp(50 - (val_comp1 - val_comp2) * 5, 5, 95);
        Debug.Log("Roll " + roll + "  > "+ treshold);
        return roll > treshold;
    }

    protected void ShowInterfaceOfChoiceIllusion()
    {
        UICombatController.Instance.DisplayChoice();
    }

    protected void AnimationResolution()
    {
        _CoinAnimator.SetTrigger(_flipsuccess ?
          "ShowSucess" : "ShowFailure");
    }

    protected void EndFlip()
    {
        _CoinAnimator.transform.GetComponent<CoinAnimator>().
            _endAnimation.RemoveListener(AnimationResolution);
        _CoinAnimator.transform.GetComponent<CoinAnimator>().
            _hideAnimation.RemoveListener(EndFlip);
        _stored_callback?.Invoke(_flipsuccess);
    }

}
