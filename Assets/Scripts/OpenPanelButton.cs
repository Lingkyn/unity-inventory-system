using UnityEngine;
using UnityEngine.UI;

public class OpenPanelButton : MonoBehaviour
{
    public PanelType panelType = PanelType.PackagePanel;  // 在Inspector里选择要打开的面板

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        string panelName = panelType.ToString();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.OpenPanel(panelName);
        }
        else
        {
            Debug.LogError("UIManager Instance is null!");
        }
    }
}
