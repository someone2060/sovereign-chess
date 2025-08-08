using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    protected bool Equals(Piece other)
    {
        return base.Equals(other) && Equals(tile, other.tile) && alignment == other.alignment && Equals(sovereignPiece, other.sovereignPiece);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Piece)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), tile, (int)alignment, sovereignPiece);
    }

    public enum Alignment
    {
        Neutral,
        White,
        Black
    }
    
    [SerializeField] protected Tile tile;
    [SerializeField] protected Alignment alignment;
    [SerializeField] protected SovereignPieceSO sovereignPiece;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    private bool _selected;

    public abstract void InitializeSprite();
    public abstract HashSet<Vector2Int> LegalMoves(HashSet<Piece> piecesToIgnore = null);
    
    protected void Awake()
    {
        _selected = false;
        InitializeSprite();
    }

    protected void Start()
    {
        if (tile is null) return;
        transform.position = tile.transform.position;
        tile.SetPiece(this);
    }

    protected void Update()
    {
        if (!_selected) return;
        transform.position = InputHandler.Instance.GetPositionWorld(Camera.main);
    }

    public virtual void DestroySelf()
    {
        tile?.SetPiece(null);
        Destroy(gameObject);
    }

    public Alignment GetAlignment() => alignment;
    public void SetAlignment(Alignment alignment) => this.alignment = alignment;

    public SovereignPieceSO GetSovereignPiece() => sovereignPiece;
    public void SetSovereignPiece(SovereignPieceSO sovereignPiece) => this.sovereignPiece = sovereignPiece;
    
    public Tile GetTile() => tile;
    public void SetTile(Tile newTile)
    {
        if (tile == newTile) return;
        tile?.SetPiece(null);
        tile = newTile;
        newTile.SetPiece(this);
    }
    
    public void SetSelected(bool selected) => _selected = selected;

    public void CentreOnTile() => transform.position = tile.transform.position;
    public void CentreOnTile(Tile selectedTile) => transform.position = selectedTile.transform.position;

    public bool CanBeMoved(Alignment alignmentMoving)
    {
        if (alignment == Alignment.Neutral)
        {
            return false;
        }
        
        return (alignmentMoving == alignment); 
    }

    public bool CanBeCaptured(Alignment alignmentCapturing)
    {
        if (alignmentCapturing == Alignment.Neutral || alignment == Alignment.Neutral)
        {
            return false;
        }
        
        return alignmentCapturing != alignment;
    }
    
    // Extends 8 tiles in search direction until colliding with another piece or reaching end of board,
    // returning valid squares that can be occupied (including capturing)
    protected HashSet<Vector2Int> SearchLegalTilesInDirection(
        Vector2Int start, Vector2Int dir, HashSet<Piece> piecesToIgnore = null)
    {
        Vector2Int increment = new Vector2Int(0, 0);
        Vector2Int endCoordinates = start + dir * 8;
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();

        Piece testPiece = Board.Instance.FindFirstPieceInDirection(start, dir, 8, piecesToIgnore);
        if (testPiece is not null)
        {
            endCoordinates = testPiece.GetTile().GetCoordinates();
        }
        
        while (!endCoordinates.Equals(start + increment))
        {
            increment += dir;
            Vector2Int testCoordinate = start + increment;
            Tile testTile = Board.Instance.GetTile(testCoordinate);

            if (testTile is not null && piecesToIgnore is not null)
            {
                if (testTile.HasPiece() && piecesToIgnore.Contains(testTile.GetPiece()))
                {
                    legalMoves.Add(testCoordinate);
                    continue;
                }
            }
            if (!LegalTile(testTile)) break;
            
            legalMoves.Add(testCoordinate);
        }
        
        return legalMoves;
    }
    
    // Returns true/false depending on whether the square can be occupied,
    // with capturing and moving optionally disabled
    protected bool LegalTile(Tile testTile, bool canMove = true, bool canCapture = true)
    {
        if (testTile is null) return false;
        if (!testTile.HasPiece() && canMove) return true;
        if (testTile.HasPiece() && canCapture)
        {
            return testTile.GetPiece().CanBeCaptured(alignment);
        }

        return false;
    }
}
