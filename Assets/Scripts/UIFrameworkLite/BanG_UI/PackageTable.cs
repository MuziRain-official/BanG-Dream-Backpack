using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包物品配置表 - 存储物品的静态配置数据
/// 创建：通过 Unity 菜单 Create/PackageTable 创建配置表资源
/// </summary>
[CreateAssetMenu(menuName = "PackageTable", fileName = "PackageTable")]
public class PackageTable : ScriptableObject
{
    public List<PackageTableItem> DataList = new List<PackageTableItem>();
}

/// <summary>
/// 背包物品配置项
/// </summary>
[System.Serializable]
public class PackageTableItem
{
    public int id;
    public string name;
    public string description;
    public Sprite sprite;
    public int level;
    public ItemType itemType;
}

public enum ItemType
{
    Equipment,  // 装备：独立实例，支持升级
    Material   // 素材：可合并显示，无等级
}