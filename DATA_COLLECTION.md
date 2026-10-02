# FoG_Walkways — Data Collection

How the project records participant movement, where the files go, what the columns
mean, and how to verify a session was captured correctly.

Companion document: [WALKWAY_SCENARIOS_DOCUMENTATION.md](WALKWAY_SCENARIOS_DOCUMENTATION.md)
(scene design and clinical rationale).

---

## 1. Overview

Three independent loggers run during a walkway scenario. They do not share state —
each writes its own file.

| Logger | Script | Output | Rate | Content |
|---|---|---|---|---|
| `SessionDataManager` | `Assets/Scripts/DataCollection/SessionDataManager.cs` | `FoG_Data/FoG_<patient>_<scenario>_<time>.csv` | 10 Hz | Head-rig position, rotation, speed |
| `BodyKinematicsLogger` | `Assets/Scripts/Core/BodyKinematicsLogger.cs` | `_LocalLogs/BodyTracking_<time>.csv` | 15 Hz | Acceleration of Head / Hips / LeftFoot / RightFoot |
| `CSVLogger` (via `WalkTimer`) | `Assets/Scripts/Core/CSVLogger.cs` | `walkway_results.csv` | per trial | One row: how long the walk took |

`SessionDataManager` also appends to `FoG_Data/SessionDebugLog.txt`, a plain-text
trace of session start/end and save operations. It is the first thing to read when
a file is missing or empty.

**One scenario run produces one `BodyTracking_*.csv` and one `FoG_*.csv`.**
If you ran two scenarios and see three files, something is wrong — see §7.

---

## 1.1 The two primary datasets, side by side

`walkway_results.csv` is a single summary row per trial. The two datasets that
matter for analysis are the pose log and the kinematics log. They are collected by
independent components, land in different folders, and are **not** interchangeable.

| | **Pose log** | **Kinematics log** |
|---|---|---|
| **Component** | `SessionDataManager` | `BodyKinematicsLogger` |
| **Script** | `Assets/Scripts/DataCollection/SessionDataManager.cs` | `Assets/Scripts/Core/BodyKinematicsLogger.cs` |
| **Scene object** | `SessionDataManager` / `SessionManager` | `BodyMovementLogging` |
| **Output folder** | `<persistentDataPath>/FoG_Data/` | `<persistentDataPath>/_LocalLogs/` |
| **Filename** | `FoG_<patientID>_<scenario>_<time>.csv` | `BodyTracking_<time>.csv` |
| **Filename clock** | **Local time** | **UTC**, with milliseconds |
| **Sample rate** | 10 Hz (`RECORD_INTERVAL = 0.1s`) | 15 Hz (`sampleRateHz`, drives `Time.fixedDeltaTime`) |
| **Update loop** | `Update()` | `FixedUpdate()` |
| **Tracked transforms** | 1 — `[BuildingBlock] Camera Rig` | 4 — Head, Hips, LeftFoot, RightFoot |
| **Quantities** | Position, rotation, speed, rotation speed | Linear acceleration, 3 axes per part |
| **Units** | m, deg, m/s, deg/s | m/s² |
| **Derivation order** | 1st (position → speed) | 2nd (position → velocity → acceleration) |
| **Data columns** | 9 | 16 (4 meta + 12 acceleration) |
| **File layout** | Sectioned — parse from `=== Frame Data ===` | Flat CSV, one header row |
| **Write strategy** | Buffered in memory, written once at session end | Streamed to disk, `AutoFlush` on |
| **If the app crashes** | Session data is **lost** | Everything up to the crash survives |
| **Starts on** | Walkway scene loads | `Start()` of the scene object |
| **Ends on** | Return to a non-walkway scene | `OnDestroy()` / app quit |
| **Lifetime** | `DontDestroyOnLoad` singleton, spans scenes | Scene-scoped, one per scene |
| **Also writes** | `FoG_Data/SessionDebugLog.txt` | — |
| **Use it for** | Path, distance, walking speed, turning, session events | Gait dynamics, step detection, tremor / freezing signatures |

### Pairing the two files from one run

The filenames use **different clocks**, so they will not look like siblings:

