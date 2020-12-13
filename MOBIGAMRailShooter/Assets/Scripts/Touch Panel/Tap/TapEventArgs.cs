using System;
using UnityEngine;

public class TapEventArgs : EventArgs
{
    // Set to private to avoid editing
    private Vector2 _tapPosition;

    public Vector2 TapPosition { get { return _tapPosition; } }

    public TapEventArgs(Vector2 pos) => _tapPosition = pos;
}
