using System;
using UnityEngine;

public class DragEventArgs : EventArgs
{
    // Set to private to avoid editing
    private Vector2 _startPosition;
    public Vector2 StartPosition { get { return _startPosition; } }

    private Vector2 _currentPosition;
    public Vector2 CurrentPosition { get { return _currentPosition; } }

    public DragEventArgs(Vector2 startPos, Vector2 curPos)
    {
        _startPosition = startPos;
        _currentPosition = curPos;
    }
}