using UnityEngine;

// Interface for the enemy AI
public interface IAIController {
    void OnPlayerMoved(Tile playerTile);
}
