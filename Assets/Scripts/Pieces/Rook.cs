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

    public override HashSet<Vector2Int> LegalMoves()
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        
        Vector2Int coordsVec = tile.GetCoordinates().GetVector2Int();
        
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.right));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.left));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.up));
        legalMoves.AddRange(SearchLegalTilesInDirection(coordsVec, Vector2Int.down));
        return legalMoves;
    }
}
