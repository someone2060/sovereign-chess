using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Queen : Piece
{
    protected new void Start()
    {
        base.Start();
        spriteRenderer.sprite = sovereignPiece.queenSprite;
    }
    
    public override HashSet<Vector2Int> LegalMoves()
    {
        if (legalMovesCheckTile.Equals(tile)) return legalMoves;
        base.LegalMoves();
        
        legalMoves = new HashSet<Vector2Int>();
        
        Vector2Int coordsVec = GetCoordinates().GetVector2Int();
        
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.right));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.left));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.up));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.down));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.right + Vector2Int.up));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.left + Vector2Int.up));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.right + Vector2Int.down));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.left + Vector2Int.down));
        return legalMoves;
    }
}
