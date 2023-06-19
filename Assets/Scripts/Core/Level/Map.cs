using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "LevelsData/Map")]
public class Map : ScriptableObject,IEnumerable<Layer>
{

    #region Members
    [Header("Data")]
    [SerializeField]
    protected int Chipset_Associated;
    [SerializeField]
    protected List<Layer> layers;
    /// <summary>
    /// Map Unique ID
    /// </summary>
    [SerializeField]
    protected uint _id;
    #endregion

    #region Getter
    public uint ID => _id;

    public Layer this[int id]
    {
        get => layers[id];
        set => layers[id] = value;
    }
    #endregion

    #region Methods 

    public void Display(Grid grid, int x, int y)
    {

    }

    public void Sort()
    {
        layers.Sort(new Mapsorter());
    }

    #endregion

    #region Enumerable 
    public IEnumerator<Layer> GetEnumerator()
    {
        return layers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return layers.GetEnumerator();
    }
    #endregion

}
