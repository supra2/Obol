using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public Sprite GetIllustration();

    public string DescriptionKey();

    public string TitleKey();

}
