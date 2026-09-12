using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        LuaManager.Instance.Init();
        LuaManager.Instance.DoString("require('main')");
    }
}
