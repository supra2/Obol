using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdversaireDisplayer : MonoBehaviour
{

    #region Members
    #region Visible
    [SerializeField]
    protected Adversaire _adversaire;
    [SerializeField]
    protected Sprite _sprite;
    #endregion
    #endregion

    protected void Awake()
    {
        _sprite = _adversaire.Illustration;
    }

}
