# BotInventory Configuration Guide

This guide is generated automatically by BotInventory next to `BotInventory.json`.

Default directory:

`addons/counterstrikesharp/configs/plugins/Botinventory/`

Files in this directory:

- `BotInventory.json`: the active configuration file.
- `BotInventory.zh-CN.md`: Chinese guide.
- `BotInventory.en-US.md`: English guide.

> BotInventory currently targets BOTs only. Human players do not enter the randomized inventory generation path.
>
> After editing `BotInventory.json`, reload the plugin or restart the server so the configuration is read again.
>
> The configuration is normalized and written back after loading. JSON comments can be parsed, but they will disappear when the file is rewritten. Do not rely on unknown custom properties. Back up the file before large edits.

---

## 1. ID lookup resources

BotInventory intentionally keeps detailed ID-based configuration so advanced users can precisely control each random pool.

ID lookup method: Open https://csgoskins.gg/. Various item categories are available at the top of the webpage. Select the weapons and knives you need, such as Rifles - AK47, where you can find Consequence of the Jinn as the first entry.
Navigate to the AK47 | Consequence of the Jinn page, scroll down the webpage, and locate the Summary section on the right. The Finish Catalog value corresponds to its PaintKit ID, which is 1466. The lookup method for knife skin IDs follows the exact same process described above.

## 2. General rules for probabilities, ranges, and IDs

### Probabilities

Fields named `Chance` or `Rate` normally use values from `0.0` to `1.0`:

- `0.0` = 0%
- `0.25` = 25%
- `0.5` = 50%
- `0.8` = 80%
- `1.0` = 100%

`Agents.Chance`, `StatTrak.Chance`, and `Souvenir.Chance` are clamped to `0.0`-`1.0` at runtime. Keep `Stickers.Kato14Rate` in the same range as well.

### IDs

BotInventory intentionally keeps detailed ID-based configuration. Common ID types include:

- Weapon Definition Index / `DefIndex`.
- Paint Kit ID for weapon or glove finishes.
- Agent Definition Index.
- Music Kit ID.
- Sticker ID.

Use IDs that match the current CS2 version. Invalid IDs may be ignored or may cause the corresponding cosmetic to fail to display correctly.

### Numeric ranges

- Keep `MaxWear` between `0.0` and `1.0`. Runtime wear generation clamps the value to this range.
- `MinSeed` must be less than or equal to `MaxSeed`.
- An empty candidate array normally means there is nothing available to randomize for that section.

---

## 3. Weapons

Example structure:

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

Enables randomized weapon skins for BOT inventories.

- `true`: configured weapons receive a random Paint Kit from `PaintKits`.
- `false`: this weapon-skin generation path is disabled.

### `Weapons.MaxWear`

Maximum random wear for weapons.

The current implementation generates wear between `0` and `MaxWear`, with `MaxWear` clamped to `0.0`-`1.0`.

### `Weapons.MinSeed` / `Weapons.MaxSeed`

Inclusive Pattern Seed randomization range.

Keep:

`MinSeed <= MaxSeed`

An invalid reversed range can cause an error while generating a random inventory.

### `Weapons.PaintKits`

Maps a weapon DefIndex to the Paint Kit IDs that may be selected for that weapon.

The key is the weapon DefIndex represented as a JSON string:

```json
"7": [302, 600, 675]
```

This means weapon DefIndex `7` randomly selects Paint Kit `302`, `600`, or `675`.

Notes:

- Keys that cannot be parsed as a valid `ushort` are ignored.
- An empty Paint Kit array means that weapon does not receive a generated random skin.
- This section controls which skins can actually be selected. `StatTrak.WeaponPaints` only controls StatTrak eligibility and does not add skins to this pool.

### Weapon DefIndex reference

These are the configurable firearm DefIndexes covered by the default `Weapons.PaintKits` configuration. JSON keys are strings; for example, M4A1-S uses `"60"`.

| DefIndex | Weapon |
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

This table intentionally lists the regular firearms used by BotInventory's weapon-skin configuration. It does not include grenades, C4, Zeus, default knives, or other items that are not configured through `Weapons.PaintKits`.

---

## 4. Knives

Example structure:

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

