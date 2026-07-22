# Black March Studios — Game Developer Intern Assignment
Grid-based tactics prototype: grid generation, obstacle editor tool, BFS pathfinding, and enemy AI.

## Unity Version
6000.3.7f1 — required version, confirmed working.

## How to Run
1. Clone the repo.
2. Open in Unity 6000.3.7f1.
3. Open `Assets/Scenes/SampleScene`
4. Press Play.
5. Click any tile to move the player; click again after obstacles/enemy react.

## Assignment 1 — Grid Generation
- `GridManager` spawns a 10x10 grid of cubes at runtime.
- `Tile` (MonoBehaviour) stores each tile's grid position and walkable state.
- `MouseHoverTileDetector` raycasts from the mouse and displays hovered tile coordinates on a TMP UI element.

## Assignment 2 — Obstacles (Custom Editor Tool)
- `ObstacleData_SO` (ScriptableObject) stores a 10x10 blocked/unblocked layout.
- `ObstacleEditorWindow` (Editor tool, accessible via Grid > Obstacle Editor) provides a toggleable button grid to paint obstacles onto the ScriptableObject.
- `ObstacleManager` reads the ScriptableObject at runtime and spawns red sphere obstacles, marking corresponding tiles unwalkable.

## Assignment 3 — Pathfinding
- `Pathfinder` implements BFS on the grid (see note below on BFS vs A*).
- `PlayerController` handles click-to-move: raycasts to a clicked tile, requests a path, and animates movement tile-by-tile via coroutine.
- Input is disabled (`isMoving` flag) while the unit is mid-move.
- Obstacle tiles and occupied tiles are rejected before pathfinding is attempted.

## Assignment 4 — Enemy AI
- `IAIController` interface defines the AI contract (`OnPlayerMoved`).
- `Enemy` implements `IAIController`, subscribes to the player's movement notifications, and paths to the closest free tile adjacent to the player using the same `Pathfinder`.
- Enemy remains stationary until notified of the next player move.

## Controls
- Click a tile to move the player there.
- Enemy reacts automatically once the player finishes moving.

## Design Note: BFS instead of A*
The grid has uniform movement cost and no diagonals, so BFS already guarantees shortest-path results identical to A* here — A*'s heuristic-guided search mainly pays off on larger or weighted graphs. Given the 10x10 scale, I choose BFS for simplicity and correctness; the algorithm could be swapped for A* by replacing the queue with a priority queue ordered by cost + Manhattan distance heuristic.