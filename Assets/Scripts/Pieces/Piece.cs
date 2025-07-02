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

    protected bool _selected;
    private bool _pSelectedDebug;

    protected void Awake()
    {
        _selected = false;
        _pSelectedDebug = false;
    }

    protected void Start()
    {
        transform.position = tile.transform.position;
        tile.SetPiece(this);
    }

    // TODO
    protected void Update()
    {
        if (!_selected) return;
        transform.position = InputHandler.Instance.GetPositionWorld(Camera.main);
    }
    
    public Coordinates GetCoordinates() => tile.GetCoordinates();

    public abstract HashSet<Vector2Int> LegalMoves();

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

    public bool CanBeCaptured(Alignment alignmentCapturing)
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
        //TODO DEBUG
        if (_selected && !_pSelectedDebug)
        {
            HashSet<Vector2Int> legalMoves = LegalMoves();
            List<string> coordStr = legalMoves.Select(legalMove => Board.Instance.GetTile(legalMove).GetCoordinates().ToString()).ToList();
            coordStr.Sort();
            
            String debugString = coordStr.Aggregate("Legal moves: ", (current, coords) => current + coords + ", ");
            Debug.Log(debugString);
        }

        _pSelectedDebug = _selected;
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

    // Extends 8 tiles in search direction until colliding with another piece or reaching end of board
    protected HashSet<Vector2Int> SearchInDirection(Vector2Int coordsVec, Vector2Int dir)
    {
        Vector2Int incrementVec = Vector2Int.zero;
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        for (int i = 0; i < 8; i++)
        {
            incrementVec += dir;
            Tile testTile = Board.Instance.GetTile(coordsVec + incrementVec);
            if (testTile is null) break; // end of board, searching illegal tile
            if (!testTile.HasPiece()) // no piece on checked tile
            {
                legalMoves.Add(coordsVec + incrementVec);
                continue;
            }

            if (testTile.GetPiece().CanBeCaptured(alignment)) // piece on checked tile can be captured
            {
                legalMoves.Add(coordsVec + incrementVec);
            }

            // piece on checked tile can't be captured
            break;
        }
        return legalMoves;
    }
}
