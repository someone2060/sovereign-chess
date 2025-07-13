using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    
    [SerializeField] private List<Row> rows;

    private int _size;

    private void Awake()
    {
        Instance = this;
        _size = rows.Count; // Assumes each row's # of tiles is the same size as rows.Count, forming a square board
    }

    private void Start()
    {
        for (int i = 0; i < rows.Count; i++)
        {
            rows[i].SetTilesCoordinateY(i);
        }
    }
    
    public Tile GetTile(Vector2Int coords)
    {
        return !ValidCoordinates(coords) ? null : rows[coords.y].GetTile(coords.x);
    }

    // Returns a Vector2Int pointing to the closest quadrant from the center
    // with range from (-1, -1) to (1, 1)
    public Vector2Int GetQuadrant(Vector2Int coords)
    {
        Vector2Int quadrant = new Vector2Int
        {
            x = (coords.x < _size / 2) ? 1 : -1,
            y = (coords.y < _size / 2) ? 1 : -1
        };

        return quadrant;
    }

    public bool OnOuterTwoX(Vector2Int coords)
    {
        return coords.x < 2 || coords.x >= _size - 2;
    }

    public bool OnOuterTwoY(Vector2Int coords)
    {
        return coords.y < 2 || coords.y >= _size - 2;
    }

    private bool ValidCoordinates(Vector2Int coords)
    {
        return coords.y >= 0 && coords.y < _size && coords.x >= 0 && coords.x < _size;
    }

    public bool InPromotionArea(Vector2Int coords)
    {
        int leftBound = _size / 2 - 2;
        int rightBound = _size / 2 + 2;
        return coords.x >= leftBound 
               && coords.x < rightBound 
               && coords.y >= leftBound
               && coords.y < rightBound;
    }

    public Piece FindFirstPieceInDirection()// TODO
    {
        // Vector2Int incrementVec = Vector2Int.zero;
        // HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        // for (int i = 0; i < 8; i++)
        // {
        //     incrementVec += dir;
        //     Tile testTile = Board.Instance.GetTile(coordsVec + incrementVec);
        //
        //     if (!LegalTile(testTile)) break;
        //     
        //     legalMoves.Add(coordsVec + incrementVec);
        //
        //     if (testTile.HasPiece()) break;
        // }
        return null;
    }
}
