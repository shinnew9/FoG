# System Architecture - FoG VR Project

**Technical system design, data flow, and component interactions**

---

## Table of Contents

1. System Overview
2. Hardware Architecture
3. Software Architecture
4. Data Flow
5. Scene Structure
6. GameObject Hierarchy
7. Script Dependencies
8. XR Integration
9. Performance Considerations
10. Deployment Architecture

---

## 1. System Overview

### High-Level Block Diagram

```
┌─────────────────────────────────────────────────────┐
│              META QUEST 3 HEADSET                    │
│                                                     │
│  ┌──────────────────────────────────────────────┐  │
│  │         VR Display & Tracking                 │  │
│  │  - HMD Screen (1832×1920 per eye)           │  │
│  │  - IMU Sensors (6-DOF tracking)            │  │
│  │  - Controller Sensors                      │  │
│  └──────────────────────────────────────────────┘  │
│                      ↑ USB-C / WiFi               │
└─────────────────────────────────────────────────────┘
                      │
                      │ Data Stream
                      ↓
┌─────────────────────────────────────────────────────┐
│           WINDOWS PC / LAPTOP                        │
│                                                     │
│  ┌──────────────────────────────────────────────┐  │
│  │         UNITY RUNTIME ENGINE                  │  │
│  │                                               │  │
│  │  ┌──────────────────────────────────────┐   │  │
│  │  │    OVR Manager & XR System           │   │  │
│  │  │  - Input handling (OVRInput)        │   │  │
│  │  │  - Tracking updates                │   │  │
│  │  │  - Lifecycle management            │   │  │
│  │  └──────────────────────────────────────┘   │  │
│  │                    ↓                         │  │
│  │  ┌──────────────────────────────────────┐   │  │
│  │  │    SCENE LOGIC LAYER                 │   │  │
│  │  │  - ScenarioManager                  │   │  │
│  │  │  - PlayerMovement                   │   │  │
│  │  │  - MetricsCollector                 │   │  │
│  │  │  - EnvironmentTriggers              │   │  │
│  │  │  - UIManager                        │   │  │
│  │  └──────────────────────────────────────┘   │  │
│  │                    ↓                         │  │
│  │  ┌──────────────────────────────────────┐   │  │
│  │  │    RENDERING & PHYSICS               │   │  │
│  │  │  - URP (Universal Render Pipeline)  │   │  │
│  │  │  - Colliders & Triggers             │   │  │
│  │  │  - Animation System                 │   │  │
│  │  └──────────────────────────────────────┘   │  │
│  │                    ↓                         │  │
│  │  ┌──────────────────────────────────────┐   │  │
│  │  │    PERSISTENT DATA LAYER             │   │  │
│  │  │  - MetricsCollector (runtime)       │   │  │
│  │  │  - DataSerializer (on completion)  │   │  │
│  │  │  → Documents/FoG_Results/*.json    │   │  │
│  │  └──────────────────────────────────────┘   │  │
│  └──────────────────────────────────────────────┘  │
│                                                     │
└─────────────────────────────────────────────────────┘
                      ↓
           ┌──────────────────────┐
           │  Local Storage       │
           │  JSON Data Files     │
           │  (Results Database)  │
           └──────────────────────┘
```

---

## 2. Hardware Architecture

### VR Headset Specifications (Meta Quest 3)

