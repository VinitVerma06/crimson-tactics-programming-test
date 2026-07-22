using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerBase;
    [SerializeField] private Vector2Int playerStartingTilePosition;
    [SerializeField] private MonoBehaviour[] enemyAIListeners;          // Drag any IAIController component
    [SerializeField] private ObstacleData_SO obstacleData;
    [SerializeField] private float moveSpeed = 5f;

    private Tile currentTile;
    private bool isMoving;

    private void Start() {
        SetPlayerPosition(playerStartingTilePosition);
    }


    private void Update() {

        if (isMoving) return;   // Blocks the input while moving

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            TryMoveToClickedTile();
        }

    }

    private void TryMoveToClickedTile() {

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        Tile clickedTile = hit.collider.GetComponent<Tile>();
        if (clickedTile == null) return;            // check if clicked on a tile 
        if (!clickedTile.isWalkable) return;        // check if tile is blocked
        if (clickedTile == currentTile) return;     // check if standing on the same tile
        if (clickedTile.occupant != null) return;   // check if tile is ocuppied


        // Reserves the end tile before moving 
        clickedTile.occupant = gameObject;
        currentTile.occupant = null;

        List<Tile> path = Pathfinder.FindPath(currentTile, clickedTile);
        if (path == null) return;   // No path exists = Fully boxed in by obstacle

        StartCoroutine(MoveAlongPath(path));
    }

    // Set the initial position of the player
    private void SetPlayerPosition(Vector2Int position) {
        if (!obstacleData.IsBlocked(position.x, position.y)) {
            currentTile = GridManager.Instance.grid[position.x, position.y];
            if (currentTile != null) {
                transform.position = currentTile.anchorPosition;
            }
        } else {
            Debug.LogWarning("PLAYERCONTROLLER: TILE SELECTED IS INVALID OR BLOCKED!");
        }
    }

    // Walk along the path tile by tile found by pathfinder
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
        NotifyAIListeners();    // Let the enemy react to the new player position 
    }

    // Tell every subscribed AI unit that player has finished moving 
    private void NotifyAIListeners() {
        
        foreach (MonoBehaviour listener in enemyAIListeners) {
            
            if (listener is IAIController ai) {
                ai.OnPlayerMoved(currentTile);
            }
        }
    }
}
