using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] ObjectPool virusPool;

    bool isBossTime = false;

    private float timer = 0.0f;

    private int currentAmountOfEnemies = 0;

    [SerializeField] Camera cam;

    private bool isLandscape;

    private float portraitHeight;

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
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 300.0f)
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
                currentAmountOfEnemies = Random.Range(1 + minutes, 3 + (minutes * 2));

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

    public void DecreaseEnemyCount()
    {
        currentAmountOfEnemies -= 1;

        if (currentAmountOfEnemies <= 0 && isBossTime)
        {
            StopAllCoroutines();
        }
    }
}
