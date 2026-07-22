using UnityEngine;
using System.Collections.Generic;
public static class Pathfinder {
    public static List<Tile> FindPath(Tile start, Tile end) {

        Queue<Tile> queue = new Queue<Tile>();      // Tiles discovered, yet to be visited

        HashSet<Tile> visited = new HashSet<Tile>();        // Tiles visited

        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile,Tile>();     // Map the tile discovered -> tile we came from

        queue.Enqueue(start);
        visited.Add(start);

        bool pathFound = false;

        while (queue.Count > 0) {
            Tile current = queue.Dequeue();

            if (current == end) {
                pathFound = true;
                break;
            }

            foreach (Tile neighbour in GetNeighbours(current)) {
                if (neighbour == null) continue;            // Check whether the neighbour exists or not
                if (visited.Contains(neighbour)) continue;  // Check whether the neighbour is already visited or not
                if (!neighbour.isWalkable) continue;        // Checks whether the neighbour is blocked or not

                visited.Add(neighbour);
                cameFrom[neighbour] = current;
                queue.Enqueue(neighbour);
            }
        }

        if (!pathFound) {
            return null;
        }

        return PathTaken(cameFrom, start, end);
    }

    // Look up all the four directions of tile using grid array from GridManager
    private static List<Tile> GetNeighbours(Tile tile) {
        List<Tile> neighbours = new List<Tile>();
        Vector2Int pos = tile.gridPosition;

        Vector2Int[] directions = {
            new Vector2Int(0, 1),   // up
            new Vector2Int(0, -1),  // down
            new Vector2Int(1, 0),   // right
            new Vector2Int(-1, 0)   // left
        };

        foreach (Vector2Int dir in directions) {
            int neighbourX = pos.x + dir.x;
            int neighbourY = pos.y + dir.y;

            // OutOfBound check before indexing in the grid array
            if (neighbourX >= 0 && neighbourX < ObstacleData_SO.GRID_WIDTH &&
                neighbourY >= 0 && neighbourY < ObstacleData_SO.GRID_HEIGHT) {
                neighbours.Add(GridManager.Instance.grid[neighbourX, neighbourY]);
            }
        }

        return neighbours;
    }

    // Tracks backward from end to start, then reverses the List to get the path
    private static List<Tile> PathTaken(Dictionary<Tile, Tile> cameFrom, Tile start, Tile end) {
        List<Tile> path = new List<Tile>();
        Tile current = end;

        while (current != start) {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }
}