```
FoG_P_20260801_210218_Walkways_20260801_210250.csv   ← local  21:02:18
BodyTracking_20260802_010218_703.csv                 ← UTC    01:02:18 (next day)
```

Both came from the same run. The minutes and seconds match; the hour and date differ
by the UTC offset (−4 h for US Eastern daylight time in this example).

To align the two time series, use the wall-clock columns rather than the relative
ones — `iso_utc` in the kinematics log and `Start Time` in the pose log (converting
local → UTC). The relative `t_since_start_s` and `Timestamp` columns have **different
origins**: the kinematics logger starts at `Start()`, the pose session starts when
`OnSceneLoaded` fires, so they are offset by a fraction of a second.

---

## 2. Where the files are

### On Quest 3 (build)

```
Quest 3\Internal shared storage\Android\data\com.lehighvrhealth.fogwalkways\files\
├── FoG_Data\
│   ├── FoG_P_20260802_020018_Walkways_20260802_020149.csv
│   └── SessionDebugLog.txt
├── _LocalLogs\
│   └── BodyTracking_20260802_020018_129.csv
└── walkway_results.csv
```

The package id is `com.lehighvrhealth.fogwalkways`. Do not confuse it with
`com.lehigh.parkinsonsvr.revised2`, which belongs to the separate FoG_Revised2 project.

> Uninstalling the app deletes this entire folder. Copy data off the headset before
> reinstalling.

### In the Unity Editor (Play mode)

Editor Play mode writes to the **PC**, never to the headset:

```
C:\Users\<user>\AppData\LocalLow\DefaultCompany\FoG_Walkways\
```

Same subfolder layout. This is the fastest way to verify a code change without
building and deploying.

---

## 3. `FoG_Data/FoG_*.csv` — position, rotation, speed

### Lifecycle

- A patient ID is generated automatically on `Awake()`: `P_yyyyMMdd_HHmmss`.
  No UI entry is required.
- A session **starts** automatically when a scene whose name contains `Walkway`
  loads, and **ends** when the app returns to a non-walkway scene (i.e. MainMenu).
- The file is written on session end. Sessions with zero recorded frames are
  skipped and logged as a warning instead of producing an empty file.

### Structure

The file is sectioned, not a flat table. A parser must skip to the
`=== Frame Data ===` marker.

```
=== Session Info ===
Patient ID,P_20260802_020018
Scenario,Walkways
Session Duration,18.57
Start Time,2026-08-02 02:01:49

=== Calibration Data ===
Yaw Angle (Y),0.00
Position Offset,(0.00, 0.00, 0.00)

=== Events ===
Timestamp,Event Type,Description
0.00,SESSION_START,"Scenario: Walkways"
18.57,SESSION_END,"Duration: 18.57s"

=== Frame Data ===
Timestamp,PosX,PosY,PosZ,RotX,RotY,RotZ,Speed,RotSpeed
0.10,-7.916,12.160,11.802,0.0,270.0,0.0,187.034,2700.000
0.22,-7.916,12.160,11.802,0.0,270.0,0.0,0.000,0.000
```

### Frame Data columns

| Column | Unit | Meaning |
|---|---|---|
| `Timestamp` | s | Seconds since session start |
| `PosX/Y/Z` | m | World position of the camera rig |
| `RotX/Y/Z` | deg | Euler rotation, 0–360 |
| `Speed` | m/s | Distance moved since previous sample ÷ 0.1 s |
| `RotSpeed` | deg/s | Euler distance since previous sample ÷ 0.1 s |

The tracked transform is `[BuildingBlock] Camera Rig`, falling back to
`Camera.main`'s parent. This follows the participant, so it is the reference frame
for walking distance and path.

### Known limitations

- **`RotSpeed` wraps incorrectly.** It is computed as a Euclidean distance between
  Euler triples, so a heading crossing 0°/360° produces a spurious spike. The first
  row of every file shows `RotSpeed 2700.000` for this reason. Recompute from
  `RotY` with proper angle wrapping if you need turn rate.
- **`Speed` on the first row is meaningless** — the previous position starts at the
  origin. Discard row 1.
