using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knight : Piece
{
    private static Coordinates[] KnightPositions { get; } = {
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

    public override HashSet<Coordinates> LegalMoves()
    {
        HashSet<Coordinates> legalMoves = new HashSet<Coordinates>();

        Coordinates position = tile.GetCoordinates();

        foreach (Coordinates offset in KnightPositions)
        {
            Coordinates testCoords = position.Add(offset);
            Tile testTile = Board.Instance.GetTile(testCoords);
            
            if (!LegalTile(testTile)) continue;
            legalMoves.Add(testCoords);
        }
        
        return legalMoves;
    }
}
