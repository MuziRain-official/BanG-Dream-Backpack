using UnityEngine;
using UnityEngine.UI;


namespace MiniUI
{
    public class Demo : MonoBehaviour
    {
        [SerializeField] private UIManager ui;

        [SerializeField] private StartWindow startWindowPrefab;   // 窗口 Prefab
        [SerializeField] private SecondWindow secondWindowPrefab;
        [SerializeField] private HUD hudPrefab;                   // 面板 Prefab

        private const string StartWindowId = "StartWindow";
        private const string SecondWindowId = "SecondWindow";
        private const string HudId = "HUD";

        private void Start()
        {
            // 1. 注册（注册后框架会自动把它们挂到对应层下）
            ui.RegisterWindow(StartWindowId, Instantiate(startWindowPrefab));
            ui.RegisterWindow(SecondWindowId, Instantiate(secondWindowPrefab));
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