using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knight : Piece
{
    private static Vector2Int[] KnightPositions { get; } = {
        new(2, -1), 
        new(2, 1), 
        new(1, -2), 
        new(1, 2), 
        new(-1, -2), 
        new(-1, 2), 
        new(-2, -1),
        new(-2, 1)
    };

    protected new void Start()
    {
        base.Start();
        spriteRenderer.sprite = sovereignPiece.knightSprite;
    }
    
    public override HashSet<Vector2Int> LegalMoves()
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();

        Vector2Int position = GetCoordinates().GetVector2Int();

        foreach (Vector2Int offset in KnightPositions)
        {
            Vector2Int testVec = position + offset;
            Tile testTile = Board.Instance.GetTile(testVec);
            
            if (!LegalTile(testTile)) continue;
            legalMoves.Add(testVec);
        }
        
        return legalMoves;
    }
}
