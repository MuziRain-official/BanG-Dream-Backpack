using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XLua;

public class CSharpCallLuaList
{
    [CSharpCallLua]
    public static List<Type> csharpCallLuaList = new List<Type>()
    {
        // Button.onClick:AddListener(...) 传入的是无参回调
        typeof(UnityAction),
        // Toggle.onValueChanged:AddListener(...) 传入的是带一个 bool 参数的回调
        typeof(UnityAction<bool>),
        
        typeof(UnityEngine.EventSystems.UIBehaviour),
    };
}
