# Development Guide - For Contributors

**Setup, build, debug, and deploy the FoG project**

---

## Table of Contents

1. Development Environment Setup
2. Project Structure Navigation
3. Building & Running
4. Debugging Techniques
5. Adding New Features
6. Testing Guidelines
7. Performance Optimization
8. Deployment Process
9. Git Workflow
10. Troubleshooting for Developers

---

## 1. Development Environment Setup

### Step 1: Prerequisites Check

```bash
# Verify you have:
├─ Windows 10/11 (64-bit)
├─ 16GB RAM (8GB minimum)
├─ NVIDIA/AMD GPU with 4GB VRAM
├─ 256GB SSD with 50GB free
├─ Visual Studio 2022 Community (or Code)
└─ Git installed
```

### Step 2: Unity Installation

```bash
1. Download Unity Hub from unity.com
2. Install Unity Hub
3. Open Unity Hub → Installs → Add
4. Select: Unity 2022.3 LTS (Long Term Support)
5. Add Modules:
   ├─ Android Build Support
   ├─ Android SDK & NDK Tools  
   ├─ OpenJDK
   └─ Windows Build Support (optional)
6. Install (takes 30-45 minutes)
```

**Verify Installation**:
```bash
# Check Unity is accessible
"C:\Program Files\Unity\Hub\Editor\2022.3.x\Editor\Unity.exe" --version
# Output: 2022.3.x.xxxxx
```

### Step 3: Clone Repository

```bash
# Using Git
git clone [repository-url]
cd FoG_Revised

# Or download ZIP from GitHub
# → Download ZIP
# → Extract to C:\Users\user\workspaces\unity\FoG_Revised
```

### Step 4: Install Meta XR SDK

```bash
1. Visit: https://developer.oculus.com/downloads/
2. Download: Meta XR All-in-One SDK
3. Extract the package
4. In Unity:
   - Assets → Import Package → Custom Package
   - Select Meta XR package
   - Click "Import"
   - Wait for import (2-3 minutes)
```

### Step 5: Configure Visual Studio

```bash
# Install C# extension pack
1. Open Visual Studio 2022
2. Extensions → Manage Extensions
3. Search: "C#"
4. Install: C# Dev Kit
5. Restart Visual Studio

# Optional: Set as default C# editor
1. Edit → Preferences → External Tools
2. External Script Editor: Visual Studio 2022
```

---

## 2. Project Structure Navigation

### Folder Overview

```
FoG_Revised/
│
├── Assets/
│   ├── Scenes/                    # Where: VR scenes
│   │   └── README.md              # ← READ THIS FIRST
│   │
│   ├── Scripts/                   # Where: All C# code
│   │   ├── README.md
│   │   ├── PlayerMovement.cs
│   │   ├── ReturnToMenu.cs
│   │   └── ...
│   │
│   ├── Materials/                 # Where: Shaders, textures
│   ├── Models/                    # Where: 3D meshes
│   ├── Prefabs/                   # Where: Reusable components
│   ├── Resources/                 # Where: Runtime-loaded assets
│   ├── Plugins/                   # Where: DLLs (Meta XR)
│   └── Editor/                    # Where: Custom editor scripts
│
├── ProjectSettings/
│   └── ProjectSettings.asset      # ← Do NOT edit manually
│
├── Builds/                        # Where: APK files are generated
│
├── docs/
│   ├── README.md                  # Project overview
│   ├── ARCHITECTURE.md            # System design
│   ├── CODE_DOCUMENTATION.md      # Code walkthroughs
│   └── DEVELOPMENT.md             # This file
│
├── .gitignore                     # What to NOT commit
└── README.md                      # ← Read first (root level)
```

### Key Files to Know

