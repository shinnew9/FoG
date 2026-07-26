# Code Documentation - Line-by-Line Explanation

**Detailed walkthrough of all C# scripts in the FoG Project**

---

## Table of Contents

1. ReturnToMenu.cs - Menu Navigation
2. PlayerMovement.cs - VR Controller Input
3. MetricsCollector.cs - Data Recording
4. ScenarioManager.cs - Scenario Logic
5. EnvironmentTriggers.cs - Event Detection
6. DataSerializer.cs - JSON Export
7. UIManager.cs - User Interface

---

## 1. ReturnToMenu.cs - Menu Navigation

**Location**: `Assets/Scripts/ReturnToMenu.cs`  
**Purpose**: Allow players to return to menu using Y button (left controller)

### Full Code

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    void Update()
    {
        // Y button (left controller)
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            Debug.Log("[Scene] Returning to menu...");
            SceneManager.LoadScene("ScenarioMenu");
        }
    }
}
```

### Line-by-Line Explanation

```csharp
using UnityEngine;
```
**What**: Import Unity core library  
**Why**: Needed for MonoBehaviour, Debug, etc.

```csharp
using UnityEngine.SceneManagement;
```
**What**: Import scene management  
**Why**: Need `SceneManager.LoadScene()` to switch scenes

---

```csharp
public class ReturnToMenu : MonoBehaviour
```
**What**: Define the script class (must inherit MonoBehaviour)  
**Why**: MonoBehaviour allows script to attach to GameObjects and use Unity lifecycle methods  
**Scope**: `public` = visible to other scripts, `MonoBehaviour` = Unity game object script

---

```csharp
void Update()
```
**What**: Built-in Unity method called once per frame  
**Frequency**: 60-90 times per second (depending on VR headset)  
**Why**: Perfect place for input checking and per-frame logic

---

```csharp
if (OVRInput.GetDown(OVRInput.Button.Four))
```
**What**: Check if Y button was PRESSED this frame (not held)  
**Breakdown**:
- `OVRInput` = Meta XR input class
- `GetDown()` = True only on frame button is pressed (not continuous)
- `Button.Four` = Y button on left controller
  - Button.One = A (right)
  - Button.Two = B (right)
  - Button.Three = X (left)
  - Button.Four = Y (left) ◄── We want this

**Alternative**: 
```csharp
if (OVRInput.Get(OVRInput.Button.Four))  // Held down
```

---

```csharp
Debug.Log("[Scene] Returning to menu...");
```
**What**: Print message to Unity Console  
**Output**: In Console tab: `[Scene] Returning to menu...`  
**Purpose**: Debugging - verify button press is detected  
**Note**: Console message appears in editor, not on headset

---

```csharp
SceneManager.LoadScene("ScenarioMenu");
```
**What**: Load the scene named "ScenarioMenu"  
**Breakdown**:
- `SceneManager` = Unity class for scene control
- `LoadScene()` = Method to switch scenes
- `"ScenarioMenu"` = Scene name (must match exactly)

**Scene Name Match**:
```
✅ CORRECT: Assets/Scenes/ScenarioMenu.unity → LoadScene("ScenarioMenu")
❌ WRONG:  LoadScene("Scenario Menu")  // Extra space
❌ WRONG:  LoadScene("ScenarioMenu.unity")  // Include extension
```

**What Happens**:
```
T+0ms:  SceneManager.LoadScene() called
T+50ms: Current scene unloads
T+100ms: ScenarioMenu.unity loads
T+150ms: Scene displays in headset
T+200ms: Player can interact
```

---

### Attachment & Usage

**Where to Attach**: 
- Create empty GameObject in scene called "MenuButton"
- Add this script as component
- Script runs automatically on Start()

**Works Globally**:
- Attach to any scene (BasicScene, Freeze_of_Gait, etc.)
- Y button will always return to menu
- No additional setup needed

---

## 2. PlayerMovement.cs - VR Controller Input

**Location**: `Assets/Scripts/PlayerMovement.cs`  
**Purpose**: Read analog stick input and move player character

### Full Code Example

```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;  // Meters per second
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Read input from left analog stick
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Convert 2D input to 3D movement
        Vector3 direction = transform.forward * input.y + 
                           transform.right * input.x;

        // Apply speed and update position
        rb.velocity = direction * walkSpeed;

        // Debug: Print current speed
        if (input.magnitude > 0.1f)
        {
            Debug.Log($"Moving at {rb.velocity.magnitude:F2} m/s");
        }
    }
}
```

### Line-by-Line Explanation

```csharp
public float walkSpeed = 2f;
```
**What**: Declare public variable for walking speed  
**Value**: `2f` = 2 meters per second  
**Why Public**: Can adjust in Inspector without code change  
**Typical Values**:
```
Human walking: 1.3 m/s
Elderly slow walk: 0.8 m/s
Fast walk: 1.5-2.0 m/s
```

---

```csharp
private Rigidbody rb;
```
**What**: Private variable to store reference to Rigidbody component  
**Why**: Need this to apply forces/velocity  
**Private**: Only this script can access (safe)

---

```csharp
void Start()
{
    rb = GetComponent<Rigidbody>();
}
```
**What**: Initialize at scene start  
**Breakdown**:
- `Start()` = Called once on first frame
- `GetComponent<Rigidbody>()` = Find Rigidbody on same GameObject
- `rb =` = Store reference for later use

**Why This**: Faster than calling GetComponent every frame

---

```csharp
Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
```
**What**: Read analog stick position from left controller  
**Returns**: 
```
Vector2 {x, y}
├─ x: Left(-1) to Right(+1)
├─ y: Back(-1) to Forward(+1)
└─ Magnitude: 0 (centered) to 1 (full deflection)
```

**Example Values**:
```
Stick Up:       Vector2(0, 1)
Stick Right:    Vector2(1, 0)
Stick Diagonal: Vector2(0.707, 0.707)
Stick Centered: Vector2(0, 0)
```

**Alternative**:
```csharp
Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);  // Right stick
```

---

```csharp
Vector3 direction = transform.forward * input.y + 
                   transform.right * input.x;
