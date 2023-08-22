using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterationsDisplayer : MonoBehaviour
{

    #region Member
    #region Visibles
    [SerializeField]
    protected EffectIcon _alterationPrefabs;
    [SerializeField]
    protected RectTransform _layoutRectTransform;
    #endregion
    #region Hidden
    protected Dictionary<AlterationType, EffectIcon> _alterationIconDictionary;
    protected List<EffectIcon> _effectIcons;
    #endregion
    #endregion

    public void Awake()
    {
        _alterationIconDictionary = new Dictionary<AlterationType, EffectIcon>();
    }


    /// <summary>
    /// Event Called On Alteration update ( first application or subsequent update);
    /// 
    /// </summary>
    /// <param name="alteration"></param>
    public void OnAlterationApplied(IAlteration alteration)
    {
        if( !_alterationIconDictionary.ContainsKey( alteration.AlterationType( )) )
        {
            EffectIcon effectIcon = GameObject.Instantiate<EffectIcon>(_alterationPrefabs);
            effectIcon.Setup(alteration);

        }
        else
        {
            _alterationIconDictionary[ alteration.AlterationType( ) ].UpdateAlteration( alteration);
        }

        if(!alteration.StillGoingOn())
        {
            EffectIcon effectIcon = _alterationIconDictionary[alteration.AlterationType()];
            _alterationIconDictionary.Remove(alteration.AlterationType());
            GameObject.Destroy(effectIcon.gameObject);
        }
    }


}
