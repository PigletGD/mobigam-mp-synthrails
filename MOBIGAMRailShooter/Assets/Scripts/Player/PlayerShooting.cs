using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public TouchPanel touchPanel;

    private Transform ownerTransform;
    public Transform planeNuzzle;
    public ObjectPool bulletPool;

    private void Start()
    {
        ownerTransform = transform;
        touchPanel.OnTap += OnTap;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Fire();
    }

    private void OnTap(object sender, TapEventArgs e)
    {
        Fire();
    }

    private void Fire()
    {
        GameObject bullet = bulletPool.RetrieveObject();

        bullet.transform.position = planeNuzzle.position;
        bullet.transform.rotation = ownerTransform.rotation;
    }
}