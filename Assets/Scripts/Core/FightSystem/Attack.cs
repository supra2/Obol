using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Obol/Characters/Attack", order = 2)]
public class Attack : MonoBehaviour
{

    #region members
    [SerializeField]
    protected string _nameKey;
    [SerializeField]
    protected string _descriptionKey;
    [SerializeField]
    protected string _effect;
    [SerializeField]
    protected Sprite _illustration;
    #endregion


}