```
**What**: Convert 2D stick input to 3D world direction  
**Breakdown**:
```
transform.forward = Where player is looking (Z-axis in local space)
transform.right = Player's right side (X-axis in local space)

direction = (forward * input.y) + (right * input.x)
          = (where player looks * forward-back input) +
            (right side * left-right input)
```

**Example - Stick Up**:
```
input = Vector2(0, 1)
direction = transform.forward * 1 + transform.right * 0
          = transform.forward
          = Move in direction player is facing ✅
```

**Example - Stick Right**:
```
input = Vector2(1, 0)
direction = transform.forward * 0 + transform.right * 1
          = transform.right
          = Move to player's right ✅
```

**Why Not Simple**: If we did `Vector3 direction = new Vector3(input.x, 0, input.y)`, player would move the same direction regardless of where they're facing (bad for VR).

---

```csharp
rb.velocity = direction * walkSpeed;
```
**What**: Apply movement to player  
**Breakdown**:
- `rb.velocity` = Rigidbody velocity (meters per second)
- `direction` = Normalized direction (0 to 1)
- `* walkSpeed` = Multiply by 2 m/s speed

**Example**:
```
direction = Vector3(1, 0, 0)  // Move right
walkSpeed = 2f
rb.velocity = (1, 0, 0) * 2 = (2, 0, 0)
Result: Move right at 2 m/s ✅
```

**Alternative (without Rigidbody)**:
```csharp
transform.Translate(direction * walkSpeed * Time.deltaTime);
```

---

```csharp
if (input.magnitude > 0.1f)
{
    Debug.Log($"Moving at {rb.velocity.magnitude:F2} m/s");
}
```
**What**: Print speed to console when moving  
**Breakdown**:
- `input.magnitude` = Distance from center (0=still, 1=full stick)
- `> 0.1f` = Only print if stick deflection > 10% (avoids noise)
- `rb.velocity.magnitude` = Speed in m/s (length of velocity vector)
- `:F2` = Format as float with 2 decimal places

**Example Output**:
```
Moving at 1.45 m/s
Moving at 1.50 m/s
Moving at 0.98 m/s
```

---

### Integration Notes

**Required Components**:
```
GameObject "Player" must have:
├─ Transform (all GameObjects have this)
├─ Rigidbody (needed for rb.velocity)
│  └─ Set Body Type = Dynamic
│  └─ Check "Freeze Rotation XYZ"  (prevent tipping)
└─ PlayerMovement script (this script)
```

**For OVRCameraRig**:
```
OVRCameraRig (the VR player)
├─ Add Rigidbody component
├─ Body Type: Dynamic
├─ Mass: 1
├─ Constraints: Freeze Rotation (X, Y, Z)
└─ Add PlayerMovement script
```

---

## 3. MetricsCollector.cs - Data Recording

**Location**: `Assets/Scripts/MetricsCollector.cs`  
**Purpose**: Record player position, speed, and trigger events in real-time

### Full Code Example

```csharp
using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class MetricSnapshot
{
    public float timestamp;
    public Vector3 position;
    public float speed;
    public bool isMoving;
    public string eventType;  // "trigger", "freeze", etc.
}

