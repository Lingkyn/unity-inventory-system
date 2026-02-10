# Unity 背包系统

本仓库提供一套可复用的 Unity 背包/收集系统，采用模块化 UI 架构。流程为：**场景拾取** → **分类浏览** → **详情展示**，背包数据通过 PlayerPrefs 持久化。当前示例内容为 Mosaic Shorelines 海岸线生态物品。



## 功能特性

- **场景拾取**：通过 `WorldPickup` 组件与 Collider 在场景中拾取物品。
- **物品配置**：使用 ScriptableObject（`PackageTable`）定义物品表，支持分类与类型。
- **模块化 UI**：背包界面由列表、格子与详情面板组成，结构清晰、便于扩展。
- **数据持久化**：使用 PlayerPrefs 存储背包数据，相同 id 的物品数量自动累加。

## 环境要求

| 依赖                   | 说明                                                                              |
| ---------------------- | --------------------------------------------------------------------------------- |
| Unity                  | 6000.x（或与目标项目版本一致）                                                    |
| TextMeshPro / Unity UI | 工程内已包含                                                                      |
| Canvas                 | 场景中须存在 Canvas（或 `UICanvas` / `PackageCanvas`）；否则由 UIManager 自动创建 |

## 快速开始

1. 克隆本仓库或下载 ZIP，使用 Unity 打开工程根目录。

2. 打开场景 **`Assets/Scenes/01Main.unity`** 并运行。

3. 在场景中点击可拾取物将其加入背包，再通过界面上的背包按钮打开背包进行查看。



## 集成与扩展

### 1. 物品表（PackageTable）

- **资源路径**：`Resources/Data/PackageTable.asset`。  
  也可在 Project 窗口右键 **Create > Custom > PackageTable** 新建。

- **条目字段**：`id`（唯一）、`categrory`、`type`、`name`、`description`、`imagePath`。  
  `imagePath` 为相对于 `Resources` 的路径，例如 `Sprites/Vegetation/Spartina`。

- **类型显示**：详情页中的类型标签由 `PackageDetail.GetTypeName` 根据 `type` 映射，可按需扩展。

### 2. 场景拾取

- 在目标物体上添加组件 **WorldPickup**（须挂载 Collider）。

- 在 Inspector 中配置 **Package Id**（与物品表中 id 一致）、**Num**、**Destroy On Pickup**。

### 3. 打开背包界面

- 在对应 Button 上添加 **OpenPanelButton**，将 Panel Type 设为 `PackagePanel`。

- 背包面板预制体路径为 `Resources/Prefabs/Panel/Package/PackagePanel.prefab`。  
  若需修改路径，请在 **UIManager** 的 `InitDicts` 中调整 `pathDict`。

### 4. 代码 API

| 功能               | 调用方式                                         |
| ------------------ | ------------------------------------------------ |
| 向背包添加物品     | `GameManager.Instance.AddItemToPackage(id, num)` |
| 获取当前背包数据   | `GameManager.Instance.GetPackageLocalData()`     |
| 根据 id 查询物品表 | `GameManager.Instance.GetPackageItemById(id)`    |
| 清空存档数据       | `GameManager.Instance.ClearGameData()`           |

背包中的每条记录为 `PackageLocalItem`（含 uid、id、num）；相同 id 的数量会累加。持久化使用的 PlayerPrefs 键为 `"PackageLocalData"`，由 **PackageLocalData** 单例负责读写。

**调用示例：**

```csharp
// 向背包添加物品
GameManager.Instance.AddItemToPackage(itemId: 1, num: 3);

// 获取当前背包数据
var list = GameManager.Instance.GetPackageLocalData();
```



## 核心脚本与职责

| 脚本                                           | 职责说明                                          |
| ---------------------------------------------- | ------------------------------------------------- |
| **GameManager**                                | 加载物品表、读写背包数据、提供 AddItem 与清档接口 |
| **PackageLocalData**                           | 单例，使用 JSON 序列化并通过 PlayerPrefs 读写     |
| **PackageTable**                               | ScriptableObject，定义物品表结构及条目            |
| **PackagePanel / PackageCell / PackageDetail** | 背包界面：列表、格子与详情展示                    |
| **WorldPickup**                                | 场景内可拾取物体的逻辑与交互                      |
| **OpenPanelButton**                            | 根据配置打开指定 UI 面板（如背包）                |
| **UIManager**                                  | 管理面板加载与预制体路径配置                      |



## 素材与致谢

本仓库示例中所使用的图片素材（如 `Assets/Resources/Sprites`、`Assets/UI/Icon` 等）由 **Nano Banana AI** 生成。

## 许可

本项目采用 [MIT License](LICENSE)。

在保留原始版权声明的前提下，允许对本作品进行使用、修改与分发。
