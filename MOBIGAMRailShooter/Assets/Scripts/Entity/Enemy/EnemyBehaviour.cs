using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private int health = 3;

    public EntityType enemyType = EntityType.NONE;
    public EntityType weaknessType = EntityType.NONE;

    private MeshRenderer MR = null;

    public GameEventsSO onEnemyDeath = null;

    public Transform aimObject = null;
    private Transform player = null;

    private ObjectPool bulletPool = null;
    private ObjectPool explosionPool = null;

    private void Awake()
    {
        MR = GetComponentInChildren<MeshRenderer>();
        /*MR.sharedMaterial = BundleManager.Instance.GetAsset<Material>("materials", "Mat_Glow");
        string shader = MR.sharedMaterial.shader.name;
        MR.sharedMaterial.shader = Shader.Find(shader);*/

        onEnemyDeath = BundleManager.Instance.GetAsset<GameEventsSO>("eventso", "EventSO_OnEnemyKilled");

        player = GameObject.FindGameObjectWithTag("Player").transform;

        bulletPool = GameObject.FindGameObjectWithTag("Bullet Pool").GetComponent<ObjectPool>();

        explosionPool = GameObject.FindGameObjectWithTag("Explosion Pool").GetComponent<ObjectPool>();
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
        if (player != null)
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

        AudioManager.Instance.Play("Explosion");

        onEnemyDeath.Raise();

        GameObject go = explosionPool.RetrieveObject();
        go.transform.position = transform.position;

        gameObject.SetActive(false);
    }

    IEnumerator Shooting(float startDelay, float interval)
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(interval + 0.25f, interval + 0.75f));

            GameObject bullet = bulletPool.RetrieveObject();

            BulletBehaviour BO = bullet.GetComponent<BulletBehaviour>();
            BO.InitializeType(enemyType);

            bullet.transform.position = aimObject.position;
            bullet.transform.rotation = transform.rotation;

            BO.Fire();
        }
    }
}
