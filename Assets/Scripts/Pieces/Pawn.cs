using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : Piece
{
    private static readonly Vector2Int HorizontalMove = new(1, 0);
    private static readonly Vector2Int HorizontalStartingMove = new(2, 0);
    private static Vector2Int[] HorizontalCaptures { get; } = { new(1, 1), new(1, -1) };
    
    private static readonly Vector2Int VerticalMove = new(0, 1);
    private static readonly Vector2Int VerticalStartingMove = new(0, 2);
    private static Vector2Int[] VerticalCaptures { get; } = { new(1, 1), new(-1, 1) };

    private bool _canMoveHorizontal;
    private bool _canMoveVertical;
    
    public override void InitializeSprite()
    {
        spriteRenderer.sprite = sovereignPiece.pawnSprite;
    }

    public override HashSet<Vector2Int> LegalMoves(HashSet<Piece> pieceToIgnore = null)
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        
        HashSet<Vector2Int> moveTests = new HashSet<Vector2Int>(); 
        HashSet<Vector2Int> captureTests = new HashSet<Vector2Int>();

        Vector2Int position = tile.GetCoordinates();
        Vector2Int quadrant = Board.Instance.GetQuadrant(position);

        Vector2Int potentialPosition = position + quadrant;
        Vector2Int potentialQuadrant = Board.Instance.GetQuadrant(potentialPosition);
        
        if (quadrant.x == potentialQuadrant.x)
        {
            moveTests.Add(HorizontalMove * quadrant);
            foreach (Vector2Int horizontalCapture in HorizontalCaptures)
            {
                captureTests.Add(horizontalCapture * quadrant);
            }
        }

        if (quadrant.y == potentialQuadrant.y)
        {
            moveTests.Add(VerticalMove * quadrant);
            foreach (Vector2Int horizontalCapture in VerticalCaptures)
            {
                captureTests.Add(horizontalCapture * quadrant);
            }
        }

        if (Board.Instance.OnOuterTwoX(position)) moveTests.Add(HorizontalStartingMove * quadrant);
        if (Board.Instance.OnOuterTwoY(position)) moveTests.Add(VerticalStartingMove * quadrant);
        
        legalMoves.AddRange(CheckForPawnMoves(
            position, 
            moveTests, 
            captureTests));
        
        return legalMoves;
    }

    public HashSet<Vector2Int> GetAttackingCoordinates()
    {
        HashSet<Vector2Int> captureDirections = new HashSet<Vector2Int>();

        Vector2Int position = tile.GetCoordinates();
        Vector2Int quadrant = Board.Instance.GetQuadrant(position);

        Vector2Int potentialPosition = position + quadrant;
        Vector2Int potentialQuadrant = Board.Instance.GetQuadrant(potentialPosition);
        
        if (quadrant.x == potentialQuadrant.x)
        {
            foreach (Vector2Int horizontalCapture in HorizontalCaptures)
            {
                captureDirections.Add(horizontalCapture * quadrant);
            }
        }

        if (quadrant.y == potentialQuadrant.y)
        {
            foreach (Vector2Int horizontalCapture in VerticalCaptures)
            {
                captureDirections.Add(horizontalCapture * quadrant);
            }
        }
        
        HashSet<Vector2Int> attackingCoordinates = new HashSet<Vector2Int>();
        foreach (Vector2Int captureDirection in captureDirections)
        {
            attackingCoordinates.Add(position + captureDirection);
        }
        
        return attackingCoordinates;
    }

    private HashSet<Vector2Int> CheckForPawnMoves(
        Vector2Int position, HashSet<Vector2Int> movePositions, HashSet<Vector2Int> capturePositions)
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        foreach (Vector2Int offset in movePositions)
        {
            Vector2Int testCoords = position + offset;
            Tile testTile = Board.Instance.GetTile(testCoords);
            
            if (testTile is null) continue;
            if (!testTile.LegalTile(this, canCapture: false)) continue;
            legalMoves.Add(testCoords);
        }

        foreach (Vector2Int offset in capturePositions)
        {
            Vector2Int testCoords = position + offset;
            Tile testTile = Board.Instance.GetTile(testCoords);
            
            if (testTile is null) continue;
            if (!testTile.LegalTile(this, canMove: false)) continue;
            legalMoves.Add(testCoords);
        }
        
        return legalMoves;
    }

    public static bool CanPromote(Tile selectedTile)
    {
        return Board.Instance.InPawnPromotionArea(selectedTile.GetCoordinates());
    }
}
