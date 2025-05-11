# CaptureTheGun

## Overview
CaptureTheGun is a single-player 3D puzzle game with a third-person perspective. Players take on the role of a detective who must locate a hidden gun to apprehend a criminal. However, there's a twist - you're trapped in a room with a dangerous criminal who appears to be your "good friend." You'll need to solve various CTF (Capture The Flag) style puzzles to find the gun left behind by your detective partner while avoiding suspicion from the criminal.

## Gameplay Features

### Core Mechanics
- **Third-person exploration**: Navigate through detailed environments while solving puzzles
- **CTF-style puzzles**: Decode clues and find hidden "flags" that reveal the gun's location
- **Suspicion meter**: Be careful not to raise the criminal's suspicion or face deadly consequences
- **Hidden tools**: Discover and use various tools throughout the environment

### Puzzle Elements
- Various puzzle types inspired by CTF challenges:
  - Cryptographic puzzles (base64 encoding/decoding)
  - Hidden visual clues in environmental objects (paintings, photographs)
  - Coordinates and geographical puzzles
  - Pattern recognition challenges
  - 3D environmental puzzles

### Suspicion System
Players must balance puzzle-solving with maintaining a low suspicion level. If the suspicion meter reaches maximum, the criminal will attack the player with a knife, resulting in game over.

## Game Objectives
1. Locate the hidden gun by solving a series of interconnected puzzles
2. Maintain a low suspicion level to avoid being killed
3. Use the environment and hidden tools to your advantage
4. Once the gun is found, apprehend the criminal

## Game Environment
The game takes place in detailed indoor environments with interactive objects that may:
- Contain hidden clues
- Function as puzzle elements
- Conceal tools (like a stove hiding a base64 encoder/decoder)
- Act as red herrings to distract the player

## Project Structure
The game is built using Unity and follows a modular organization structure:

```
Assets/
├── Scenes/         - Game scenes including levels, main menu, etc.
├── Artworks/       - Visual assets, textures, and artistic elements
├── TextMesh Pro/   - Text rendering and UI font system
├── Prefabs/        - Reusable game objects and components
├── Model/          - 3D models for characters, environments, and props
└── Utils/          - Utility scripts and helper functions
```

### Key Components
- **Scenes**: Contains different game levels and environments where the player will solve puzzles
- **Artworks**: Visual elements including textures, UI components, and 2D art assets
- **Models**: 3D assets for the detective, criminal, furniture, and interactive objects
- **Prefabs**: Pre-configured game objects like puzzle components, tools, and interactive elements
- **Utils**: Helper scripts for puzzle logic, encoding/decoding tools, and suspicion system

## Development Status
*[Current development status information will be added here]*

## Technical Requirements
- **Engine**: Unity (Version TBD)
- **Platforms**: PC (Windows) initially, with potential for additional platforms
- **Input**: Keyboard and mouse, with potential gamepad support

## Team
*[Team information will be added here]*

## Screenshots
*[Screenshots will be added here when available]*

---

*CaptureTheGun combines elements of escape room puzzles, detective work, and the tension of being trapped with a dangerous criminal. Every object could be a clue, a tool, or a trap - choose wisely and stay vigilant!*