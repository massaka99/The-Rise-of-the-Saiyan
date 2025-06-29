# The Rise of the Saiyan 🐉

A 2D top-down RPG game developed in Unity, inspired by the Dragon Ball manga series.

## 🎮 Game Overview

**The Rise of the Saiyan** is an action-packed RPG where players embark on an epic journey through multiple levels, completing quests and battling iconic villains from the Dragon Ball universe. Control Goku as he fights through hordes of enemies and faces legendary opponents in an immersive top-down adventure.

## ✨ Features

### Core Gameplay

- **2D Top-Down Perspective**: Classic RPG-style movement and exploration
- **Dynamic Combat System**: Melee attacks and special abilities including the iconic Kamehameha wave
- **Quest System**: Multi-layered quest progression with NPC interactions
- **Multiple Levels**: Journey through various environments and challenges
- **Boss Battles**: Face off against legendary Dragon Ball villains

### Combat System

- **Melee Combat**: Close-range punch attacks with sound effects
- **Special Attacks**: Kamehameha energy blasts with extended range
- **Enemy AI**: Multiple AI behaviors including random movement, player awareness, and combat states
- **Health System**: Floating health bars for enemies and damage mechanics
- **Audio Integration**: Combat sound effects and environmental audio

### Quest & Progression

- **Multi-Level Quest System**: Progressive storyline across multiple areas
- **NPC Interactions**: Dialog system with characters like Chi-Chi, Gohan, and Beerus
- **Kill Quests**: Eliminate specific numbers of enemies (Saibamen) to progress
- **Boss Progression**: Sequential boss battles (Frieza → Cell → Buu → Vegeta → Beerus)
- **Level Gating**: Progression locked behind quest completion

## 🎯 Game Structure

### Levels & Scenes

1. **Main Menu** - Game start and navigation
2. **Intro** - Opening cutscene
3. **Level 1** - Initial area with Chi-Chi's Saibamen quest (10 kills required)
4. **Level 2** - Advanced area with expanded Saibamen quest (20 kills required)
5. **Level 3** - Boss battle arena
6. **Level 4** - Final challenges
7. **Outro** - Ending sequence

### Key Characters & NPCs

- **Player Character**: Goku with full movement, combat, and interaction abilities
- **Chi-Chi**: Quest giver for Saibamen elimination missions
- **Gohan**: Advanced quest coordinator and story progression
- **Beerus**: Final boss encounter
- **Vegeta**: Major boss opponent
- **Frieza, Cell, Buu**: Sequential boss battles in Level 2

### Enemy Types

- **Saibamen**: Basic enemies for kill quests
- **Boss Enemies**: Scripted AI with advanced combat patterns
- **Standard Enemies**: Random movement patterns with player awareness

## 🛠️ Technical Details

### Built With

- **Unity 2022.3.44f1** - Game engine
- **C# Scripting** - Game logic and systems
- **2D Physics** - Movement and collision detection
- **Audio System** - Sound effects and background music

### Key Systems

- **Player Controller**: Movement, running, interaction, and audio
- **Quest Manager**: Centralized quest state management
- **Dialog System**: NPC conversation management
- **Audio Manager**: Sound effect and music coordination
- **Teleport System**: Level transitions and area management
- **Combat System**: Attack mechanics and damage dealing

### Architecture

- **Singleton Pattern**: Used for QuestManager and Dialog_Manager
- **Event System**: Enemy death notifications and quest updates
- **State Machine**: Game state management (FreeRoam, Dialog, Battle)
- **Component-Based**: Modular enemy AI and player systems

## 🎵 Audio Features

Rich audio experience with:

- Combat sound effects (punches, Kamehameha, damage)
- Character-specific audio (Goku sounds, boss audio)
- Environmental music for different levels
- Teleportation and interaction sound effects

## 🎬 Visual Elements

- **Sprite-based Graphics**: 2D character and environment sprites
- **Animation System**: Character movement and combat animations
- **UI Elements**: Health bars, dialog boxes, and menus
- **Tile-based Environments**: Level design using Unity's tilemap system

## 🚀 Getting Started

### Prerequisites

- Unity 2022.3.44f1 or later
- Windows, Mac, or Linux system

### Installation

#### Option 1: Play the Built Game

1. Navigate to the `Executable` folder which contains the pre-built Unity executable
2. Run `TheRiseOfTheSaiyan.exe` to start playing immediately
3. No Unity installation required for this option

#### Option 2: Development Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/[username]/The-Rise-of-the-Saiyan.git
   ```
2. Open the project in Unity
3. Load the `Main Menu` scene to start
4. Build and run the project

### Controls

- **WASD/Arrow Keys**: Movement
- **Left Shift**: Run (while moving)
- **Space**: Melee attack
- **Q**: Kamehameha special attack
- **E**: Interact with NPCs and objects

## 🎯 Game Progression

### Quest Flow

1. **Level 1**: Complete Chi-Chi's Saibamen quest (10 kills)
2. **Gohan Interaction**: Unlock Vegeta quest
3. **Level 2**: Complete advanced Saibamen quest (20 kills)
4. **Boss Sequence**: Defeat Frieza → Cell → Buu in order
5. **Final Battle**: Face Vegeta and ultimately Beerus

### Unlocking System

- Areas locked behind quest completion
- Boss fights require previous quest completion
- Dialog system provides hints and story progression

## 🏗️ Project Structure

```
Assets/
├── _GAME_/
│   ├── Art/          # Sprites, animations, tiles
│   ├── Audio/        # Sound effects and music
│   ├── Enemy/        # Enemy scripts and prefabs
│   ├── Player/       # Player controller and systems
│   ├── NPC/          # Non-player character scripts
│   ├── Scenes/       # Game levels and menus
│   ├── Scripts/      # Core game systems
│   └── Dialog/       # Conversation system
```

## 🎮 Development Status

**Current Status**: Finished
**Platform**: Windows

## 📄 License

This project is inspired by Dragon Ball manga and is intended for educational/personal use.

---

_Experience the power of the Saiyans in this epic 2D adventure! Transform into the legendary Super Saiyan and save the universe from ultimate evil._
