using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniUI
{
    /// <summary>
    /// 导航面板（Panel 类型，挂在 NavPanel 预制体根节点上）。
    /// 常驻显示，两个按钮切换两个游戏场景：C# UI 框架背包 / Lua 背包。
    /// 因为整套 UI 框架 DontDestroyOnLoad，切场景后本面板仍保留。
    /// </summary>
    public class NavPanel : UIScreen
    {
        [Header("场景名（必须已加入 Build Settings）")]
        [Tooltip("C# UI 框架背包系统场景")]
        [SerializeField] private string uiFrameworkSceneName = "UIFrameworkScene";

        [Tooltip("Lua 背包系统场景")]
        [SerializeField] private string luaSceneName = "SampleScene";

        /// <summary>切换到 UI 框架背包系统场景。</summary>
        public void UI_SwitchToUIFramework()
        {
            SwitchScene(uiFrameworkSceneName);
        }

        /// <summary>切换到 Lua 背包系统场景。</summary>
        public void UI_SwitchToLua()
        {
            SwitchScene(luaSceneName);
        }

        private void SwitchScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;

            // 切场景前清掉上一个场景的窗口（StorePanel / PackagePanel 等）。
            // 本面板是 Panel，ClearWindows 不会清掉它，跨场景后仍常驻。
            if (UIManager.I != null)
            {
                UIManager.I.ClearWindows();
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}
