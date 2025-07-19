using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rook : Piece
{
    public static readonly Vector2Int[] Orthogonals =
    {
        Vector2Int.right, 
        Vector2Int.up, 
        Vector2Int.left,
        Vector2Int.down
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
        spriteRenderer.sprite = sovereignPiece.rookSprite;
    }

    public override HashSet<Vector2Int> LegalMoves(List<Piece> pieceToIgnore = null)
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        Vector2Int coordinates = tile.GetCoordinates();

        foreach (Vector2Int direction in Orthogonals)
        {
            legalMoves.AddRange(SearchLegalTilesInDirection(coordinates, direction, pieceToIgnore));
        }
        
        return legalMoves;
    }
}
