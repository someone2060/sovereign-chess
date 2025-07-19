using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingCastler : MonoBehaviour
{
    public static KingCastler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public HashSet<Vector2Int> GetLegalCastles(King king, Rook rook)
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

    // Assumes that GetLegalCastles() has already been ran
    public void CastleKing(King king, Rook rook, Vector2Int targetCoordinates)
    {
        int kingXMoveDirection = king.GetTile().GetCoordinates().x < rook.GetTile().GetCoordinates().x ? 1 : -1;
        Tile newKingTile = Board.Instance.GetTile(targetCoordinates);
        Tile newRookTile = Board.Instance.GetTile(targetCoordinates + (Vector2Int.right * kingXMoveDirection));
        king.SetTile(newKingTile);
        rook.SetTile(newRookTile);
    }
}
