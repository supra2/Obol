using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacteristic
{

    public int GetCharacteristicsByName(string characName);

    public int GetCompetenceModifierByName(string compName);

}
