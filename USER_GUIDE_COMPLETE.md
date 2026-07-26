# FoG VR Project - Complete User Guide for Non-Technical Users
## How to Use the System from Setup to Data Collection

---

## Table of Contents

1. Purpose of This Guide
2. Before You Start - Preparation
3. VR Project Setup (Step-by-Step)
4. Understanding the VR Headset
5. Understanding the Scenarios
6. Running Your First Scenario
7. Data Collection Guide
8. Troubleshooting
9. Quick Reference Checklist
10. FAQ - Frequently Asked Questions

---

## 1. Purpose of This Guide

This guide is written **for people who:**
- Have never used VR before
- Don't understand coding or technical details
- Just want to know: "How do I use this system?"

This guide answers:
- "What equipment do I need?"
- "How do I set it up?"
- "How do I run a scenario?"
- "What happens to my data?"

**You do NOT need to know:**
- C# programming
- How Unity works internally
- System architecture details

---

## 2. Before You Start - Preparation

### What You Need

**Equipment**:
- [ ] **Meta Quest 3 Headset** (the VR device you wear on your head)
- [ ] **USB-C Cable** (connects headset to PC)
- [ ] **Windows PC or Laptop** (runs the VR application)
- [ ] **Comfortable Shoes** (for walking scenarios)

**Software** (will be installed for you):
- [ ] **Meta Horizon App** (controls headset settings)
- [ ] **Unity (already installed)**
- [ ] **FoG Application** (the VR program)

### Physical Space

**Safe area needed**:
- **Size**: 3m × 3m minimum (about 10ft × 10ft)
- **Obstacles**: Clear of furniture, pets, people
- **Floor**: Non-slippery, level surface
- **Lighting**: Bright enough to see (no dark rooms)
- **Ceiling**: 2.4m minimum height (8 feet)

**Visual**: Imagine a square dance floor. You need that much clear space.

### Time Required

- **First Time Setup**: 30-45 minutes
- **Running Scenario**: 15-30 minutes per session
- **Total**: First session = 1 hour

---

## 3. VR Project Setup (Step-by-Step)

### Step 1: Charge Your Headset (20 minutes)

**What to do**:
1. Find the Meta Quest 3 charger
2. Plug into headset (USB-C port on bottom)
3. Let it charge fully (takes 2-3 hours)
4. Indicator light: Red = charging, Green = charged

**Why**: Battery must be full for testing. VR uses lots of power.

**Check**: Battery icon on headset shows percentage

---

### Step 2: Install Meta Horizon App on PC (10 minutes)

**What to do**:
1. Go to: https://www.meta.com/en/quest/setup/
2. Click "Download Meta Horizon App"
3. Run installer (FamilySetup.exe or similar)
4. Follow on-screen instructions
5. Create Meta account (or use existing)
6. Wait for installation (~500 MB)

**When Done**: Application appears on desktop

---

### Step 3: Connect Headset with USB Cable (5 minutes)

**What to do**:
1. Plug USB-C cable into headset (bottom)
2. Plug USB end into PC USB port
3. Put on headset
4. A popup appears asking "Allow USB access?"
5. Select "Allow" or "Always Allow"
6. Remove headset

**What you'll see**: 
- Windows shows new device connected
- Meta Horizon app recognizes headset

**If not working**: Try different USB port on PC

---

### Step 4: Enable Developer Mode (5 minutes)

**Why needed**: Allows custom VR apps to run

**What to do**:
1. Open Meta Horizon app on PC
2. Look for headset name in device list
3. Click headset → Settings
4. Find "Developer Mode"
5. Toggle to ON (blue)

**You should see**: Confirmation that Developer Mode is enabled

---

### Step 5: Launch Quest Link (5 minutes)

**What is Quest Link**: Wireless connection from PC to headset

**What to do**:
1. Ensure USB cable still connected
2. Open Meta Horizon app
3. Click "Quest Link" button
4. Wait for connection (takes 10-15 seconds)
5. Status shows: "Connected" (green indicator)

**Put on headset**: Should see PC desktop on screen

---

### Step 6: Open FoG Application (10 minutes)

