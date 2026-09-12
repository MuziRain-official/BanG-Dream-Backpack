using System.IO;
using UnityEngine;
using XLua;

public class LuaManager : MonoBehaviour
{
    private static LuaManager _instance;

    public static LuaManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<LuaManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<LuaManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    private LuaEnv luaEnv;
    [SerializeField]
    private string luaBundleName = "lua"; 

    //得到lua中的_G
    public LuaTable Global
    {
        get
        {
            return luaEnv.Global;
        }
    }

    public void Init()
    {
        if (luaEnv != null)
            return;
        luaEnv = new LuaEnv();
        
        luaEnv.AddLoader(MyCustomLoader);
        luaEnv.AddLoader(MyCustomABLoader);
    }
    
    public void DoString(string str)
    {
        luaEnv.DoString(str);
    }

    public void Tick()
    {
        luaEnv.Tick();
    }

    public void DisPose()
    {
        luaEnv.Dispose();
        luaEnv = null;
    }
    
    private byte[] MyCustomLoader(ref string filePath)
    {
        string path = Application.dataPath + "/Scripts/Lua/" + filePath + ".lua";
        Debug.Log(path);

        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        else
        {
            Debug.Log("文件重定向失败");
        }

        return null;
    }
    
    private byte[] MyCustomABLoader(ref string filePath)
    {
        Debug.Log("进入AB包加载");

        TextAsset tx = ABResManager.Instance.LoadRes<TextAsset>(luaBundleName,filePath + ".lua");
        if (tx == null)
        {
            Debug.Log("AB包中未找到: " + filePath);
            return null;
        }
        return tx.bytes;
    }
    
}