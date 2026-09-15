using UnityEngine;
using UnityEngine.UI;

namespace MiniUI
{
    // ============================================================
    // 示例：一个窗口 + 一个面板，展示框架的完整用法
    // ============================================================

    /// <summary>窗口的属性对象：打开窗口时传进来的数据。</summary>
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

    /// <summary>示例面板控制器（常驻 HUD）。</summary>
    public class HUD : UIScreen
    {
        [SerializeField] private Text scoreLabel;

        // 面板是常驻的，通常由外部直接调用刷新方法
        public void SetScore(int score)
        {
            scoreLabel.text = "分数：" + score;
        }
    }

    // ============================================================
    // 使用示例（随便挂在一个场景物体上）
    // ============================================================
    public class Demo : MonoBehaviour
    {
        [SerializeField] private UIManager ui;

        [SerializeField] private StartWindow startWindowPrefab;   // 窗口 Prefab
        [SerializeField] private HUD hudPrefab;                   // 面板 Prefab

        private const string StartWindowId = "StartWindow";
        private const string HudId = "HUD";

        private void Start()
        {
            // 1. 注册（注册后框架会自动把它们挂到对应层下）
            ui.RegisterWindow(StartWindowId, Instantiate(startWindowPrefab));
            ui.RegisterPanel(HudId, Instantiate(hudPrefab));

            // 2. 打开窗口 + 传入属性
            ui.OpenWindow(StartWindowId, new StartWindowData { PlayerName = "小明" });

            // 3. 显示面板
            ui.ShowPanel(HudId);

            // 4. 关闭当前窗口（会自动回到上一个窗口，若没有上一个则什么都不显示）
            // ui.CloseCurrentWindow();
        }
    }
}