**What to do**:
1. Connect to PC via USB/Quest Link (from Step 5)
2. Windows → Start Unity Editor
3. Open FoG_Revised project
4. Wait for project to load (2-3 minutes first time)
5. You'll see Unity editor interface

**When loaded**: You're ready to run scenarios!

---

## 4. Understanding the VR Headset

### What is a VR Headset?

A VR (Virtual Reality) headset is:
- A screen you wear on your face
- Shows you a 3D virtual world
- Tracks your head movements
- Includes controllers in your hands

**Think of it like**: A 3D movie you can look around and interact with

### Meta Quest 3 - Key Features

```
WHAT YOU WEAR:
├─ Headset (main device on face)
│  ├─ Screens inside (shows VR)
│  ├─ Cameras (track movement)
│  ├─ Speakers (audio)
│  └─ Power button + volume buttons
│
├─ Left Controller (hand device)
│  ├─ Buttons: X, Y
│  ├─ Analog stick (thumbstick)
│  ├─ Triggers
│  └─ Grip buttons
│
└─ Right Controller (hand device)
   ├─ Buttons: A, B
   ├─ Analog stick
   ├─ Triggers
   └─ Grip buttons

SAFETY FEATURES:
├─ Guardian: Virtual boundary keeps you safe
├─ Pass-Through: See real world if needed
└─ Motion Sickness Detection: Warns if uncomfortable
```

### Basic Controls

**For Walking Scenarios**:
- **Analog Stick**: Move forward/backward/sideways
  - Push UP = walk forward
  - Push DOWN = walk backward
  - Push LEFT/RIGHT = strafe (sideways)
  - Magnitude = speed

- **Y Button** (left controller): **RETURN TO MENU**
  - Press at ANY time to go back to menu
  - Scenario stops and saves data
  - Always available

- **Head Movement**: Look around naturally
  - Turn head left/right
  - Look up/down
  - Tilt head side-to-side

**What You DON'T Do**:
- ❌ Don't reach outside your play area (you'll hit real walls)
- ❌ Don't run (walking is faster than you can run)
- ❌ Don't remove headset mid-scenario (data might not save)

---

## 5. Understanding the Scenarios

### What is a Scenario?

A **scenario** is:
- A virtual environment you walk through
- Designed to evaluate your walking ability
- Automatically records your performance
- Takes 10-20 minutes to complete

**Think of it like**: A medical test, but in VR

### The 6 Scenarios Explained

#### **1. Menu (Starting Point)**
**Purpose**: Choose which scenario to run  
**What you do**: Look at buttons, press Y or A to select  
**Duration**: <1 minute  
**Difficulty**: None (just selection)

---

#### **2. BasicScene (Warmup)**
**Purpose**: Get comfortable with VR and controls  
**Environment**: Simple apartment room  
**What you do**: Walk around freely, get used to headset  
**Duration**: 5 minutes  
**Difficulty**: ⭐ Very Easy

**Good for**:
- First time using VR
- Testing if headset works
- Warming up before main test

---

#### **3. Freeze of Gait (Main Test)**
**Purpose**: Evaluate your walking and response to obstacles  
**Environment**: Apartment with walking path  
**What you do**: 
1. Start at beginning of path
2. Walk straight ahead (8-10 meters)
3. Path shows where to go
4. At midpoint, special signal happens (beep or light)
5. Continue walking to end
6. Data automatically collected

**Duration**: 10-15 minutes  
**Difficulty**: ⭐⭐⭐ Medium

**What gets measured**:
- How fast you walk
- How smooth your movements are
- How you react to signals
- Any pauses or hesitations

---

#### **4. 6m/3m Variant**
**Purpose**: Test walking at different distances  
**Difference from #3**: Breaks path into two sections
- First 3 meters: Short distance test
- Then 6 meters: Longer distance test

**Duration**: 12-15 minutes  
**Difficulty**: ⭐⭐⭐ Medium

**Good for**: Understanding how distance affects your performance

---

#### **5. Doorway Variant**
**Purpose**: Test how you handle doorways  
**Environment**: Same as #3, but with doorway  
**What changes**: 
- Doorway appears mid-path
- Door is visible and realistic
- You must pass through doorway
- System measures how you handle it

