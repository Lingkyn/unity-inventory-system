using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PackagePanel : BasePanel
{
    private Transform UIMenu;
    private Transform UIMenuVegetation;
    private Transform UIMenuSand;
    private Transform UIMenuRocks;
    private Transform UIMenuShellfish;
    private Transform UITabName;
    private Transform UICloseBtn;
    private Transform UICenter;
    private Transform UIScrollView;
    private Transform UIDetailPanel;
    private Transform UILeftBtn;
    private Transform UIRightBtn;
    private Transform UIDeletePanel;
    private Transform UIDeleteBackBtn;
    private Transform UIDeleteConfirmBtn;
    private Transform UIBottomMenus;
    private Transform UIDeleteBtn;
    private Transform UIDetailBtn;

    private Transform currentSelectedMenu;  // 当前选中的菜单
    private PackageCell currentSelectedCell;  // 当前选中的格子
    private System.Collections.Generic.List<PackageCell> allPackageCells = new System.Collections.Generic.List<PackageCell>();  // 所有的格子列表

    public GameObject PackageUIItemPrefab;


    override protected void Awake()
    {
        base.Awake();
        InitUI();
    }

    private void Start()
    {
        RefreshUI();
    }

    private void InitUI()
    {
        InitUIName();
        InitClick();
    }

    public void RefreshUI()
    {
        RefreshScroll();
    }


    private void RefreshScroll()
    {
        // 清理滚动容器中原本的物品
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        for (int i = scrollContent.childCount - 1; i >= 0; i--)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }
        // 清空格子列表
        allPackageCells.Clear();
        PackageLocalItem firstItem = null;
        int count = 0;
        foreach (PackageLocalItem localData in GameManager.Instance.GetSortPackageLocalData())
        {
            Transform PackageUIItem = Instantiate(PackageUIItemPrefab.transform, scrollContent) as Transform;
            PackageCell packageCell = PackageUIItem.GetComponent<PackageCell>();
            // 关键：把本地数据和父面板传给格子，否则格子不会填充名称和图片
            packageCell.Refresh(localData, this);
            // 添加到格子列表
            allPackageCells.Add(packageCell);
            if (firstItem == null) firstItem = localData;
            count++;
        }

        // 自动显示第一个物品（若存在），保证右侧 DetailPanel 在打开时有内容
        if (firstItem != null)
        {
            ShowDetail(firstItem);
        }

    }

    private void InitUIName()
    {
        //Top Center
        UIMenu = transform.Find("TopCenter/Menu");
        UIMenuVegetation = transform.Find("TopCenter/Menus/Vegetation");
        UIMenuSand = transform.Find("TopCenter/Menus/Sand");
        UIMenuRocks = transform.Find("TopCenter/Menus/Rocks");
        UIMenuShellfish = transform.Find("TopCenter/Menus/Shellfish");

        //Left Top
        UITabName = transform.Find("LeftTop/TabName");


        //Right Top（NumText 若需单独引用可增加 UINumText 变量）
        UICloseBtn = transform.Find("RightTop/CloseBtn");

        //Center
        UICenter = transform.Find("Center");
        UIScrollView = transform.Find("Center/Scroll View");
        UIDetailPanel = transform.Find("Center/DetailPanel");

        //Left and Right Btn
        UIRightBtn = transform.Find("Right/Button");
        UILeftBtn = transform.Find("Left/Button");

        //Bottom
        UIDeletePanel = transform.Find("Bottom/DeletePanel");
        UIDeleteBackBtn = transform.Find("Bottom/DeletePanel/Back");
        UIDeleteConfirmBtn = transform.Find("Bottom/DeletePanel/ConfirmBtn");
        UIBottomMenus = transform.Find("Bottom/BottomMenus");
        UIDeleteBtn = transform.Find("Bottom/BottomMenus/DeleteBtn");
        UIDetailBtn = transform.Find("Bottom/BottomMenus/DetailBtn");

        UIDeletePanel.gameObject.SetActive(false);
        UIBottomMenus.gameObject.SetActive(true);
    }

    private void InitClick()
    {
        UIMenuVegetation.GetComponent<Button>().onClick.AddListener(OnClickVegetation);
        UIMenuSand.GetComponent<Button>().onClick.AddListener(OnClickSand);
        UIMenuRocks.GetComponent<Button>().onClick.AddListener(OnClickRocks);
        UIMenuShellfish.GetComponent<Button>().onClick.AddListener(OnClickShellfish);

        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
        UILeftBtn.GetComponent<Button>().onClick.AddListener(OnClickLeft);
        UIRightBtn.GetComponent<Button>().onClick.AddListener(OnClickRight);

        UIDeleteBackBtn.GetComponent<Button>().onClick.AddListener(OnDeleteBack);
        UIDeleteConfirmBtn.GetComponent<Button>().onClick.AddListener(OnDeleteConfirm);
        UIDeleteBtn.GetComponent<Button>().onClick.AddListener(OnDelete);
        UIDetailBtn.GetComponent<Button>().onClick.AddListener(OnDetail);

        // 默认选中 Vegetation
        SwitchMenu(UIMenuVegetation);
    }

    private void SwitchMenu(Transform selectedMenu)
    {
        // 重置所有菜单按钮状态
        ResetMenuState(UIMenuVegetation);
        ResetMenuState(UIMenuSand);
        ResetMenuState(UIMenuRocks);
        ResetMenuState(UIMenuShellfish);

        // 激活选中的菜单
        ActivateMenuState(selectedMenu);
        currentSelectedMenu = selectedMenu;

        // 更新TabName文本
        UpdateTabName(selectedMenu);
    }

    private void UpdateTabName(Transform selectedMenu)
    {
        if (UITabName == null) return;

        TextMeshProUGUI tabNameText = UITabName.GetComponent<TextMeshProUGUI>();
        if (tabNameText == null) return;

        if (selectedMenu == UIMenuVegetation)
            tabNameText.text = "VEGETATION";
        else if (selectedMenu == UIMenuSand)
            tabNameText.text = "SAND";
        else if (selectedMenu == UIMenuRocks)
            tabNameText.text = "ROCKS";
        else if (selectedMenu == UIMenuShellfish)
            tabNameText.text = "SHELLFISH";
    }

    private void ResetMenuState(Transform menu)
    {
        Transform icon1 = menu.Find("Icon1");
        Transform icon2 = menu.Find("Icon2");
        Transform select = menu.Find("Select");

        if (icon1 != null) icon1.gameObject.SetActive(true);
        if (icon2 != null) icon2.gameObject.SetActive(false);
        if (select != null) select.gameObject.SetActive(false);
    }

    private void ActivateMenuState(Transform menu)
    {
        Transform icon1 = menu.Find("Icon1");
        Transform icon2 = menu.Find("Icon2");
        Transform select = menu.Find("Select");

        if (icon1 != null) icon1.gameObject.SetActive(false);
        if (icon2 != null) icon2.gameObject.SetActive(true);
        if (select != null) select.gameObject.SetActive(true);
    }
    private void OnClickVegetation()
    {
        print(">>>>> OnClickVegetation");
        SwitchMenu(UIMenuVegetation);
    }

    private void OnClickSand()
    {
        print(">>>>> OnClickSand");
        SwitchMenu(UIMenuSand);
    }
    private void OnClickRocks()
    {
        print(">>>>> OnClickRocks");
        SwitchMenu(UIMenuRocks);
    }
    private void OnClickShellfish()
    {
        print(">>>>> OnClickShellfish");
        SwitchMenu(UIMenuShellfish);
    }

    private void OnClickClose()
    {
        print(">>>>> OnClickClose");
        ClosePanel();
        // UIManager.Instance.ClosePanel(UIConst.PackagePanel);
    }

    private void OnClickLeft()
    {
        print(">>>>> OnClickLeft");
        if (allPackageCells.Count == 0) return;

        int currentIndex = -1;
        if (currentSelectedCell != null)
        {
            currentIndex = allPackageCells.IndexOf(currentSelectedCell);
        }

        // 切换到上一个
        int previousIndex = currentIndex - 1;
        if (previousIndex < 0)
        {
            previousIndex = allPackageCells.Count - 1;  // 循环到最后一个
        }

        // 选中并显示详情
        PackageCell targetCell = allPackageCells[previousIndex];
        SelectCell(targetCell);
        ShowDetail(targetCell.GetLocalData());
    }

    private void OnClickRight()
    {
        print(">>>>> OnClickRight");
        if (allPackageCells.Count == 0) return;

        int currentIndex = -1;
        if (currentSelectedCell != null)
        {
            currentIndex = allPackageCells.IndexOf(currentSelectedCell);
        }

        // 切换到下一个
        int nextIndex = currentIndex + 1;
        if (nextIndex >= allPackageCells.Count)
        {
            nextIndex = 0;  // 循环到第一个
        }

        // 选中并显示详情
        PackageCell targetCell = allPackageCells[nextIndex];
        SelectCell(targetCell);
        ShowDetail(targetCell.GetLocalData());
    }

    private void OnDeleteBack()
    {
        print(">>>>> onDeleteBack");
    }

    private void OnDeleteConfirm()
    {
        print(">>>>> OnDeleteConfirm");
    }

    private void OnDelete()
    {
        print(">>>>> OnDelete");
    }

    private void OnDetail()
    {
        print(">>>>> OnDetail");
    }

    // Called by PackageCell when a cell is clicked to select it
    public void SelectCell(PackageCell cell)
    {
        // 取消之前选中的格子
        if (currentSelectedCell != null)
        {
            currentSelectedCell.SetSelectState(false);
        }

        // 设置新选中的格子
        currentSelectedCell = cell;
        if (currentSelectedCell != null)
        {
            currentSelectedCell.SetSelectState(true);
        }
    }

    // Called by PackageCell when a cell is clicked to display detail info
    public void ShowDetail(PackageLocalItem item)
    {
        if (UIDetailPanel == null)
        {
            Debug.LogWarning("PackagePanel.ShowDetail: UIDetailPanel is null", this);
            return;
        }

        PackageDetail detail = UIDetailPanel.GetComponent<PackageDetail>();
        if (detail == null)
        {
            Debug.LogWarning("PackagePanel.ShowDetail: PackageDetail component not found on UIDetailPanel", UIDetailPanel);
            return;
        }

        detail.Refresh(item, this);
    }
}
