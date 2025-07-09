using System;
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
    
    protected new void Start()
    {
        base.Start();
        spriteRenderer.sprite = sovereignPiece.pawnSprite;
    }
    
    public override HashSet<Vector2Int> LegalMoves()
    {
        if (legalMovesCheckTile.Equals(tile)) return legalMoves;
        base.LegalMoves();
        
        legalMoves = new HashSet<Vector2Int>();
        
        HashSet<Vector2Int> moveTests = new HashSet<Vector2Int>(); 
        HashSet<Vector2Int> captureTests = new HashSet<Vector2Int>();

        Vector2Int position = GetCoordinates().GetVector2Int();
        Vector2Int quadrant = Board.Instance.GetQuadrant(position);

        Vector2Int potentialPosition = position + quadrant;
        Vector2Int potentialQuadrant = Board.Instance.GetQuadrant(potentialPosition);
        
        if (quadrant.x == potentialQuadrant.x)
        {
            moveTests.Add(HorizontalMove);
            captureTests.AddRange(HorizontalCaptures);
        }

        if (quadrant.y == potentialQuadrant.y)
        {
            moveTests.Add(VerticalMove);
            captureTests.AddRange(VerticalCaptures);
        }

        if (Board.Instance.OnOuterTwoX(position)) moveTests.Add(HorizontalStartingMove);
        if (Board.Instance.OnOuterTwoY(position)) moveTests.Add(VerticalStartingMove);
        
        Debug.Log("movePositions: " + DebugHashSetLog(moveTests));
        Debug.Log("capturePositions: " + DebugHashSetLog(captureTests));
        
        legalMoves.AddRange(CheckForPawnMoves(position, quadrant, 
            moveTests, 
            captureTests));
        
        return legalMoves;
    }

    private HashSet<Vector2Int> CheckForPawnMoves(
        Vector2Int position, Vector2Int quadrant, 
        HashSet<Vector2Int> movePositions, HashSet<Vector2Int> capturePositions)
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        foreach (Vector2Int offset in movePositions)
        {
            Vector2Int testVec = position + offset * quadrant;
            Tile testTile = Board.Instance.GetTile(testVec);
            
            if (!LegalTile(testTile, canCapture: false)) continue;
            legalMoves.Add(testVec);
        }

        foreach (Vector2Int offset in capturePositions)
        {
            Vector2Int testVec = position + offset * quadrant;
            Tile testTile = Board.Instance.GetTile(testVec);
            
            Debug.Log("testTile: " + testTile.GetCoordinates());
            if (!LegalTile(testTile, canMove: false)) continue;
            legalMoves.Add(testVec);
        }
        
        return legalMoves;
    }

    private String DebugHashSetLog(HashSet<Vector2Int> hashSet)
    {
        String output = "";

        foreach (Vector2Int vector in hashSet)
        {
            output += vector + ", ";
        }
        
        return output;
    }
}
