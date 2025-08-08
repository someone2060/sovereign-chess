using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Queen : Piece
{
    public override void InitializeSprite()
    {
        spriteRenderer.sprite = sovereignPiece.queenSprite;
    }

    public override HashSet<Vector2Int> LegalMoves(HashSet<Piece> pieceToIgnore = null)
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        Vector2Int coordinates = tile.GetCoordinates();

        foreach (Vector2Int direction in Bishop.Diagonals)
        {
            legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, direction, pieceToIgnore));
        }

        foreach (Vector2Int direction in Rook.Orthogonals)
        {
            legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, direction, pieceToIgnore));
        }
        
        return legalMoves;
    }
}