**Duration**: 10-12 minutes  
**Difficulty**: ⭐⭐⭐⭐ Medium-Hard

**Why this matters**: Doorways are common places where people with Parkinson's have trouble walking

---

#### **6. Closed Door Variant**
**Purpose**: Test your response to unexpected obstacles  
**Environment**: Same as #3, but door is CLOSED  
**What happens**:
1. You approach doorway
2. Door is closed (blocked)
3. Waits 1-3 seconds
4. Door opens automatically
5. You pass through

**Duration**: 10-12 minutes  
**Difficulty**: ⭐⭐⭐⭐ Hard

**Why this matters**: Tests how you handle surprises and stress

---

### How to Choose Which Scenario

| Your Situation | Choose |
|---|---|
| First time ever | BasicScene |
| Want standard test | Freeze_of_Gait |
| Want longer challenge | 6m/3m variant |
| Have doorway problems | Doorway or Closed_Door |
| Want complete evaluation | Do all of them in order |

---

## 6. Running Your First Scenario

### Pre-Scenario Checklist

**Before putting on headset**:
- [ ] Play area is clear
- [ ] No people within 3 meters
- [ ] No obstacles on ground
- [ ] Wearing flat, non-slippery shoes
- [ ] Felt comfortable in BasicScene first

**Equipment check**:
- [ ] Headset charged (battery > 50%)
- [ ] Controllers batteries fresh
- [ ] USB cable connected
- [ ] Quest Link shows "Connected"

---

### Start Scenario (Step-by-Step)

**Step 1: Put on Headset** (2 min)
1. Put on headset and fasten straps
2. Adjust so screen is clear (not blurry)
3. Adjust head strap (should be snug, not tight)
4. You should see menu or scenario screen

**Step 2: Read Instructions on Screen** (1 min)
1. Virtual text appears explaining scenario
2. Read what you need to do
3. Ask questions now before starting
4. Press button to confirm ready

**Step 3: Position at Start** (1 min)
1. You'll see virtual "start" marker
2. Walk to that location in VR
3. Stop when aligned with marker
4. Look straight ahead

**Step 4: Scenario Begins** (auto-start usually)
- Recording starts automatically
- Text may say "Go" or "Begin"
- Start walking along the path

**Step 5: During Scenario** (10-20 min)
```
DO:
✅ Walk naturally at comfortable pace
✅ Look where you're going
✅ React normally to any signals
✅ Walk straight along path
✅ Complete full path to end

DON'T:
❌ Stop suddenly unless told
❌ Run (too fast)
❌ Look only at feet (look ahead)
❌ Remove headset
❌ Leave play area
```

**Step 6: Scenario Ends**
- You reach end point
- System shows "Complete!" message
- Can see your results on screen
- Data saved automatically

**Step 7: Return to Menu**
- Press **Y button** on left controller
- Scene fades and menu returns
- Headset ready for next scenario

---

### What Happens to Your Data

**During Scenario**:
- System records every movement
- Saves position every 1/90th of a second
- Records when you hesitate
- Records your speed
- Records if anything special happens (doorway, etc.)

**When Scenario Ends**:
- All data automatically compiled
- Saved to computer as file
- Can be analyzed by therapist
- Shows your performance over time

**Your Data is Safe**:
- Stays on your computer
- Only you/therapist can see
- Encrypted if on network
- Deleted when you delete it

---

## 7. Data Collection Guide

### Understanding Your Results

**After each scenario, you'll see metrics**:

```
TIME TAKEN: 245 seconds (4 minutes 5 seconds)
├─ How long it took you to complete the path

DISTANCE WALKED: 8.5 meters
├─ Total distance you actually traveled

SPEED: 0.035 meters per second
├─ Average walking speed
├─ Typical healthy speed: 1.2-1.4 m/s
├─ Slower is okay, not a problem

FREEZE EVENTS: 2 times
├─ Number of times you paused/froze
├─ Lower is better

EFFICIENCY: 92%
├─ How straight was your path (0-100%)
├─ 100% = perfectly straight
└─ Less means more wandering
```

### Comparing Over Time

**Week 1 Test**:
- Time: 250 sec
- Speed: 0.033 m/s
- Freezes: 3

