using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject poolableObject = null;

    public int initialObjectCapacity = 0;
    private int currentIndex = -1;

    private List<GameObject> objectPool = null;

    private Transform ownerTransform = null;
    
    // Start is called before the first frame update
    private void Start()
    {
        ownerTransform = transform;

        objectPool = new List<GameObject>();

        for(int i = 0; i < initialObjectCapacity; i++)
        {
            GameObject go = Instantiate(poolableObject, ownerTransform);
            go.SetActive(false);

            objectPool.Add(go);
        }
    }

    public GameObject RetrieveObject()
    {
        int searchCount = 0;
        int poolSize = objectPool.Count;

        do
        {
            if (currentIndex < poolSize - 1) currentIndex++;
            else currentIndex = 0;

            if (!objectPool[currentIndex].activeSelf)
            {
                objectPool[currentIndex].SetActive(true);

                return objectPool[currentIndex];
            }

            searchCount++;
        } while (searchCount < poolSize);

        return AddNewObjectToPool();
    }

    private GameObject AddNewObjectToPool()
    {
        GameObject go = Instantiate(poolableObject, ownerTransform);
        go.SetActive(true);

        objectPool.Add(go);

        currentIndex++;

        return go;
    }

    public void ReturnObject(int index)
    {
        if (index >= 0 && index < objectPool.Count)
            objectPool[index].SetActive(false);
    }
}