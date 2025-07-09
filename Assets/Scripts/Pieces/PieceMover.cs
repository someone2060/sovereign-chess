using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceMover : MonoBehaviour
{
    public static PieceMover Instance { get; private set; }

    private Piece _piece;
    private HashSet<Vector2Int> _legalMoves;
    private bool _selectedSameTile;
    
    public event EventHandler<OnPieceSelectedEventArgs> OnPieceSelected;
    public class OnPieceSelectedEventArgs : EventArgs { public HashSet<Vector2Int> legalMoves; }
    
    public event EventHandler<OnPieceSelectedEventArgs> OnPieceDeselected;

    private void Awake()
    {
        _selectedSameTile = false;
        Instance = this;
    }
    
    private void Start()
    {
        InputHandler.Instance.OnSelectPerformed += InputHandler_OnSelectPerformed;
    }
    
    // On selecting a tile with a piece on it, selection event is sent to piece
    private void InputHandler_OnSelectPerformed(object sender, EventArgs e)
    {
        Vector2 positionWorld = InputHandler.Instance.GetPositionWorld(Camera.main);

        Tile tile = TileSelector.GetTileOnWorld(positionWorld);
        if (tile is null) return;

        if (!tile.HasPiece()) return;

        Piece piece = tile.GetPiece();

        if (_selectedSameTile && piece.Equals(_piece))
        {
            _piece.SetSelected(true);
            return;
        }
        
        SelectPiece(piece);
    }

    private void SelectPiece(Piece piece)
    {
        if (_selectedSameTile)
        {
            InputHandler_OnSelectCanceled(this, EventArgs.Empty);
        }
        
        _piece = piece;
        _piece.SetSelected(true);
        _legalMoves = _piece.LegalMoves();
        _selectedSameTile = false;
        
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
        
        OnPieceSelected?.Invoke(this, new OnPieceSelectedEventArgs { legalMoves = _legalMoves });
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        Tile selectedTile = TileSelector.GetTileOnWorld(InputHandler.Instance.GetPositionWorld(Camera.main));
        _piece.SetSelected(false);

        do
        {
            if (selectedTile is null) break; // no tile on where user stopped selecting
            
            Debug.Log("selectedSameTile: " + _selectedSameTile);
            // selected tile is same as tile piece is on; not done before
            if (selectedTile.Equals(_piece.GetTile()) && !_selectedSameTile)
            {
                Debug.Log("changing selectedSameTile");
                _selectedSameTile = true;
                break;
            }
            
            TryMovePiece(selectedTile);
        } while (false);
        
        _piece.CentreOnTile();
    }

    private void TryMovePiece(Tile selectedTile)
    {
        InputHandler.Instance.OnSelectCanceled -= InputHandler_OnSelectCanceled;
        _selectedSameTile = false;
        
        do
        {
            if (!_legalMoves.Contains(selectedTile.GetCoordinates().GetVector2Int())) break; // invalid movement square
            if (selectedTile.HasPiece() && selectedTile.GetPiece() != _piece) // moved to a different tile that has another piece
            {
                selectedTile.DestroyPiece();
            }

            _piece.SetTile(selectedTile);
        } while (false);
        
        OnPieceDeselected?.Invoke(this, null);        
    }

    private void DebugLegalMoves()
    {
        List<string> coordStr = _legalMoves.Select(
            legalMove => Board.Instance.GetTile(legalMove).GetCoordinates().ToString()).ToList();
        coordStr.Sort();
        
        String debugString = coordStr.Aggregate(
            "Legal moves: ", (current, coords) => current + coords + ", ");
        Debug.Log(debugString);
    }
}
