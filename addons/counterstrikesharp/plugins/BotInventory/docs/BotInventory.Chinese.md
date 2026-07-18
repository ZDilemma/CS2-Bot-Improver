# BotInventory 配置说明

本说明文件由 BotInventory 自动生成，位置与 `BotInventory.json` 相同。

默认目录：

`addons/counterstrikesharp/configs/plugins/Botinventory/`

目录中会看到：

- `BotInventory.json`：实际配置文件。
- `BotInventory.zh-CN.md`：中文说明。
- `BotInventory.en-US.md`：英文说明。

> BotInventory 当前只处理 BOT。真人玩家不会进入随机库存生成流程。
>
> 修改 `BotInventory.json` 后，请重新加载插件或重启服务器，使配置重新读取。
>
> 配置文件在加载后会被规范化并重新写入。JSON 注释虽然可以被读取，但重新保存后会消失；未知字段也不应依赖。建议只使用本文列出的字段，并在大改前备份配置。

---

## 1. ID 查询

BotInventory 有意保留详细 ID 配置，以便高级用户精确控制随机池。

ID查询方法：打开https://csgoskins.gg/，网页顶部拥有各种物品的选择，选择需要的武器和刀具，例如Rifles-AK47，能看见第一个Consequence of the Jinn，
进入到AK47 | Consequence of the Jinn页面，网页往下拉右边的Summary类目里，Finish Catalog就是它的PaintKit，就是1466，刀具皮肤ID查找方法和上述一致。

## 2. 概率、范围和 ID 的通用规则

### 概率

`Chance`、`Rate` 一类字段通常使用 `0.0` 到 `1.0`：

- `0.0` = 0%
- `0.25` = 25%
- `0.5` = 50%
- `0.8` = 80%
- `1.0` = 100%

`Agents.Chance`、`StatTrak.Chance`、`Souvenir.Chance` 会在运行时限制到 `0.0`～`1.0`。`Stickers.Kato14Rate` 建议同样保持在这个范围内。

### ID

本插件保留详细 ID 配置。常见 ID 类型包括：

- Weapon Definition Index / `DefIndex`：武器定义 ID。
- Paint Kit ID：皮肤或手套涂装 ID。
- Agent Definition Index：探员物品定义 ID。
- Music Kit ID：音乐盒 ID。
- Sticker ID：印花 ID。

请使用与当前 CS2 版本匹配的 ID。无效 ID 可能被忽略，也可能导致对应物品无法正确显示。

### 数值范围

- `MaxWear` 建议填写 `0.0`～`1.0`。运行时会限制到这个范围。
- `MinSeed` 必须小于或等于 `MaxSeed`。
- 数组为空通常表示该功能没有可供随机的候选项。

---

## 3. Weapons — 武器皮肤

示意结构：

```json
"Weapons": {
  "Enabled": true,
  "MaxWear": 0.2,
  "MinSeed": 1,
  "MaxSeed": 10000,
  "PaintKits": {
    "7": [302, 600, 675],
    "16": [309, 449, 632]
  }
}
```

### `Weapons.Enabled`

是否启用 BOT 普通武器的随机皮肤库存。

- `true`：根据 `PaintKits` 为配置过的武器生成随机皮肤。
- `false`：不为普通武器生成这部分随机皮肤。

### `Weapons.MaxWear`

武器随机磨损值的最大值。

当前实现会从 `0` 到 `MaxWear` 之间随机生成磨损，并将 `MaxWear` 限制到 `0.0`～`1.0`。

例如：

- `0.07`：只生成较低磨损。
- `0.2`：磨损随机范围更大。
- `1.0`：允许完整磨损范围。

### `Weapons.MinSeed` / `Weapons.MaxSeed`

武器 Pattern Seed 的随机范围，包含最小值和最大值。

请保持：

`MinSeed <= MaxSeed`

否则在生成随机库存时可能出现范围错误。

### `Weapons.PaintKits`

武器与可随机 Paint Kit 的映射。

