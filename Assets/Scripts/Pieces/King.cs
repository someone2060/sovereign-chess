using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class King : Piece
{
    private static Vector2Int[] KingPositions { get; } = {
        new(1, -1), 
        new(1, 0), 
        new(1, 1), 
        new(0, -1), 
        new(0, 1), 
        new(-1, -1), 
        new(-1, 0),
        new(-1, 1)
    };

    protected new void Start()
    {
        base.Start();
        spriteRenderer.sprite = sovereignPiece.kingSprite;
    }
    
    public override HashSet<Vector2Int> LegalMoves()
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();

        Vector2Int position = tile.GetCoordinates().GetVector2Int();

        foreach (Vector2Int offset in KingPositions)
        {
            Vector2Int testVec = position + offset;
            Tile testTile = Board.Instance.GetTile(testVec);
            
            if (!LegalTile(testTile)) continue;
            legalMoves.Add(testVec);
        }
        
        return legalMoves;
    }
}
