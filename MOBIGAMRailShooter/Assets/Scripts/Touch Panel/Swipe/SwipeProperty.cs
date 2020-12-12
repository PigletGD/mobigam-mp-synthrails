using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwipeProperty
{
    [Tooltip("Maximum gesture time until its not a swipe anymore")]
    public float swipeTime = 0.7f;
    [Tooltip("Minimum Distance covered to be considered a swipe")]
    public float swipeMinDistance = 0.1f;
}