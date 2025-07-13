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
        
        if (king.HasMoved() || rook.HasMoved() 
                            || king.GetAlignment() == Piece.Alignment.Neutral 
                            || king.GetAlignment() != rook.GetAlignment())
        {
            return legalCastles;
        }

        Vector2Int kingCoordinates = king.GetTile().GetCoordinates();
        Vector2Int rookCoordinates = rook.GetTile().GetCoordinates();
        
        int leftX = kingCoordinates.x < rookCoordinates.x
            ? kingCoordinates.x : rookCoordinates.x;
        int rightX = kingCoordinates.x > rookCoordinates.x
            ? kingCoordinates.x : rookCoordinates.x;
        
        List<Tile> tilesToSearch = new List<Tile>();

        for (int x = leftX; x < rightX; x++)
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

    public void CastleKing(King king, Rook rook, Vector2Int targetCoordinates)
    {
        // TODO
    }
}

public class InvalidCastleX : Exception
{
}
