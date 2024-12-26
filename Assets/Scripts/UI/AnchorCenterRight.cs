using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class AnchorCenterRight : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _offset;

    protected Camera _cameraRef;
    private void OnEnable()
    {
        _cameraRef = FindFirstObjectByType<Camera>();
    }

    private void Update()
    {
        transform.position = _cameraRef.ViewportToWorldPoint(new Vector3(1.0f, 0.5f)) + _offset;
    }
}
