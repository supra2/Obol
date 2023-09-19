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
    [SerializeField]
    protected Animator _displayerAnimator;
    #endregion
    #region Hidden 
    protected AdversaireDisplayerMode _currentMode;
    protected bool _picked;
    #endregion
    #endregion

    #region Event
    public UnityCharacterEvent _onAdversairePicked;
    public UnityCharacterEvent _onDisplayDestroyed;
    #endregion

    #region Getter

    public Core.FightSystem.Adversaire Adversaire
    {
        get => _adversaire;
        set
        {
            _adversaire = value;

            _image.sprite = _adversaire.Illustration;

            AttachListeners();
        }
    }



    #endregion

    #region Init
    private void AttachListeners()
    {
        Adversaire.LifeChangeEvent.AddListener(LifeChanged);

        if (Adversaire.AlterationAppliedEvent == null)
            Adversaire.AlterationAppliedEvent = new AlterationEvent();

        Adversaire.AlterationAppliedEvent.AddListener(
            _alterationDisplayer.OnAlterationApplied);

        Adversaire.Attacked.AddListener(AdversaireAttacked);
        Adversaire._dodged.AddListener(DodgeAnimation);
        Adversaire._died.AddListener(DiedAnimation);
    }

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

    #region InnerMethods

    protected void DodgeAnimation()
    {
        _displayerAnimator.SetTrigger("Dodged");
    }

    protected void DiedAnimation()
    {
        _displayerAnimator.SetTrigger("_died");

    }

    protected void DestroyDisplayer()
    {
        DetachListener();
        _onDisplayDestroyed?.Invoke(Adversaire);
     
    }


    private void DetachListener()
    {
        Adversaire.LifeChangeEvent.RemoveListener(LifeChanged);
        Adversaire.AlterationAppliedEvent.RemoveListener(
            _alterationDisplayer.OnAlterationApplied);

        Adversaire.Attacked.RemoveListener(AdversaireAttacked);
        Adversaire._dodged.RemoveListener(DodgeAnimation);
        Adversaire._died.RemoveListener(DiedAnimation);
    }
    #endregion

}