public class MetricsCollector : MonoBehaviour
{
    private List<MetricSnapshot> sessionData = new List<MetricSnapshot>();
    private Vector3 lastPosition;
    private DateTime sessionStartTime;
    public bool isRecording = false;

    void Start()
    {
        sessionStartTime = DateTime.Now;
        lastPosition = transform.position;
        isRecording = true;
    }

    void Update()
    {
        if (!isRecording) return;

        // Calculate velocity
        float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
        float speed = distanceThisFrame / Time.deltaTime;

        // Create snapshot
        MetricSnapshot snapshot = new MetricSnapshot()
        {
            timestamp = Time.time,
            position = transform.position,
            speed = speed,
            isMoving = speed > 0.1f
        };

        sessionData.Add(snapshot);
        lastPosition = transform.position;
    }

    public void RecordEvent(string eventName)
    {
        if (!isRecording) return;

        MetricSnapshot eventSnapshot = new MetricSnapshot()
        {
            timestamp = Time.time,
            position = transform.position,
            eventType = eventName
        };

        sessionData.Add(eventSnapshot);
        Debug.Log($"[Event] {eventName} at {Time.time:F1}s");
    }

    public void FinalizeSession()
    {
        isRecording = false;
        
        // Calculate summary statistics
        float totalTime = Time.time;
        float totalDistance = CalculateTotalDistance();
        int freezeEvents = CountFreezeEvents();

        Debug.Log($"Session Summary:");
        Debug.Log($"  Duration: {totalTime:F1}s");
        Debug.Log($"  Distance: {totalDistance:F2}m");
        Debug.Log($"  Freeze Events: {freezeEvents}");
    }

    private float CalculateTotalDistance()
    {
        float totalDistance = 0f;
        for (int i = 1; i < sessionData.Count; i++)
        {
            totalDistance += Vector3.Distance(
                sessionData[i].position,
                sessionData[i-1].position
            );
        }
        return totalDistance;
    }

    private int CountFreezeEvents()
    {
        int freezeCount = 0;
        for (int i = 0; i < sessionData.Count; i++)
        {
            if (sessionData[i].eventType == "freeze")
                freezeCount++;
        }
        return freezeCount;
    }

