﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 20;

    public EntityType bulletType = EntityType.NONE;

    Transform ownerTransform = null;

    [SerializeField] private float bulletLifetime = 3.0f;

    private bool hasLaunched = false;
    private float scaleSize = 0.8f;
    private float scaleTime = 1.0f;
    private float scaleTick = 0;

    private bool fullyCharged = false;

    [SerializeField] private Vector3 rotationVector = new Vector3(4, 20, 12);

    public Transform childTransform = null;
    public Rigidbody RB = null;

    private void Awake()
    {
        ownerTransform = transform;
    }

    private void FixedUpdate()
    {
        if (!hasLaunched) 
        {
            float scaleValue;
            if (scaleTick >= scaleTime) scaleValue = scaleSize;
            else scaleValue = Mathf.Lerp(0.4f, scaleSize, scaleTick / scaleTime);

            ownerTransform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

            scaleTick += Time.deltaTime;
            if (scaleTick > scaleTime)
            {
                scaleTick = scaleTime;
                fullyCharged = true;
            }

            childTransform.rotation *= Quaternion.Euler(rotationVector * scaleValue);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyBehaviour EB = collision.gameObject.GetComponent<EnemyBehaviour>();
            if (EB.weaknessType == bulletType)
            {
                if (fullyCharged) EB.TakeDamage(SaveManager.Instance.state.bulletDamage + 2);
                else EB.TakeDamage(SaveManager.Instance.state.bulletDamage);
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(1);
        }
        else if (collision.gameObject.tag == "Boss One")
        {
            BossOneBehaviour BOB = collision.gameObject.GetComponent<BossOneBehaviour>();
            if (BOB.weaknessType == bulletType)
            {
                if (fullyCharged) BOB.TakeDamage(SaveManager.Instance.state.bulletDamage + 2);
                else BOB.TakeDamage(SaveManager.Instance.state.bulletDamage);
            }
        }

        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Boss One")
        {
            StopAllCoroutines();

            ownerTransform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            scaleTick = 0.0f;
            hasLaunched = false;

            gameObject.SetActive(false);
        }
    }

    public void InitializeType(EntityType newBulletType)
    {
        bulletType = newBulletType;

        MeshRenderer MR = GetComponentInChildren<MeshRenderer>();

        if (MR != null)
        {
            switch (bulletType)
            {
                case EntityType.RED: MR.material.SetColor("_EmissionColor", Color.red * 2.5f); break;
                case EntityType.BLUE: MR.material.SetColor("_EmissionColor", Color.blue * 2.5f); break;
                case EntityType.GREEN: MR.material.SetColor("_EmissionColor", Color.green * 2.5f); break;
            }
        }
    }

    public void Fire()
    {
        StartCoroutine("Travel");
    }

    private IEnumerator Travel()
    {
        float tick = 0.0f;

        hasLaunched = true;

        do
        {
            tick += Time.deltaTime;

            RB.MovePosition(ownerTransform.position + (ownerTransform.forward * speed * Time.deltaTime));
            childTransform.rotation *= Quaternion.Euler(rotationVector);

            yield return null;
        } while (tick < bulletLifetime);

        ownerTransform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        scaleTick = 0.0f;
        hasLaunched = false;

        gameObject.SetActive(false);
    }
}