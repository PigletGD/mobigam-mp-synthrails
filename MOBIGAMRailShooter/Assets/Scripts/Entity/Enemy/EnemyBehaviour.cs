using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private int health = 3;

    public EntityType enemyType = EntityType.NONE;
    public EntityType weaknessType = EntityType.NONE;

    private MeshRenderer MR;

    [SerializeField] GameEventsSO onEnemyDeath;

    [SerializeField] Transform aimObject;
    private Transform player;

    private ObjectPool bulletPool;

    private void Awake()
    {
        MR = GetComponentInChildren<MeshRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        bulletPool = GameObject.FindGameObjectWithTag("Bullet Pool").GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        health = 3;

        switch (Random.Range(0, 3))
        {
            case 0:
                enemyType = EntityType.RED;
                weaknessType = EntityType.BLUE;
                MR.material.SetColor("_EmissionColor", Color.red);
                break;
            case 1:
                enemyType = EntityType.GREEN;
                weaknessType = EntityType.RED;
                MR.material.SetColor("_EmissionColor", Color.green);
                break;
            case 2:
                enemyType = EntityType.BLUE;
                weaknessType = EntityType.GREEN;
                MR.material.SetColor("_EmissionColor", Color.blue);
                break;
        }

        StartCoroutine(Shooting(0.5f, 1));
    }

    private void Update()
    {
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        transform.LookAt(player);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        StopAllCoroutines();

        onEnemyDeath.Raise();

        gameObject.SetActive(false);
    }

    IEnumerator Shooting(float startDelay, float interval)
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(interval - 0.25f, interval + 0.25f));

            GameObject bullet = bulletPool.RetrieveObject();

            BulletBehaviour BO = bullet.GetComponent<BulletBehaviour>();
            BO.InitializeType(enemyType);

            bullet.transform.position = aimObject.position;
            bullet.transform.rotation = transform.rotation;

            BO.Fire();
        }
    }
}
