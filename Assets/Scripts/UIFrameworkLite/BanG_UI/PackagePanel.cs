using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniUI
{
    /// <summary>
    /// 背包窗口（挂在 PackagePanel 预制体根节点上）。
    /// 展示 PlayerData 里的物品：装备每件独立一格并显示等级，素材同类合并一格并显示数量。
    /// </summary>
    public class PackagePanel : UIScreen
    {
        [Header("数据")]
        [Tooltip("物品配置表 SO")]
        [SerializeField] private PackageTable configTable;

        [Tooltip("背包格子预制体（根节点需挂 PackageGrid + Button）")]
        [SerializeField] private GameObject itemGridPrefab;

        [Header("UI 控件")]
        [SerializeField] private Transform gridContent;      // 格子容器（GridLayoutGroup）
        [SerializeField] private Image detailIcon;           // 详情图标
        [SerializeField] private Text detailName;            // 详情名称
        [SerializeField] private Text detailDescription;     // 详情描述
        [SerializeField] private Text detailNum;             // 详情数量 / 等级

        private readonly List<PackageGrid> gridList = new();
        private Dictionary<int, PackageTableItem> configDict;
        private PackageItem selectedItem;
        private PackageGrid selectedGrid;

        protected override void OnShow()
        {
            PlayerData.I.Load();      // 打开时加载存档（只加载一次）
            BuildConfigDict();
            ClearDetail();
            RefreshGrid();
        }

        protected override void OnHide()
        {
            ClearGrid();
            selectedItem = null;
        }

        // ==================== 格子渲染 ====================

        private void RefreshGrid()
        {
            ClearGrid();

            foreach (PackageItem item in PlayerData.I.items)
            {
                CreateGrid(item);
            }
        }

        private void CreateGrid(PackageItem item)
        {
            if (itemGridPrefab == null)
            {
                Debug.LogWarning("[PackagePanel] 未设置格子预制体 itemGridPrefab");
                return;
            }

            GameObject go = Instantiate(itemGridPrefab, gridContent);
            PackageGrid grid = go.GetComponent<PackageGrid>();
            if (grid == null)
            {
                Debug.LogError("[PackagePanel] 格子预制体缺少 PackageGrid 脚本");
                Destroy(go);
                return;
            }

            grid.Init(item, GetConfig(item.id), OnGridClick);
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

        // ==================== 选中与详情 ====================

        private void OnGridClick(PackageGrid grid)
        {
            // 取消上一个格子的高亮，高亮当前格子
            if (selectedGrid != null && selectedGrid != grid)
            {
                selectedGrid.SetSelected(false);
            }

            selectedGrid = grid;
            grid.SetSelected(true);
            selectedItem = grid.Data;
            RefreshDetail(selectedItem);
        }

        private void RefreshDetail(PackageItem item)
        {
            PackageTableItem config = GetConfig(item.id);
            if (config == null) return;

            detailIcon.sprite = config.sprite;
            detailName.text = config.name;
            detailDescription.text = config.description;

            // 装备显示等级，素材显示数量，共用一个文本
            detailNum.text = config.itemType == ItemType.Equipment
                ? "等级：" + item.level
                : "数量：" + item.count;
        }

        private void ClearDetail()
        {
            selectedItem = null;

            if (selectedGrid != null)
            {
                selectedGrid.SetSelected(false);
                selectedGrid = null;
            }

            if (detailIcon != null) detailIcon.sprite = null;
            if (detailName != null) detailName.text = "";
            if (detailDescription != null) detailDescription.text = "";
            if (detailNum != null) detailNum.text = "";
        }

        // ==================== 配置表查询 ====================

        private void BuildConfigDict()
        {
            configDict = new Dictionary<int, PackageTableItem>();
            if (configTable == null) return;

            foreach (PackageTableItem item in configTable.DataList)
            {
                if (item != null && !configDict.ContainsKey(item.id))
                {
                    configDict[item.id] = item;
                }
            }
        }

        private PackageTableItem GetConfig(int id)
        {
            if (configDict == null) BuildConfigDict();
            configDict.TryGetValue(id, out PackageTableItem item);
            return item;
        }

        // ==================== 按钮方法（Unity Inspector 里自己绑定）====================

        /// <summary>关闭背包。</summary>
        public void UI_Close()
        {
            Close();
        }

        /// <summary>出售选中物品（出售规则后续细化）。</summary>
        public void UI_Sell()
        {
            if (selectedItem == null) return;

            PlayerData.I.RemoveItem(selectedItem);
            PlayerData.I.Save();
            selectedItem = null;

            RefreshGrid();
            ClearDetail();
        }

        /// <summary>升级选中装备（升级规则后续细化）。</summary>
        public void UI_Upgrade()
        {
            if (selectedItem == null) return;

            PackageTableItem config = GetConfig(selectedItem.id);
            if (config == null || config.itemType != ItemType.Equipment) return;

            PlayerData.I.LevelUp(selectedItem);
            PlayerData.I.Save();

            RefreshGrid();
            RefreshDetail(selectedItem);
        }
    }
}
