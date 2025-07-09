using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rook : Piece
{
    public override void InitializeSprite()
    {
        spriteRenderer.sprite = sovereignPiece.rookSprite;
    }

    public override HashSet<Vector2Int> LegalMoves()
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        
        Vector2Int coordsVec = tile.GetCoordinates().GetVector2Int();
        
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.right));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.left));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.up));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.down));
        return legalMoves;
    }
}