键必须是武器 `DefIndex` 的字符串形式，例如：

```json
"7": [302, 600, 675]
```

表示 DefIndex `7` 的武器会从 `302`、`600`、`675` 中随机选择一个 Paint Kit。

注意：

- 无法解析为有效 `ushort` 的键会被忽略。
- 某武器对应的数组为空时，该武器不会生成随机皮肤。
- 这里决定“会随机到哪些皮肤”。StatTrak 的 `WeaponPaints` 只决定哪些武器+皮肤组合有资格生成 StatTrak，不会额外添加皮肤到随机池。

### 常用武器 DefIndex 对照表

下面这些是当前 `Weapons.PaintKits` 默认覆盖的可配置普通武器 DefIndex。JSON 中的键要写成字符串，例如 M4A1-S 使用 `"60"`。

| DefIndex | 武器 |
| ---: | --- |
| 1 | Desert Eagle |
| 2 | Dual Berettas |
| 3 | Five-SeveN |
| 4 | Glock-18 |
| 7 | AK-47 |
| 8 | AUG |
| 9 | AWP |
| 10 | FAMAS |
| 11 | G3SG1 |
| 13 | Galil AR |
| 14 | M249 |
| 16 | M4A4 |
| 17 | MAC-10 |
| 19 | P90 |
| 23 | MP5-SD |
| 24 | UMP-45 |
| 25 | XM1014 |
| 26 | PP-Bizon |
| 27 | MAG-7 |
| 28 | Negev |
| 29 | Sawed-Off |
| 30 | Tec-9 |
| 32 | P2000 |
| 33 | MP7 |
| 34 | MP9 |
| 35 | Nova |
| 36 | P250 |
| 38 | SCAR-20 |
| 39 | SG 553 |
| 40 | SSG 08 |
| 60 | M4A1-S |
| 61 | USP-S |
| 63 | CZ75-Auto |
| 64 | R8 Revolver |

这张表只列 BotInventory 武器皮肤配置实际使用的常规枪械 DefIndex，不包含手雷、C4、Zeus、默认刀等没有按 `Weapons.PaintKits` 方式配置皮肤的物品。

---

## 4. Knives — 刀具

示意结构：

```json
"Knives": {
  "Enabled": true,
  "MaxWear": 0.09,
  "Items": [
    {
      "Def": 507,
      "Enabled": true,
      "PaintKits": [38, 59, 413]
    }
  ]
}
```

### `Knives.Enabled`

是否启用随机刀具。

### `Knives.MaxWear`

刀具随机磨损最大值。运行时限制到 `0.0`～`1.0`。

### `Knives.Items`

刀具候选列表。每个对象代表一种刀型。

#### `Knives.Items[].Def`

刀具的 Definition Index。

必须填写有效刀具 DefIndex。

#### `Knives.Items[].Enabled`

是否允许这一种刀进入随机池。

#### `Knives.Items[].PaintKits`

该刀型可随机的 Paint Kit ID 数组。

数组为空时，这一刀型不会被选中。

CT 和 T 会分别从启用的刀具池中随机，因此两边可能获得不同刀型或皮肤。

### 刀具 DefIndex 对照表

`Knives.Items` 是可扩展列表，不限制为默认配置中已有的几把刀。只要填写有效的刀具 `Def`、至少一个有效 `PaintKits`，并设置 `Enabled: true`，就可以把其他刀型加入随机池。

| DefIndex | 刀具 |
| ---: | --- |
| 500 | Bayonet（刺刀） |
| 503 | Classic Knife（经典匕首） |
| 505 | Flip Knife（折叠刀） |
| 506 | Gut Knife（穿肠刀） |
| 507 | Karambit（爪子刀） |
| 508 | M9 Bayonet（M9 刺刀） |
| 509 | Huntsman Knife（猎杀者匕首） |
| 512 | Falchion Knife（弯刀） |
| 514 | Bowie Knife（鲍伊猎刀） |
| 515 | Butterfly Knife（蝴蝶刀） |
| 516 | Shadow Daggers（暗影双匕） |
| 517 | Paracord Knife（系绳匕首） |
| 518 | Survival Knife（求生匕首） |
| 519 | Ursus Knife（熊刀） |
| 520 | Navaja Knife（折刀） |
| 521 | Nomad Knife（流浪者匕首） |
| 522 | Stiletto Knife（短剑） |
| 523 | Talon Knife（锯齿爪刀） |
| 525 | Skeleton Knife（骷髅匕首） |
| 526 | Kukri Knife（廓尔喀刀） |

