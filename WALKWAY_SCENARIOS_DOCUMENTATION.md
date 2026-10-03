# FoG_Walkways Scenario Documentation

## 📋 Overview

This document describes how the walking simulation scenarios in the FoG_Walkways project work and how they are structured.

The project focuses on assessing a user's mobility and adaptability across a range of walking environments.

---

## 🎮 Scenario List (3)

### 1. **MainMenu.unity** — Main Menu
**Location**: `Assets/Scenes/MainMenu.unity`  
**File size**: 67.3 KB

**Purpose**: The entry point of the FoG_Walkways application; presents a menu for choosing a walking scenario.

**Structure**:
- Canvas (UI rendering)
- Button layout (choose ClutteredWalkway or NarrowedWalkway)
- Background and title (optional)

**How it works**:
```
[Application starts]
         ↓
  [MainMenu loads]
         ↓
[User selects a scenario]
         ↓
[Selected scenario loads]
```

**UI components**:
- Scenario selection buttons
- Menu background
- Text labels (scenario name / description)

---

### 2. **ClutteredWalkway.unity** — Cluttered Walkway Scenario
**Location**: `Assets/Scenes/ClutteredWalkway.unity`  
**File size**: 301.8 KB  
**Last modified**: 2026-07-20

**Purpose**: Simulates a real walking environment crowded with obstacles, to assess the user's attention, avoidance ability, and stability.

**Environment**:
```
┌─────────────────────────────────────┐
│      ClutteredWalkway environment   │
│                                     │
│  ╱╲ obstacle 1                      │
│  ║║                                 │
│ ╱╲╱╲ obstacles 2-3                  │
│ ║║║║                                │
│ ╱╲╱╲╱╲ ... more obstacles           │
│                                     │
│ [walking path] ← the user follows   │
│              this, avoiding obstacles│
└─────────────────────────────────────┘
```

**Structure**:
- **Obstacle placement**: several 3D models (cones, boxes, cylinders, and so on)
- **Walking path**: clearly defined start and end points
- **Environment**: indoors (apartment or corridor)
- **Floor**: uses the Floor prefab (at `Assets/Scenes/Floor.prefab`)

**Flow**:
```
1. Scenario starts → player spawn position is set
2. Obstacle collision detection → avoidance required
3. Walking proceeds → each obstacle is worked around
4. Path is recorded → statistics collected
5. Goal reached → complete
```

**Measures**:
| Measure | Description |
|------|------|
| **Traversal time** | Time taken from start to finish |
| **Path efficiency** | Ratio of actual distance travelled to straight-line distance |
| **Avoidance count** | Number of obstacle avoidance attempts |
| **Collision detection** | Whether obstacles were touched (safety measure) |
| **Walking speed** | Average walking speed |
| **Stop count** | Number of stops during the walk |

**Difficulty**:
- **Low**: obstacles spaced far apart (for beginners)
- **Medium**: standard obstacle placement (general assessment)
- **High**: densely packed obstacles (advanced assessment)

**Clinical applications**:
- Assessing divided attention in a realistic walking environment
- Screening fall risk in older adults
- Measuring improvement after physical therapy

---

### 3. **NarrowedWalkway.unity** — Narrow Walkway Scenario
**Location**: `Assets/Scenes/NarrowedWalkway.unity`  
**File size**: 521.6 KB  
**Last modified**: 2026-07-19

**Purpose**: Assesses balance, body control, and gait stability in a spatially constrained environment.

**Environment**:
```
Narrow walkway layout:

Top view:
┌────┐
│ ▓▓ │  <- walls on both sides
│ ▓▓ │  <- space constraint
│ ▓▓ │
└────┘

Side view:
│                │
│     [user]     │  <- walking path (restricted space)
│                │

Width: roughly 0.5 m - 1.5 m
Length: roughly 10 m
```

**Structure**:
- **Walking path**: a narrow corridor
- **Boundaries**: walls on both sides, ceiling, floor (an enclosed space)
- **Lighting**: indoor lighting (can simulate an unsettling environment)
- **Material**: floor with friction (prevents slipping)

**Flow**:
```
1. Scenario starts → the user enters the narrow space
2. Balance is maintained → walls on both sides are sensed
3. The user moves forward → speed is kept low
4. Gait stability is monitored → real-time feedback
5. Exit reached → complete
```

**Measures**:
| Measure | Description |
|------|------|
| **Traversal time** | Total time taken to walk through |
| **Balance** | Degree of left-right deviation (lower is better) |
| **Wall contacts** | Number of contacts with the walls (stability measure) |
| **Walking speed** | Walking speed in the confined space |
| **Gait regularity** | Consistency of the walking pattern |
| **Postural stability** | Degree of sway and twisting |
| **Perceived anxiety** | User self-report (questionnaire based) |

**Difficulty**:
- **Low**: 1.5 m wide, bright lighting
- **Medium**: 1.0 m wide, standard lighting
- **High**: 0.5 m wide, dim lighting, uneven floor

**Clinical applications**:
- Assessing walking ability in confined spaces in Parkinson's patients
- Balance rehabilitation for patients with vestibular impairment
- Screening fall risk in older adults
- Assessing gait disorders in neurological conditions

---

## 🔗 Additional Environment Assets

### Floor.prefab
**Location**: `Assets/Scenes/Floor.prefab`

**Used for**: The walking surface in both ClutteredWalkway and NarrowedWalkway.

**Structure**:
- Basic floor mesh (with collider)
- Material: ordinary indoor flooring
- Size: scalable to fit the scenario

