using UnityEngine;

namespace MiniUI
{
    /// <summary>
    /// UIFrameworkScene 的入口：注册背包/商店窗口（场景专属，每次进场景重注册）
    /// 和导航面板（常驻，只注册一次）。商店/背包在切场景时由 NavPanel 清掉。
    /// </summary>
    public class PackageBootstrap : MonoBehaviour
    {
        [SerializeField] private StorePanel storePanelPrefab;
        [SerializeField] private PackagePanel packagePanelPrefab;
        [SerializeField] private NavPanel navPanelPrefab;

        private const string StorePanelId = "StorePanel";
        private const string PackagePanelId = "PackagePanel";
        private const string NavPanelId = "NavPanel";

        private static bool navRegistered;   // NavPanel 常驻，只注册一次

        private void Start()
        {
            // 先读存档，再开商店：避免“没读档就购买 → 保存时用空列表覆盖旧存档”
            PlayerData.I.Load();

            // 必须用静态单例：框架 DontDestroyOnLoad 后，切场景再切回来时，
            // 场景里会重新实例化一套 UIFramework，但那套会被单例保护销毁；
            // 序列化的 ui 引用指向的是那套即将销毁的新实例，静态单例才指向常驻实例。
            UIManager ui = UIManager.I;

            // 背包 + 商店（窗口，场景专属）：每次进入本场景都重新注册
            //（离开场景时 NavPanel 已把它们清掉，所以不会重复）
            ui.RegisterWindow(StorePanelId, Instantiate(storePanelPrefab));
            ui.RegisterWindow(PackagePanelId, Instantiate(packagePanelPrefab));

            // 导航面板（面板，常驻）：只注册一次
            if (!navRegistered)
            {
                navRegistered = true;
                ui.RegisterPanel(NavPanelId, Instantiate(navPanelPrefab));
            }

            ui.OpenWindow(StorePanelId);
            ui.ShowPanel(NavPanelId);
        }
    }
}
