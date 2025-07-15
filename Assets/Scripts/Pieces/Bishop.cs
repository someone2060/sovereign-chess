using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bishop : Piece
{
    public static readonly Vector2Int[] Diagonals =
    {
        new( 1,  1),
        new(-1,  1),
        new( 1, -1),
        new(-1, -1)
    };
    
    public override void InitializeSprite()
    {
        spriteRenderer.sprite = sovereignPiece.bishopSprite;
    }

    public override HashSet<Vector2Int> LegalMoves()
    {        
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        Vector2Int coordinates = tile.GetCoordinates();

        foreach (Vector2Int direction in Diagonals)
        {
            legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, direction));
        }
        
        return legalMoves;
    }
}
