using Core.FightSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static AdversaireDisplayer;

public class Adversaire : UnityEvent<AdversaireDisplayer>
{ 

}

public class AdversaireLayout : MonoBehaviour
{

    #region Members
    #region Visible
    /// <summary>
    /// prefabD
    /// </summary>
    [SerializeField]
    protected AdversaireDisplayer _prefabDisplayers;
    #endregion
    #region Hidden
    protected List<AdversaireDisplayer> _displayers;
    #endregion
    #endregion

    #region Initialisation
    public void Awake()
    {
        _displayers = new List<AdversaireDisplayer>();
    }
    #endregion

    #region Public Methods

    public void AddAdversaire(Core.FightSystem.Adversaire _adversaire)
    { 
        AdversaireDisplayer displayer =
            GameObject.Instantiate(_prefabDisplayers,transform);
        displayer.Adversaire = _adversaire;
        _displayers = new List<AdversaireDisplayer>();
    }

    public void SelectAdversaireMode( )
    {
        foreach(AdversaireDisplayer advDisplayer in _displayers)
        {
           advDisplayer.SetMode( AdversaireDisplayerMode.Selection );
        }
    }

    #endregion

}
