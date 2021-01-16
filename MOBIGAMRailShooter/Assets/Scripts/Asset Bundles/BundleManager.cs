using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BundleManager : MonoBehaviour
{
    public static BundleManager Instance { set; get; }

    public string BundleRoot
    {
        get
        {
#if UNITY_EDITOR
            return Application.streamingAssetsPath;
#elif UNITY_ANDROID
            return Application.persistentDataPath;
#endif
        }
    }

    Dictionary<string, AssetBundle> LoadedBundles = new Dictionary<string, AssetBundle>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public AssetBundle LoadBundle(string bundleTarget)
    {
        if (LoadedBundles.ContainsKey(bundleTarget))
        {
            return LoadedBundles[bundleTarget];
        }

        AssetBundle ret = AssetBundle.LoadFromFile(Path.Combine(BundleRoot, bundleTarget));

        if (ret == null)
        {
            Debug.LogError($"{bundleTarget} does not exist");
        }
        else
        {
            LoadedBundles.Add(bundleTarget, ret);
        }

        return ret;
    }

    public T GetAsset<T>(string bundleTarget, string assetName) where T : UnityEngine.Object
    {
        T ret = null;
        AssetBundle bundle = LoadBundle(bundleTarget);

        if (bundle != null)
        {
            ret = bundle.LoadAsset<T>(assetName);
        }

        return ret;
    }
}