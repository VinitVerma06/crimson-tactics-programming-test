using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float moveSpeed = 5f;

    private Tile currentTile;
    private bool isMoving;

    private void Start() {
        currentTile = GetTileUnderPosition(transform.position);
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
        if (Physics.Raycast(worldPosition + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f)) {
            return hit.collider.GetComponent<Tile>();
        }
        
        return null;
    }

    private IEnumerator MoveAlongPath(List<Tile> path) {
        isMoving = true;

        foreach (Tile step in path) {
            Vector3 targetPosition = step.transform.position + Vector3.up * 0.5f;

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f) {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            currentTile = step; 
        }

        isMoving = false;
    }
}