```
META QUEST 3 HARDWARE
│
├── Display System
│   ├─ Type: OLED
│   ├─ Resolution: 1832×1920 per eye
│   ├─ Refresh Rate: 90Hz (target)
│   ├─ Field of View: ~90°
│   └─ Display Pipeline: Direct to GPU
│
├── Tracking System
│   ├─ Position Tracking: 6-DOF (XYZ + Rotation)
│   ├─ Sensors:
│   │   ├─ Inertial Measurement Unit (IMU)
│   │   │   ├─ Accelerometer (3-axis)
│   │   │   ├─ Gyroscope (3-axis)
│   │   │   └─ Magnetometer (3-axis)
│   │   └─ Optical Tracking (inside-out, front cameras)
│   ├─ Tracking Space: Guardian area (2m×2m default)
│   └─ Latency: <20ms (critical for VR)
│
├── Input Devices (Controllers)
│   ├─ Left Controller
│   │   ├─ Analog Stick (thumbstick)
│   │   ├─ Buttons (X, Y)
│   │   ├─ Triggers (Index, Grip)
│   │   └─ IMU (6-DOF tracking)
│   │
│   └─ Right Controller (identical)
│       ├─ Analog Stick
│       ├─ Buttons (A, B)
│       ├─ Triggers
│       └─ IMU
│
├── Processing
│   ├─ SoC: Snapdragon XR1 Gen 1
│   ├─ RAM: 8GB
│   ├─ Storage: 128GB/512GB
│   └─ Battery: ~2-3 hours per charge
│
└── Communication
    ├─ USB-C: High-speed data transfer
    ├─ WiFi 6E: Wireless tracking (experimental)
    └─ Bluetooth: Controller pairing
```

### PC Hardware Requirements

**Minimum**:
```
CPU:    Intel i7-8700K / AMD Ryzen 5 3600
GPU:    NVIDIA GTX 1080 / RTX 2060
RAM:    16GB DDR4
Storage: 256GB SSD (for OS + Unity)
```

**Recommended**:
```
CPU:    Intel i9-12900K / AMD Ryzen 9 5900X
GPU:    NVIDIA RTX 3080 / RTX 4070
RAM:    32GB DDR4
Storage: 512GB NVMe SSD
GPU VRAM: 8GB+ (for textures, models)
```

### Connection Architecture

```
Meta Quest 3
    │
    ├─ USB-C Cable (High-speed USB 3.1)
    │   │
    │   ├─ Data: Tracking, input, scene commands
    │   ├─ Power: Optional (can charge while running)
    │   └─ Latency: <5ms
    │
    └─ WiFi 6E (Optional, for wireless)
        └─ Latency: 10-20ms (depends on connection quality)
        
PC USB Port ← USB-C Cable ← Meta Quest 3
    │
    ├─ USB Controller Driver (Meta XR)
    ├─ Device Manager (shows "Meta Quest 3")
    └─ Meta Horizon App (shows connection status)
```

---

## 3. Software Architecture

### Unity Project Organization

```
Assets/
│
├── Scenes/                          # VR scenes (6 total)
│   ├── ScenarioMenu.unity
│   │   ├── Canvas (UI)
│   │   ├── Button_FOG
│   │   ├── Button_BasicScene
│   │   └── Button_Doorway (etc.)
│   │
│   ├── BasicScene.unity
│   │   ├── OVRCameraRig (player)
│   │   ├── EnvironmentMeshes
│   │   │   ├── Walls
│   │   │   ├── Floor
│   │   │   └── Furniture
│   │   ├── Lighting
│   │   └── MetricsCollector (script component)
│   │
│   ├── Freeze_of_Gait.unity         # Primary scenario
│   │   ├── OVRCameraRig
│   │   ├── PathVisualizer (optional)
│   │   ├── TriggerZones (doorway detection)
│   │   ├── AudioSystem (beep/voice)
│   │   └── ScenarioManager
│   │
│   └── [Other variants]
│
├── Scripts/                         # All C# gameplay code
│   ├── PlayerMovement.cs            # OVRInput handling
│   ├── ReturnToMenu.cs              # Y button navigation
│   ├── MetricsCollector.cs          # Real-time data recording
│   ├── ScenarioManager.cs           # Scenario state machine
│   ├── EnvironmentTriggers.cs       # Doorway/obstacle detection
│   ├── DataSerializer.cs            # JSON export
│   └── UIManager.cs                 # Menu & feedback
│
├── Prefabs/                         # Reusable components
│   ├── Player/
│   │   └── PlayerController.prefab  # OVRCameraRig + scripts
│   ├── UI/
│   │   ├── ScenarioButton.prefab
│   │   └── FeedbackPanel.prefab
│   └── Environment/
│       ├── Doorway.prefab
│       ├── Obstacle.prefab
│       └── TriggerZone.prefab
│
├── Materials/                       # Shaders, textures
│   ├── Floor.mat
│   ├── Wall.mat
│   └── [Standard materials]
│
├── Models/                          # 3D geometry
│   ├── Environment/
│   │   ├── Apartment/
│   │   ├── Walls/
│   │   └── Furniture/
│   └── Props/
│       ├── Doorways/
│       └── Obstacles/
│
└── Resources/                       # Runtime-loaded assets
    ├── Audio/
    │   ├── beep.wav
    │   └── voice_instructions.wav
    └── Configs/
        └── scenario_settings.json
```