Enables randomized knives.

### `Knives.MaxWear`

Maximum randomized knife wear. Runtime wear generation clamps the value to `0.0`-`1.0`.

### `Knives.Items`

Candidate knife definitions.

#### `Knives.Items[].Def`

The knife Definition Index. Use a valid knife DefIndex.

#### `Knives.Items[].Enabled`

Controls whether this knife definition is allowed in the random pool.

#### `Knives.Items[].PaintKits`

Paint Kit IDs that may be selected for this knife definition.

An item with an empty Paint Kit array is not selected.

CT and T are randomized separately, so the two teams can receive different knife definitions or finishes.

### Knife DefIndex reference and extending the pool

`Knives.Items` is extensible and is not limited to the knife entries present in the default configuration. Add another object with a valid knife `Def`, at least one valid `PaintKits` entry, and `Enabled: true` to include another knife type in randomization.

| DefIndex | Knife |
| ---: | --- |
| 500 | Bayonet |
| 503 | Classic Knife |
| 505 | Flip Knife |
| 506 | Gut Knife |
| 507 | Karambit |
| 508 | M9 Bayonet |
| 509 | Huntsman Knife |
| 512 | Falchion Knife |
| 514 | Bowie Knife |
| 515 | Butterfly Knife |
| 516 | Shadow Daggers |
| 517 | Paracord Knife |
| 518 | Survival Knife |
| 519 | Ursus Knife |
| 520 | Navaja Knife |
| 521 | Nomad Knife |
| 522 | Stiletto Knife |
| 523 | Talon Knife |
| 525 | Skeleton Knife |
| 526 | Kukri Knife |

> DefIndexes `42` and `59` are the default team knives and are not recommended as special cosmetic knife candidates here. The table above lists the special inventory knife types normally used for knife skins.

For example, to add the Skeleton Knife, append this object to `Knives.Items`:

```json
{
  "Def": 525,
  "Enabled": true,
  "PaintKits": [0, 413, 44, 415, 416]
}
```

Full example:

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

No C# source change is required after adding an entry. BotInventory builds the knife randomization pool from enabled JSON entries. `PaintKits` should still contain finish IDs that are valid for that knife type.

---

## 5. Gloves

Example structure:

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

Enables randomized BOT gloves.

### `Gloves.MaxWear`

Maximum randomized glove wear. Keep it between `0.0` and `1.0`.

### `Gloves.MinSeed` / `Gloves.MaxSeed`

Pattern Seed randomization range.

Keep `MinSeed <= MaxSeed`.

### `Gloves.Items`

Candidate glove entries.

#### `Gloves.Items[].Def`

Glove Definition Index.

#### `Gloves.Items[].Paint`

Glove Paint Kit ID.


#### `Gloves.Items[].Enabled`

Controls whether the glove entry can be selected.

Only entries with `Enabled = true`, `Def > 0`, and `Paint >= 0` enter the candidate pool.

---

## 6. MusicKits

Example structure:

```json
"MusicKits": {
  "Enabled": true,
  "Ids": [3, 4, 5, 6]
}
```

### `MusicKits.Enabled`

Enables randomized BOT Music Kits.

### `MusicKits.Ids`

Candidate Music Kit IDs.

An empty array effectively disables Music Kit selection.

---

## 7. Agents

Example structure:

```json
"Agents": {
  "Enabled": true,
  "Chance": 1.0,
  "CtDefIndexes": [5308, 5404, 5602],
  "TDefIndexes": [4726, 5504, 5208]
}
```

### `Agents.Enabled`

This is now the only switch that controls randomized BOT Agents.

- `true`: BotInventory uses `Agents.Chance` and the team-specific Agent DefIndex pools, then applies the current Agent model/voice synchronization path.
- `false`: BotInventory does not assign a randomized special Agent and leaves the BOT on the game's default team/map character behavior.

The old raw `Models` `.vmdl` randomization section has been removed to avoid maintaining two separate concepts for randomized BOT Agents.

### `Agents.Chance`

Probability that a BOT receives a special Agent from the configured pools.

- `1.0`: always attempt to use a configured special Agent.
- `0.5`: approximately 50% chance.
- `0.0`: use the default team/map character definitions.

