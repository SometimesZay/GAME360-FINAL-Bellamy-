# Task 2: GameManager Implementation

## Student Info
- Name: [Isaiah]
- ID: [01219974]

## Pattern: Singleton
### Implementation
[GameManager can be called from outside the singleton via GameManager.Instance.{Method()}. 
The Collectible script calls the GameManager to manage the score change when a collectible is picked up: GameManager.Instance.CollectiblePickedUp(value);. 
The Enemy script calls the GameManager to manage the score change when an enemy is killed: GameManager.Instance.EnemyKilled(). 
The PlayerController script calls the GameManager to manage the players lives when taking collision: GameManager.Instance.LoseLife();, and to manager the players fireRate increases with GameManager.Instance.score. ]

### Game Integration
[GameManager controls the Game Stats: score, lives, enemiesKilled, and enemiesValue. Each has respective methods that can be called from outside the singleton: EnemiesKilled(), CollectiblePickedUp(int value), LoseLife(), and AddScore(int points).

GameManager controls the UI: scoreText, livesText, enemiesKilledText, and gameOverPanel. Updates and refreshes the UI upon loading a new scene. This enables the Replay feature to work, resetting the scene and the trackers to their initial values. 

GameManager controls restarting and quitting the Game, destroying all the game objects before reloading the scene and re-establishing all the Game Stats and UI elements]

## Game Description
- Title: [SimpleShooter]
- Controls: [A,D = Move left and Right][Hold Left/ Left Click = Shoot] [Press Spacebar = Shoot]
- Objective: [Survive for as long as possible]

## Repository Stats
- Total Commits: [11]
- Development Time: [6]

# For Final Project

## Game Description
Star Fighter is a vertical-scrolling arcade shooter where players pilot a customizable spacecraft through waves of enemies. Players destroy hostile ships, dodge bullets, and collect power-ups that enhance weapons or shields. Each stage increases in speed and density, rewarding accuracy, survival, and upgrade strategy across progressively challenging galactic battlefields.
## Current Status
- In-progress
## Development Timeline
- Add Event Manager & State Machines because I'm going back to my task 2 project for the final
- 


