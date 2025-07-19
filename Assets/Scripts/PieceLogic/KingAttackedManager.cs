using System.Collections.Generic;
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
        _attackingPieces = king.GetTile().GetAllAttackers(king.GetAlignment());
        _kingAttacked = _attackingPieces.Count > 0;
    }

    public bool KingAttacked() => _kingAttacked;

    public HashSet<Vector2Int> FilterLegalMoves(HashSet<Vector2Int> legalMoves)
    {
        HashSet<Vector2Int> filteredLegalMoves = new HashSet<Vector2Int>(legalMoves);
        if (!_kingAttacked) return filteredLegalMoves;
        if (_attackingPieces.Count > 1) return new HashSet<Vector2Int>();
        
        Vector2Int attackingPieceCoordinate = _attackingPieces[0].GetTile().GetCoordinates();
        foreach (Vector2Int legalMove in filteredLegalMoves)
        {
            if (legalMove == attackingPieceCoordinate) continue;
            filteredLegalMoves.Remove(legalMove);
        }
        //TODO
        
        return filteredLegalMoves;
    }
}