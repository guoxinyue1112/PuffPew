# PuffPew

Version: `0.1.0`

![PuffPew cover](Cover.png)

`PuffPew` is a bright, cute top-down auto-shooter made in Unity. Move through a colorful forest arena, survive escalating enemy waves, and build a powerful weapon combination while the combat system aims and attacks automatically.

## Overview

Step into a lively 2D battlefield where positioning and upgrade choices matter more than manual aiming. The player can focus on moving through danger while pistols, axes, and bombs automatically engage nearby enemies.

Each defeated enemy drops experience. Collect it to level up, choose new upgrades, and shape a run around fast ranged fire, wide melee swings, or explosive area damage. From wave 5 onward, massive slow enemies join the horde and turn the arena into a real survival challenge.

![PuffPew gameplay HUD](Docs/Images/gameplay-hud.png)

## Features

- Playable Unity 6 top-down survival shooter
- Automatic pistol, axe, and bomb combat
- Fixed full-arena camera for clear battlefield awareness
- Experience orbs that automatically pull toward the player
- Level-up weapon choices and progression
- Health pickups dropped by defeated enemies
- Escalating enemy counts, health, damage, and spawn rates
- Large enemies from wave 5 onward, with 3x scale and 10x base health
- Integrated background, character, weapon, UI, explosion, BGM, hurt, and kill assets

## Gameplay

The player begins in the forest arena with automatic weapons. Move to avoid the enemy swarm while attacks target nearby threats. Experience orbs are collected within a short radius, so combat remains fast and movement stays focused on survival.

After gaining enough experience, choose an upgrade to improve your current loadout. Complete waves to face increasing enemy pressure, including large enemies that arrive after the early game.

### Level-Up Choices

Every level presents three permanent upgrades. Choose between stronger attacks, faster attacks, more maximum health, defense, or movement speed to adapt your build to the current run.

![PuffPew level-up choice](Docs/Images/level-up-choice.png)

### Weapon Choices

Weapon selection adds new automatic attacks to the loadout. Choose between pistol, axe, and bomb options to combine rapid fire, wide melee hits, and explosive damage.

![PuffPew weapon choice](Docs/Images/weapon-choice.png)

## Controls

- `W A S D`: move
- Arrow keys: move when the legacy Unity input path is enabled
- Mouse: choose level-up and menu options

## Build And Run

### Unity Editor

1. Open the project in Unity `6000.5.10f1` or a compatible Unity 6 version.
2. Open `Assets/Scenes/GameScene.unity`.
3. Press `Play`.

The runtime bootstrap creates the core game systems, player, enemies, weapons, and HUD when the scene starts.

## Art Setup

The project includes an editor helper for imported art assets:

1. Open Unity.
2. Select `Tools > PuffPew > Setup Art Assets`.
3. The setup tool scans `Assets/Resources` and applies the configured artwork references.

The player sprite has separate `Player_Left` and `Player_Right` slices. Player movement changes the displayed sprite based on the most recent horizontal direction.

## Project Structure

- `Assets/Scenes/GameScene.unity`: main playable scene
- `Assets/Scripts/Core/`: runtime bootstrap, wave flow, art, and audio systems
- `Assets/Scripts/Enemy/`: enemy behaviour, spawning, and management
- `Assets/Scripts/Pickups/`: experience orbs and health pickups
- `Assets/Scripts/Player/`: movement, health, experience, and sprite direction
- `Assets/Scripts/UI/`: HUD, level-up, end-game, and floating text
- `Assets/Scripts/Weapons/`: pistol, axe, bomb, projectiles, and effects
- `Assets/Resources/`: game art and UI assets
- `Assets/Audio/`: BGM, hurt, and kill sounds
- `Assets/Editor/`: art setup and editor preview helpers

## Release Notes

`0.1.0` is the first presentation-ready gameplay release. It adds the complete placeholder art pass, audio integration, automatic experience collection, health pickup drops, large enemies, wave scaling, and the first project documentation.

Current limitations:

- The HUD is still being refined for final production layout.
- Gameplay balancing is tuned for an MVP and will continue to evolve.
- Imported third-party art and audio should retain their original license information before a public commercial release.