### Script Architecture

```
GAME LOOP (Every Frame)
│
├─── INPUT PHASE ────────────────────────────────
│    PlayerMovement.cs
│    └─ void Update()
│       ├─ Read OVRInput.GetDown(Button.Four) → Y button
│       ├─ Read OVRInput.Get(Axis2D.PrimaryThumbstick) → movement
│       └─ Move player based on input
│
├─── LOGIC PHASE ────────────────────────────────
│    ScenarioManager.cs
│    └─ void Update()
│       ├─ Check if player at waypoint
│       ├─ Activate triggers if in range
│       └─ Update scenario state
│
│    EnvironmentTriggers.cs
│    └─ void OnTriggerEnter/Stay/Exit()
│       ├─ Detect doorway collision
│       ├─ Play audio cue
│       └─ Record trigger event
│
├─── METRICS PHASE ──────────────────────────────
│    MetricsCollector.cs
│    └─ void Update()
│       ├─ Record player position
│       ├─ Calculate distance, speed
│       ├─ Detect freeze events
│       └─ Store in session data
│
├─── RENDERING PHASE ────────────────────────────
│    (Handled by Unity/URP)
│    └─ Render scene to VR displays
│       ├─ Separate eye images
│       ├─ Apply lens distortion
│       └─ Send to headset
│
└─── OUTPUT PHASE ───────────────────────────────
     (OnSceneUnload)
     DataSerializer.cs
     └─ Save metrics to JSON file
```

---

## 4. Data Flow

### During Runtime (Per-Frame)

```
FRAME N
│
├─1. Input Capture ──────────────────────
│   └─ OVRInput reads controller state
│      ├─ Button.Four (Y) → Return to menu?
│      ├─ Thumbstick → Move direction
│      └─ Trigger → Interact
│
├─2. Player Movement ────────────────────
│   └─ PlayerMovement.cs
│      ├─ Translate player position
│      ├─ Update OVRCameraRig.localPosition
│      └─ Emit OnPlayerMoved event
│
├─3. Environment Update ─────────────────
│   └─ EnvironmentTriggers.cs
│      ├─ Check player collision with triggers
│      ├─ If in doorway → Play beep
│      └─ Record event: {timestamp, type, position}
│
├─4. Metrics Collection ─────────────────
│   └─ MetricsCollector.cs
│      ├─ snapshot = {
│      │     timestamp: Time.time,
│      │     position: player.position,
│      │     velocity: (pos - lastPos) / deltaTime,
│      │     isMoving: velocity.magnitude > 0.01,
│      │     triggers_active: [list]
│      │  }
│      └─ sessionData.Add(snapshot)
│
├─5. UI Update ──────────────────────────
│   └─ UIManager.cs
│      ├─ Update timer display
│      ├─ Show distance remaining
│      └─ Display feedback if freezing
│
└─6. Render ─────────────────────────────
    └─ URP renders left eye + right eye
       ├─ Apply lens distortion
       └─ Send to Meta Quest display
```

### On Completion (Scene End)

```
SCENARIO COMPLETE
│
├─1. Stop Recording ─────────────────────
│   └─ MetricsCollector.Finalize()
│      └─ Calculate summary statistics
│
├─2. Calculate Results ──────────────────
│   └─ Process sessionData
│      ├─ Total time: max(timestamp)
│      ├─ Distance: sum(velocity × deltaTime)
│      ├─ Avg speed: distance / totalTime
│      ├─ Freeze events: count(isMoving=false segments)
│      └─ Path efficiency: directDistance / actualDistance
│
├─3. Serialize Data ─────────────────────
│   └─ DataSerializer.SaveJSON()
│      └─ Create output:
│         {
│           "scenario": "Freeze_of_Gait",
│           "timestamp": "2026-07-22T14:30:00Z",
│           "duration_seconds": 245,
│           "metrics": { ... },
│           "raw_trace": [ ... ]
│         }
│
├─4. Save File ──────────────────────────
│   └─ File.WriteAllText(
│        "Documents/FoG_Results/FOG20260722_001.json",
│        jsonString
│      )
│
└─5. Wait for Navigation ────────────────
    └─ Await OVRInput.GetDown(Button.Four)
       └─ SceneManager.LoadScene("ScenarioMenu")
```

