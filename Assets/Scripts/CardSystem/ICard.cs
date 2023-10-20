using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard:IEquatable<ICard>
{
    public Sprite GetIllustration();

    public string DescriptionKey();

    public string TitleKey();

    public void Play(  );

    public void Resolve();

    public int GetCardId();

    public string[] GetTags();
}
