using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerBase;
    [SerializeField] private Vector2Int playerStartingTilePosition;
    [SerializeField] private MonoBehaviour[] enemyAIListeners; 
    [SerializeField] private float moveSpeed = 5f;

    private Tile currentTile;
    private bool isMoving;

    private void Start() {
        SetPlayerPosition(playerStartingTilePosition);
    }


    private void Update() {

        if (isMoving) return;

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            TryMoveToClickedTile();
        }

    }

    private void TryMoveToClickedTile() {

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        Tile clickedTile = hit.collider.GetComponent<Tile>();
        if (clickedTile == null) return;
        if (!clickedTile.isWalkable) return;
        if (clickedTile == currentTile) return;

        List<Tile> path = Pathfinder.FindPath(currentTile, clickedTile);
        if (path == null) return;

        StartCoroutine(MoveAlongPath(path));
    }

    private Tile GetTileUnderPosition(Vector3 worldPosition) {
        if (Physics.Raycast(playerBase.position, Vector3.down, out RaycastHit hit, 10f)) {
            return hit.collider.GetComponent<Tile>();
        }
        
        return null;
    }

    private void SetPlayerPosition(Vector2Int position) {
        if (GridManager.Instance.grid[position.x, position.y].isWalkable) {
            currentTile = GridManager.Instance.grid[position.x, position.y];
            if (currentTile != null) {
                transform.position = currentTile.anchorPosition;
            }
        } else {
            Debug.LogWarning("PLAYERCONTROLLER: TILE SELECTED IS INVALID OR BLOCKED!");
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

            currentTile = step; 
        }

        isMoving = false;
        NotifyAIListeners();
    }

    private void NotifyAIListeners() {
        
        foreach (MonoBehaviour listener in enemyAIListeners) {
            
            if (listener is IAIController ai) {
                ai.OnPlayerMoved(currentTile);
            }
        }
    }
}
