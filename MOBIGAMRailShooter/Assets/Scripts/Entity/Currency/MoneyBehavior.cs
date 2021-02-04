using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBehavior : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    public Rigidbody RB = null;

    private ScoreManager SM = null;

    private void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Score Manager");
        if (temp != null)
            SM = temp.GetComponent<ScoreManager>();
    }

    private void Update()
    {
        if (SM == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Score Manager");
            if (temp != null)
                SM = temp.GetComponent<ScoreManager>();
        }
    }

    private void OnEnable()
    {
        StartCoroutine("Travel");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();

            SM.AddMoney();

            gameObject.SetActive(false);
        }
    }

    IEnumerator Travel()
    {
        float tick = 0.0f;

        while (tick < 10.0f)
        {
            RB.MovePosition(transform.position + (transform.forward * speed * Time.deltaTime));

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
