Sync Dash

#Project Overview

Sync Dash is a hyper-casual endless runner game built in Unity. The screen is split into two halves:

Right Side: The player controls a glowing cube that moves forward automatically.

Left Side: A ghost player mimics the player's movements in real-time with a slight delay, simulating multiplayer syncing.

The goal is to jump over obstacles and collect orbs while managing increasing speed.



#Game Features

Endless Runner Mechanics – Auto-moving player with increasing speed.✔ Ghost Player Synchronization – The left-side ghost mimics the player with real-time state syncing.✔ Obstacle & Orb System – Obstacles spawn randomly, and orbs are collectible and recycled.✔ Optimized Object Pooling – Efficient recycling of ground, obstacles, and orbs to improve performance.✔ Score System – Score increases with distance and orb collection.✔ Game Over & Restart System – Stops the game on collision and allows restarting.



#Setup & Installation

1️ Requirements

Unity 2021.3 LTS or later

Universal Render Pipeline (URP) [Optional]

Compatible with Windows, Mac, and Android

2️ Clone the Repository

git clone https://github.com/SonOfIgneel/Sync-Dash.git

3️ Open in Unity

Open Unity Hub

Click "Open" and select the project folder

Open "Sync Dash" scene inside Assets/Scenes/

4️ Play the Game

Click Play in the Unity Editor.

To build for Android, go to File → Build Settings, select Android, and click Build & Run.

Controls

Action

Input

Jump

Left Click / Tap on Screen

Restart Game

Click "Restart" Button

Exit Game

Click "Exit" Button



#How to Play

The Player Cube automatically moves forward.

Tap to Jump over obstacles.

Collect Orbs to increase the score.

The Ghost Cube mimics the player's moves with a delay.

The game ends if you hit an obstacle.

Click Restart to play again.



#Technical Details

1️ Object Pooling

Uses Queue<GameObject> for reusing ground, obstacles, and orbs.

Prevents unnecessary instantiation/destruction for better performance.

2️ Ghost Player Synchronization

Queue<Vector3> stores past player positions.

Ghost follows stored positions with a slight delay.

Ensures smooth movement using interpolation.

3️ Adaptive Difficulty

Speed increases gradually over time.

Obstacles spawn more frequently at higher speeds.


#Known Issues & Future Improvements

Current Issues

Occasionally, ghost orbs may become desynced if the player jumps while collecting.

UI animations could be added.


#Planned Improvements
Improved ghost movement smoothing using interpolation.
player glow and shaders for good visuals


