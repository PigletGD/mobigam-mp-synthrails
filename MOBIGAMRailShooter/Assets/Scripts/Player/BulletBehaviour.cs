using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 10;

    Transform ownerTransform = null;

    private void Start()
    {
        ownerTransform = transform;
    }

    private void FixedUpdate()
    {
        ownerTransform.position += ownerTransform.forward * speed;
    }
}
