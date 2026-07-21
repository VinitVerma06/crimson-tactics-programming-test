using JetBrains.Annotations;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {

    public static ObstacleManager Instance { get; private set; }

    [SerializeField] private ObstacleData_SO obstacleData;
    [SerializeField] private GameObject obstacle;

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

    private void SpawnObstacles() {

        for (int x = 0; x < ObstacleData_SO.GRID_WIDTH; x++) {
            for (int y = 0; y < ObstacleData_SO.GRID_HEIGHT; y++) {
                if (obstacleData.IsBlocked(x, y)) {
                    Tile tile = GridManager.Instance.grid[x, y];
                    tile.isWalkable = false;
                    Vector3 spawnPosition = tile.transform.position + Vector3.up * 0.45f;
                    Instantiate(obstacle, spawnPosition, Quaternion.identity, transform);
                }
            }
        }
    }
}