---

## 5. Scene Structure

### BasicScene.unity - Component Hierarchy

```
Scene: BasicScene
│
├─ OVRCameraRig [Transform at 0,0,0]
│  ├─ Anchor
│  │  └─ CenterEyeAnchor [Camera component]
│  │     ├─ Left eye renderTexture
│  │     └─ Right eye renderTexture
│  │
│  ├─ TrackingSpace [Transform, offset adjustments]
│  │  ├─ LeftController
│  │  │  ├─ Model (visual mesh)
│  │  │  ├─ Grip Trigger (collider)
│  │  │  └─ InputManager script
│  │  │
│  │  └─ RightController
│  │     ├─ Model
│  │     ├─ Buttons (colliders)
│  │     └─ InputManager script
│  │
│  └─ OVRManager [Singleton, lifecycle management]
│     ├─ Display refresh rate: 90 Hz
│     ├─ Tracking Origin: Floor Level
│     ├─ IPD (Interpupillary Distance): Auto-detect
│     └─ Guardian visible: false (during gameplay)
│
├─ Environment
│  ├─ Walls [StaticBatchingEnabled]
│  │  ├─ WallLeft [MeshFilter + MeshCollider]
│  │  ├─ WallRight
│  │  ├─ WallFront
│  │  └─ WallBack
│  │
│  ├─ Floor [Plane, non-slippery material]
│  │  ├─ BoxCollider [Is Trigger: false]
│  │  └─ Material: Floor.mat
│  │
│  └─ Lighting
│     ├─ Directional Light [Intensity: 1.42]
│     ├─ Ambient Intensity: 1.0
│     └─ Skybox: SkyboxMaterial
│
├─ Scenario Objects
│  ├─ StartMarker [Transform at entrance]
│  │  └─ Visualization (optional quad)
│  │
│  ├─ EndMarker [Transform at exit]
│  │  └─ Visualization
│  │
│  └─ InteractionZones
│     ├─ [Doors/obstacles as needed]
│     └─ [Colliders with "is Trigger" = true]
│
└─ Canvas [UI Overlay]
   ├─ TimerDisplay [Text]
   ├─ DistanceDisplay
   ├─ FeedbackPanel [Image + Text]
   └─ ReturnToMenuButton [optional, or Y button only]
```

### Freeze_of_Gait.unity - Additional Components

```
Scene: Freeze_of_Gait
│
├─ [All from BasicScene]
│  └─ OVRCameraRig, Environment, Lighting
│
├─ Scenario-Specific Objects
│  │
│  ├─ PathWaypoints [Array of transforms]
│  │  ├─ Waypoint_0 [Position: 0, 0, 0]
│  │  ├─ Waypoint_1 [Position: 0, 0, 2]
│  │  ├─ Waypoint_2 [Position: 0, 0, 4]
│  │  └─ Waypoint_N [Position: 0, 0, 16]
│  │
│  ├─ TriggerZone_Doorway
│  │  ├─ Collider [Type: Box, Is Trigger: true]
│  │  ├─ Scale: [1, 3, 0.1]
│  │  ├─ Position: [0, 1.5, 8]
│  │  └─ Script: EnvironmentTriggers.cs
│  │     └─ OnTriggerEnter:
│  │        ├─ AudioSource.PlayOneShot(beep)
│  │        └─ MetricsCollector.RecordEvent("trigger_doorway")
│  │
│  ├─ AudioSystem [GameObject]
│  │  ├─ AudioSource [Volume: 0.7]
│  │  ├─ Clips:
│  │  │  ├─ beep.wav [1000ms]
│  │  │  ├─ step_sync.wav [200ms]
│  │  │  └─ completion.wav [2000ms]
│  │  └─ SpatialAudio: disabled (non-3D)
│  │
│  └─ VisualsController [GameObject]
│     ├─ PathVisualization [LineRenderer, optional]
│     │  └─ Draw straight line from start to end
│     └─ TriggerVisualization [Gizmos, editor only]
│
├─ ScenarioManager [Script Component]
│  ├─ Property: currentWaypoint = 0
│  ├─ Property: isRunning = true
│  │
│  ├─ Method: Update()
│  │  ├─ Check if player reached nextWaypoint
│  │  ├─ If yes: currentWaypoint++
│  │  └─ If currentWaypoint == lastWaypoint: EndScenario()
│  │
│  └─ Method: EndScenario()
│     ├─ isRunning = false
│     ├─ MetricsCollector.Finalize()
│     └─ Await Y button for menu return
│
└─ MetricsCollector [Script Component, Singleton]
   ├─ Property: sessionData = { ... }
   ├─ Property: startTime = Time.time
   │
   ├─ Method: RecordSnapshot()
   │  ├─ Record position, velocity, triggers
   │  └─ Called every Update()
   │
   └─ Method: SaveToJSON()
      └─ Write to Documents/FoG_Results/
```