The value is clamped to `0.0`-`1.0`.

### `Agents.CtDefIndexes`

Candidate CT Agent Definition Index values.

### `Agents.TDefIndexes`

Candidate T Agent Definition Index values.

BotInventory filters:

- Non-positive IDs.
- IDs outside the `ushort` range.
- IDs that AgentCatalog does not recognize as belonging to the requested team.
- Duplicate IDs.

The plugin tries to avoid assigning the same special Agent to multiple connected BOTs on the same team until the candidate pool is exhausted. Duplicates can still occur when there are not enough unique candidates.

> Dedicated Agent voices are still subject to CS2 BOT Response/Voice Bank behavior. A valid Agent ID does not guarantee that every edge-case BOT voice event exists in that Agent's voice bank.

CS2 Paid Agent DefIndex List
Fields: DefIndex | Team | English Name
Total: 63 Agents (29 CT, 34 T)

ALL Agents ID List:

CT Agents (29)
4619 | CT | 'Blueberries' Buckshot | NSWC SEAL
4680 | CT | 'Two Times' McCoy | TACP Cavalry
4711 | CT | Cmdr. Mae 'Dead Cold' Jamison | SWAT
4712 | CT | 1st Lieutenant Farlow | SWAT
4713 | CT | John 'Van Healen' Kask | SWAT
4714 | CT | Bio-Haz Specialist | SWAT
4715 | CT | Sergeant Bombson | SWAT
4716 | CT | Chem-Haz Specialist | SWAT
4749 | CT | Sous-Lieutenant Medic | Gendarmerie Nationale
4750 | CT | Chem-Haz Capitaine | Gendarmerie Nationale
4751 | CT | Chef d'Escadron Rouchard | Gendarmerie Nationale
4752 | CT | Aspirant | Gendarmerie Nationale
4753 | CT | Officer Jacques Beltram | Gendarmerie Nationale
4756 | CT | Lieutenant 'Tree Hugger' Farlow | SWAT
4757 | CT | Cmdr. Davida 'Goggles' Fernandez | SEAL Frogman
4771 | CT | Cmdr. Frank 'Wet Sox' Baroud | SEAL Frogman
4772 | CT | Lieutenant Rex Krikey | SEAL Frogman
5305 | CT | Operator | FBI SWAT
5306 | CT | Markus Delrow | FBI HRT
5307 | CT | Michael Syfers | FBI Sniper
5308 | CT | Special Agent Ava | FBI
5400 | CT | 3rd Commando Company | KSK
5401 | CT | Seal Team 6 Soldier | NSWC SEAL
5402 | CT | Buckshot | NSWC SEAL
5403 | CT | 'Two Times' McCoy | USAF TACP
5404 | CT | Lt. Commander Ricksaw | NSWC SEAL
5405 | CT | Primeiro Tenente | Brazilian 1st Battalion
5601 | CT | B Squadron Officer | SAS
5602 | CT | D Squadron Officer | NZSAS

T Agents (34)
4613 | T | Bloody Darryl The Strapped | The Professionals
4718 | T | Rezan the Redshirt | Sabre
4726 | T | Sir Bloody Miami Darryl | The Professionals
4727 | T | Safecracker Voltzmann | The Professionals
4728 | T | Little Kev | The Professionals
4730 | T | Getaway Sally | The Professionals
4732 | T | Number K | The Professionals
4733 | T | Sir Bloody Silent Darryl | The Professionals
4734 | T | Sir Bloody Skullhead Darryl | The Professionals
4735 | T | Sir Bloody Darryl Royale | The Professionals
4736 | T | Sir Bloody Loudmouth Darryl | The Professionals
4773 | T | Elite Trapper Solman | Guerrilla Warfare
4774 | T | Crasswater The Forgotten | Guerrilla Warfare
4775 | T | Arno The Overgrown | Guerrilla Warfare
4776 | T | Col. Mangos Dabisi | Guerrilla Warfare
4777 | T | Vypa Sista of the Revolution | Guerrilla Warfare
4778 | T | Trapper Aggressor | Guerrilla Warfare
4780 | T | 'Medium Rare' Crasswater | Guerrilla Warfare
4781 | T | Trapper | Guerrilla Warfare
5105 | T | Ground Rebel | Elite Crew
5106 | T | Osiris | Elite Crew
5107 | T | Prof. Shahmat | Elite Crew
5108 | T | The Elite Mr. Muhlik | Elite Crew
5109 | T | Jungle Rebel | Elite Crew
5205 | T | Soldier | Phoenix
5206 | T | Enforcer | Phoenix
5207 | T | Slingshot | Phoenix
5208 | T | Street Soldier | Phoenix
5500 | T | Dragomir | Sabre
5501 | T | Maximus | Sabre
5502 | T | Rezan The Ready | Sabre
5503 | T | Blackwolf | Sabre
5504 | T | 'The Doctor' Romanov | Sabre
5505 | T | Dragomir | Sabre Footsoldier

