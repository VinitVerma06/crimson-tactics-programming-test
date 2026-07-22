using System;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public static GridManager Instance { get; private set; }
    
    public Tile[,] grid = new  Tile[10,10];


    [SerializeField] private GameObject cubeTile;
    [SerializeField] private Transform gridOrigin;
    [SerializeField] private float tileSpacing = 1f;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        GenerateGrid();
    }


    private void GenerateGrid() {
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {

                Vector3 tileWorldPosition = gridOrigin.position + new Vector3(x * tileSpacing, 0f, y * tileSpacing);
                GameObject cube = Instantiate(cubeTile, tileWorldPosition, Quaternion.identity, transform);
                cube.name = $"Tile_{x}_{y}";

                Tile tile = cube.GetComponent<Tile>();
                tile.gridPosition = new Vector2Int(x, y);
                grid[x,y] = tile;
            }
        }

    }
}
