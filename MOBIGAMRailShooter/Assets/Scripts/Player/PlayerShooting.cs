using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public TouchPanel touchPanel;

    private Transform ownerTransform;
    public Transform planeNuzzle;
    public GameObject bullet;

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
        Instantiate(bullet, planeNuzzle.position, ownerTransform.rotation);
    }
}