**Week 2 Test** (after therapy):
- Time: 240 sec ← Improvement! (10 sec faster)
- Speed: 0.035 m/s ← Improvement! (faster)
- Freezes: 2 ← Improvement! (fewer freezes)

**What This Means**: Therapy is working!

### Data Export for Analysis

**To get data to therapist**:
1. Find file on computer
   - Folder: `Documents/FoG_Results/`
2. Right-click → Send to → Email
   - Or attach to email manually
3. Therapist analyzes results
4. Discusses with you

**Data files are JSON** (computer-readable format):
```json
{
  "scenario": "Freeze_of_Gait",
  "duration_seconds": 245,
  "metrics": {
    "total_distance": 8.5,
    "average_speed": 0.035,
    "freeze_events": 2
  }
}
```

---

## 8. Troubleshooting

### Problem: "Headset won't connect to PC"

**You'll see**: USB error or Meta Horizon says "Device not found"

**Solutions** (try in order):
1. **Check cable**: Is USB-C cable plugged into both PC and headset?
2. **Try different port**: Unplug and plug into different USB port on PC
3. **Restart both**: 
   - Turn off headset (power button)
   - Restart PC
   - Turn on headset
4. **Update drivers**: Windows might need updates (rare)
5. **Call technical support** if still not working

---

### Problem: "Screen is blurry in VR"

**You'll see**: Can't read text clearly in headset

**Solutions**:
1. **Adjust headset position**:
   - Move up/down on face
   - Try different position until clear
   
2. **Adjust lens focus** (some headsets have this):
   - Small dial on headset side
   - Rotate until picture clears
   
3. **Check for smudges**: Clean lenses with microfiber cloth (included)

4. **Check vision**:
   - Sometimes people need glasses in VR too
   - Can wear glasses under headset or use prescription inserts

---

### Problem: "Feel motion sickness or dizziness"

**You'll feel**: Nauseous, dizzy, off-balance

**Immediate solutions**:
1. **Stop immediately** - remove headset
2. **Sit down** - find chair
3. **Look at fixed point** - focus on one object
4. **Breathe slowly** - calm breathing
5. **Wait** - usually passes in 5-10 minutes

**Prevention next time**:
1. **Start with BasicScene** - shorter, easier
2. **Don't look down** - look where you're going
3. **Walk slowly** - don't rush
4. **Take breaks** - don't go longer than 15 min first time
5. **Eat light snack first** - but not right before

---

### Problem: "Controller buttons don't work"

**You'll notice**: Y button doesn't do anything, analog stick doesn't move player

**Solutions**:
1. **Check batteries**: Are controller batteries dead?
   - Try new batteries
   
2. **Re-pair controllers**:
   - On headset: Settings → Devices → Pair New Controller
   - Physically press pairing button on controller (usually top)
   
3. **Restart headset**:
   - Power off completely
   - Wait 10 seconds
   - Power back on
   
4. **Test in Meta home menu**:
   - Close VR app
   - Try controlling home screen with controllers
   - If works there, problem is in app

---

### Problem: "Data didn't save"

**You'll notice**: Finished scenario but can't find results file

**Solutions**:
1. **Check folder**: 
   - Open Windows File Explorer
   - Go to: Documents → FoG_Results
   - Look for file with today's date

2. **Look for error message**:
   - When scenario ended, was there an error?
   - Write down exactly what it said
   
3. **Run scenario again**:
   - This time watch for any warnings
   - Report to technical support with details

---

## 9. Quick Reference Checklist

### Before Each Session

**Pre-Test (5 minutes)**:
- [ ] Headset charged (>50% battery)
- [ ] Play area clear (3m × 3m)
- [ ] Wearing comfortable shoes
- [ ] Calm state (no stress)
- [ ] Bathroom break taken

**Equipment Check**:
- [ ] USB cable connected
- [ ] Meta Horizon shows "Connected"
- [ ] Headset powers on correctly
- [ ] Controllers have batteries
- [ ] No visual/audio issues

**Mental Preparation**:
- [ ] Read scenario instructions
- [ ] Know what to expect
- [ ] No time pressure
- [ ] Willing to complete test

### During Scenario

