using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class King : Piece
{
    public static readonly Vector2Int[] KingMoves = {
        new(1, -1), 
        new(1, 0), 
        new(1, 1), 
        new(0, -1), 
        new(0, 1), 
        new(-1, -1), 
        new(-1, 0),
        new(-1, 1)
    };
    
    [SerializeField] private KingAttackedManager kingAttackedManager;
    [SerializeField] private GameObject inCheckVisual;

    private bool _hasMoved;
    private Tile _startingTile;

    private new void Awake()
    {
        base.Awake();
        _hasMoved = false;
        _startingTile = tile;
        inCheckVisual.SetActive(false);
        
        kingAttackedManager.OnUpdate += KingAttackedManager_OnUpdate;
    }

    private void KingAttackedManager_OnUpdate(object sender, EventArgs e)
    {
        inCheckVisual.SetActive(kingAttackedManager.KingAttacked());
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

    public override HashSet<Vector2Int> LegalMoves(HashSet<Piece> piecesToIgnore = null)
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();

        Vector2Int position = tile.GetCoordinates();

        HashSet<Piece> piecesToIgnoreCopy = new HashSet<Piece>(piecesToIgnore ?? new HashSet<Piece>()) { this };

        foreach (Vector2Int offset in KingMoves)
        {
            Vector2Int testCoords = position + offset;
            Tile testTile = Board.Instance.GetTile(testCoords);
            
            if (!LegalTile(testTile)) continue;
            if (testTile.IsAttacked(alignment, piecesToIgnoreCopy)) continue;
            legalMoves.Add(testCoords);
        }

        legalMoves.AddRange(KingCastler.Instance.GetAllLegalCastles(this));
        
        return legalMoves;
    }

    public bool IsCastling(Tile selectedTile)
    {
        int xAbsDelta = Mathf.Abs(GetTile().GetCoordinates().x - selectedTile.GetCoordinates().x);
        return xAbsDelta > 1;
    }
}