---

## 6. GameObject Hierarchy

### Player (OVRCameraRig) Movement

```
POSITION IN WORLD SPACE
│
├─ OVRCameraRig [Transform]
│  └─ localPosition = [0, 1.6, 0]  (player eye height)
│
├─ PlayerMovement.cs [Script]
│  │
│  ├─ void Update()
│  │  │
│  │  ├─1. Read input
│  │  │   input = OVRInput.Get(Axis2D.PrimaryThumbstick)
│  │  │   └─ input ∈ [-1, 1] for X,Z (Y-up forward motion)
│  │  │
│  │  ├─2. Calculate movement
│  │  │   direction = Quaternion.AngleAxis(yaw, up) * forward
│  │  │   movementVector = direction * input.magnitude * speed
│  │  │
│  │  ├─3. Update position
│  │  │   rigidbody.velocity = movementVector
│  │  │   OR (if no physics)
│  │  │   transform.Translate(movementVector * Time.deltaTime)
│  │  │
│  │  └─4. Emit event
│  │     OnPlayerMoved?.Invoke(newPosition, velocity)
│  │
│  └─ Code Snippet
│     ```csharp
│     Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
│     Vector3 direction = transform.forward * input.y + 
│                         transform.right * input.x;
│     rigidbody.velocity = direction * walkSpeed;
│     ```
```

### Tracking Accuracy

```
HEADSET → PC COMMUNICATION

Meta Quest 3 sends (120 Hz):
├─ Head position: [x, y, z]
├─ Head rotation: [pitch, yaw, roll] (quaternion)
├─ Controller L position
├─ Controller L rotation
├─ Controller R position
├─ Controller R rotation
└─ Button states (binary)

PC Unity receives and updates:
├─ OVRCameraRig.localPosition = headPos
├─ OVRCameraRig.localRotation = headRot
├─ LeftController.localPosition = leftPos
├─ LeftController.localRotation = leftRot
└─ [Similarly for right]

Visual updates (90 Hz):
├─ Render each eye
├─ Apply lens distortion
├─ Send to display
└─ Latency: ~20ms (acceptable for VR)
```

---

## 7. Script Dependencies

### Dependency Graph

