using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        LuaManager.Instance.Init();

        // 首次进入：加载所有 lua 脚本（之后 require 有缓存，不会重复执行）
        LuaManager.Instance.DoString("require('main')");

        // 每次进入场景：重置并重新显示 UI（切场景后 Canvas / 面板引用已失效）
        LuaManager.Instance.DoString("InitScene()");
    }
}
