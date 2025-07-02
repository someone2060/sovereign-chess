using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rook : Piece
{
    protected new void Start()
    {
        base.Start();
        spriteRenderer.sprite = sovereignPiece.rookSprite;
    }
    
    public override HashSet<Vector2Int> LegalMoves()
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        
        Vector2Int coordsVec = GetCoordinates().GetVector2Int();
        
        legalMoves.AddRange(SearchInDirection(coordsVec, Vector2Int.right));
        legalMoves.AddRange(SearchInDirection(coordsVec, Vector2Int.left));
        legalMoves.AddRange(SearchInDirection(coordsVec, Vector2Int.up));
        legalMoves.AddRange(SearchInDirection(coordsVec, Vector2Int.down));
        return legalMoves;
    }
}