```
                    ┌─────────────────┐
                    │  Unity (Core)   │
                    │ - SceneManager  │
                    │ - Time          │
                    │ - Input         │
                    └────────┬────────┘
                             │
                    ┌────────┴────────┐
                    │                 │
            ┌───────▼──────┐  ┌───────▼──────┐
            │   OVRManager │  │  OVRInput    │
            │ (Meta XR)    │  │ (Meta XR)    │
            └───────┬──────┘  └───────┬──────┘
                    │                 │
        ┌───────────┼─────────────────┼────────────┐
        │           │                 │            │
        │      ┌────▼──────┐   ┌──────▼────┐      │
        │      │PlayerMove │   │ReturnToMenu   │
        │      │  ment.cs  │   │  .cs        │      │
        │      └────┬──────┘   └─────┬──────┘      │
        │           │                │             │
        │      ┌────▼──────────────────▼────┐      │
        │      │  ScenarioManager.cs        │      │
        │      └────┬──────────────────┬────┘      │
        │           │                  │           │
        │      ┌────▼──┐      ┌────────▼─────┐    │
        │      │Environ│      │  MetricsCol  │    │
        │      │Trigger│      │  lector.cs   │    │
        │      └────────┘     └────────┬─────┘    │
        │                             │           │
        │                      ┌──────▼──────┐   │
        │                      │DataSerializer   │
        │                      │    .cs      │   │
        │                      └─────────────┘   │
        │                                        │
        └────────────────────┬───────────────────┘
                             │
                      ┌──────▼──────┐
                      │ File System │
                      │ JSON Output │
                      └─────────────┘
```

### Import Order

```csharp
// PlayerMovement.cs depends on:
using UnityEngine;           // Core
using OVR;                   // Meta XR (must be imported first)

// ReturnToMenu.cs depends on:
using UnityEngine;
using UnityEngine.SceneManagement; // Scene switching

// ScenarioManager.cs depends on:
using UnityEngine;
using System.Collections.Generic;

// MetricsCollector.cs depends on:
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// DataSerializer.cs depends on:
using UnityEngine;
using System.IO;
using System.Text;
```

---

## 8. XR Integration

### Meta XR SDK Pipeline

```
INSTALL & IMPORT SEQUENCE

1. Download Meta XR All-in-One SDK
   └─ https://developer.oculus.com/

2. Unity Project Setup
   └─ Assets → Import Package → Meta XR SDK

3. XR Plug-in Management
   └─ Edit → Project Settings → XR Plug-in Management
      ├─ Select Android Platform
      ├─ ☑ Meta Quest Support
      ├─ ☑ OpenXR
      └─ Install Validation Tool

4. Input System
   └─ Project Settings → Input Manager
      ├─ Add OVRInput bindings
      ├─ Button.Four = Y button
      ├─ Axis2D.PrimaryThumbstick = Movement
      └─ Trigger = Interact
```

### Runtime XR Initialization

```csharp
// At scene start (implicit in OVRManager)

void Start()
{
    // OVRManager auto-initializes in Awake()
    // This includes:
    
    // 1. Display initialization
    OVRManager.instance.DisplayRefreshRate = 90f;
    
    // 2. Tracking setup
    OVRManager.instance.TrackingOriginType = 
        OVRManager.TrackingOrigin.FloorLevel;
    
    // 3. Input system activation
    OVRInput.Initialize(); // Auto called
    
    // 4. Camera rig setup
    // OVRCameraRig handles stereoscopic rendering
    //   ├─ Left eye camera
    //   ├─ Right eye camera
    //   └─ Head tracking input
}
```

### Controller Input Mapping

```
META QUEST 3 CONTROLLER BUTTONS
│
├─ LEFT CONTROLLER
│  ├─ Thumbstick: Axis2D.PrimaryThumbstick
│  ├─ X Button: Button.Three
│  ├─ Y Button: Button.Four ◄─ RETURN TO MENU
│  ├─ Trigger: Axis1D.PrimaryIndexTrigger
│  └─ Grip: Axis1D.PrimaryHandTrigger
│
└─ RIGHT CONTROLLER
   ├─ Thumbstick: Axis2D.SecondaryThumbstick
   ├─ A Button: Button.One
   ├─ B Button: Button.Two
   ├─ Trigger: Axis1D.SecondaryIndexTrigger
   └─ Grip: Axis1D.SecondaryHandTrigger

CODE USAGE:
if (OVRInput.GetDown(OVRInput.Button.Four))
{
    // Y button pressed on left controller
    SceneManager.LoadScene("ScenarioMenu");
}

Vector2 movement = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
// movement.x = left/right
// movement.y = forward/backward
```

---

## 9. Performance Considerations

### Frame Budget (90 FPS target)

