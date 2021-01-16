using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossOneBehaviour : MonoBehaviour
{
    private int maxHealth = 100;
    private int health = 0;

    public EntityType enemyType = EntityType.NONE;
    public EntityType weaknessType = EntityType.NONE;

    private MeshRenderer MR = null;

    [SerializeField] GameEventsSO onBossDeath = null;

    [SerializeField] Transform aimObject = null;
    private Transform player = null;

    private ObjectPool bulletPool = null;
    private ObjectPool explosionPool = null;

    private Color currentColor = Color.white;
    private Color targetColor = Color.white;
    private float currentLerpTime = 0.0f;
    private float lerpTime = 0.1f;
    private bool transitioningColor = false;

    private int currentIndexColor = -1;

    [SerializeField] private Image healthBar = null;

    [SerializeField] private GameObject bossUI = null;

    private void Awake()
    {
        MR = GetComponentInChildren<MeshRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        bulletPool = GameObject.FindGameObjectWithTag("Bullet Pool").GetComponent<ObjectPool>();

        explosionPool = GameObject.FindGameObjectWithTag("Explosion Pool").GetComponent<ObjectPool>();

        StartCoroutine("ChangeColor");
    }

    private void OnEnable()
    {
        switch (SaveManager.Instance.currentLevel)
        {
            case 1: health = maxHealth; break;
            case 2: health = (maxHealth * 3) / 2; break;
            case 3: health = maxHealth * 2; break;
        }

        StartCoroutine(Shooting(1, 1));
    }

    private void Update()
    {
        LookAtPlayer();
        TransitioningColors();
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {

            int previousIndex = currentIndexColor;
            do
            {
                currentIndexColor = Random.Range(0, 3);
            } while (previousIndex == currentIndexColor);

            switch (currentIndexColor)
            {
                case 0:
                    enemyType = EntityType.RED;
                    weaknessType = EntityType.BLUE;
                    targetColor = Color.red;
                    break;
                case 1:
                    enemyType = EntityType.GREEN;
                    weaknessType = EntityType.RED;
                    targetColor = Color.green;
                    break;
                case 2:
                    enemyType = EntityType.BLUE;
                    weaknessType = EntityType.GREEN;
                    targetColor = Color.blue;
                    break;
            }

            currentColor = MR.material.GetColor("_EmissionColor");

            currentLerpTime = 0.0f;

            transitioningColor = true;

            yield return new WaitForSeconds(5);
        }
    }

    private void TransitioningColors()
    {
        if (transitioningColor)
        {
            currentLerpTime += Time.deltaTime;
            float lerpValue = currentLerpTime / lerpTime;

            Color lerpedColor = Color.Lerp(currentColor, targetColor, lerpValue);

            if (currentLerpTime > lerpTime)
            {
                MR.material.SetColor("_EmissionColor", targetColor);
                transitioningColor = false;
            }
            else MR.material.SetColor("_EmissionColor", lerpedColor);
        }
    }

    private void LookAtPlayer()
    {
        transform.LookAt(player);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Yeah");

        if (health <= 0)
            Die();
        else
            healthBar.fillAmount = (float)health / (float)maxHealth;
    }

    public void Die()
    {
        StopAllCoroutines();

        onBossDeath.Raise();

        GameObject go = explosionPool.RetrieveObject();
        go.transform.position = transform.position;
        go.transform.localScale = new Vector3(8, 8, 8);

        gameObject.SetActive(false);
    }

    IEnumerator Shooting(float startDelay, float interval)
    {
        yield return new WaitForSeconds(startDelay);

        float tick = 0;
        float currentRate = 0;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(interval, interval + 2f));

            tick = 0;
            currentRate = 0;

            while (tick < 3)
            {
                tick += Time.deltaTime;

                if (tick > currentRate)
                {
                    GameObject bullet = bulletPool.RetrieveObject();

                    BulletBehaviour BO = bullet.GetComponent<BulletBehaviour>();
                    BO.InitializeType(enemyType);

                    bullet.transform.position = aimObject.position;
                    bullet.transform.rotation = transform.rotation;
                    bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    BO.Fire();

                    currentRate += 0.2f;
                }

                yield return null;
            }
        }
    }

    public void DisableUI()
    {
        bossUI.SetActive(false);
    }
}