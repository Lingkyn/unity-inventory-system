using System;
using System.Collections.Generic;
using UnityEngine;

//将背包数据以Json格式存储在本地，以及读取

public class PackageLocalData
{
    private static PackageLocalData _instance;
    public static PackageLocalData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PackageLocalData();
            }
            return _instance;
        }
    }
    public List<PackageLocalItem> items;

    public void SavePackage()
    {
        string inventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PackageLocalData", inventoryJson);
        PlayerPrefs.Save();
    }
    public List<PackageLocalItem> LoadPackage()
    {
        if (items != null)
        {
            return items;
        }
        if (PlayerPrefs.HasKey("PackageLocalData"))
        {
            string inventoryJson = PlayerPrefs.GetString("PackageLocalData");
            PackageLocalData packageLocalData = JsonUtility.FromJson<PackageLocalData>(inventoryJson);
            items = packageLocalData.items;
            return items;
        }
        else
        {
            items = new List<PackageLocalItem>();
            return items;
        }
    }

    public PackageLocalItem AddItem(int id, int num = 1)
    {
        if (num <= 0)
        {
            Debug.LogWarning("PackageLocalData.AddItem: num must be greater than zero");
            return null;
        }

        List<PackageLocalItem> list = LoadPackage();
        PackageLocalItem existed = list.Find(item => item.id == id);
        if (existed != null)
        {
            existed.num += num;
        }
        else
        {
            existed = new PackageLocalItem
            {
                uid = System.Guid.NewGuid().ToString(),
                id = id,
                num = num
            };
            list.Add(existed);
        }

        SavePackage();
        return existed;
    }

}

[System.Serializable]

public class PackageLocalItem
{
    public string uid;
    public int id;
    public int num;
}