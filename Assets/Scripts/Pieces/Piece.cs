using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
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
    protected Tile legalMovesCheckTile;
    protected HashSet<Vector2Int> legalMoves;

    protected void Awake()
    {
        _selected = false;
    }

    protected void Start()
    {
        transform.position = tile.transform.position;
        tile.SetPiece(this);
    }

    protected void Update()
    {
        if (!_selected) return;
        transform.position = InputHandler.Instance.GetPositionWorld(Camera.main);
    }
    
    public Coordinates GetCoordinates() => tile.GetCoordinates();

    public virtual HashSet<Vector2Int> LegalMoves()
    {
        legalMovesCheckTile = tile;
        return null;
    }

    public void SetAlignment(Alignment alignment) => this.alignment = alignment;

    public void MoveTile(Tile newTile)
    {
        if (tile == newTile) return;
        tile.SetPiece(null);
        tile = newTile;
        newTile.SetPiece(this);
    }

    public bool CanBeMoved(Alignment alignmentMoving)
    {
        if (alignment == Alignment.Neutral)
        {
            return false;
        }
        
        return (alignmentMoving == alignment); 
    }

    private bool CanBeCaptured(Alignment alignmentCapturing)
    {
        if (alignmentCapturing == Alignment.Neutral || alignment == Alignment.Neutral)
        {
            return false;
        }
        
        return (alignmentCapturing != alignment);
    }

    public void Select()
    {
        _selected = true;
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        InputHandler.Instance.OnSelectCanceled -= InputHandler_OnSelectCanceled;
        Tile selectedTile = TileSelector.GetTileOnWorld(transform.position);
        _selected = false;

        do
        {
            if (selectedTile is null) break; // no tile on where user stopped selecting 
            if (!LegalMoves().Contains(selectedTile.GetCoordinates().GetVector2Int())) break; // invalid movement square
            if (selectedTile.HasPiece() && selectedTile.GetPiece() != this) // moved to a different tile that has another piece
            {
                selectedTile.DestroyPiece();
            }
            
            MoveTile(selectedTile);
        } while (false);
        
        transform.position = tile.transform.position;
    }

    // Extends 8 tiles in search direction until colliding with another piece or reaching end of board,
    // returning valid squares that can be occupied (including capturing)
    protected HashSet<Vector2Int> SearchLegalTilesInDirection(Vector2Int coordsVec, Vector2Int dir)
    {
        Vector2Int incrementVec = Vector2Int.zero;
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        for (int i = 0; i < 8; i++)
        {
            incrementVec += dir;
            Tile testTile = Board.Instance.GetTile(coordsVec + incrementVec);

            if (!LegalTile(testTile)) break;
            
            legalMoves.Add(coordsVec + incrementVec);

            if (testTile.HasPiece()) break;
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
