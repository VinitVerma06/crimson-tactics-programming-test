using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseHoverTileDetector : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI tileInfoText;


    private void Update() {
        if (Mouse.current == null) return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {

            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile != null) {
                tileInfoText.text = $"Tile: ({tile.gridPosition.x},{tile.gridPosition.y})";
            }

        } else {
            tileInfoText.text = ""; 
        }
    }
}
