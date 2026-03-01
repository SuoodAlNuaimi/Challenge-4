# Soccer Challenge 4 – Unity Project

## Table of Contents
- [Project Overview](#project-overview)
- [Game Features](#game-features)
- [Core Systems](#core-systems)
- [Difficulty System](#difficulty-system)
- [Power-Up System](#power-up-system)
- [Wave System](#wave-system)
- [UI System](#ui-system)
- [Audio System](#audio-system)
- [Code Architecture](#code-architecture)
- [Version Control Practices](#version-control-practices)
- [Branch Structure](#branch-structure)
- [Major Commits](#major-commits)
- [Conflict Resolution](#conflict-resolution)
- [Team Members and Roles](#team-members-and-roles)
- [How to Run](#how-to-run)

---

## Project Overview

Soccer Challenge 4 is a 3D physics-based arena game developed in Unity.  
The player controls a rolling soccer ball and competes against AI-controlled enemy balls.  
The objective is to score more goals than the opponent before the match timer ends.

This project demonstrates:

- AI behavior systems  
- Power-up mechanics  
- Difficulty scaling  
- UI architecture  
- Audio management  
- Game state management  
- Structured Git workflow  

---

## Game Features

- Physics-based rolling player movement  
- AI-driven enemy balls with state logic  
- Wave-based spawning system  
- Three selectable difficulty modes  
- Configurable match timer  
- Multiple power-ups  
- Freeze mechanic  
- Smash mechanic  
- Score tracking system  
- Music and SFX toggle system  
- Modular UI panel management  

---

## Core Systems

### Enemy AI

State-based AI implementation:

- Attack  
- Intercept  
- Score  

Key features:

- Predictive interception using player velocity  
- Steering offset for natural movement  
- Difficulty-driven reaction rate  
- Wave-based speed scaling  
- Freeze support  

---

## Difficulty System

Three difficulty levels dynamically modify gameplay balance.

| Difficulty | AI Reaction | AI Max Speed | Player Strength | Power Duration | Extra Enemy | Interception Strength |
|------------|------------|--------------|------------------|----------------|-------------|------------------------|
| Easy       | Slow       | Low          | Strong           | Long           | No          | Weak                   |
| Medium     | Balanced   | Medium       | Balanced         | Normal         | No          | Medium                 |
| Hard       | Fast       | High         | Reduced          | Short          | Yes         | Strong                 |

### Systems Affected by Difficulty

| Category | Parameters Modified |
|----------|---------------------|
| Player   | Movement Force, Turbo Force, Power Duration, Smash Radius Multiplier |
| Enemy    | Speed Multiplier, Max Speed, Decision Rate, Intercept Prediction |
| Spawn    | Extra Enemy Per Wave |

---

## Power-Up System

| Power-Up | Effect | Duration | Special Behavior | UI Integration |
|----------|--------|----------|------------------|----------------|
| Normal Power-Up | Increases hit force and push strength | Configurable | Enhances collision impulse | Slider + Status Text |
| Smash Power-Up | Jump and slam area attack | Configurable | Explosion force based on distance | Slider + Shield Visual + Hint |
| Freeze Power-Up | Freezes all active enemies | 3 Seconds | Stops AI logic and Rigidbody movement | Freeze Hint Display |

---

## Wave System

- Automatically spawns new wave when enemies are cleared  
- Enemy count increases per wave  
- Hard mode adds extra enemy per wave  
- Player reset on wave start  
- Animated new wave UI notification  

---

## UI System

Panels implemented:

- Main Menu  
- Difficulty Selection  
- In-Game HUD  
- Help Panel  
- Settings Panel  
- Game Over Panel  

HUD Elements:

- Match timer  
- Player score  
- Enemy score  
- Power-up sliders  
- Power-up status indicators  
- Smash hint  
- Freeze hint  
- Wave notification popup  

---

## Audio System

- Background music with loop  
- PlayOneShot SFX system  
- PlayerPrefs-based toggle persistence  
- Singleton audio manager  
- Music and SFX toggle buttons  
- Runtime state synchronization  

---

## Code Architecture

### Main Scripts

- SpawnManagerX.cs  
- PlayerControllerX.cs  
- EnemyX.cs  
- UIManager.cs  
- GameTimer.cs  
- SoundsController.cs  
- DifficultyConfigurator.cs  
- GameSettings.cs  
- RotateCameraX.cs  

### Architectural Principles

- Inspector-driven configurable values  
- Centralized difficulty configuration  
- Reusable helper methods  
- Reduced hard-coded values  
- Coroutine-based timed mechanics  
- Clean separation of responsibilities  
- Refactored for readability and maintainability  

---

## Version Control Practices

This project follows structured GitHub workflow:

- Feature-based branching  
- Frequent descriptive commits  
- Pull requests before merging  
- Documented conflict resolution  
- Stable master branch  

---

## Branch Structure

Example feature branches:

- feature/enemy-ai  
- feature/ui-improvements  
- feature/difficulty-system  
- feature/powerups  
- feature/audio-system  

---

## Major Commits

Key development milestones:

- Implemented enemy AI states (Attack, Intercept, Score)  
- Added enemy behavior personalities  
- Implemented smash power-up logic  
- Added smash duration and particle effects  
- Implemented freeze power-up system  
- Designed difficulty selection UI  
- Implemented difficulty impact system  
- Integrated UI manager and game timer  
- Added wave counter animation and game end popup  
- Implemented settings UI and audio controller  
- Finalized music and SFX system  
- Refactored core classes for readability and inspector-driven configuration  

---

## Conflict Resolution

Conflicts were resolved by:

- Separating UI prefabs to minimize scene conflicts  
- Isolating feature changes in independent scripts  
- Centralizing configuration in GameSettings  
- Reviewing merge differences before integration  
- Controlled merging of feature branches into master  

Example merge:

Merge branch 'master' into feature/ui-improvements  

---

## Team Members and Roles

| GitHub Username | Full Name | Responsibilities |
|-----------------|-----------|------------------|
| b6ybb | Buti Khalfan | Enemy AI states, Behavior logic, Interception system |
| Victory | Abdulla AlZaabi | Smash power-up, Freeze mechanic, Power-up systems |
| rashed | Rashed AlSereidi | Difficulty system, Gameplay tuning, Code refactoring |
| SuoodAlNuaimi | Suood AlNuaimi | UI system, Audio integration, Settings system, Wave UI, Scene management, Final integration |

---

## How to Run

1. Open the project in Unity (2022 or newer recommended).  
2. Open the main scene.  
3. Press Play.  
4. Select difficulty and match duration.  
5. Start the game.

## Link
https://github.com/SuoodAlNuaimi/Challenge-4.git 
