using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KingAttackedManager : MonoBehaviour
{
    [SerializeField] private King king;
    [SerializeField] private Piece.Alignment alignment;

    private bool _kingAttacked;
    private List<Piece> _attackingPieces;

    private void Awake()
    {
        _kingAttacked = false;
        _attackingPieces = new List<Piece>();
    }

    private void Start()
    {
        PieceMover.Instance.OnPieceDeselected += PieceMover_OnPieceDeselected;
    }

    private void PieceMover_OnPieceDeselected(object sender, PieceMover.OnPieceEventArgs e)
    {
        HashSet<Piece> attackers = king.GetTile().GetAllAttackers(king.GetAlignment()); 
        _attackingPieces = attackers.ToList();
        _kingAttacked = _attackingPieces.Count > 0;
    }

    public bool KingAttacked() => _kingAttacked;

    // Takes legal moves of a piece and removes ones that would threaten this king; non-destructive
    public HashSet<Vector2Int> FilterLegalMoves(Piece piece, HashSet<Vector2Int> legalMoves)
    {
        HashSet<Vector2Int> filteredLegalMoves = new HashSet<Vector2Int>();

        Piece pinningPiece = GetPinningPiece(piece);
        if (pinningPiece is not null)
        {
            if (_kingAttacked) return filteredLegalMoves;
            filteredLegalMoves = Board.Instance.GetCoordinatesBetweenPoints(
                king.GetTile().GetCoordinates(), 
                pinningPiece.GetTile().GetCoordinates());
            filteredLegalMoves.Add(pinningPiece.GetTile().GetCoordinates());
            filteredLegalMoves.IntersectWith(legalMoves);
            return filteredLegalMoves;
        }
        
        if (!_kingAttacked) return legalMoves;
        if (_attackingPieces.Count > 1) return filteredLegalMoves;
        
        Vector2Int attackingPieceCoordinate = _attackingPieces[0].GetTile().GetCoordinates();
        
        filteredLegalMoves = Board.Instance.GetCoordinatesBetweenPoints(
            king.GetTile().GetCoordinates(),
            attackingPieceCoordinate);
        filteredLegalMoves.Add(attackingPieceCoordinate);
        filteredLegalMoves.IntersectWith(legalMoves);
        
        return filteredLegalMoves;
    }
    
    private Piece GetPinningPiece(Piece blockingPiece)
    {
        Vector2Int direction = Board.GetDirectionToCoordinate(
            king.GetTile().GetCoordinates(), 
            blockingPiece.GetTile().GetCoordinates());
        if (direction == Vector2Int.zero) return null;
        Piece pinningPiece = Board.Instance.FindFirstPieceInDirection(
            king.GetTile().GetCoordinates(), direction, 8, 
            new HashSet<Piece> { blockingPiece });
        if (pinningPiece is null) return null;
        if (!pinningPiece.CanBeCaptured(alignment)) return null;
        if (pinningPiece.gameObject.GetComponent<Queen>() is not null || 
            (pinningPiece.gameObject.GetComponent<Bishop>() is not null && Bishop.Diagonals.Contains(direction)) ||
            (pinningPiece.gameObject.GetComponent<Rook>() is not null && Rook.Orthogonals.Contains(direction)))
        {
            return pinningPiece;
        }

        return null;
    }
}