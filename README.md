# PuffPew

<p align="center">
  <img src="Cover.png" alt="PuffPew cover" width="900">
</p>

一款可爱、明亮的 Unity 2D 俯视角自动射击游戏。移动小英雄，在逐波增强的敌群中生存；武器会自动瞄准并攻击，经验球会自动吸附，升级后选择更适合本局的强化组合。

## 游戏特色

- 自动战斗：手枪自动射击，斧头范围挥击，炸弹造成范围爆炸。
- 轻松移动：支持 `WASD` / 方向键移动，镜头保持全景固定视角。
- 成长循环：击败敌人掉落经验球，拾取后升级并选择武器或强化。
- 波次挑战：敌人的数量、生命和伤害会随波次提升；第 5 波起会出现 3 倍体型、10 倍基础生命的大型敌人。
- 生存补给：敌人有概率掉落血包，满血时也会自动拾取以保持操作节奏。
- 正式占位美术与音频：已接入背景、角色、敌人、武器、爆炸、UI 和 BGM / 命中音效资源。

## 游戏画面

<p align="center">
  <img src="Assets/Resources/background.png" alt="Battlefield background" width="720">
</p>

| Player | Enemy | Pistol | Axe | Bomb |
| :---: | :---: | :---: | :---: | :---: |
| <img src="Assets/Resources/Player.png" alt="Player sprites" width="130"> | <img src="Assets/Resources/anemy.png" alt="Enemy sprite" width="110"> | <img src="Assets/Resources/gun.png" alt="Pistol sprite" width="110"> | <img src="Assets/Resources/axe.png" alt="Axe sprite" width="110"> | <img src="Assets/Resources/bomb.png" alt="Bomb sprite" width="110"> |

## 运行方式

1. 使用 Unity `6000.5.10f1` 或兼容的 Unity 6 版本打开项目。
2. 打开 `Assets/Scenes/GameScene.unity`。
3. 点击 Play 即可运行。运行时会自动构建游戏所需的角色、敌人、武器和 HUD。

## 操作

| 按键 | 功能 |
| --- | --- |
| `W` `A` `S` `D` | 移动角色 |
| 方向键 | 移动角色（旧输入系统可用时） |
| 鼠标 | 菜单与升级选择 |

## 项目结构

```text
Assets/
  Audio/          BGM、受伤与击杀音效
  Editor/         美术资源绑定与编辑器辅助工具
  Resources/      背景、角色、敌人、武器、UI 等美术资源
  Scenes/         GameScene
  Scripts/
    Core/         游戏初始化、波次、资源与音频管理
    Enemy/        敌人、生成与管理
    Pickups/      经验球与血包
    Player/       移动、血量、经验与角色朝向
    UI/           HUD、升级、结算与飘字
    Weapons/      手枪、斧头、炸弹及投射物
```

## 版本

当前发布版本：`0.1.0`

`0.1.0` 固化了正式占位美术、音频、经验球自动吸附、血包掉落、大型敌人与波次成长等核心体验。
