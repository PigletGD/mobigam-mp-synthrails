using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionControl : MonoBehaviour
{
    public List<Material> materials;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Material material in materials)
            material.EnableKeyword("_EMISSION");

    }

    private void Update()
    {
        foreach (Material material in materials)
            material.EnableKeyword("_EMISSION");
    }
}
