using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PieceMover : MonoBehaviour
{
    private enum State
    {
        Unselected,
        DragSelecting,
        ClickSelecting,
        PieceChanging
    }
    
    public static PieceMover Instance { get; private set; }

    [SerializeField] private Rook debugRook; // TODO DEBUG  

    private Piece _piece;
    private HashSet<Vector2Int> _legalMoves;
    private State _state;
    
    public event EventHandler<OnPieceEventArgs> OnPieceSelected;
    
    public event EventHandler<OnPieceEventArgs> OnPieceDeselected;
    public class OnPieceEventArgs : EventArgs
    {
        public HashSet<Vector2Int> legalMoves;
        public Piece piece;
    }

    private void Awake()
    {
        Instance = this;
        _state = State.Unselected;
    }
    
    private void Start()
    {
        InputHandler.Instance.OnSelectPerformed += InputHandler_OnSelectPerformed;
        PawnPromoter.Instance.OnPawnPromotion += PawnPromoter_OnPawnPromotion;
    }
    
    private void InputHandler_OnSelectPerformed(object sender, EventArgs e)
    {
        if (_state == State.PieceChanging) return;
        
        Vector2 positionWorld = InputHandler.Instance.GetPositionWorld(Camera.main);

        Tile tile = TileSelector.GetTileOnWorld(positionWorld);
        if (tile is null) return;

        if (!tile.HasPiece()) return;

        Piece piece = tile.GetPiece();

        if (_state == State.ClickSelecting && piece.Equals(_piece))
        {
            _piece.SetSelected(true);
            return;
        }
        
        SelectPiece(piece);
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        Tile selectedTile = TileSelector.GetTileOnWorld(InputHandler.Instance.GetPositionWorld(Camera.main));
        _piece.SetSelected(false);

        do
        {
            if (selectedTile is null) break;
            
            if (selectedTile.Equals(_piece.GetTile()) && _state == State.DragSelecting)
            {
                _state = State.ClickSelecting;
                break;
            }
            
            TryMovePiece(selectedTile);
        } while (false);
        
        _piece.CentreOnTile();
    }

    private void PawnPromoter_OnPawnPromotion(object sender, PawnPromoter.OnPawnPromotionEventArgs e)
    {
        SetPieceTile(e.tile);
        _piece.CentreOnTile();
    }

    private void SelectPiece(Piece piece)
    {
        if (_state == State.ClickSelecting)
        {
            InputHandler_OnSelectCanceled(this, EventArgs.Empty);
        }
        
        _piece = piece;
        _piece.SetSelected(true);
        _legalMoves = _piece.LegalMoves();
        _state = State.DragSelecting;
        
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
        
        OnPieceSelected?.Invoke(this, new OnPieceEventArgs
        {
            legalMoves = _legalMoves,
            piece = _piece
        });
    }

    private void TryMovePiece(Tile selectedTile)
    {
        InputHandler.Instance.OnSelectCanceled -= InputHandler_OnSelectCanceled;
        
        if (!_legalMoves.Contains(selectedTile.GetCoordinates()))
        {
            SetPieceTile(_piece.GetTile());
            return;
        }
        
        Pawn pawn = _piece.GetComponent<Pawn>();
        if (pawn is not null)
        {
            if (pawn.CanPromote(selectedTile))
            {
                _piece.CentreOnTile(selectedTile);
                _state = State.PieceChanging;
                PawnPromoter.Instance.PromptPawnPromotion(pawn, selectedTile);
                return;
            }
        }

        King king = _piece.gameObject.GetComponent<King>();
        if (king is not null)
        {
            if (king.IsCastling(selectedTile))
            {
                KingCastler.Instance.CastleKing(king, selectedTile.GetCoordinates());
            }
        }
        
        SetPieceTile(selectedTile);
    }

    private void SetPieceTile(Tile selectedTile)
    {
        _state = State.Unselected;
        OnPieceDeselected?.Invoke(this, new OnPieceEventArgs
        {
            legalMoves = null,
            piece = _piece 
        });
        
        if (selectedTile is null) return;
        
        if (selectedTile.HasPiece() && !selectedTile.GetPiece().Equals(_piece))
        {
            selectedTile.DestroyPiece();
        }

        _piece.SetTile(selectedTile);
    }

    // used like DebugDisplayHashSetCoords("Legal moves:", _legalMoves);
    private void DebugDisplayHashSetCoords(string preamble, HashSet<Vector2Int> coordinates)
    {
        List<string> coordStr = coordinates.Select(
            legalCastle => CoordinatesToString.Convert(
                Board.Instance.GetTile(legalCastle).GetCoordinates())).ToList();
        coordStr.Sort();
        
        String debugString = coordStr.Aggregate(
            preamble, (current, coords) => current + coords + ", ");
        Debug.Log(debugString);
    }
}
