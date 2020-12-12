using System;
using UnityEngine;

public class TapEventArgs : EventArgs
{
    // Set to private to avoid editing
    private Vector2 _tapPosition;
    private GameObject _hitObject;

    public Vector2 TapPosition { get { return _tapPosition; } }
    public GameObject HitObject { get { return _hitObject; } }

    public TapEventArgs(Vector2 pos, GameObject obj = null)
    {
        _tapPosition = pos;
        _hitObject = obj;
    }
}
