# FoG (Freeze of Gait) Project Scenarios Documentation

## 📋 Overview

This document describes the operation methods and structure of 6 total scenarios in the FoG_Revised and FoG_Walkways projects.

---

## 🎮 FoG_Revised Scenarios (6 Total)

### 1. **ScenarioMenu.unity** - Menu Scenario
**Location**: `Assets/Scenes/ScenarioMenu.unity`

**Purpose**: Entry point for the VR application, serving as the menu to select different scenarios

**Structure**:
- Menu UI panel (one or more buttons)
- Each button connects to the corresponding scenario scene

**How It Works**:
```
[ScenarioMenu] → User selects button → Corresponding scenario loads
```

**Key Components**:
- Canvas (UI rendering)
- Scenario selection buttons

---

### 2. **BasicScene.unity** - Basic Scenario
**Location**: `Assets/Scenes/BasicScene.unity`

**Purpose**: A simple scenario to test the basic VR environment setup

**Structure**:
- Brick Project Studio apartment kit base environment
- First Person Controller (player perspective)
- Basic lighting and environmental settings

**How It Works**:
```
Start → Player spawns → Free movement and exploration
```

**Key Features**:
- First-person movement using mouse/VR controller
- Environment exploration and interaction

**Return to Menu**:
- Click VR controller Y button (left) → Return to ScenarioMenu (ReturnToMenu.cs)

---

### 3. **Freeze_of_Gait.unity** - Main Freeze of Gait Scenario
**Location**: `Assets/Scenes/Freeze_of_Gait.unity`

**Purpose**: Primary scenario to simulate Freeze of Gait symptoms and conduct rehabilitation training

**Structure**:
- Actual walking path setup
- Obstacles and trigger points
- Environmental feedback system
- Player movement tracking

**How It Works**:
```
Start → Set player spawn position → 
Follow walking path → Detect triggers → 
Activate control mechanism (voice signal, visual signal, etc.) →
Continue walking
```

**Key Environment**:
- Indoor apartment setting
- Marked walking paths
- Obstacles of varying difficulty

**Measurement Items**:
- Walking speed
- Movement time
- Number of pauses/stops
- Path deviation distance

**Return to Menu**:
- Click VR controller Y button

---

### 4. **Freeze_of_Gait_6m_3m.unity** - 6m/3m Distance Variant
**Location**: `Assets/Scenes/Freeze_of_Gait_6m_3m.unity`

**Purpose**: Combine shorter (3m) and longer (6m) distances to test diverse walking patterns

**Structure**:
- Two-section walking path:
  - **3m Section**: Short-distance walking test
  - **6m Section**: Medium-distance walking test
- Distance markers and clear start/end points

**How It Works**:
```
3m Section Start → Walk 3 meters → 
6m Section Start → Walk 6 meters → Complete
```

**Unique Features**:
- Subdivides distance testing compared to basic Freeze_of_Gait scenario
- Allows comparison of performance across different walking distances

**Data Collection**:
- Movement time per distance
- Performance characteristics by distance

---

### 5. **Freeze_of_Gait_Doorway.unity** - Doorway Scenario
**Location**: `Assets/Scenes/Freeze_of_Gait_Doorway.unity`

**Purpose**: Simulate Freeze of Gait symptoms that occur when passing through doorways

**Structure**:
- Doorway visual elements (door frame)
- Doorway passage detection trigger
- Walking paths before and after passage

**How It Works**:
```
Initial position → Approach doorway → 
[Doorway passage detected] → Activate control signal →
Pass through doorway → Continue walking
```

**Special Features**:
- Provide specific stimuli when doorway is detected (voice, light, vibration, etc.)
- Measure doorway passage time
- Record passage success/failure

**Clinical Significance**:
- Freeze of Gait symptoms are more prominent when Parkinson's patients pass through doorways
- Specialized for training to overcome this challenge

---

### 6. **Freeze_of_Gait_Closed_Door.unity** - Closed Door Scenario
**Location**: `Assets/Scenes/Freeze_of_Gait_Closed_Door.unity`

**Purpose**: Simulate Freeze of Gait in closed door situations and test coping ability

**Structure**:
- Closed passage (closed door)
- Door open/close trigger system
- Waiting area and walking path

**How It Works**:
```
Start → Wait or approach door →
[Door opens signal or auto-opens] →
Walk through door → Continue
```

**Advanced Features**:
- Conditional door opening (time-based, signal-based, automatic)
- Provide control signal when passing through door
- Measure Freeze of Gait severity based on door opening delay

**Clinical Application**:
- Recreate real-life scenarios with closed doors
- Assess emergency situation response ability

---

## 🎮 FoG_Walkways Scenarios (3 Main)

### 7. **MainMenu.unity** - Menu
**Location**: `Assets/Scenes/MainMenu.unity`

