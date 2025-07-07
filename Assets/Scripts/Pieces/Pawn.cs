using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : Piece
{
    private static Vector2Int[] HorizontalMove { get; } = { new(1, 0) };
    private static Vector2Int[] HorizontalStartingMove { get; } = { new Vector2Int(2, 0) };
    private static Vector2Int[] HorizontalCaptures { get; } = { new(1, 1), new(1, -1) };
    
    private static Vector2Int[] VerticalMove { get; } = { new(0, 1) };
    private static Vector2Int[] VerticalStartingMove { get; } = { new Vector2Int(0, 2) };
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
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        
        HashSet<Vector2Int> movePositions = new HashSet<Vector2Int>(); 
        HashSet<Vector2Int> capturePositions = new HashSet<Vector2Int>();

        Vector2Int position = GetCoordinates().GetVector2Int();
        Vector2Int quadrant = Board.Instance.GetQuadrant(position);

        Vector2Int potentialPosition = position + quadrant;
        Vector2Int potentialQuadrant = Board.Instance.GetQuadrant(potentialPosition);
        
        if (quadrant.x == potentialQuadrant.x)
        {
            movePositions.AddRange(HorizontalMove);
            capturePositions.AddRange(HorizontalCaptures);
        }

        if (quadrant.y == potentialQuadrant.y)
        {
            movePositions.AddRange(VerticalMove);
            capturePositions.AddRange(VerticalCaptures);
        }
        
        Debug.Log("movePositions: " + DebugHashSetLog(movePositions));
        Debug.Log("capturePositions: " + DebugHashSetLog(capturePositions));
        
        legalMoves.AddRange(CheckForPawnMoves(position, quadrant, 
            movePositions, 
            capturePositions));
        
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
