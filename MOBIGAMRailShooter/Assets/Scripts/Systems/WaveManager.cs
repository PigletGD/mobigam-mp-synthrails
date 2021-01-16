using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] ObjectPool virusPool = null;
    [SerializeField] ObjectPool moneyPool = null;
    [SerializeField] ObjectPool verticalWallPool = null;
    [SerializeField] ObjectPool horizontalWallPool = null;
    [SerializeField] GameObject boss = null;

    bool isBossTime = false;

    private float timer = 0.0f;

    private int currentAmountOfEnemies = 0;

    [SerializeField] Camera cam = null;

    private void Start()
    {
        switch (SaveManager.Instance.currentLevel)
        {
            case 1:
                StartCoroutine("SpawnEnemiesEndless");
                StartCoroutine("SpawnMoney");
                break;
            case 2:
                StartCoroutine("SpawnHorizontalWalls");
                StartCoroutine("SpawnVerticalWalls");
                StartCoroutine("SpawnMoney");
                break;
            case 3:
                StartCoroutine("SpawnEnemiesEndless");
                StartCoroutine("SpawnHorizontalWalls");
                StartCoroutine("SpawnVerticalWalls");
                StartCoroutine("SpawnMoney");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 300.0f)
            isBossTime = true;

        if (currentAmountOfEnemies <= 0 && isBossTime)
        {
            StopAllCoroutines();

            SpawnBoss();
        }
    }

    IEnumerator SpawnVerticalWalls()
    {
        yield return new WaitForSeconds(3);

        while (true)
        {
            Vector3 pos = cam.WorldToViewportPoint(new Vector3(0, 0, 15));
            pos.x = Random.Range(0.35f, 0.65f);

            /*if (!OrientationManager.Instance.isLandscape)
            {
                float portraitHeight = OrientationManager.Instance.portraitHeight;

                pos.y = Mathf.Clamp(pos.y, ((Screen.height * 0.5f) - (portraitHeight * 0.5f)) / Screen.height, ((Screen.height * 0.5f) + (portraitHeight * 0.5f)) / Screen.height);
            }*/

            GameObject go = verticalWallPool.RetrieveObject();
            float xLoc = cam.ViewportToWorldPoint(pos).x;
            go.transform.position = new Vector3(xLoc, go.transform.position.y, go.transform.position.z);

            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnHorizontalWalls()
    {
        yield return new WaitForSeconds(5.5f);

        while (true)
        {
            Vector3 pos = cam.WorldToViewportPoint(new Vector3(0, 0, 15));
            pos.y = Random.Range(0.35f, 0.65f);

            if (!OrientationManager.Instance.isLandscape)
            {
                float portraitHeight = OrientationManager.Instance.portraitHeight;

                pos.y = Mathf.Clamp(pos.y, ((Screen.height * 0.5f) - (portraitHeight * 0.5f)) / Screen.height, ((Screen.height * 0.5f) + (portraitHeight * 0.5f)) / Screen.height);
            }

            GameObject go = horizontalWallPool.RetrieveObject();
            float yLoc = cam.ViewportToWorldPoint(pos).y;
            go.transform.position = new Vector3(go.transform.position.x, yLoc, go.transform.position.z);

            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnEnemiesEndless()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            if (currentAmountOfEnemies <= 0)
            {
                int minutes = (int)timer / 60;
                currentAmountOfEnemies = Random.Range(1, 2 + minutes);

                for (int i = 0; i < currentAmountOfEnemies; i++)
                {
                    yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));

                    Vector3 pos = cam.WorldToViewportPoint(new Vector3(0, 0, 15));
                    pos.x = Random.Range(0.1f, 0.9f);
                    pos.y = Random.Range(0.4f, 0.9f);

                    if (!OrientationManager.Instance.isLandscape)
                    {
                        float portraitHeight = OrientationManager.Instance.portraitHeight;

                        pos.y = Mathf.Clamp(pos.y, ((Screen.height * 0.5f) - (portraitHeight * 0.5f)) / Screen.height, ((Screen.height * 0.5f) + (portraitHeight * 0.5f)) / Screen.height);
                    }
                        

                    GameObject go = virusPool.RetrieveObject();
                    go.transform.position = cam.ViewportToWorldPoint(pos);
                }
            }

            yield return null;
        }
    }

    IEnumerator SpawnMoney()
    {
        yield return new WaitForSeconds(1);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));

            Vector3 pos = cam.WorldToViewportPoint(new Vector3(0, 0, cam.nearClipPlane));
            pos.x = Random.Range(0.25f, 0.75f);
            pos.y = Random.Range(0.5f, 0.8f);

            if (!OrientationManager.Instance.isLandscape)
            {
                float portraitHeight = OrientationManager.Instance.portraitHeight;

                pos.y = Mathf.Clamp(pos.y, ((Screen.height * 0.5f) - (portraitHeight * 0.5f)) / Screen.height, ((Screen.height * 0.5f) + (portraitHeight * 0.5f)) / Screen.height);
            }

            GameObject go = moneyPool.RetrieveObject();
            go.transform.position = cam.ViewportToWorldPoint(pos);
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 20);

            yield return null;
        }
    }

    private void SpawnBoss()
    {
        Instantiate(boss, new Vector3(0, 8, 40), boss.transform.rotation);
    }

    public void DecreaseEnemyCount()
    {
        currentAmountOfEnemies -= 1;
    }
}
