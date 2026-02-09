using UnityEngine;

/// <summary>
/// 场景菜单控制器：挂在当前场景的某个 GameObject 上，供按钮 OnClick 引用。
/// 通过 GameManager.Instance 调用全局逻辑，避免跨场景后 Inspector 引用到已销毁的 GameManager。
/// 用法：00Start / 01Main 等有菜单的场景各挂一个，按钮拖本场景的此组件即可。
/// </summary>
public class SceneMenuController : MonoBehaviour
{
    public void QuitGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.QuitGame();
    }

    public void NewGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.NewGame();
    }

    public void LoadGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LoadGame();
    }
}