    public List<MetricSnapshot> GetSessionData()
    {
        return sessionData;
    }
}
```

### Line-by-Line Explanation

```csharp
[System.Serializable]
public class MetricSnapshot
```
**What**: Define data structure to hold one frame's metrics  
**[System.Serializable]**: Allow Unity to serialize (save) this class  
**Why**: Need structured way to store multiple data points

---

```csharp
public float timestamp;
public Vector3 position;
public float speed;
public bool isMoving;
public string eventType;
```
**What**: Data fields in the snapshot  
**Breakdown**:
- `timestamp`: When (seconds into scenario)
- `position`: Where (world X,Y,Z)
- `speed`: How fast (m/s)
- `isMoving`: Boolean if speed > threshold
- `eventType`: What happened ("trigger", "freeze", etc.)

**Example Snapshot**:
```json
{
  "timestamp": 5.2,
  "position": [0.5, 1.6, 8.2],
  "speed": 1.34,
  "isMoving": true,
  "eventType": null
}
```

---

```csharp
private List<MetricSnapshot> sessionData = new List<MetricSnapshot>();
```
**What**: List to store all snapshots during session  
**Why List**: Dynamic size, ordered collection  
**Private**: Only this script accesses it (safe)  
**Initialization**: Empty list `new List<>()`

**Grows Over Time**:
```
Frame 1: sessionData.Count = 1
Frame 2: sessionData.Count = 2
...
Frame 9000: sessionData.Count = 9000  (at 90 FPS = 100 seconds)
```

---

```csharp
void Start()
{
    sessionStartTime = DateTime.Now;
    lastPosition = transform.position;
    isRecording = true;
}
```
**What**: Initialize recording at scenario start  
**Breakdown**:
- `DateTime.Now` = Current system time (for timestamps)
- `lastPosition = transform.position` = Baseline position
- `isRecording = true` = Start collecting data

---

```csharp
void Update()
{
    if (!isRecording) return;
```
**What**: Check if should record this frame  
**Return Early**: If `!isRecording`, skip rest of method (no data collected)

---

```csharp
float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
```
**What**: Calculate how far player moved this frame  
**Breakdown**:
- `transform.position` = Current location
- `lastPosition` = Position from last frame
- `Vector3.Distance()` = Euclidean distance formula: √((x₂-x₁)² + (y₂-y₁)² + (z₂-z₁)²)

**Example**:
```
Current:  (1, 1.6, 10)
Last:     (1, 1.6, 8)
Distance: √((1-1)² + (1.6-1.6)² + (10-8)²) = √4 = 2 meters
```

---

```csharp
float speed = distanceThisFrame / Time.deltaTime;
```
**What**: Calculate speed in meters per second  
**Formula**: speed = distance / time  
**Breakdown**:
- `distanceThisFrame` = Meters moved
- `Time.deltaTime` = Seconds elapsed (typically 0.011 at 90 FPS)

**Example at 90 FPS**:
```
Distance: 0.023 meters (2.3 cm)
DeltaTime: 0.0111 seconds (1/90)
Speed: 0.023 / 0.0111 = 2.07 m/s
```

---

```csharp
MetricSnapshot snapshot = new MetricSnapshot()
{
    timestamp = Time.time,
    position = transform.position,
    speed = speed,
    isMoving = speed > 0.1f
};
```
**What**: Create snapshot object with current frame data  
**Breakdown**:
- `new MetricSnapshot()` = Create new instance
- `timestamp = Time.time` = Seconds since start
- `position = transform.position` = Current location
- `speed = speed` = Calculated speed (m/s)
- `isMoving = speed > 0.1f` = Is speed > 0.1 m/s?

**isMoving Logic**:
```
if speed > 0.1 m/s  → isMoving = true  (player is moving)
else                → isMoving = false (player is stationary/frozen)
```

---

```csharp
sessionData.Add(snapshot);
lastPosition = transform.position;
```
**What**: Store snapshot and update baseline  
**Breakdown**:
- `sessionData.Add()` = Add to list
- `lastPosition =` = Update for next frame comparison

---

```csharp
public void RecordEvent(string eventName)
{
    if (!isRecording) return;
    
    MetricSnapshot eventSnapshot = new MetricSnapshot()
    {
        timestamp = Time.time,
        position = transform.position,
        eventType = eventName
    };
    
    sessionData.Add(eventSnapshot);
}
```
**What**: Called when special event occurs (e.g., trigger hit)  
**Usage Example**:
```csharp
// In EnvironmentTriggers.cs:
metricsCollector.RecordEvent("doorway_entered");
metricsCollector.RecordEvent("freeze_detected");
```

**Output**:
```
[Event] doorway_entered at 8.5s
[Event] freeze_detected at 9.2s
```

---

```csharp
public void FinalizeSession()
{
    isRecording = false;
    
    // Calculate summary statistics
    float totalTime = Time.time;
    float totalDistance = CalculateTotalDistance();
    int freezeEvents = CountFreezeEvents();
```
**What**: Called when scenario ends  
**Breakdown**:
- Stop recording (no more data)
- Calculate summary stats
- Log results

---

```csharp
private float CalculateTotalDistance()
{
    float totalDistance = 0f;
    for (int i = 1; i < sessionData.Count; i++)
    {
        totalDistance += Vector3.Distance(
            sessionData[i].position,
            sessionData[i-1].position
        );
    }
    return totalDistance;
}
```
**What**: Sum up all distances between consecutive snapshots  
**Algorithm**:
```
totalDistance = 0
for each pair of consecutive points:
    distance = distance between point[i] and point[i-1]
    totalDistance += distance
return totalDistance
```

**Example**:
```
Points: [A, B, C, D]
Distances: A→B = 2m, B→C = 1m, C→D = 3m
Total: 2 + 1 + 3 = 6m
```

---

```csharp
private int CountFreezeEvents()
{
    int freezeCount = 0;
    for (int i = 0; i < sessionData.Count; i++)
    {
        if (sessionData[i].eventType == "freeze")
            freezeCount++;
    }
    return freezeCount;
}
```
**What**: Count how many "freeze" events were recorded  
**Algorithm**: Loop through all snapshots, count matches

---

## 4. ScenarioManager.cs - Scenario Logic

**Location**: `Assets/Scripts/ScenarioManager.cs`  
**Purpose**: Manage scenario state, waypoints, and completion

### Core Concept

```csharp
public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waypointTolerance = 0.5f;
    
    private int currentWaypoint = 0;
    private bool scenarioComplete = false;
    private MetricsCollector metricsCollector;

    void Start()
    {
        metricsCollector = GetComponent<MetricsCollector>();
    }

    void Update()
    {
        if (scenarioComplete) return;

        // Check if player near current waypoint
        float distanceToWaypoint = Vector3.Distance(
            transform.position,
            waypoints[currentWaypoint].position
        );

        if (distanceToWaypoint < waypointTolerance)
        {
            Debug.Log($"Reached waypoint {currentWaypoint}");
            currentWaypoint++;

            // Check if all waypoints completed
            if (currentWaypoint >= waypoints.Length)
            {
                CompleteScenario();
            }
        }
    }

    private void CompleteScenario()
    {
        scenarioComplete = true;
        metricsCollector.FinalizeSession();
        Debug.Log("Scenario Complete - Waiting for Y button to return to menu");
    }
}
```

**Key Logic**:
```
SCENARIO FLOW:

START
  ↓
Initialize waypoints [0, 1, 2, ...]
  ↓
Each frame:
  ├─ Calculate distance to waypoint[current]
  ├─ If distance < tolerance:
  │   └─ Advance to waypoint[current+1]
  └─ If current >= total waypoints:
     └─ CompleteScenario()
  ↓
ON COMPLETE:
  ├─ Finalize metrics
  └─ Wait for Y button input
```

---

## 5. EnvironmentTriggers.cs - Event Detection

**Location**: `Assets/Scripts/EnvironmentTriggers.cs`  
**Purpose**: Detect when player enters/exits trigger zones (doorways, obstacles)

### Core Concept

```csharp
public class EnvironmentTriggers : MonoBehaviour
{
    private MetricsCollector metricsCollector;
    private AudioSource audioSource;

    void Start()
    {
        metricsCollector = FindObjectOfType<MetricsCollector>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Entered trigger: {gameObject.name}");
            metricsCollector.RecordEvent("trigger_entered");
            audioSource.PlayOneShot(audioClip);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player still in trigger
            // Could check for freezing duration here
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Exited trigger: {gameObject.name}");
            metricsCollector.RecordEvent("trigger_exited");
        }
    }
}
```

**Physics Callbacks**:
```
COLLISION DETECTION:

