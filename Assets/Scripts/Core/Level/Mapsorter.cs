using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapsorter : IComparer<Layer>
{
    public int Compare(Layer x, Layer y)
    {
        return x.Depth - y.Depth;
    }
}
