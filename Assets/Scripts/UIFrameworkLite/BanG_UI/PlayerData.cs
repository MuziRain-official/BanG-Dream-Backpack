using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MiniUI
{
    /// <summary>背包里单条物品的运行时数据。</summary>
    [System.Serializable]
    public class PackageItem
    {
        public int id;      // 对应 PackageTableItem.id
        public int level;   // 装备等级（素材恒为 0）
        public int count;   // 素材数量（装备恒为 1）
    }

    /// <summary>存档结构（JsonUtility 需要包一层才能序列化 List）。</summary>
    [System.Serializable]
    public class PackageSaveData
    {
        public List<PackageItem> items = new List<PackageItem>();
    }

    /// <summary>
    /// 玩家背包数据（全局唯一，静态访问）。
    /// 负责：持有物品列表 + JSON 存档。存档只存 id / level / count，
    /// 名称、图标、描述等静态信息走配置表 PackageTable，不入存档。
    /// </summary>
    public class PlayerData
    {
        public static PlayerData I { get; } = new PlayerData();

        public List<PackageItem> items = new List<PackageItem>();

        private const string FileName = "packageData.json";
        private static string SavePath => Path.Combine(Application.persistentDataPath, FileName);

        private bool loaded;

        /// <summary>从 JSON 读取存档（只读一次，避免反复覆盖内存数据）。</summary>
        public void Load()
        {
            if (loaded) return;
            loaded = true;

            if (!File.Exists(SavePath)) return;

            string json = File.ReadAllText(SavePath);
            PackageSaveData data = JsonUtility.FromJson<PackageSaveData>(json);
            if (data != null && data.items != null)
            {
                items = data.items;
            }
        }

        /// <summary>把当前背包写回 JSON。</summary>
        public void Save()
        {
            if (!loaded) Load();   // 兜底：没读档先读，避免用空列表覆盖已有存档
            PackageSaveData data = new PackageSaveData { items = items };
            File.WriteAllText(SavePath, JsonUtility.ToJson(data));
        }

        /// <summary>
        /// 添加物品。装备(isEquipment=true)：每件独立占一格；素材：同类合并数量。
        /// </summary>
        public void AddItem(int id, int count, bool isEquipment, int level = 1)
        {
            if (!loaded) Load();   // 兜底：确保在已读档的数据上操作，而不是空列表
            if (isEquipment)
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(new PackageItem { id = id, level = level, count = 1 });
                }
            }
            else
            {
                PackageItem exist = items.Find(p => p.id == id);
                if (exist != null) exist.count += count;
                else items.Add(new PackageItem { id = id, level = 0, count = count });
            }
        }

        /// <summary>移除指定物品（出售 / 消耗）。</summary>
        public void RemoveItem(PackageItem item) => items.Remove(item);

        /// <summary>装备升一级。</summary>
        public void LevelUp(PackageItem item) => item.level++;

        /// <summary>清空背包。</summary>
        public void Clear() => items.Clear();
    }
}