> `42` 和 `59` 是阵营默认刀具定义，不建议作为这里的特殊外观刀具候选。上表列的是通常用于库存皮肤刀具的特殊刀型 DefIndex。

例如，要手动加入骷髅匕首，可以在 `Knives.Items` 数组中追加一个对象：

```json
{
  "Def": 525,
  "Enabled": true,
  "PaintKits": [0, 413, 44, 415, 416]
}
```

完整结构示例：

```json
"Knives": {
  "Enabled": true,
  "MaxWear": 0.09,
  "Items": [
    {
      "Def": 507,
      "Enabled": true,
      "PaintKits": [38, 59, 413]
    },
    {
      "Def": 525,
      "Enabled": true,
      "PaintKits": [0, 413, 44, 415, 416]
    }
  ]
}
```

新增对象后不需要修改 C# 源码；插件会按 JSON 中启用的候选项构建刀具随机池。请注意，`PaintKits` 仍应填写该刀型实际可用的 Paint Kit ID。

---

## 5. Gloves — 手套

示意结构：

```json
"Gloves": {
  "Enabled": true,
  "MaxWear": 0.06,
  "MinSeed": 1,
  "MaxSeed": 10000,
  "Items": [
    {
      "Def": 5030,
      "Paint": 10019,
      "Enabled": true
    }
  ]
}
```

### `Gloves.Enabled`

是否启用 BOT 随机手套。

### `Gloves.MaxWear`

手套随机磨损最大值。建议 `0.0`～`1.0`。

### `Gloves.MinSeed` / `Gloves.MaxSeed`

手套 Pattern Seed 随机范围。

请保持 `MinSeed <= MaxSeed`。

### `Gloves.Items`

可随机手套列表。

#### `Gloves.Items[].Def`

手套 Definition Index。

#### `Gloves.Items[].Paint`

手套 Paint Kit ID。


#### `Gloves.Items[].Enabled`

是否允许该手套进入随机池。

只有 `Enabled = true`、`Def > 0` 且 `Paint >= 0` 的项目会进入候选池。

---

## 6. MusicKits — 音乐盒

示意结构：

```json
"MusicKits": {
  "Enabled": true,
  "Ids": [3, 4, 5, 6]
}
```

### `MusicKits.Enabled`

是否启用 BOT 随机音乐盒。

### `MusicKits.Ids`

可随机的 Music Kit ID 数组。

数组为空时不会分配随机音乐盒。

---

## 7. Agents — 探员

示意结构：

```json
"Agents": {
  "Enabled": true,
  "Chance": 1.0,
  "CtDefIndexes": [5308, 5404, 5602],
  "TDefIndexes": [4726, 5504, 5208]
}
```

### `Agents.Enabled`

这是当前唯一控制 BOT 随机探员的开关。

- `true`：BotInventory 会根据 `Agents.Chance`，从对应阵营的 Agent DefIndex 池中选择探员，并使用当前 Agent 模型/语音同步逻辑。
- `false`：BotInventory 不主动为 BOT 分配随机特殊探员，BOT 保持游戏默认阵营/地图角色逻辑。

旧的 `Models` 直接 `.vmdl` 随机配置已经移除，避免与真正的 Agent DefIndex 随机产生重复概念。

### `Agents.Chance`

BOT 使用特殊探员的概率。

- `1.0`：总是尝试从配置的特殊探员池中选择。
- `0.5`：约 50% 概率使用特殊探员。
- `0.0`：使用默认阵营/地图角色定义。

