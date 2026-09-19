using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniUI
{
    /// <summary>
    /// 背包/商店通用格子（挂在 Item 预制体根节点上）。
    /// 对应预制体结构：
    ///   Item            (挂本脚本)
    ///     ├── bk            (Image 背景)
    ///     ├── Image         (Image 图标)
    ///     └── Text (Legacy) (Text：背包显示数量/等级，商店显示名称)
    /// 无需 Button：本脚本实现 IPointerClickHandler，点 bk/Image/Text 任一即可选中。
    /// </summary>
    public class PackageGrid : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image iconImg;    // 图标（子物体 Image）
        [SerializeField] private Text countText;   // 文本（子物体 Text (Legacy)）
        [SerializeField] private Image bgImg;      // 背景（子物体 bk，用于选中高亮）

        [Tooltip("选中时的背景色")]
        [SerializeField] private Color selectedColor = new Color(1f, 0.85f, 0.4f, 1f);

        private Color normalBgColor = Color.white;
        private Action<PackageGrid> onClick;

        /// <summary>背包模式下的运行时数据（商店模式下为 null）。</summary>
        public PackageItem Data { get; private set; }

        /// <summary>本格子对应的配置数据。</summary>
        public PackageTableItem Config { get; private set; }

        private void Awake()
        {
            // 未在 Inspector 拖拽时，按预制体固定的子物体名自动查找
            if (iconImg == null) iconImg = transform.Find("Image")?.GetComponent<Image>();
            if (countText == null) countText = transform.Find("Text (Legacy)")?.GetComponent<Text>();
            if (bgImg == null) bgImg = transform.Find("bk")?.GetComponent<Image>();

            if (bgImg != null) normalBgColor = bgImg.color;
        }

        /// <summary>背包模式：显示图标 + 等级/数量。</summary>
        public void Init(PackageItem item, PackageTableItem config, Action<PackageGrid> clickCallback)
        {
            Data = item;
            Config = config;
            onClick = clickCallback;

            if (config == null) return;

            if (iconImg != null) iconImg.sprite = config.sprite;

            if (countText != null)
            {
                // 装备显示等级，素材显示数量，共用一个文本
                countText.text = config.itemType == ItemType.Equipment
                    ? item.level.ToString()
                    : item.count.ToString();
            }
        }

        /// <summary>商店模式：显示图标 + 名称。</summary>
        public void Init(PackageTableItem config, Action<PackageGrid> clickCallback)
        {
            Data = null;
            Config = config;
            onClick = clickCallback;

            if (config == null) return;

            if (iconImg != null) iconImg.sprite = config.sprite;
            if (countText != null) countText.text = config.name;
        }

        /// <summary>选中 / 取消选中时的高亮表现。</summary>
        public void SetSelected(bool selected)
        {
            if (bgImg != null)
            {
                bgImg.color = selected ? selectedColor : normalBgColor;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(this);
        }
    }
}
