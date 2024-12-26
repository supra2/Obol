using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Recale the root transform of the scene to adapt for different aspect ration and resolution
/// 
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{

    private  float _aspectRatio;
    private Camera _cameraRef;
    private Transform _rootchild;
    private bool _setDirty;

    //called when monobehaviour is enabled
    void OnEnable()
    {
        _cameraRef = Camera.current;
        Vector2 dim = GetScreenToWorld();
        _rootchild = transform.GetChild(0);
        _rootchild.transform.localScale = new Vector3(dim.x ,dim.y,1.0f);
    }

    void OnDisable()
    {
        
    }

    private Vector2 GetScreenToWorld()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = _cameraRef.ViewportToWorldPoint(topRightCorner);
        
        return new Vector2(edgeVector.x * 2, edgeVector.y * 2);
    }

}