运行时会限制到 `0.0`～`1.0`。

### `Agents.CtDefIndexes`

CT 阵营可随机的 Agent Definition Index 数组。

### `Agents.TDefIndexes`

T 阵营可随机的 Agent Definition Index 数组。

BotInventory 会过滤：

- 非正数 ID。
- 超出 `ushort` 范围的 ID。
- AgentCatalog 中不属于对应阵营的 ID。
- 重复 ID。

插件会尽量避免同一时间给多个 BOT 分配相同阵营的同一特殊探员；当可用候选不足时仍可能重复。

> Agent 专属语音目前仍受 CS2 BOT Response/Voice Bank 行为影响。配置有效 Agent ID 不代表所有边缘 BOT 语音事件都一定存在。

所有探员一览:

CT 探员（29）
4619 | CT | 'Blueberries' Buckshot | NSWC SEAL | “蓝莓” 铅弹 | 海军水面战中心海豹部队
4680 | CT | 'Two Times' McCoy | TACP Cavalry | “两次”麦考伊 | 战术空中管制部队装甲兵
4711 | CT | Cmdr. Mae 'Dead Cold' Jamison | SWAT | 指挥官 梅 “极寒” 贾米森 | 特警
4712 | CT | 1st Lieutenant Farlow | SWAT | 第一中尉法洛 | 特警
4713 | CT | John 'Van Healen' Kask | SWAT | 约翰 “范·海伦” 卡斯克 | 特警
4714 | CT | Bio-Haz Specialist | SWAT | 生物防害专家 | 特警
4715 | CT | Sergeant Bombson | SWAT | 军士长炸弹森 | 特警
4716 | CT | Chem-Haz Specialist | SWAT | 化学防害专家 | 特警
4749 | CT | Sous-Lieutenant Medic | Gendarmerie Nationale | 军医少尉 | 法国宪兵特勤队
4750 | CT | Chem-Haz Capitaine | Gendarmerie Nationale | 化学防害上尉 | 法国宪兵特勤队
4751 | CT | Chef d'Escadron Rouchard | Gendarmerie Nationale | 中队长鲁沙尔·勒库托 | 法国宪兵特勤队
4752 | CT | Aspirant | Gendarmerie Nationale | 准尉 | 法国宪兵特勤队
4753 | CT | Officer Jacques Beltram | Gendarmerie Nationale | 军官雅克·贝尔特朗 | 法国宪兵特勤队
4756 | CT | Lieutenant 'Tree Hugger' Farlow | SWAT | 中尉法洛（抱树人） | 特警
4757 | CT | Cmdr. Davida 'Goggles' Fernandez | SEAL Frogman | 指挥官黛维达·费尔南德斯（护目镜） | 海豹蛙人
4771 | CT | Cmdr. Frank 'Wet Sox' Baroud | SEAL Frogman | 指挥官弗兰克·巴鲁德（湿袜） | 海豹蛙人
4772 | CT | Lieutenant Rex Krikey | SEAL Frogman | 中尉雷克斯·克里奇 | 海豹蛙人
5305 | CT | Operator | FBI SWAT | 特种兵 | 联邦调查局（FBI）特警
5306 | CT | Markus Delrow | FBI HRT | 马尔库斯·戴劳 | 联邦调查局（FBI）人质营救队
5307 | CT | Michael Syfers | FBI Sniper | 迈克·赛弗斯 | 联邦调查局（FBI）狙击手
5308 | CT | Special Agent Ava | FBI | 爱娃特工 | 联邦调查局（FBI）
5400 | CT | 3rd Commando Company | KSK | 第三特种兵连 | 德国特种部队突击队
5401 | CT | Seal Team 6 Soldier | NSWC SEAL | 海豹突击队第六分队士兵 | 海军水面战中心海豹部队
5402 | CT | Buckshot | NSWC SEAL | 铅弹 | 海军水面战中心海豹部队
5403 | CT | 'Two Times' McCoy | USAF TACP | “两次”麦考伊 | 美国空军战术空中管制部队
5404 | CT | Lt. Commander Ricksaw | NSWC SEAL | 海军上尉里克索尔 | 海军水面战中心海豹部队
5405 | CT | Primeiro Tenente | Brazilian 1st Battalion | 陆军中尉普里米罗 | 巴西第一营
5601 | CT | B Squadron Officer | SAS | B 中队指挥官 | SAS
5602 | CT | D Squadron Officer | NZSAS | D 中队军官 | 新西兰特种空勤团


