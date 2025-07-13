using System;
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

    private bool _hasMoved;
    private Tile _startingTile;

    private new void Awake()
    {
        base.Awake();
        _hasMoved = false;
        _startingTile = tile;
    }

    private new void Start()
    {
        base.Start();
        PieceMover.Instance.OnPieceDeselected += PieceMover_OnPieceDeselected;
    }

    public bool HasMoved() => _hasMoved;
    
    private void PieceMover_OnPieceDeselected(object sender, PieceMover.OnPieceEventArgs e)
    {
        if (!Equals(e.piece)) return;

        if (GetTile().Equals(_startingTile)) return;
        
        _hasMoved = true;
    }

    public override void InitializeSprite()
    {
        spriteRenderer.sprite = sovereignPiece.kingSprite;
    }

    public override HashSet<Vector2Int> LegalMoves()
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();

        Vector2Int position = tile.GetCoordinates();

        foreach (Vector2Int offset in KingPositions)
        {
            Vector2Int testCoords = position + offset;
            Tile testTile = Board.Instance.GetTile(testCoords);
            
            if (!LegalTile(testTile)) continue;
            legalMoves.Add(testCoords);
        }
        
        return legalMoves;
    }
}