**Properties**:
```
- Mesh Filter: floor mesh
- Collider: Box Collider (walk detection)
- Material: material with friction
```

---

## 📊 Scenario Comparison

| Property | ClutteredWalkway | NarrowedWalkway |
|------|------------------|-----------------|
| **Primary measures** | Attention, avoidance, stability | Balance, body control, stability |
| **Environment** | Multiple obstacles, open space | Narrow space, enclosed environment |
| **Difficulty** | Medium to high (depends on obstacles) | Medium (depends on the space) |
| **Clinical use** | Distraction, fall risk assessment | Balance disorders, anxiety assessment |
| **Rehabilitation use** | Recovery of motor function | Stabilising the nervous system |
| **Assessment time** | 5-10 minutes | 5-15 minutes |
| **Safety** | Relatively safe (open) | Less safe (enclosed, confining) |

---

## 🎮 User Interface Flow

```
┌──────────────────────┐
│  Application starts  │
└──────────┬───────────┘
           │
           ▼
    ┌────────────────┐
    │ MainMenu loads │
    └────────┬───────┘
             │
        ┌────┴────────────────┐
        │                     │
        ▼                     ▼
 ┌───────────────┐   ┌──────────────────┐
 │ Cluttered     │   │ Narrowed         │
 │ Walkway       │   │ Walkway          │
 │ [selected]    │   │ [selected]       │
 └───────┬───────┘   └────────┬─────────┘
         │                    │
         ▼                    ▼
  [scenario runs]      [scenario runs]
       │                    │
       │   [assessment]     │
       │                    │
       └────┬─────────────┬─┘
            │             │
            ▼             ▼
    [results collected and saved]
            │
            ▼
    [return to MainMenu]
    (or quit the app)
```

---

## ⚙️ Technical Specifications

### Platform
- **Game Engine**: Unity (with VR support)
- **VR Hardware**: Meta Quest (OVR SDK)
- **Scripting Language**: C#
- **Asset Store**: Brick Project Studio, Studio Billion, Free Assets

### Performance requirements
- **Frame rate**: 90 FPS (VR standard)
- **Resolution**: at least 1440×1440 (for Quest)
- **Memory**: 3-4 GB (high graphics settings)

### Data collection
- **Real-time tracking**: position, speed, direction, rotation
- **Event logging**: collisions, stops, path deviations
- **Result storage**: JSON or CSV format

---

## 🚀 Quick Start Guide

### Step 1: Open the project
```
Path: the FoG_Walkways project directory on your machine
```

### Step 2: Load the MainMenu scene
```
Unity Editor → Project tab → Assets/Scenes → double-click MainMenu.unity
```

### Step 3: Enter Play mode
```
Click the Play button → the MainMenu screen appears
```

### Step 4: Select a scenario
```
Click the ClutteredWalkway or NarrowedWalkway button
```

### Step 5: Walk through the simulation
```
Move with the VR controller or the keyboard
- Forward:   W / analog stick up
- Backward:  S / analog stick down
- Turn left: A / analog stick left
- Turn right:D / analog stick right
```

### Step 6: Check the results
```
Scenario finishes → results screen, or saved automatically
```

---

## 📝 Data Record Format

### Basic record fields
```json
{
  "scenario_name": "ClutteredWalkway",
  "start_time": "2026-07-22T10:30:00Z",
  "end_time": "2026-07-22T10:38:45Z",
  "duration_seconds": 525,
  "player_data": {
    "start_position": [0, 1.6, 0],
    "end_position": [10.5, 1.6, -2.3],
    "total_distance": 12.8,
    "average_speed": 0.024
  },
  "events": [
    {
      "timestamp": 12.5,
      "event_type": "obstacle_collision",
      "obstacle_id": 3
    }
  ],
  "metrics": {
    "total_collisions": 1,
    "path_efficiency": 0.82,
    "average_speed_ms": 0.024
  }
}
```

---

## ⚠️ Cautions

### VR safety rules
1. **Clear a play space**: at least 2 m × 2 m is needed
2. **Remove obstacles**: clear physical obstacles out of the real environment
3. **Wear shoes**: safe, comfortable shoes are recommended
4. **Light the room well**: do not test in a dark environment
5. **Have a spotter**: older adults and patients should be accompanied

### Technical cautions
1. **Back up data**: assessment data should be backed up regularly
2. **Headset calibration**: check before every session
3. **Check batteries**: check the VR controllers' battery level
4. **Network**: a stable network is needed if multiplayer features are used
5. **Data security**: patient data must be stored on a secure server

---

## 📚 Related Projects

### FoG_Revised
- **Focus**: scenarios specific to freezing of gait
- **Scenarios**: 6 (BasicScene, Freeze_of_Gait variants, and others)

### Using them together
```
FoG_Revised (primary assessment) + FoG_Walkways (environmental variety)
= a comprehensive gait assessment system
```

---

## 📞 Support and Feedback

### Where data is stored
```
Documents/FoG_Walkways_Results/
├── 2026-07-22_ClutteredWalkway_Result.json
├── 2026-07-22_NarrowedWalkway_Result.json
└── ...
```

### Troubleshooting
1. **Scene fails to load**: check the Assets/Scenes folder
2. **VR not detected**: check that the Oculus runtime is installed
3. **Frame rate drops**: lower the graphics settings

---

**Written**: 2026-07-22  
**Last updated**: 2026-07-22  
**Project version**: 1.0
