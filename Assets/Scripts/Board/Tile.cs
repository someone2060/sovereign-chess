using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileVisual;
    [SerializeField] private Piece piece;
    
    private Vector2Int _coordinates;

    private void Awake()
    {
        _coordinates = Vector2Int.down;
    }

    public Vector2Int GetCoordinates() => _coordinates;
    public void SetX(int x) => _coordinates.x = x;
    public void SetY(int y) => _coordinates.y = y;
    
    public bool HasPiece() => piece is not null;
    public Piece GetPiece() => piece;

    public void SetPiece(Piece newPiece)
    {
        if (newPiece is null)
        {
            piece = null;
            return;
        }
        if (piece == newPiece) return;
        piece = newPiece;
        newPiece.SetTile(this);
    }

    public void DestroyPiece()
    {
        Destroy(piece.gameObject);
        piece = null;
    }
  
    public bool IsAttacked(Piece.Alignment alignment)
    {
        return GetAllAttackers(alignment).Count > 0;
    }

    // Returns all pieces targeting this tile hostile to the inputted alignment;
    // no pieces can ever attack neutral alignment, and white/black alignments are hostile to each other
    public List<Piece> GetAllAttackers(Piece.Alignment alignment)
    {
        List<Piece> attackers = new List<Piece>();
        Piece testPiece;
        
        if (alignment == Piece.Alignment.Neutral) return attackers;
        
        // Search for rooks, queens along orthogonals
        foreach (Vector2Int direction in Rook.Orthogonals)
        {
            testPiece = Board.Instance.FindFirstPieceInDirection(GetCoordinates(), direction, 8);
            if (testPiece?.gameObject.GetComponent<Queen>() is null 
                && testPiece?.gameObject.GetComponent<Rook>() is null) continue;
            if (!testPiece.CanBeCaptured(alignment)) continue;
            if (!testPiece.LegalMoves().Contains(_coordinates)) continue;
            attackers.Add(testPiece);
        }
        
        // Search for bishops, queens along diagonals
        foreach (Vector2Int direction in Bishop.Diagonals)
        {
            testPiece = Board.Instance.FindFirstPieceInDirection(GetCoordinates(), direction, 8);
            if (testPiece?.gameObject.GetComponent<Queen>() is null
                && testPiece?.gameObject.GetComponent<Bishop>() is null) continue;
            if (!testPiece.CanBeCaptured(alignment)) continue;
            if (!testPiece.LegalMoves().Contains(_coordinates)) continue;
            attackers.Add(testPiece);
        }

        // Search for knights
        foreach (Vector2Int knightMove in Knight.KnightMoves)
        {
            Tile testTile = Board.Instance.GetTile(_coordinates + knightMove);
            testPiece = testTile?.GetPiece();
            if (testPiece?.gameObject.GetComponent<Knight>() is null) continue;
            if (!testPiece.CanBeCaptured(alignment)) continue;
            if (!testPiece.LegalMoves().Contains(_coordinates)) continue;
            attackers.Add(testPiece);
        }
        
        // Search for pawns
        foreach (Vector2Int direction in Bishop.Diagonals)
        {
            testPiece = Board.Instance.FindFirstPieceInDirection(GetCoordinates(), direction, 1);
            if (testPiece?.gameObject.GetComponent<Pawn>() is null) continue;
            if (!testPiece.CanBeCaptured(alignment)) continue;
            if (!testPiece.LegalMoves().Contains(_coordinates)) continue;
            attackers.Add(testPiece);
        }

        return attackers;
    }
}
    