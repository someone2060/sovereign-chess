using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bishop : Piece
{
    protected new void Start()
    {
        base.Start();
        spriteRenderer.sprite = sovereignPiece.bishopSprite;
    }
    
    public override HashSet<Vector2Int> LegalMoves()
    {
        if (tile.Equals(legalMovesCheckTile)) return legalMoves;
        base.LegalMoves();
        
        legalMoves = new HashSet<Vector2Int>();
        
        Vector2Int coordsVec = GetCoordinates().GetVector2Int();
        
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.right + Vector2Int.up));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.left + Vector2Int.up));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.right + Vector2Int.down));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.left + Vector2Int.down));
        return legalMoves;
    }
}
