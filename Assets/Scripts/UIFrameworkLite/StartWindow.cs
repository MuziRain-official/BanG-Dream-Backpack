using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniUI
{
    public class StartWindowData
    {
        public string PlayerName = "玩家";
    }

    /// <summary>示例窗口控制器（挂在窗口 Prefab 根节点上）。</summary>
    public class StartWindow : UIScreen
    {
        [SerializeField] private Text titleLabel;

        // 属性设置后触发：在这里读打开时传进来的数据
        protected override void OnPropertiesSet()
        {
            StartWindowData data = Props as StartWindowData;
            if (data != null)
            {
                titleLabel.text = "欢迎，" + data.PlayerName;
            }
        }

        // 挂到 Button 的 OnClick 上（方法名带 UI_ 前缀方便在 Inspector 里查找）
        public void UI_Start()
        {
            Debug.Log("开始游戏！");
            Close();   // 请求关闭自己，窗口层会自动返回上一个窗口
        }
    }
    
}