Player (Rigidbody) ← collides with → TriggerZone (Collider, Is Trigger=true)

CALLBACK SEQUENCE:
├─ OnTriggerEnter() - First frame of overlap
├─ OnTriggerStay() - Every frame while overlapping
└─ OnTriggerExit() - First frame after separation

REQUIREMENTS:
├─ Player must have Rigidbody (Body Type = Dynamic)
├─ TriggerZone must have Collider (Is Trigger = ☑)
└─ Must have this script on TriggerZone
```

---

## 6. DataSerializer.cs - JSON Export

**Location**: `Assets/Scripts/DataSerializer.cs`  
**Purpose**: Convert metrics to JSON and save to file

### Core Concept

```csharp
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DataSerializer : MonoBehaviour
{
    public void SaveSessionToJSON(
        string scenarioName,
        List<MetricSnapshot> data,
        float totalTime
    )
    {
        // Calculate metrics
        float totalDistance = CalculateDistance(data);
        float avgSpeed = totalDistance / totalTime;

        // Create JSON object
        var result = new
        {
            scenario = scenarioName,
            timestamp = System.DateTime.Now.ToString("O"),
            duration_seconds = totalTime,
            metrics = new
            {
                total_distance = totalDistance,
                average_speed = avgSpeed,
                distance_unit = "meters",
                speed_unit = "m/s"
            },
            data_points = data.Count
        };

        // Serialize to JSON string
        string json = JsonUtility.ToJson(result, true);

        // Save to file
        string filename = $"Documents/FoG_Results/{scenarioName}_{System.DateTime.Now:yyyyMMdd_HHmmss}.json";
        File.WriteAllText(filename, json);
        
        Debug.Log($"Saved to: {filename}");
    }
}
```

**JSON Output**:
```json
{
  "scenario": "Freeze_of_Gait",
  "timestamp": "2026-07-22T14:30:00.1234567Z",
  "duration_seconds": 245.5,
  "metrics": {
    "total_distance": 8.5,
    "average_speed": 0.0347,
    "distance_unit": "meters",
    "speed_unit": "m/s"
  },
  "data_points": 2205
}
```

---

## 7. UIManager.cs - User Interface

**Location**: `Assets/Scripts/UIManager.cs`  
**Purpose**: Display timer, feedback, and instructions

### Core Concept

```csharp
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text timerDisplay;
    [SerializeField] private Text feedbackDisplay;
    [SerializeField] private Image feedbackPanel;
    
    private Color normalColor = Color.white;
    private Color warningColor = new Color(1, 0.5f, 0);  // Orange
    private Color errorColor = Color.red;

    void Update()
    {
        // Update timer display
        float timeElapsed = Time.time;
        timerDisplay.text = $"Time: {timeElapsed:F1}s";

        // Change color based on scenario state
        UpdateFeedback();
    }

    private void UpdateFeedback()
    {
        // This could check if player is freezing
        // and change panel color accordingly
        
        if (IsPlayerFreezing())
        {
            feedbackDisplay.text = "FREEZE DETECTED";
            feedbackPanel.color = Color.red;
        }
        else
        {
            feedbackDisplay.text = "Keep moving...";
            feedbackPanel.color = Color.green;
        }
    }

    private bool IsPlayerFreezing()
    {
        // Check with MetricsCollector if speed < threshold
        return false;  // Placeholder
    }
}
```

---

## Script Interaction Diagram

```
GAME LOOP (Every Frame at 90 FPS = 11.1ms)

