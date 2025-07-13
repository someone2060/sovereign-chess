using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : Piece
{
    private static readonly Coordinates HorizontalMove = new(1, 0);
    private static readonly Coordinates HorizontalStartingMove = new(2, 0);
    private static Coordinates[] HorizontalCaptures { get; } = { new(1, 1), new(1, -1) };
    
    private static readonly Coordinates VerticalMove = new(0, 1);
    private static readonly Coordinates VerticalStartingMove = new(0, 2);
    private static Coordinates[] VerticalCaptures { get; } = { new(1, 1), new(-1, 1) };

    private bool _canMoveHorizontal;
    private bool _canMoveVertical;
    
    public override void InitializeSprite()
    {
        spriteRenderer.sprite = sovereignPiece.pawnSprite;
    }

    public override void DestroySelf()
    {
        base.DestroySelf();
        Destroy(gameObject);
    }

    public override HashSet<Coordinates> LegalMoves()
    {
        HashSet<Coordinates> legalMoves = new HashSet<Coordinates>();
        
        HashSet<Coordinates> moveTests = new HashSet<Coordinates>(); 
        HashSet<Coordinates> captureTests = new HashSet<Coordinates>();

        Coordinates position = tile.GetCoordinates();
        Coordinates quadrant = Board.Instance.GetQuadrant(position);

        Coordinates potentialPosition = position.Add(quadrant);
        Coordinates potentialQuadrant = Board.Instance.GetQuadrant(potentialPosition);
        
        if (quadrant.GetX() == potentialQuadrant.GetX())
        {
            moveTests.Add(HorizontalMove);
            captureTests.AddRange(HorizontalCaptures);
        }

        if (quadrant.GetY() == potentialQuadrant.GetY())
        {
            moveTests.Add(VerticalMove);
            captureTests.AddRange(VerticalCaptures);
        }

        if (Board.Instance.OnOuterTwoX(position)) moveTests.Add(HorizontalStartingMove);
        if (Board.Instance.OnOuterTwoY(position)) moveTests.Add(VerticalStartingMove);
        
        legalMoves.AddRange(CheckForPawnMoves(position, quadrant, 
            moveTests, 
            captureTests));
        
        return legalMoves;
    }

    private HashSet<Coordinates> CheckForPawnMoves(
        Coordinates position, Coordinates quadrant, 
        HashSet<Coordinates> movePositions, HashSet<Coordinates> capturePositions)
    {
        HashSet<Coordinates> legalMoves = new HashSet<Coordinates>();
        foreach (Coordinates offset in movePositions)
        {
            Coordinates testCoords = position.Add(offset.Mult(quadrant));
            Tile testTile = Board.Instance.GetTile(testCoords);
            
            if (!LegalTile(testTile, canCapture: false)) continue;
            legalMoves.Add(testCoords);
        }

        foreach (Coordinates offset in capturePositions)
        {
            Coordinates testCoords = position.Add(offset.Mult(quadrant));
            Tile testTile = Board.Instance.GetTile(testCoords);
            
            if (!LegalTile(testTile, canMove: false)) continue;
            legalMoves.Add(testCoords);
        }
        
        return legalMoves;
    }

    private String DebugHashSetLog(HashSet<Coordinates> hashSet)
    {
        String output = "";

        foreach (Coordinates vector in hashSet)
        {
            output += vector + ", ";
        }
        
        return output;
    }
}