- [ ] Move at comfortable pace
- [ ] Follow the path (virtual lines/markers)
- [ ] Walk straight (don't wander)
- [ ] Look ahead (not at feet)
- [ ] React naturally
- [ ] Complete full path

### After Scenario

- [ ] Results displayed on screen
- [ ] Write down key metrics
- [ ] Remove headset carefully
- [ ] Rest for a minute
- [ ] Drink water
- [ ] Note how you felt

---

## 10. FAQ - Frequently Asked Questions

### Q: "Do I need to be in perfect health to use this?"
**A**: No! The test is designed for people with various abilities. It measures YOUR performance, not against anyone else. Go at your natural pace.

---

### Q: "What if I fall or trip?"
**A**: Very unlikely because:
1. You're in a safe, clear area
2. Therapist is watching you
3. Virtual path is easy to follow
4. Speed is controlled

But if you feel unsteady: **Stop immediately and press Y button to exit.**

---

### Q: "Can I pause the scenario?"
**A**: Not officially, but pressing **Y button** ends the scenario at any time:
- Data up to that point is saved
- You can start fresh next time
- No penalty for early stopping

---

### Q: "Why does the therapist have me do the same scenario multiple times?"
**A**: To track progress:
- Week 1 = Baseline (your starting point)
- Week 3 = Mid-point (are you improving?)
- Week 5 = End-point (did therapy work?)

Comparison over time is more meaningful than single test.

---

### Q: "What if I'm too nervous to do VR?"
**A**: That's normal! Solutions:
1. **Start with BasicScene** - just walking around
2. **Do it with therapist** in the room - watch what they do
3. **Ask questions first** - therapist can explain
4. **Start slow** - you set the pace, not a timer

---

### Q: "Can someone help me if I'm stuck?"
**A**: Yes!
- Therapist watches during test
- Can guide you verbally
- Can pause/restart if needed
- Safety is priority #1

---

### Q: "What happens to my data if I do it wrong?"
**A**: Data is still useful:
- Therapist knows you were nervous
- Results still show how YOU moved
- Even "imperfect" tests give information
- They compare to previous tests (YOUR progression, not perfection)

---

### Q: "Does my brain need to be trained to use VR?"
**A**: No:
- Your eyes adjust in 1-2 minutes
- Balance system works normally
- Natural movement (no special skills needed)
- If you can walk, you can do VR

---

### Q: "Is there radiation or other danger?"
**A**: No:
- VR just uses screens (like phone)
- Headset is made of safe materials
- No radiation
- Only risk: motion sickness (temporary)

---

### Q: "Can I do this at home?"
**A**: Ask your therapist:
- Technically possible (just need PC + headset)
- Therapist may want to observe
- Safety matters
- Data analysis is therapist's job
- Check with facility rules

---

### Q: "What if I can't see the VR clearly?"
**A**: Common! Solutions:
1. Adjust headset on face
2. Clean lenses
3. Wear glasses under headset
4. Ask about prescription inserts
5. Lighting matters - try different room

---

## Conclusion

You now have all the information needed to:
1. ✅ Set up the VR system
2. ✅ Understand the equipment
3. ✅ Choose appropriate scenarios
4. ✅ Run scenarios safely
5. ✅ Understand your results
6. ✅ Troubleshoot common problems

**Remember**: VR is a tool for evaluation and therapy. It's not a game or entertainment - it's a medical tool. Therapist guidance is important.

---

## Emergency Contact

If you have problems:
1. **Technical issue**: Contact [IT Support Email]
2. **Medical concern**: Tell therapist immediately
3. **Safety issue**: Stop immediately and remove headset

---

## Glossary

**VR (Virtual Reality)**: Computer-generated 3D world you can walk through

**Headset**: Device you wear on head (shows the VR)

**Controller**: Handheld device in each hand (input device)

**Scenario**: A test/walking path in VR

**Metrics**: Measurements of your performance (time, distance, speed)

**Quest Link**: Connection from PC to headset

**Developer Mode**: Setting that allows custom VR apps to run

**Guardian**: Invisible boundary that keeps you safe

---

**Last Updated**: 2026-07-22  
**Intended For**: Patients, therapists, care staff  
**Reading Time**: 15-20 minutes  
**Skill Level Required**: None - written for non-technical people
