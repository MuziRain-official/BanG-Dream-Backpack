using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

namespace MiniUI
{
    /// <summary>打开第二个窗口时传的数据</summary>
    public class SecondWindowData
    {
        public string Message = "这是第二个窗口";
    }

    /// <summary>第二个窗口（挂在它的 Prefab 根节点上）</summary>
    public class SecondWindow : UIScreen
    {
        [SerializeField] private Text titleLabel;

        // 打开时读传入数据，填到标题
        protected override void OnPropertiesSet()
        {
            var data = Props as SecondWindowData;
            if (data != null)
            {
                titleLabel.text = data.Message;
            }
        }

        // 关闭按钮：请求关闭自己，窗口层会自动回到 StartWindow
        public void UI_Close()
        {
            Close();
        }
    }
}
