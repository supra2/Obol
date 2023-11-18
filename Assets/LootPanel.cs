using Core.Items;
using System.Collections;
using System.Collections.Generic;
using UI.ItemSystem;
using UnityEngine;

public class LootPanel : MonoBehaviour
{
    #region Members
    #region Members
    [SerializeField]
    protected ItemDisplayer _lootPrefab;
    [SerializeField]
    protected RectTransform _Content;
    #endregion
    #region Hidden
    protected List<ItemDisplayer> _instanciatedDisplayer;
    #endregion
    #endregion

    #region Methods
    //------------------------------------------------

    public void AddNewObject( Item item )
    {
        ItemDisplayer displayer = 
            GameObject.Instantiate( _lootPrefab )
            as ItemDisplayer;
        displayer.Item = item;
        displayer.transform.SetParent(_Content);
        displayer.gameObject.SetActive(true);
    }

    //------------------------------------------------

    protected void ObjectTaken(ItemDisplayer displayer)
    {
        _instanciatedDisplayer.Remove(displayer);

    }

    //------------------------------------------------
    #endregion
}