T 探员（34）
4613 | T | Bloody Darryl The Strapped | The Professionals | 残酷的达里尔（穷鬼） | 专业人士
4718 | T | Rezan the Redshirt | Sabre | 红衫列赞 | 军刀
4726 | T | Sir Bloody Miami Darryl | The Professionals | 残酷的达里尔爵士（迈阿密） | 专业人士
4727 | T | Safecracker Voltzmann | The Professionals | 飞贼波兹曼 | 专业人士
4728 | T | Little Kev | The Professionals | 小凯夫 | 专业人士
4730 | T | Getaway Sally | The Professionals | 出逃的萨莉 | 专业人士
4732 | T | Number K | The Professionals | 老K | 专业人士
4733 | T | Sir Bloody Silent Darryl | The Professionals | 残酷的达里尔爵士（沉默） | 专业人士
4734 | T | Sir Bloody Skullhead Darryl | The Professionals | 残酷的达里尔爵士（头盖骨） | 专业人士
4735 | T | Sir Bloody Darryl Royale | The Professionals | 残酷的达里尔爵士（皇家） | 专业人士
4736 | T | Sir Bloody Loudmouth Darryl | The Professionals | 残酷的达里尔爵士（聒噪） | 专业人士
4773 | T | Elite Trapper Solman | Guerrilla Warfare | 精锐捕兽者索尔曼 | 游击队
4774 | T | Crasswater The Forgotten | Guerrilla Warfare | 遗忘者克拉斯沃特 | 游击队
4775 | T | Arno The Overgrown | Guerrilla Warfare | 亚诺（野草） | 游击队
4776 | T | Col. Mangos Dabisi | Guerrilla Warfare | 上校曼戈斯·达比西 | 游击队
4777 | T | Vypa Sista of the Revolution | Guerrilla Warfare | 薇帕姐（革新派） | 游击队
4778 | T | Trapper Aggressor | Guerrilla Warfare | 捕兽者（挑衅者） | 游击队
4780 | T | 'Medium Rare' Crasswater | Guerrilla Warfare | 克拉斯沃特（三分熟） | 游击队
4781 | T | Trapper | Guerrilla Warfare | 捕兽者 | 游击队
5105 | T | Ground Rebel | Elite Crew | 地面叛军 | 精锐分子
5106 | T | Osiris | Elite Crew | 奥西瑞斯 | 精锐分子
5107 | T | Prof. Shahmat | Elite Crew | 沙哈马特教授 | 精锐分子
5108 | T | The Elite Mr. Muhlik | Elite Crew | 精英穆哈里克先生 | 精锐分子
5109 | T | Jungle Rebel | Elite Crew | 丛林反抗者 | 精锐分子
5205 | T | Soldier | Phoenix | 枪手 | 凤凰战士
5206 | T | Enforcer | Phoenix | 执行者 | 凤凰战士
5207 | T | Slingshot | Phoenix | 弹弓 | 凤凰战士
5208 | T | Street Soldier | Phoenix | 街头士兵 | 凤凰战士
5500 | T | Dragomir | Sabre | 德拉戈米尔 | 军刀
5501 | T | Maximus | Sabre | 马克西姆斯 | 军刀
5502 | T | Rezan The Ready | Sabre | 准备就绪的列赞 | 军刀
5503 | T | Blackwolf | Sabre | 黑狼 | 军刀
5504 | T | 'The Doctor' Romanov | Sabre | “医生”罗曼诺夫 | 军刀
5505 | T | Dragomir | Sabre Footsoldier | 德拉戈米尔 | 军刀勇士

