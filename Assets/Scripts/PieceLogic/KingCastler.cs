using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KingCastler : MonoBehaviour
{
    public static KingCastler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public HashSet<Vector2Int> GetAllLegalCastles(King king)
    {
        HashSet<Vector2Int> legalCastles = new HashSet<Vector2Int>();
        if (king.HasMoved()) return legalCastles;
        HashSet<Rook> castlingRooks = Board.Instance.GetPotentialCastlingRooks(
            king.GetTile().GetCoordinates(), king.GetAlignment());
        foreach (Rook castlingRook in castlingRooks)
        {
            legalCastles.AddRange(GetLegalCastles(king, castlingRook));
        }
        return legalCastles;
    }

    private static HashSet<Vector2Int> GetLegalCastles(King king, Rook rook)
    {
        HashSet<Vector2Int> legalCastles = new HashSet<Vector2Int>();
        
        if (king.HasMoved() 
            || rook.HasMoved()
            || king.GetAlignment() == Piece.Alignment.Neutral
            || king.GetAlignment() != rook.GetAlignment())
        {
            return legalCastles;
        }
        
        Vector2Int kingCoordinates = king.GetTile().GetCoordinates();
        Vector2Int rookCoordinates = rook.GetTile().GetCoordinates();

        int kingXMoveDirection = kingCoordinates.x < rookCoordinates.x ? 1 : -1;

        Tile tileOneAwayFromKing = Board.Instance.GetTile(kingCoordinates + Vector2Int.right * kingXMoveDirection); 
        if (king.GetTile().IsAttacked(king.GetAlignment())
            || tileOneAwayFromKing.HasPiece()
            || tileOneAwayFromKing.IsAttacked(king.GetAlignment()))
        {
            return legalCastles;
        }
        
        Vector2Int startSearchCoordinate = kingCoordinates + Vector2Int.right * 2 * kingXMoveDirection;
        Vector2Int endSearchCoordinate = rookCoordinates + Vector2Int.left * kingXMoveDirection;

        List<Tile> tilesToSearch = new List<Tile>();
        
        for (int x = startSearchCoordinate.x; 
             x != endSearchCoordinate.x + kingXMoveDirection; 
             x += kingXMoveDirection)
        {
            tilesToSearch.Add(Board.Instance.GetTile(new Vector2Int(x, kingCoordinates.y)));
        }
        
        if (tilesToSearch.Any(tile => tile.HasPiece()))
        {
            return legalCastles;
        }

        foreach (Tile tile in tilesToSearch)
        {
            if (tile.IsAttacked(king.GetAlignment())) return legalCastles;
            legalCastles.Add(tile.GetCoordinates());
        }
        
        return legalCastles;
    }

    // Assumes that GetAllLegalCastles() has already been ran
    public void CastleKing(King king, Vector2Int targetCoordinates)
    {
        int kingXMoveDirection = king.GetTile().GetCoordinates().x < targetCoordinates.x ? 1 : -1;
        Rook rook = Board.Instance.FindFirstPieceInDirection(
            king.GetTile().GetCoordinates(),
            Vector2Int.right * kingXMoveDirection, 
            Board.Instance.GetSize())
            .gameObject.GetComponent<Rook>();
        
        Tile newKingTile = Board.Instance.GetTile(targetCoordinates);
        Tile newRookTile = Board.Instance.GetTile(targetCoordinates + (Vector2Int.left * kingXMoveDirection));
        
        king.SetTile(newKingTile);
        rook.SetTile(newRookTile);
        rook.CentreOnTile();
    }
}
