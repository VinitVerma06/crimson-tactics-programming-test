using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Enemy : MonoBehaviour, IAIController {

    [SerializeField] private ObstacleData_SO obstacleData;
    [SerializeField] private Vector2Int startingGridPosition;
    [SerializeField] private float moveSpeed = 5f;

    private Tile currentTile;
    private bool isMoving;

    private void Start() {
        SetEnemyPosition(startingGridPosition);
    }

    public void OnPlayerMoved(Tile playerTile) {
        if (isMoving) return;

        Tile targetTile = FindBestAdjacentTile(playerTile);
        if (targetTile == null) return;

        List<Tile> path = Pathfinder.FindPath(currentTile, targetTile);
        if (path == null) return;


        StartCoroutine(MoveAlongPath(path));
    }

    private Tile FindBestAdjacentTile(Tile playerTile) {
        Vector2Int pos = playerTile.gridPosition;
        Vector2Int[] directions = {
            new Vector2Int(0, 1), 
            new Vector2Int(0, -1),
            new Vector2Int(1, 0), 
            new Vector2Int(-1, 0)
    };

        Tile bestTile = null;
        int shortestDistance = int.MaxValue;

        foreach (Vector2Int dir in directions) {
            int nx = pos.x + dir.x;
            int ny = pos.y + dir.y;

            if (nx < 0 || nx >= ObstacleData_SO.GRID_WIDTH || ny < 0 || ny >= ObstacleData_SO.GRID_HEIGHT)
                continue;

            Tile candidate = GridManager.Instance.grid[nx, ny];
            if (candidate == null || !candidate.isWalkable) continue;

            // Prefer the adjacent tile closest to where the enemy currently stands
            int distance = Mathf.Abs(currentTile.gridPosition.x - nx) + Mathf.Abs(currentTile.gridPosition.y - ny);
            if (distance < shortestDistance) {
                shortestDistance = distance;
                bestTile = candidate;
            }
        }

        return bestTile;
    }

    // Set the initial position of the enemy
    private void SetEnemyPosition(Vector2Int position) {
        if (!obstacleData.IsBlocked(position.x, position.y)) {
            currentTile = GridManager.Instance.grid[position.x, position.y];
            if (currentTile != null) {
                transform.position = currentTile.anchorPosition;
            }
        } else {
            Debug.LogWarning("ENEMY: TILE SELECTED IS INVALID OR BLOCKED!");
        }
    }

    private IEnumerator MoveAlongPath(List<Tile> path) {
        isMoving = true;

        foreach (Tile step in path) {
            Vector3 targetPosition = step.anchorPosition;

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f) {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            currentTile.occupant = null;
            currentTile = step;
            currentTile.occupant = gameObject;
        }

        isMoving = false;
    }
}
