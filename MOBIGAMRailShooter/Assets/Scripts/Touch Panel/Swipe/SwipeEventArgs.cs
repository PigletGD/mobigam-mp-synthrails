using System;
using UnityEngine;

public enum SwipeDirections
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class SwipeEventArgs : EventArgs
{
    // Position where you swiped
    private Vector2 swipePos;
    public Vector2 SwipePos { get { return swipePos; } }

    // Raw Swipe Direction
    private Vector2 swipeVector;
    public Vector2 SwipeVector { get { return swipeVector; } }
    
    // Direction of the swipe
    private SwipeDirections swipeDirections;
    public SwipeDirections SwipeDirection { get { return swipeDirections; } }

    public SwipeEventArgs(Vector2 pos, Vector2 v, SwipeDirections dir)
    {
        swipePos = pos;
        swipeVector = v;
        swipeDirections = dir;
    }
}