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
    
    public Tile GetTile(Coordinates coords)
    {
        return !ValidCoordinates(coords) ? null : rows[coords.GetY()].GetTile(coords.GetX());
    }

    // Returns a Vector2Int pointing to the closest quadrant from the center
    // with range from (-1, -1) to (1, 1)
    public Coordinates GetQuadrant(Coordinates coords)
    {
        Coordinates quadrant = new Coordinates(0, 0);

        quadrant.SetX((coords.GetX() < _size / 2) ? 1 : -1);
        quadrant.SetY((coords.GetY() < _size / 2) ? 1 : -1);
        
        return quadrant;
    }

    public bool OnOuterTwoX(Coordinates coords)
    {
        return coords.GetX() < 2 || coords.GetX() >= _size - 2;
    }

    public bool OnOuterTwoY(Coordinates coords)
    {
        return coords.GetY() < 2 || coords.GetY() >= _size - 2;
    }

    private bool ValidCoordinates(Coordinates coords)
    {
        return coords.GetY() >= 0 && coords.GetY() < _size && coords.GetX() >= 0 && coords.GetX() < _size;
    }

    public bool InPromotionArea(Coordinates coords)
    {
        int leftBound = _size / 2 - 2;
        int rightBound = _size / 2 + 2;
        return coords.GetX() >= leftBound 
               && coords.GetX() < rightBound 
               && coords.GetY() >= leftBound
               && coords.GetY() < rightBound;
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
