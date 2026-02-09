using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldPickup : MonoBehaviour
{
    [Tooltip("对应 PackageTable 中的 id")]
    public int packageId;

    [Tooltip("拾取数量")]
    public int num = 1;

    [Tooltip("拾取后是否销毁场景物体")]
    public bool destroyOnPickup = true;

    private void OnMouseDown()
    {
        // 背包界面打开时不再响应场景拾取，避免误触
        if (UIManager.Instance != null && UIManager.Instance.GetPanel(UIConst.PackagePanel) != null)
        {
            return;
        }

        GameManager gm = EnsureGameManager();
        if (gm == null)
        {
            Debug.LogWarning("WorldPickup: GameManager not found in scene, cannot add to package", this);
            return;
        }

        PackageLocalItem added = gm.AddItemToPackage(packageId, num);
        if (added != null && destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }

    private GameManager EnsureGameManager()
    {
        if (GameManager.Instance != null)
        {
            return GameManager.Instance;
        }

        // 尝试在场景中查找 GameManager
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            return gm;
        }

        return null;
    }
}
