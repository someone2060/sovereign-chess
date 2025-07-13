using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rook : Piece
{
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
        spriteRenderer.sprite = sovereignPiece.rookSprite;
    }

    public override HashSet<Coordinates> LegalMoves()
    {
        HashSet<Coordinates> legalMoves = new HashSet<Coordinates>();
        Coordinates coordinates = tile.GetCoordinates();
        
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates(-1,  0)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates( 1,  0)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates( 0, -1)));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, new Coordinates( 0,  1)));
        
        return legalMoves;
    }
}
