using System.Collections.Generic;
using UnityEngine;

namespace MiniUI
{
    /// <summary>
    /// 商店窗口（挂在 StorePanel 预制体根节点上）。
    /// 展示配置表里所有物品，可用素材/装备筛选；点击商品后 Buy 购买入包。
    /// 商品格子复用背包的 Item 预制体 + PackageGrid（商店模式显示名称）。
    /// 购买规则：装备每件独立（每次购买生成一条 level=1 的独立记录），素材同类合并数量。
    /// </summary>
    public class StorePanel : UIScreen
    {
        [Header("数据")]
        [Tooltip("物品配置表 SO")]
        [SerializeField] private PackageTable configTable;

        [Tooltip("商品格子预制体（复用背包 Item 预制体，根节点挂 PackageGrid）")]
        [SerializeField] private GameObject itemGridPrefab;

        [Header("UI 控件")]
        [SerializeField] private Transform gridContent;   // 商品容器（GridLayoutGroup）

        private const string PackagePanelId = "PackagePanel";   // 与入口里注册背包用的 id 保持一致

        private readonly List<PackageGrid> gridList = new();
        private ItemType? filterType;      // null=全部，Material=只看素材，Equipment=只看装备
        private PackageTableItem selectedConfig;
        private PackageGrid selectedGrid;

        protected override void OnShow()
        {
            filterType = null;      // 每次打开默认显示全部
            RefreshGrid();
        }

        protected override void OnHide()
        {
            ClearGrid();
            selectedConfig = null;
            selectedGrid = null;
        }

        // ==================== 商品列表 ====================

        private void RefreshGrid()
        {
            ClearGrid();
            selectedConfig = null;

            if (configTable == null) return;

            foreach (PackageTableItem cfg in configTable.DataList)
            {
                if (cfg == null) continue;
                if (filterType.HasValue && cfg.itemType != filterType.Value) continue;
                CreateGrid(cfg);
            }
        }

        private void CreateGrid(PackageTableItem config)
        {
            if (itemGridPrefab == null)
            {
                Debug.LogWarning("[StorePanel] 未设置商品格子预制体 itemGridPrefab");
                return;
            }

            GameObject go = Instantiate(itemGridPrefab, gridContent);
            PackageGrid grid = go.GetComponent<PackageGrid>();
            if (grid == null)
            {
                Debug.LogError("[StorePanel] 商品格子预制体缺少 PackageGrid 脚本");
                Destroy(go);
                return;
            }

            grid.Init(config, OnGridClick);   // 商店模式：显示名称
            gridList.Add(grid);
        }

        private void ClearGrid()
        {
            foreach (PackageGrid grid in gridList)
            {
                if (grid != null) Destroy(grid.gameObject);
            }
            gridList.Clear();
            selectedGrid = null;
        }

        // ==================== 选中 ====================

        private void OnGridClick(PackageGrid grid)
        {
            if (selectedGrid != null && selectedGrid != grid)
            {
                selectedGrid.SetSelected(false);
            }

            selectedGrid = grid;
            grid.SetSelected(true);
            selectedConfig = grid.Config;
        }

        // ==================== 按钮方法（Unity Inspector 里自己绑定）====================

        /// <summary>购买选中的物品：装备独立一件，素材 +1 合并。</summary>
        public void UI_Buy()
        {
            if (selectedConfig == null) return;

            bool isEquipment = selectedConfig.itemType == ItemType.Equipment;
            PlayerData.I.AddItem(selectedConfig.id, 1, isEquipment);
            PlayerData.I.Save();

            Debug.Log($"[StorePanel] 购买: {selectedConfig.name}");
        }

        /// <summary>只看素材（再次点击切回全部）。</summary>
        public void UI_Material()
        {
            SetFilter(ItemType.Material);
        }

        /// <summary>只看装备（再次点击切回全部）。</summary>
        public void UI_Equipment()
        {
            SetFilter(ItemType.Equipment);
        }

        /// <summary>打开背包。</summary>
        public void UI_Package()
        {
            UIManager.I.OpenWindow(PackagePanelId);
        }

        private void SetFilter(ItemType type)
        {
            // 点击当前已激活的分类，切回“全部”
            filterType = (filterType == type) ? null : type;
            RefreshGrid();
        }
    }
}
