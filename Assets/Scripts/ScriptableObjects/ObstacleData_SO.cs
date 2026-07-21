using UnityEngine;

// SO asset storing which of the 100 tiles are blocked in the grid.
[CreateAssetMenu(fileName = "ObstacleData", menuName = "Scriptable Grid/Obstacle Data")]
public class ObstacleData_SO : ScriptableObject {

    public const int GRID_WIDTH = 10;
    public const int GRID_HEIGHT = 10;

    public bool[] blockedTiles = new bool[GRID_WIDTH * GRID_HEIGHT];    // Flatten array: true = tile is blocked
    
    public bool IsBlocked(int x, int y) {
        return blockedTiles[x + y * GRID_WIDTH];
    }

    public void SetBlocked(int x, int y, bool blocked) {
        blockedTiles[x + y * GRID_WIDTH] = blocked;
    }
}