- `Calibration Data` is only populated if `RecordCalibration()` is called; it is
  otherwise all zeros.

---

## 4. `_LocalLogs/BodyTracking_*.csv` — acceleration

### Columns

```
frame_index,t_since_start_s,delta_time_s,iso_utc,
Head_ax,Head_ay,Head_az,
Hips_ax,Hips_ay,Hips_az,
LeftFoot_ax,LeftFoot_ay,LeftFoot_az,
RightFoot_ax,RightFoot_ay,RightFoot_az
```

| Column | Unit | Meaning |
|---|---|---|
| `frame_index` | — | Sequential, starts at 0 |
| `t_since_start_s` | s | Wall-clock seconds since logger start |
| `delta_time_s` | s | Fixed timestep, `1 / sampleRateHz` (0.066667 at 15 Hz) |
| `iso_utc` | — | ISO-8601 UTC timestamp, used to align with the pose file |
| `<Part>_ax/ay/az` | m/s² | World-space linear acceleration |

`sampleRateHz` is set on the component (default 15) and drives
`Time.fixedDeltaTime`. Sampling happens in `FixedUpdate`.

### How acceleration is derived

Positions are differentiated twice:

```
v(t) = (p(t) - p(t-1)) / dt
a(t) = (v(t) - v(t-1)) / dt
```

This is **not** an IMU reading. Double-differencing pose amplifies tracking noise,
so treat the raw signal accordingly:

- Typical magnitude during normal walking: **1–6 m/s²**.
- Spikes of **100–400 m/s²** are tracking jumps, not real motion. Low-pass filter
  or clip before analysis.
- Rows 0 and 1 always contain a large **equal-and-opposite pair** — a startup
  artifact from `prevVel` beginning at zero. Discard the first three rows.

### Tracking dropouts

When all twelve acceleration values are **exactly** `0.000000` across a run of rows,
tracking was frozen — the headset was removed, the proximity sensor triggered, or
the app was backgrounded. Real standing-still always jitters and never yields exact
zeros.

A healthy session sits near **0–1 %** all-zero rows. Above ~15 % the recording has
gaps and should be re-run. See §6.

---

## 5. Required scene setup

Every walkway scene needs **exactly one** of each:

| GameObject | Component | Notes |
|---|---|---|
| `SessionDataManager` | `SessionDataManager` | Singleton, survives scene loads |
| `BodyMovementLogging` | `BodyKinematicsLogger` | Root-level object; assign the four transforms in the Inspector |

Current state:

| Scene | SessionDataManager | BodyKinematicsLogger |
|---|---|---|
| `MainMenu.unity` | — | — |
| `ClutteredWalkway.unity` | ✅ 1 | ✅ 1 |
| `NarrowedWalkway.unity` | ✅ 1 | ✅ 1 |

### Inspector fields on `BodyKinematicsLogger`

Assign `Head`, `Hips`, `Left Foot`, `Right Foot` to the tracked avatar transforms.
Any field left empty is auto-detected by name at `Start()`:

| Field | Names tried |
|---|---|
| `head` | `Head`, `head`, `[BuildingBlock] Camera Rig`, then `Camera.main` |
| `hips` | `Hips`, `hips` |
| `leftFoot` | `LeftFoot`, `leftFoot`, `Left_Foot`, `Left Foot` |
| `rightFoot` | `RightFoot`, `rightFoot`, `Right_Foot`, `Right Foot` |

> **Do not attach `BodyKinematicsLogger` to `StartPoint_head`.** That object is a
> static spawn marker; binding `head` to it yields all-zero head acceleration for
> the whole session. See §7.

---

## 6. Verifying a session

Run the checker after every collection session:

```powershell
# Editor Play mode results (default)
.\Tools\Check-BodyTracking.ps1

# Data copied off the headset
.\Tools\Check-BodyTracking.ps1 -Folder "D:\path\to\copied\_LocalLogs"
```

Sample output:

```
BodyTracking_20260802_020149_732.csv
  filename _fff  : OK  (new build)
  header         : OK
  duration       : 16.4s  (218 rows)
  tracking lost  : 3.2% (7 of 218 rows all-zero)
  body parts     : all 4 have data
```

