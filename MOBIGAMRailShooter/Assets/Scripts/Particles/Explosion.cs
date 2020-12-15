using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem PS = null;

    private void OnEnable()
    {
        StartCoroutine("PlayParticle");
    }

    IEnumerator PlayParticle()
    {
        PS.Play();

        yield return new WaitForSeconds(1);

        gameObject.SetActive(false);
    }
}
