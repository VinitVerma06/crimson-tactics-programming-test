using System;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {

    public static ObstacleManager Instance { get; private set; }

    [SerializeField] private ObstacleData_SO obstacleData;
    [SerializeField] private GameObject obstacle;   // Obstacle prefab

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start() {
        SpawnObstacles();
    }


    // Reads the ObstacleData and spawns the prefab on the blocked tile
    private void SpawnObstacles() {

        for (int x = 0; x < ObstacleData_SO.GRID_WIDTH; x++) {
            for (int y = 0; y < ObstacleData_SO.GRID_HEIGHT; y++) {
                if (obstacleData.IsBlocked(x, y)) {
                    Tile tile = GridManager.Instance.grid[x, y];
                    tile.isWalkable = false;
                    Vector3 spawnPosition = tile.anchorPosition;
                    Instantiate(obstacle, spawnPosition, Quaternion.identity, transform);
                }
            }
        }
    }
}