**Purpose**: Menu for the walking simulation application

**Structure**:
- Main menu UI
- Walking scenario selection buttons

---

### 8. **ClutteredWalkway.unity** - Complex Walkway
**Location**: `Assets/Scenes/ClutteredWalkway.unity`

**Purpose**: Assess walking ability in complex environments with many obstacles

**Structure**:
- Multiple obstacles placed
- Walking path setup
- High-complexity environment

**How It Works**:
```
Start → Avoid obstacles while walking → Complete path
```

**Features**:
- Recreate actual daily walking environment
- Measure obstacle avoidance ability

---

### 9. **NarrowedWalkway.unity** - Narrow Walkway
**Location**: `Assets/Scenes/NarrowedWalkway.unity`

**Purpose**: Assess walking stability and Freeze of Gait symptoms in narrow spaces

**Structure**:
- Narrow walking path
- Space constraint simulation
- Walking stability tracking

**How It Works**:
```
Start → Enter narrow walkway → Maintain balance while walking → Complete path
```

**Features**:
- Analyze walking patterns under spatial constraints
- Assess instability and Freeze of Gait symptom manifestation

---

## 🔧 Common Systems

### Menu Return Mechanism
**Script**: `Assets/Scripts/ReturnToMenu.cs`

```csharp
public class ReturnToMenu : MonoBehaviour
{
    void Update()
    {
        // VR controller Y button (left controller)
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            Debug.Log("[Scene] Returning to menu...");
            SceneManager.LoadScene("ScenarioMenu");
        }
    }
}
```

**How It Works**:
- ReturnToMenu script is included in all scenario scenes
- Pressing the Y button on VR controller returns to ScenarioMenu
- Current scenario state saving depends on application settings

---

## 📊 Scenario Flow Diagram

```
┌─────────────────┐
│  ScenarioMenu   │ (FoG_Revised)
└────────┬────────┘
         │
    ┌────┴────────────────────────┬─────────────┬─────────────┬──────────────┐
    │                            │             │             │              │
    ▼                            ▼             ▼             ▼              ▼
┌──────────────┐    ┌─────────────────┐ ┌──────────────┐ ┌──────────────┐ ┌──────────────┐
│BasicScene    │    │Freeze_of_Gait   │ │6m/3m Variant │ │Doorway Variant│ │Closed Door   │
└──────────────┘    └─────────────────┘ └──────────────┘ └──────────────┘ └──────────────┘
      ↓                    ↓                   ↓              ↓               ↓
      └────────────────────┴───────────────────┴──────────────┴───────────────┘
                                   ↓
                        ┌─────────────────────┐
                        │   Press Y button    │
                        │  (Return to menu)   │
                        └──────────┬──────────┘
                                   ↓
                        ┌─────────────────────┐
                        │  ScenarioMenu       │
                        └─────────────────────┘
```

---

## ⚙️ Technology Stack

- **Game Engine**: Unity (VR support)
- **VR Platform**: Meta Quest (OVR Input)
- **Asset Pack**: Brick Project Studio Apartment Kit
- **Scripting**: C#
- **Scene Management**: UnityEngine.SceneManagement

---

## 📝 Scenario Selection Guide

| Goal | Recommended Scenario | Reason |
|------|-------------|------|
| Basic VR environment test | BasicScene | Verify system with minimal features |
| Freeze of Gait assessment | Freeze_of_Gait | Complete walking path and measurements |
| Distance-dependent performance comparison | Freeze_of_Gait_6m_3m | Evaluate at various distances |
| Doorway passage ability | Freeze_of_Gait_Doorway | Recreate real-world situation |
| Emergency situation | Freeze_of_Gait_Closed_Door | Assess response to closed door situation |
| Complex environment | ClutteredWalkway | Evaluate obstacle avoidance ability |
| Narrow space | NarrowedWalkway | Assess space constraint response |

---

## 🚀 Getting Started

1. **Open Unity Project**
   ```
   C:\Users\user\workspaces\unity\FoG_Revised
   ```

2. **Load ScenarioMenu**
   - Open `Assets/Scenes/ScenarioMenu.unity` in Project window
   - Click Play button

3. **Select and Test Scenario**
   - Click desired scenario button
   - Run scenario

4. **Return to Menu**
   - Press VR controller Y button (or ESC key during testing)
   - Automatically switch to ScenarioMenu

---

## 📌 Important Notes

- **Menu Return**: Y button available in all scenarios to return to menu
- **VR Headset**: Meta Quest device connection required (or emulation)
- **Data Saving**: Verify automatic data saving after each scenario completion
- **Performance**: Complex scenarios require high-performance PC

---

**Created**: 2026-07-22  
**Last Updated**: 2026-07-22
