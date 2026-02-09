using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private PackageTable packageTable;

    [Header("Scene Management")]
    public string gameSceneName = "01Main";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // UIManager.Instance.OpenPanel(UIConst.PackagePanel);
        // print(GetPackageLocalData().Count);
        // print(GetPackageTable().DataList.Count);
    }

    public PackageTable GetPackageTable()
    {
        if (packageTable == null)
        {
            packageTable = Resources.Load<PackageTable>("Data/PackageTable");
        }
        return packageTable;
    }

    public List<PackageLocalItem> GetPackageLocalData()
    {
        return PackageLocalData.Instance.LoadPackage();
    }

    public PackageTableItem GetPackageItemById(int id)
    {
        List<PackageTableItem> packageDataList = GetPackageTable().DataList;
        foreach (PackageTableItem item in packageDataList)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }

    public PackageLocalItem GetPackageLocalItemByUId(string uid)
    {
        List<PackageLocalItem> packageDataList = GetPackageLocalData();
        foreach (PackageLocalItem item in packageDataList)
        {
            if (item.uid == uid)
            {
                return item;
            }
        }
        return null;
    }


    public PackageLocalItem AddItemToPackage(int id, int num = 1, bool refreshUI = true)
    {
        PackageLocalItem added = PackageLocalData.Instance.AddItem(id, num);

        if (refreshUI && UIManager.Instance != null)
        {
            PackagePanel panel = UIManager.Instance.GetPanel(UIConst.PackagePanel) as PackagePanel;
            if (panel != null)
            {
                panel.RefreshUI();
            }
        }

        return added;
    }


    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        // 不进行排序，直接返回本地包数据（保持加载时的原始顺序）
        return PackageLocalData.Instance.LoadPackage();
    }

    public void ClearGameData()
    {
        // 清除本地储存的背包数据
        PlayerPrefs.DeleteKey("PackageLocalData");
        PlayerPrefs.Save();
        
        // 重置背包数据实例
        PackageLocalData.Instance.items = new List<PackageLocalItem>();
    }

    public void NewGame()
    {
        // 清除游戏数据
        ClearGameData();
        
        // 跳转到游戏场景
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadGame()
    {
        // 加载本地数据（自动从 PlayerPrefs 中读取）
        PackageLocalData.Instance.LoadPackage();
        
        // 跳转到游戏场景
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // 编辑器中退出播放模式
        EditorApplication.isPlaying = false;
#else
        Debug.Log("正在退出游戏...");
        Application.Quit(0);
#endif
    }
}
