using UnityEngine;

// Tile data container 
public class Tile : MonoBehaviour {
    public Vector2Int gridPosition;     // Holds tile position in the grid
    public bool isWalkable = true;      // is obstructed or not

    [SerializeField] private Transform unitAnchor;     // Tile standing position 
    public Vector3 anchorPosition => unitAnchor != null? unitAnchor.position : transform.position;
}
