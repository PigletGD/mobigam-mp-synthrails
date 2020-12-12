using System;
using UnityEngine;

public class DragEventArgs : EventArgs
{
    // Current state of the finger
    private Touch targetFinger;
    public Touch TargetFinger { get { return targetFinger; } }

    // Hit object
    private GameObject hitObject;
    public GameObject HitObject { get { return hitObject; } }

    public DragEventArgs(Touch finger, GameObject obj)
    {
        targetFinger = finger;
        hitObject = obj;
    }
}