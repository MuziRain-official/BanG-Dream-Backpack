using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LuaCopyEditor : Editor
{
    [MenuItem("XLua/Copy lua to txt")]
    public static void CopyLuaToTxt()
    {
        //找到所有lua文件
        string path = Application.dataPath + "/Scripts/Lua/";
        if (!Directory.Exists(path))
            return;
        string[] strs = Directory.GetFiles(path, "*.lua");
        //把所有lua文件拷贝到一个新的文件夹
        string newPath = Application.dataPath + "/LuaTxt/";
        //拷贝之前先清空
        if(!Directory.Exists(newPath))
            Directory.CreateDirectory(newPath);
        else
        {
            string[] oldPaths = Directory.GetFiles(newPath, "*.txt");
            foreach (string oldPath in oldPaths)
            {
                File.Delete(oldPath);
            }
        }

        string fileName;
        List<string> lines = new List<string>();
        for (int i = 0; i < strs.Length; i++)
        {
            fileName = newPath + strs[i].Substring(strs[i].LastIndexOf('/') + 1) + ".txt";
            lines.Add(fileName);
            File.Copy(strs[i], fileName, true);
        }
        
        AssetDatabase.Refresh();
        
        //刷新后才能打AB包
        for (int i = 0; i < lines.Count; i++)
        {
            AssetImporter importer = AssetImporter.GetAtPath(lines[i].Substring(lines[i].LastIndexOf("Assets")));
            importer.assetBundleName = "lua";
        }
    }
}