---

## 8. Stickers — 印花

示意结构：

```json
"Stickers": {
  "Enabled": true,
  "Kato14Rate": 0.6
}
```

### `Stickers.Enabled`

是否为随机武器皮肤生成印花。

- `true`：启用当前的随机印花生成逻辑。
- `false`：武器不会由这套随机逻辑附加印花。

### `Stickers.Kato14Rate`

每次随机印花 ID 时，使用内置第一组印花 ID 范围的概率。

建议填写 `0.0`～`1.0`：

- `0.0`：始终从内置普通印花范围随机。
- `0.6`：约 60% 使用内置第一组范围，约 40% 使用内置普通范围。
- `1.0`：始终从内置第一组范围随机。

印花 ID 的两个数值范围现在固定在插件源码内部，不再暴露为 JSON 配置项，以减少无意义或容易误填的参数。当前内部范围仍保持原有行为不变。

### 当前贴纸槽位行为

当前实现会：

- 槽位 0、1、2：使用同一个随机 Sticker ID。
- 槽位 3、4：每个槽位各有 50% 概率生成一个随机 Sticker ID。
- Wear 固定为 `0.0`，Rotation 固定为 `0`。

这些槽位概率目前不是 JSON 可配置项。

---

## 9. StatTrak — StatTrak™

示意结构：

```json
"StatTrak": {
  "Enabled": true,
  "Chance": 0.6,
  "CounterMin": 1,
  "CounterMax": 10001,
  "WeaponPaints": {
    "7": [302, 600],
    "16": [309]
  }
}
```

### `StatTrak.Enabled`

是否允许生成 StatTrak 武器。

### `StatTrak.Chance`

对于“符合 StatTrak 资格”的武器+Paint Kit 组合，生成 StatTrak 的概率。

运行时限制到 `0.0`～`1.0`。

### `StatTrak.CounterMin` / `StatTrak.CounterMax`

生成的 StatTrak 计数器随机范围，包含两端。

当前实现会自动处理最小值和最大值顺序，即使两者写反也会使用较小值作为下限。

### `StatTrak.WeaponPaints`

StatTrak 资格白名单。

键是武器 DefIndex，数组是允许生成 StatTrak 的 Paint Kit ID。

例如：

```json
"7": [302, 600]
```

表示只有当 DefIndex `7` 实际随机到 Paint Kit `302` 或 `600` 时，才会继续进行 `StatTrak.Chance` 抽取。

重要：

- `WeaponPaints` 不会把新的皮肤加入 `Weapons.PaintKits`。
- 一个 Paint Kit 必须先在 `Weapons.PaintKits` 中被随机选中，再与这里的白名单匹配。
- 没有匹配白名单的武器不会生成 StatTrak，但仍可以参与 Souvenir 抽取。

---

## 10. Souvenir — 纪念品

示意结构：

```json
"Souvenir": {
  "Enabled": true,
  "Chance": 0.6
}
```

### `Souvenir.Enabled`

是否允许生成 Souvenir 品质。

### `Souvenir.Chance`

在当前武器没有成功生成 StatTrak 时，生成 Souvenir 的概率。

运行时限制到 `0.0`～`1.0`。

当前逻辑中 StatTrak 与 Souvenir 互斥：

1. 如果当前武器+Paint Kit 在 `StatTrak.WeaponPaints` 中，先尝试 StatTrak。
2. 只要最终没有生成 StatTrak，就继续进行 Souvenir 概率抽取。
3. 因此一个物品不会同时是 StatTrak 和 Souvenir。

---

## 11. 推荐的编辑方式

1. 停止服务器或卸载 BotInventory。
2. 备份 `BotInventory.json`。
3. 修改 JSON。
4. 检查括号、逗号、数组和 ID。
5. 重新加载插件或启动服务器。

---