| File | Purpose | Edit Frequency |
|------|---------|---|
| **ProjectSettings.asset** | XR, rendering config | Rarely |
| **Assets/Scripts/*.cs** | Game logic | Often |
| **Assets/Scenes/*.unity** | Scene setup | Often |
| **.gitignore** | Exclude build artifacts | Once |

---

## 3. Building & Running

### Method 1: Editor Play Mode (Testing)

**Fastest for iteration**

```bash
1. Open Unity Editor
2. File → Open Scene → Assets/Scenes/BasicScene.unity
3. Click Play ▶ button
4. In Scene view, press Play to move in editor
5. View output in Console tab
```

**Pros**:
- Instant feedback
- Full debugging with breakpoints
- No headset needed initially

**Cons**:
- Not VR (regular 3D view)
- Can't test controller input without headset

---

### Method 2: VR Testing (Headset Connected)

**For full VR testing**

```bash
SETUP:
1. Connect Meta Quest 3 via USB-C
2. Put on headset
3. Allow USB permission popup
4. In Meta Horizon app → Quest Link → Launch
5. Back in Unity → File → Open Scene

PLAY:
1. Click Play ▶ button
2. Scene renders to headset
3. Look around with head (6-DOF tracking)
4. Move with analog stick
5. Press Y button to test menu return
```

**Expected Performance**:
- Framerate: 90 FPS (critical for VR)
- Latency: <20ms (head-to-display)
- No motion sickness (smooth movement)

---

### Method 3: Build APK (Deployment)

**For installing on headset permanently**

```bash
BUILD STEPS:

1. Prepare scenes:
   File → Build Settings
   ├─ Platform: Android
   ├─ Scenes in Build:
   │  ├─ 0: Assets/Scenes/ScenarioMenu.unity
   │  ├─ 1: Assets/Scenes/BasicScene.unity
   │  ├─ 2: Assets/Scenes/Freeze_of_Gait.unity
   │  └─ ... (all 6 scenarios)
   └─ Click "Build"

2. Configure Player Settings:
   Edit → Project Settings → Player
   ├─ Company Name: Lehigh University
   ├─ Product Name: FoG_Revised
   ├─ Version: 1.0.0
   ├─ Package Name: com.lehigh.fog
   ├─ Minimum API Level: 29
   ├─ Target API Level: 33
   └─ Graphics API: Vulkan

3. Click "Build" button:
   └─ Choose output folder
   └─ File generated: FoG_Revised.apk

4. Install on headset:
   Option A: ADB command
   ```
   adb install FoG_Revised.apk
   ```
   
   Option B: SideQuest
   ```
   1. Open SideQuest
   2. Drag & drop APK file
   3. Wait for installation
   4. Launch from headset menu
   ```

5. Verify:
   └─ On headset, look for "FoG_Revised" in apps list
```

**Build Time**: 5-15 minutes (depends on PC speed)

---

## 4. Debugging Techniques

### Technique 1: Console Logging

**For checking code execution**

```csharp
// In any script:
Debug.Log("Message: " + variable);
Debug.LogWarning("Something suspicious");
Debug.LogError("Critical error!");

// View in:
// 1. Unity Console (Window → General → Console)
// 2. Device logs (adb logcat)
// 3. Player.log on headset
```

**Example**:
```csharp
void Update()
{
    Debug.Log($"Player position: {transform.position}");
    Debug.Log($"Speed: {velocity:F2} m/s");
    
    if (Input.GetKeyDown(KeyCode.Space))
    {
        Debug.LogError("Space key pressed (shouldn't happen in VR!)");
    }
}
```

---

### Technique 2: Breakpoints & Stepping

**For inspecting variables**

```
SETUP:
1. Open Script in Visual Studio
2. Click left margin next to line number
   └─ Red circle appears (breakpoint)
3. Click Play in Unity
4. Code execution pauses at breakpoint
5. Inspect variables in Watch window
6. Press Step Over/Into to continue

EXAMPLE:
Line 45: rb.velocity = direction * walkSpeed;  ← Click here for breakpoint
         └─ Execution pauses
         └─ Hover over 'direction' to see value
         └─ Inspect 'walkSpeed' in Watch
         └─ Press F10 to step to next line
```

---

### Technique 3: Visual Debugging

**For checking scene state**

```csharp
// In Update():
public bool showDebugVisuals = true;

void Update()
{
    if (showDebugVisuals)
    {
        // Draw a line from player to waypoint
        Debug.DrawLine(transform.position, waypoint.position, Color.green);
        
        // Draw a sphere at waypoint
        Debug.DrawWireSphere(waypoint.position, 0.5f, Color.red);
    }
}
```

**In Scene View**:
- Gizmo mode shows debug lines
- Helpful for seeing triggers, paths, etc.

---

### Technique 4: Performance Profiling

**For identifying bottlenecks**

```bash
Window → Analysis → Profiler

Tabs to check:
├─ CPU Usage
│  └─ Check which scripts take longest
├─ Memory
│  └─ Watch for memory leaks
├─ GPU
│  └─ Ensure <90% usage at 90 FPS
└─ Frame Time
   └─ Breakdown of where time is spent
```

---

## 5. Adding New Features

### Example: Add a New Scenario

```bash
STEPS:

1. Create New Scene:
   File → New Scene → 3D
   → File → Save As
   → Assets/Scenes/Freeze_of_Gait_NewVariant.unity

2. Setup Base Objects:
   ├─ Right-click → 3D Object → Plane (for floor)
   ├─ Add OVRCameraRig prefab
   │  └─ Assets/OVR/Prefabs → Drag OVRCameraRig
   └─ Add ReturnToMenu script to empty GameObject

3. Add Scenario-Specific Objects:
   ├─ Create empty → Add waypoints (child transforms)
   ├─ Create trigger zones (Box Collider, Is Trigger: ☑)
   └─ Add script components

4. Test in Editor:
   └─ Play and verify basic functionality

5. Register in ScenarioMenu:
   └─ Edit ScenarioMenu.unity
   └─ Add button that loads new scene
   └─ Button onClick → SceneManager.LoadScene("new scene name")

6. Update Build Settings:
   └─ File → Build Settings
   └─ Add new scene to Scenes in Build list

7. Commit to Git:
   git add Assets/Scenes/Freeze_of_Gait_NewVariant.unity
   git commit -m "Add new FOG variant scenario"
```

---

## 6. Testing Guidelines

### Unit Testing (Optional but Recommended)

```csharp
// Using Unity Test Framework (optional package)
// File: Assets/Tests/MetricsCollectorTest.cs

using NUnit.Framework;
using UnityEngine;

public class MetricsCollectorTest
{
    [Test]
    public void TestDistanceCalculation()
    {
        // Arrange
        var collector = new MetricsCollector();
        Vector3[] positions = {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1)
        };
        
        // Act
        float distance = collector.CalculateDistance(positions);
        
        // Assert
        Assert.AreEqual(2f, distance, 0.01f);  // ±0.01m tolerance
    }
}
```

### Integration Testing (More Important)

```bash
MANUAL TEST SEQUENCE:

1. Scene Load Test:
   ├─ Load each scene
   └─ Verify no errors in console

2. Input Test:
   ├─ Connect headset
   ├─ Test each controller button
   ├─ Test analog stick movement
   └─ Verify Y button returns to menu

3. Metrics Test:
   ├─ Run scenario for 1 minute
   ├─ Check data is recorded
   ├─ Verify JSON file created
   └─ Inspect values make sense

4. Performance Test:
   ├─ Open Profiler
   ├─ Run scenario
   ├─ Check FPS stays at 90
   ├─ Check memory stable (no leaks)
   └─ Check GPU usage <90%
```

---

## 7. Performance Optimization

### Profiling Workflow

```bash
1. Identify bottleneck:
   Window → Analysis → Profiler
   ├─ Run scenario
   ├─ Look for spikes in CPU/GPU usage
   └─ Note which script is slow

2. Optimize problem area:
   Common fixes:
   ├─ Cache GetComponent() calls
   ├─ Use Object Pooling for repeatedly created objects
   ├─ Reduce draw calls (combine meshes)
   ├─ Use LOD (Level of Detail) for distant objects
   └─ Profile again to verify improvement

3. Example optimization:
```

**Before (Slow)**:
```csharp
void Update()
{
    // ❌ SLOW: GetComponent called every frame
    MetricsCollector metrics = GetComponent<MetricsCollector>();
    metrics.RecordData();
}
```

**After (Fast)**:
```csharp
private MetricsCollector metrics;

void Start()
{
    // ✅ FAST: Cache reference
    metrics = GetComponent<MetricsCollector>();
}

void Update()
{
    metrics.RecordData();  // Reuse cached reference
}
```

---

## 8. Deployment Process

### Step-by-Step Deployment

```bash
BEFORE DEPLOYING:

1. Code Review:
   ├─ Run all tests
   ├─ Check Profiler
   ├─ Code review with team
   └─ No compiler warnings

2. Version Bump:
   Edit → Project Settings → Player
   └─ Increment version: 1.0.0 → 1.0.1

3. Build APK:
   File → Build Settings → Build
   └─ Select output folder
   └─ Wait for build completion

4. Test Built APK:
   adb install FoG_Revised_1.0.1.apk
   └─ Run through test sequence on real headset
   └─ Check no issues introduced

5. Create Release Notes:
   Document:
   ├─ What changed
   ├─ Known issues
   └─ Version number

6. Deploy:
   Option A: Direct installation (development)
   Option B: SideQuest (internal testing)
   Option C: Meta Store (production, requires review)

7. Verify Deployment:
   └─ Test on multiple headsets
   └─ Check data collection works
   └─ Monitor for crashes (via logs)
```

---

## 9. Git Workflow

### Basic Commands

```bash
# Clone project
git clone [url]
cd FoG_Revised

# Create feature branch
git checkout -b feature/add-new-scenario
# Work on changes...

# Check status
git status
# Output: Modified files...

# Stage changes
git add Assets/Scripts/NewScript.cs
git add Assets/Scenes/NewScene.unity

# Commit
git commit -m "Add new scenario variant with improved metrics"

# Push to GitHub
git push origin feature/add-new-scenario

# Create Pull Request on GitHub
# → GitHub interface → New Pull Request
# → Describe changes, request reviewers
# → After approval, merge to main
```

### Branching Strategy

```
main (production-ready)
 ├─ feature/add-new-scenario (your work)
 ├─ feature/improve-graphics
 └─ bugfix/fix-input-handling (urgent fixes)

WORKFLOW:
1. Create branch from main
2. Make changes
3. Push to GitHub
4. Create Pull Request
5. Team reviews & approves
6. Merge to main
7. Delete feature branch
```

### Commit Message Format

```bash
# Good commit message format:
git commit -m "Add new scenario: FOG with obstacles

- Implement 3 difficulty levels
- Add obstacle physics and collisions
- Record trigger events in metrics
- Tested with 90 FPS performance

Fixes #42"

# Components:
├─ First line: Clear summary (50 chars max)
├─ Blank line
├─ Detailed explanation
├─ Bullet points for major changes
└─ Reference issue number (Fixes #X)
```

---

## 10. Troubleshooting for Developers

### Problem: "Scene loads but no VR view"

```
Symptoms:
- Game runs in editor view
- Headset shows black screen
- Console has no errors

Solutions:
1. Check Quest Link is enabled
   └─ Meta Horizon App → Settings → Quest Link

2. Verify XR settings:
   └─ Edit → Project Settings → XR Plug-in Management
   └─ Ensure "Meta Quest" is checked

3. Check OVRCameraRig exists in scene:
   └─ Hierarchy window
   └─ Should show OVRCameraRig object
   └─ Has children: LeftEyeAnchor, RightEyeAnchor

4. Verify headset connection:
   └─ Device Manager in Meta Horizon App
   └─ Should show "Connected"
   └─ Battery indicator visible
```

---

### Problem: "Controls don't work"

```
Symptoms:
- Analog stick doesn't move player
- Y button doesn't return to menu

Solutions:
1. Check Input Project Settings:
   └─ Edit → Project Settings → Input Manager
   └─ OVRInput mappings present

2. Verify script has OVRInput code:
   └─ PlayerMovement.cs
   └─ Should have: OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick)

3. Controller pairing:
   └─ On headset: Settings → Pair New Controller
   └─ Physically press pairing button on controller

4. Test in native app first:
   └─ Close Unity
   └─ Use Meta Horizon home menu
   └─ Controllers should work there
   └─ If not, hardware issue
```

---

### Problem: "Performance is 60 FPS instead of 90"

```
Symptoms:
- Game runs but feels laggy
- Profiler shows low FPS
- Motion sickness during testing

Solutions:
1. Check Profiler:
   └─ Window → Analysis → Profiler
   └─ Identify which component is slow
   └─ Follow optimization tips in Section 7

2. Reduce graphics:
   └─ Edit → Project Settings → Graphics
   └─ Set to "Very Low" quality
   └─ Check FPS recovers

3. Check background processes:
   └─ Task Manager
   └─ Close unnecessary apps
   └─ Leave >50% GPU available

4. Rebuild with optimization:
   └─ File → Build Settings
   └─ Development Build: ☑
   └─ IL2CPP: ☑ (slower build, faster runtime)
```

---

### Problem: "Build fails with errors"

```
Symptoms:
- Build button → Error popup
- Console full of red messages

Common causes & solutions:

1. Missing scenes:
   └─ File → Build Settings
   └─ Add all scenes (Assets/Scenes/*.unity)

2. Script compilation errors:
   └─ Console tab → look for red errors
   └─ Fix syntax errors in scripts
   └─ Click error → opens script at line

3. Missing dependencies:
   └─ Ensure Meta XR SDK is imported
   └─ Ensure OVR camera rig prefab exists
   └─ Try reimporting assets

4. API level mismatch:
   └─ Edit → Project Settings → Player
   └─ Minimum API: 29
   └─ Target API: 33+

5. Clean and rebuild:
   └─ File → Build Settings → Clean Build
   └─ Then try building again
```

---

## Quick Reference Checklist

**Before Starting Development**:
- [ ] Unity 2022.3 LTS installed
- [ ] Meta XR SDK imported
- [ ] Project opens without errors
- [ ] Play mode works in editor
- [ ] Headset connects via Quest Link

**While Developing**:
- [ ] Use version control (Git)
- [ ] Test frequently (Play mode)
- [ ] Profile performance (Profiler)
- [ ] Write clean code (follow patterns)
- [ ] Document complex logic (comments)

**Before Committing**:
- [ ] No compiler warnings/errors
- [ ] Tested in editor
- [ ] Tested on headset (if possible)
- [ ] Profiler shows stable 90 FPS
- [ ] Console has no errors

**Before Deploying**:
- [ ] All tests pass
- [ ] Code review complete
- [ ] Version bumped
- [ ] APK built successfully
- [ ] Tested on physical headset

---

**Last Updated**: 2026-07-22  
**Target Audience**: Developers, engineers  
**Skill Level**: Intermediate (some Unity experience recommended)
