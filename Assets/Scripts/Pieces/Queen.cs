using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Queen : Piece
{
    public override void InitializeSprite()
    {
        spriteRenderer.sprite = sovereignPiece.queenSprite;
    }

    public override HashSet<Coordinates> LegalMoves()
    {
        HashSet<Coordinates> legalMoves = new HashSet<Coordinates>();
        Coordinates coordinates = tile.GetCoordinates();
        
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates(-1,  0)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates( 1,  0)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates( 0, -1)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates( 0,  1)));
        
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates(-1, -1)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates( 1, -1)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates(-1,  1)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates( 1,  1)));
        
        return legalMoves;
    }
}
