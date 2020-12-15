using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayLoop("Background Music");
        AudioManager.Instance.PlayLoop("Fire Crackling");
    }
}
