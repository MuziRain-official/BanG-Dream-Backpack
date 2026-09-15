using UnityEngine;

namespace MiniUI
{
    /// <summary>
    /// 门面（Facade）：业务代码只跟它打交道，不用关心 WindowLayer / PanelLayer 内部。
    ///
    /// 场景搭建方式：
    ///   Canvas
    ///    └── UIManager (挂本脚本)
    ///         ├── WindowLayer (空物体，挂 WindowLayer，RectTransform 铺满)
    ///         └── PanelLayer  (空物体，挂 PanelLayer，RectTransform 铺满)
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>全局唯一实例。</summary>
        public static UIManager I { get; private set; }

        [SerializeField] private WindowLayer windowLayer;
        [SerializeField] private PanelLayer panelLayer;

        private void Awake()
        {
            I = this;

            // 没在 Inspector 里拖的话，自动从子物体查找
            if (windowLayer == null) windowLayer = GetComponentInChildren<WindowLayer>();
            if (panelLayer == null) panelLayer = GetComponentInChildren<PanelLayer>();

            // 让 UI 跨场景常驻（不需要可以删掉这行）
            DontDestroyOnLoad(gameObject);
        }

        // ==================== 注册 ====================

        public void RegisterWindow(string id, UIScreen screen) => windowLayer.Register(id, screen);
        public void RegisterPanel(string id, UIScreen screen) => panelLayer.Register(id, screen);
        public void UnregisterWindow(string id) => windowLayer.Unregister(id);
        public void UnregisterPanel(string id) => panelLayer.Unregister(id);

        // ==================== 窗口 ====================

        /// <summary>打开窗口（可传属性）。</summary>
        public void OpenWindow(string id, object props = null) => windowLayer.ShowById(id, props);

        /// <summary>关闭指定窗口。</summary>
        public void CloseWindow(string id) => windowLayer.HideById(id);

        /// <summary>关闭当前窗口（自动回到上一个）。</summary>
        public void CloseCurrentWindow()
        {
            if (windowLayer.Current != null)
            {
                windowLayer.HideScreen(windowLayer.Current);
            }
        }

        // ==================== 面板 ====================

        public void ShowPanel(string id, object props = null) => panelLayer.ShowById(id, props);
        public void HidePanel(string id) => panelLayer.HideById(id);

        // ==================== 查询 ====================

        public bool IsWindowOpen(string id) => windowLayer.IsVisible(id);
        public bool IsPanelShown(string id) => panelLayer.IsVisible(id);

        // ==================== 批量 ====================

        public void HideAll(bool animate = true)
        {
            windowLayer.HideAll(animate);
            panelLayer.HideAll(animate);
        }
    }
}
