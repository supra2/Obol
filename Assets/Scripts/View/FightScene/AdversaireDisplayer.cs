using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Adversaire Displayer
/// </summary>
public class AdversaireDisplayer : MonoBehaviour, IPointerClickHandler
{

    #region Enum 

    public enum AdversaireDisplayerMode
    {
        Neutral,
        Selection
    }

    #endregion

    #region Members
    #region Visible
    /// <summary>
    /// inner Adversaire Data class
    /// </summary>
    [SerializeField]
    protected Core.FightSystem.Adversaire _adversaire;
    /// <summary>
    /// Image displaying adversaire Illustration
    /// </summary>
    [SerializeField]
    protected Image _image;
    [SerializeField]
    protected Slider _lifeslider;
    [SerializeField]
    protected AlterationsDisplayer _alterationDisplayer;
    #endregion
    #region Hidden 
    protected AdversaireDisplayerMode _currentMode;
    protected bool _picked;
    #endregion
    #endregion

    #region Event
    public UnityCharacterEvent _onAdversairePicked;
    #endregion

    #region Getter

    public Core.FightSystem.Adversaire Adversaire
    {
        get => _adversaire;
        set
        {
            _adversaire = value;

            _image.sprite = _adversaire.Illustration;

            Adversaire.LifeChangeEvent.AddListener(LifeChanged);

            if (Adversaire.AlterationAppliedEvent == null)
                Adversaire.AlterationAppliedEvent = new AlterationEvent();

            Adversaire.AlterationAppliedEvent.AddListener(
                _alterationDisplayer.OnAlterationApplied);

            Adversaire.Attacked.AddListener(AdversaireAttacked);
        }
    }

    #endregion

    #region Init


    #endregion

    #region Public Methods

    public void SetMode( AdversaireDisplayerMode newmode )
    {
        switch (newmode)
        {
            case AdversaireDisplayerMode.Selection:
                _picked = false;
                break;
        }
        _currentMode = newmode;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentMode == AdversaireDisplayerMode.Selection)
        {
            _picked = !_picked;
            _onAdversairePicked?.Invoke(_adversaire);
        }
    }

    public void LifeChanged( int Diff  )
    {
        _lifeslider.value = (float)Adversaire.Life / _adversaire.MaxLife;
    }

    public void AlterationChanged(IAlteration alteration)
    {
        Image image = GameObject.Instantiate( Resources.Load( alteration.GetIconPath()) )as Image;
        
    }

    public void AdversaireAttacked( Attack attack_Launch )
    {
        transform.GetComponentInParent<AdversaireLayout>().ShowAttack(attack_Launch);
    }

    #endregion

}