---

## 8. Stickers

Example structure:

```json
"Stickers": {
  "Enabled": true,
  "Kato14Rate": 0.6
}
```

### `Stickers.Enabled`

Controls generated stickers on randomized weapon skins.

- `true`: enables the current random sticker generation logic.
- `false`: BotInventory does not attach stickers through this randomization path.

### `Stickers.Kato14Rate`

Probability that each randomized Sticker ID is selected from the built-in first Sticker ID range.

Recommended range: `0.0` to `1.0`.

- `0.0`: always use the built-in normal Sticker ID range.
- `0.6`: roughly 60% first-range selections and 40% normal-range selections.
- `1.0`: always use the built-in first Sticker ID range.

The numeric Sticker ID ranges are now fixed inside the plugin source instead of being exposed in JSON. Their current internal values preserve the previous behavior while reducing confusing configuration fields.

### Current sticker-slot behavior

The current implementation:

- Uses one identical random Sticker ID for slots 0, 1, and 2.
- Gives slot 3 an independent 50% chance to receive a random Sticker ID.
- Gives slot 4 an independent 50% chance to receive a random Sticker ID.
- Uses wear `0.0` and rotation `0`.

These per-slot probabilities are not currently JSON-configurable.

---

## 9. StatTrak

Example structure:

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

Allows generated StatTrak weapons.

### `StatTrak.Chance`

For a weapon + Paint Kit combination that is eligible for StatTrak, this is the probability that StatTrak is actually generated.

The value is clamped to `0.0`-`1.0`.

### `StatTrak.CounterMin` / `StatTrak.CounterMax`

Inclusive random range for the generated StatTrak counter value.

The implementation safely normalizes the order, so the lower of the two values becomes the minimum.

### `StatTrak.WeaponPaints`

StatTrak eligibility whitelist.

Keys are weapon DefIndex values and arrays contain Paint Kit IDs that may receive StatTrak.

Example:

```json
"7": [302, 600]
```

Only when weapon DefIndex `7` has already selected Paint Kit `302` or `600` does BotInventory continue to the `StatTrak.Chance` roll.

Important:

- `WeaponPaints` does not add new skins to `Weapons.PaintKits`.
- A Paint Kit must first be selected from `Weapons.PaintKits`, then match this whitelist.
- Weapons that do not match the StatTrak whitelist cannot receive StatTrak, but they can still participate in the Souvenir roll.

---

## 10. Souvenir

Example structure:

```json
"Souvenir": {
  "Enabled": true,
  "Chance": 0.6
}
```

### `Souvenir.Enabled`

Allows generated Souvenir quality.

### `Souvenir.Chance`

Probability of generating Souvenir quality when the current weapon did not successfully receive StatTrak.

The value is clamped to `0.0`-`1.0`.

StatTrak and Souvenir are mutually exclusive in the current implementation:

1. If the selected weapon + Paint Kit is listed in `StatTrak.WeaponPaints`, BotInventory tries the StatTrak roll first.
2. If no StatTrak value is generated, the item proceeds to the Souvenir roll.
3. The same generated item will not be both StatTrak and Souvenir.

---

## 11. Recommended editing workflow

1. Stop the server or unload BotInventory.
2. Back up `BotInventory.json`.
3. Edit the JSON file.
4. Check brackets, commas, arrays, ranges, and IDs.
5. Reload the plugin or start the server.

---