### Acceptance criteria

| Check | Pass | Meaning if it fails |
|---|---|---|
| `filename _fff` | `OK` | Build predates the millisecond-filename fix; rebuild |
| `header` | `OK` | Two loggers wrote the same file (§7) |
| `body parts` | `all 4 have data` | A transform is unassigned or bound to a static object |
| `tracking lost` | under ~1 % | Headset removed or app backgrounded mid-run |
| file count | one per scenario run | Compare by timestamp — the folder accumulates old files |

`body parts : all 4 have data` only requires a single non-zero value. For a stricter
check, confirm each part's mean magnitude is in the 1–6 m/s² range rather than
near zero.

---

## 7. Troubleshooting

### CSV has no header row and starts with padding

The first ~175 bytes are `NUL` (0x00), not spaces. Two `BodyKinematicsLogger`
instances opened the same file: the filename used second resolution, so loggers
starting in the same second collided, and one truncated the file while the other
kept writing at its old offset.

Fixed by: millisecond-resolution filenames (`yyyyMMdd_HHmmss_fff`) and a static
duplicate guard that disables any second logger and logs a warning.

**If it recurs**, a scene has two loggers. Find them by GUID:

```powershell
$guid = (Select-String Assets\Scripts\Core\BodyKinematicsLogger.cs.meta -Pattern 'guid: (\w+)').Matches[0].Groups[1].Value
Get-ChildItem Assets\Scenes -Filter *.unity | ForEach-Object {
    "$($_.Name): $((Select-String $_.FullName -Pattern $guid -AllMatches).Matches.Count)"
}
```

### One body part is all zeros for a whole session

The transform is bound to something that does not move. The most common cause is a
stray logger on `StartPoint_head` whose `head` field points at itself while
`hips`/`leftFoot`/`rightFoot` are empty — auto-detection then fills the three feet
fields with real moving objects, so only `Head` reads zero, which makes the fault
easy to miss.

Check which transforms were bound at runtime in the Editor Console or `Editor.log`:

```
[BodyKinematicsLogger] Found - Head: ..., Hips: ..., LeftFoot: ..., RightFoot: ...
```

### Empty `FoG_*.csv` with zero frames

`SessionDataManager` subscribes to `SceneManager.sceneLoaded`. If the object is
destroyed while still subscribed, the delegate keeps the C# object alive: `Update()`
stops (no frames recorded) but `OnSceneLoaded` keeps firing, so every later scene
change writes an empty CSV. Unity also reports the destroyed instance as `== null`,
letting a second manager claim `Instance` and mint a second patient ID for one run.

Fixed by unsubscribing and clearing `Instance` in `OnDestroy()`, plus skipping saves
with zero frames.

### Nothing appears on the headset after pressing Stop

Editor Play mode writes to `AppData\LocalLow\...` on the PC. The headset folder only
updates when a **build** runs on the device.

### `SessionDebugLog.txt` stops mid-session

`InitializeDebugLog()` opens the file exclusively. A second manager instance fails
with an `IOException`, leaves `debugLogWriter` null, and silently logs only to the
Unity Console from then on. Missing log lines are therefore a symptom of a duplicate
manager, not of a crash.

---

## 8. `WalkTimer` vs `SessionDataManager`

These overlap in name only and both should stay enabled.

| | `WalkTimer` | `SessionDataManager` |
|---|---|---|
| Purpose | How long the walk took | How the participant moved |
| Starts | Player enters `StartGate` trigger | Walkway scene loads |
| Ends | Player enters `EndGate` trigger | Return to MainMenu |
| Records | Elapsed time only | Position, rotation, speed at 10 Hz |
| Output | `walkway_results.csv` (one row) | `FoG_Data/FoG_*.csv` |

`StartGate` and `EndGate` are both `WalkTimerGate` components; the `gateType` enum
(`Start` / `End`) decides whether entering calls `StartTimer()` or `StopTimer()`.

---

**Last verified:** 2026-08-02 against ClutteredWalkway and NarrowedWalkway,
Unity 6000.4.6f1, Quest 3.
