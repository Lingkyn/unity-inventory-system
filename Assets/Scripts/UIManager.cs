using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    private Transform _uiRoot;
    // 路径配置字典
    private Dictionary<string, string> pathDict;
    // 预制件缓存字典
    private Dictionary<string, GameObject> prefabDict;
    // 已打开界面的缓存字典
    public Dictionary<string, BasePanel> panelDict;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<UIManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    _instance = go.AddComponent<UIManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    public Transform UIRoot
    {
        get
        {
            if (_uiRoot == null)
            {
                // 按优先级查找场景中的 Canvas，与 Hierarchy 中命名一致（UICanvas / PackageCanvas）
                string[] canvasNames = { "Canvas", "UICanvas", "PackageCanvas" };
                foreach (string canvasName in canvasNames)
                {
                    GameObject canvasObj = GameObject.Find(canvasName);
                    if (canvasObj != null)
                    {
                        _uiRoot = canvasObj.transform;
                        break;
                    }
                }
                if (_uiRoot == null)
                {
                    GameObject go = new GameObject("Canvas");
                    go.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                    go.AddComponent<CanvasScaler>();
                    go.AddComponent<GraphicRaycaster>();
                    _uiRoot = go.transform;
                }
            }
            return _uiRoot;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitDicts();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();

        pathDict = new Dictionary<string, string>()
        {
            {UIConst.PackagePanel, "Package/PackagePanel"},
        };
    }

    private void Start()
    {
        if (pathDict == null)
        {
            InitDicts();
        }
    }

    public BasePanel GetPanel(string name)
    {
        BasePanel panel = null;
        // 检查是否已打开
        if (panelDict.TryGetValue(name, out panel))
        {
            return panel;
        }
        return null;
    }

    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        // 检查是否已打开
        if (panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("界面已打开: " + name);
            return null;
        }

        // 检查路径是否配置
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.Log("界面名称错误，或未配置路径: " + name);
            return null;
        }

        // 使用缓存预制件
        GameObject panelPrefab = null;
        if (!prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = "Prefabs/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            if (panelPrefab == null)
            {
                Debug.LogError("UIManager.OpenPanel: 无法加载预制件: " + realPath);
                return null;
            }
            prefabDict.Add(name, panelPrefab);
        }

        // 打开界面
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        panel.OpenPanel(name);
        return panel;
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("界面未打开: " + name);
            return false;
        }

        panel.ClosePanel();
        // panelDict.Remove(name);
        return true;
    }
}

public enum PanelType
{
    PackagePanel
}

public class UIConst
{
    // menu panels

    public const string PackagePanel = "PackagePanel";
}

