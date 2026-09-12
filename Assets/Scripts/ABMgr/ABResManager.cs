using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ABResManager : MonoBehaviour
{
    private static ABResManager _instance;

    public static ABResManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<ABResManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<ABResManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [Header("主包(Manifest)名，即打包输出文件夹名，例如 StandaloneWindows")]
    [SerializeField] private string manifestBundleName = "StandaloneWindows";

    //字典缓存：包名 -> 已加载的 AssetBundle
    private Dictionary<string, AssetBundle> abDic = new();

    //主包及其依赖清单（懒加载，只加载一次）
    private AssetBundle manifestAB;
    private AssetBundleManifest manifest;

    /// <summary>
    /// 加载主包并取出 AssetBundleManifest（依赖关系表）
    /// </summary>
    private AssetBundleManifest GetManifest()
    {
        if (manifest != null)
        {
            return manifest;
        }

        string path = Application.streamingAssetsPath + "/" + manifestBundleName;
        manifestAB = AssetBundle.LoadFromFile(path);
        if (manifestAB == null)
        {
            Debug.LogError("[ABResManager] 主包加载失败: " + path);
            return null;
        }

        manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        return manifest;
    }

    /// <summary>
    /// 获取某个包的所有依赖（直接 + 间接）
    /// </summary>
    private string[] GetDependencies(string abName)
    {
        AssetBundleManifest m = GetManifest();
        if (m == null)
        {
            return Array.Empty<string>();
        }

        return m.GetAllDependencies(abName);
    }

    /// <summary>
    /// 加载单个包（含依赖），若已缓存则直接返回。必须先加载依赖，再加载目标包。
    /// </summary>
    private AssetBundle LoadAB(string abName)
    {
        if (abDic.TryGetValue(abName, out AssetBundle cached))
        {
            return cached;
        }

        // 1. 先加载所有依赖包
        foreach (string dep in GetDependencies(abName))
        {
            if (!abDic.ContainsKey(dep))
            {
                AssetBundle depAB = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + dep);
                if (depAB != null)
                {
                    abDic.Add(dep, depAB);
                }
                else
                {
                    Debug.LogError("[ABResManager] 依赖包加载失败: " + dep);
                }
            }
        }

        // 2. 再加载目标包
        AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abName);
        if (ab != null)
        {
            abDic.Add(abName, ab);
        }
        else
        {
            Debug.LogError("[ABResManager] 包加载失败: " + abName);
        }

        return ab;
    }

    // ===================== 同步加载 =====================

    /// <summary>同步加载资源，返回强类型</summary>
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        AssetBundle ab = LoadAB(abName);
        if (ab == null)
        {
            return null;
        }

        return ab.LoadAsset<T>(resName);
    }

    /// <summary>同步加载资源，返回 Object 基类型</summary>
    public Object LoadRes(string abName, string resName)
    {
        AssetBundle ab = LoadAB(abName);
        if (ab == null)
        {
            return null;
        }

        return ab.LoadAsset(resName);
    }

    // ===================== 异步加载 =====================

    /// <summary>异步加载资源，完成后通过回调返回</summary>
    public void LoadResAsync<T>(string abName, string resName, Action<T> callback) where T : Object
    {
        StartCoroutine(LoadResAsyncRoutine(abName, resName, callback));
    }

    private IEnumerator LoadResAsyncRoutine<T>(string abName, string resName, Action<T> callback) where T : Object
    {
        AssetBundle ab = LoadAB(abName);
        if (ab == null)
        {
            callback?.Invoke(null);
            yield break;
        }

        AssetBundleRequest req = ab.LoadAssetAsync<T>(resName);
        yield return req;
        callback?.Invoke(req.asset as T);
    }

    // ===================== 卸载 =====================

    /// <summary>
    /// 卸载单个包
    /// </summary>
    /// <param name="abName">包名</param>
    /// <param name="unloadAllLoadedObjects">true 会同时销毁该包加载出来的所有资源实例</param>
    public void UnLoad(string abName, bool unloadAllLoadedObjects = false)
    {
        if (abDic.TryGetValue(abName, out AssetBundle ab))
        {
            ab.Unload(unloadAllLoadedObjects);
            abDic.Remove(abName);
        }
    }

    /// <summary>卸载全部包（含主包 Manifest）</summary>
    public void ClearAB(bool unloadAllLoadedObjects = false)
    {
        foreach (KeyValuePair<string, AssetBundle> kv in abDic)
        {
            if (kv.Value != null)
            {
                kv.Value.Unload(unloadAllLoadedObjects);
            }
        }
        abDic.Clear();

        // 卸载主包，并重置 manifest，下次用到时重新加载
        if (manifestAB != null)
        {
            manifestAB.Unload(true);
            manifestAB = null;
        }
        manifest = null;
    }
}
