using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public static readonly Vector2Int[] KnightMoves = {
        new(2, -1), 
        new(2, 1), 
        new(1, -2), 
        new(1, 2), 
        new(-1, -2), 
        new(-1, 2), 
        new(-2, -1),
        new(-2, 1)
    };

    public override void InitializeSprite()
    {
        spriteRenderer.sprite = sovereignPiece.knightSprite;
    }

    public override HashSet<Vector2Int> LegalMoves(HashSet<Piece> pieceToIgnore = null)
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();

        Vector2Int position = tile.GetCoordinates();

        foreach (Vector2Int offset in KnightMoves)
        {
            Vector2Int testCoords = position + offset;
            Tile testTile = Board.Instance.GetTile(testCoords);
            
            if (testTile is null) continue;
            if (!testTile.LegalTile(alignment)) continue;
            legalMoves.Add(testCoords);
        }
        
        return legalMoves;
    }
}
