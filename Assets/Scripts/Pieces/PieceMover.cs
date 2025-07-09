using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceMover : MonoBehaviour
{
    public static PieceMover Instance { get; private set; }

    private Piece _piece;
    private HashSet<Vector2Int> _legalMoves;
    
    public event EventHandler<OnPieceSelectedEventArgs> OnPieceSelected;
    public class OnPieceSelectedEventArgs : EventArgs { public HashSet<Vector2Int> legalMoves; }
    
    public event EventHandler<OnPieceSelectedEventArgs> OnPieceDeselected;
    
    private void Start()
    {
        InputHandler.Instance.OnSelectPerformed += InputHandler_OnSelectPerformed;
        Instance = this;
    }
    
    // On selecting a tile with a piece on it, selection event is sent to piece
    private void InputHandler_OnSelectPerformed(object sender, EventArgs e)
    {
        Vector2 positionWorld = InputHandler.Instance.GetPositionWorld(Camera.main);

        Tile tile = TileSelector.GetTileOnWorld(positionWorld);
        if (tile is null) return;
        
        if (!tile.HasPiece()) return;

        Piece piece = tile.GetPiece();
        SelectPiece(piece);
    }

    private void SelectPiece(Piece piece)
    {
        _piece = piece;
        _piece.SetSelected(true);
        _legalMoves = _piece.LegalMoves();
        
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
        
        OnPieceSelected?.Invoke(this, new OnPieceSelectedEventArgs { legalMoves = _legalMoves });
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        InputHandler.Instance.OnSelectCanceled -= InputHandler_OnSelectCanceled;
        Tile selectedTile = TileSelector.GetTileOnWorld(InputHandler.Instance.GetPositionWorld(Camera.main));
        _piece.SetSelected(false);
        
        do
        {
            if (selectedTile is null) break; // no tile on where user stopped selecting 
            if (!_legalMoves.Contains(selectedTile.GetCoordinates().GetVector2Int())) break; // invalid movement square
            if (selectedTile.HasPiece() && selectedTile.GetPiece() != _piece) // moved to a different tile that has another piece
            {
                selectedTile.DestroyPiece();
            }

            _piece.SetTile(selectedTile);
        } while (false);
        
        _piece.CentreOnTile();
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