1. Input Phase
   └─ OVRInput.Get() in PlayerMovement.cs
      └─ Read controller joystick
      └─ Emit movement

2. Physics Phase
   └─ Rigidbody applies velocity
   └─ Collision detection
      └─ OnTriggerEnter/Stay/Exit()
         └─ EnvironmentTriggers.cs calls metricsCollector.RecordEvent()

3. Logic Phase
   └─ ScenarioManager.cs
      └─ Check waypoint distance
      └─ Update scenario state

4. Metrics Phase
   └─ MetricsCollector.cs
      └─ Record position snapshot
      └─ Calculate speed
      └─ Add to sessionData

5. Rendering Phase
   └─ Unity renders to VR display

6. Output Phase (On Completion)
   └─ MetricsCollector.FinalizeSession()
   └─ DataSerializer.SaveSessionToJSON()
   └─ Wait for ReturnToMenu Y button input
```

---

## Common Patterns

### Pattern 1: Singleton Access

```csharp
// In any script, get access to MetricsCollector:
MetricsCollector metrics = FindObjectOfType<MetricsCollector>();
metrics.RecordEvent("some_event");
```

### Pattern 2: Component Reference

```csharp
// In Start(), cache component reference (faster than GetComponent each frame):
private Rigidbody rb;
void Start()
{
    rb = GetComponent<Rigidbody>();
}

void Update()
{
    rb.velocity = ...  // Now can use cached reference
}
```

### Pattern 3: Debug Logging

```csharp
// During development:
Debug.Log("Normal message");
Debug.LogWarning("Warning message");
Debug.LogError("Error message!");

// In production, these appear in:
// 1. Unity Console (editor)
// 2. Device logs (ADB)
// 3. Player.log (on headset)
```

---

## Best Practices

✅ **DO**:
- Keep Update() loops fast (<1ms)
- Cache component references in Start()
- Use private for encapsulation
- Document with comments (why, not what)
- Handle null references

❌ **DON'T**:
- Allocate memory in Update() (causes GC)
- Use GameObject.Find() in Update()
- Use GetComponent() in loops
- Leave Debug.Log() in production (spam)
- Modify transform.position directly (use Rigidbody instead)

---

**Last Updated**: 2026-07-22  
**Code Examples**: All runnable (stripped of some implementation details for clarity)  
**Version**: 1.0