```
TOTAL FRAME TIME: 11.1ms per frame (90 FPS)

├─ Input Processing:      0.5ms  (4.5%)
├─ Physics & Logic:        2.0ms  (18%)
├─ Rendering:              6.0ms  (54%)
│  ├─ Culling
│  ├─ Draw calls
│  └─ GPU rendering
├─ Metrics Collection:     0.5ms  (4.5%)
├─ UI:                     1.0ms  (9%)
└─ Overhead:               1.1ms  (10%)
```

### Optimization Guidelines

**For 90 FPS stability**:

```
✅ DO:
├─ Use StaticBatchingEnabled for environment
├─ Keep draw calls <1000 per frame
├─ Use LOD (Level of Detail) for far objects
├─ Recycle pooled GameObjects (buttons, UI)
├─ Use native arrays (C# jobs) for metrics
│
❌ DON'T:
├─ Spawn new objects during gameplay
├─ Use expensive physics queries every frame
├─ Load assets (textures, meshes) on-demand
├─ Use LINQ in Update() loops
└─ Render at > 2K resolution per eye
```

### Memory Usage

```
TYPICAL SESSION MEMORY

Base (Unity + OVR):        ~400 MB
├─ Scene assets            ~150 MB
├─ Metrics buffer          ~50 MB  (per session)
├─ Textures/Materials      ~100 MB
└─ Shaders/Rendering       ~100 MB

Total per session:         ~600-800 MB
Available headset RAM:     8 GB (plenty)
```

---

## 10. Deployment Architecture

### Build Pipeline

```
DEVELOPMENT → BUILD → DEPLOYMENT

1. Development
   └─ Edit in Unity Editor
      ├─ Test in Play Mode (PC)
      ├─ Connect headset for VR testing
      └─ Iterate on features

2. Build (Android APK)
   ├─ File → Build Settings
   ├─ Select Android platform
   ├─ Configure Build Settings:
   │  ├─ Scenes in Build: All 6 scenarios
   │  ├─ Company Name, Product Name
   │  ├─ Version: 1.0.0
   │  └─ Minimum API: Level 29
   ├─ Player Settings:
   │  ├─ Graphics API: Vulkan
   │  ├─ Package Name: com.lehigh.fog
   │  └─ Target API: Level 33+
   └─ Click "Build"
      └─ Output: FoG_Revised.apk

3. Deployment
   ├─ Option A: Direct Install
   │  └─ adb install FoG_Revised.apk
   │
   ├─ Option B: SideQuest
   │  └─ Upload APK via SideQuest tool
   │
   └─ Option C: Meta App Lab (production)
       └─ Submit for review
       └─ Distribute through Meta store

4. Runtime
   └─ App starts on headset
      ├─ ScenarioMenu loads
      ├─ User selects scenario
      └─ Scenario runs, collects data
```

### Data Pipeline (Post-Session)

```
ON DEVICE (Meta Quest 3)
    ↓
[Session complete]
    ↓
[Data saved to local storage]
    ↓
OPTION 1: USB Transfer
    ├─ Connect to PC via USB-C
    ├─ Use SideQuest or adb
    ├─ Pull file: adb pull /sdcard/FoG_Results/
    └─ File lands on PC

OPTION 2: Cloud Sync (future)
    ├─ Auto-upload to server
    ├─ Schedule: per-session or nightly
    └─ Sync to research database

OPTION 3: Manual Export
    ├─ App menu → Export Data
    ├─ Create shareable JSON
    └─ Email or upload
```

---

## Summary: Architecture Layers

| Layer | Components | Responsibility |
|-------|-----------|-----------------|
| **Hardware** | Meta Quest 3, PC, USB | Tracking, display, input |
| **XR Runtime** | OVR SDK, Unity | Headset lifecycle, rendering |
| **Input** | OVRInput, PlayerMovement | Controller input → player motion |
| **Logic** | ScenarioManager, Triggers | Scenario state, events |
| **Metrics** | MetricsCollector | Real-time data recording |
| **Output** | DataSerializer, File I/O | Results persistence |
| **UI** | Canvas, UIManager | User feedback & navigation |

---

**Last Updated**: 2026-07-22  
**Diagram Format**: Plain text ASCII (for GitHub)  
**Version**: 1.0
