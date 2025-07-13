using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Queen : Piece
{
    public override void InitializeSprite()
    {
        spriteRenderer.sprite = sovereignPiece.queenSprite;
    }

    public override HashSet<Vector2Int> LegalMoves()
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        Vector2Int coordinates = tile.GetCoordinates();
        
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Vector2Int(-1,  0)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Vector2Int( 1,  0)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Vector2Int( 0, -1)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Vector2Int( 0,  1)));
        
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Vector2Int(-1, -1)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Vector2Int( 1, -1)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Vector2Int(-1,  1)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Vector2Int( 1,  1)));
        
        return legalMoves;
    }
}
