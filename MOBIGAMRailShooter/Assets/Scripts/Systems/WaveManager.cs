using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] ObjectPool virusPool = null;
    [SerializeField] ObjectPool moneyPool = null;
    [SerializeField] GameObject boss = null;

    bool isBossTime = false;

    private float timer = 0.0f;

    private int currentAmountOfEnemies = 0;

    [SerializeField] Camera cam = null;

    private bool isLandscape = false;

    private float portraitHeight = 0;
    private void Start()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            portraitHeight = (Screen.width * Screen.width) / Screen.height;
            portraitHeight = portraitHeight / Screen.height;

            isLandscape = false;
        }
        else
        {
            portraitHeight = (Screen.height * Screen.height) / Screen.width;
            portraitHeight = portraitHeight / Screen.width;

            isLandscape = true;
        }

        StartCoroutine("SpawnEnemies");
        StartCoroutine("SpawnMoney");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1.0f)
            isBossTime = true;

        if ((Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) && isLandscape)
            isLandscape = false;
        else if ((Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeRight) && !isLandscape)
            isLandscape = true;
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(3);

        while (true)
        {
            if (currentAmountOfEnemies <= 0)
            {
                int minutes = (int)timer / 60;
                currentAmountOfEnemies = Random.Range(1, 2 + minutes);

                for (int i = 0; i < currentAmountOfEnemies; i++)
                {
                    yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));

                    Vector3 pos = cam.WorldToViewportPoint(new Vector3(0, 0, Random.Range(12.0f, 18.0f)));
                    pos.x = Random.Range(0.1f, 0.9f);
                    pos.y = Random.Range(0.4f, 0.9f);

                    if (!isLandscape)
                        pos.y = Mathf.Clamp(pos.y, ((Screen.height * 0.5f) - (portraitHeight * 0.5f)) / Screen.height, ((Screen.height * 0.5f) + (portraitHeight * 0.5f)) / Screen.height);

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

            if (!isLandscape)
                pos.y = Mathf.Clamp(pos.y, ((Screen.height * 0.5f) - (portraitHeight * 0.5f)) / Screen.height, ((Screen.height * 0.5f) + (portraitHeight * 0.5f)) / Screen.height);

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

        if (currentAmountOfEnemies <= 0 && isBossTime)
        {
            StopAllCoroutines();

            SpawnBoss();
        }
    }
}
