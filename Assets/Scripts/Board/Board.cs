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

    // Returns a Vector2Int pointing to the closest quadrant from the center
    // with range from (-1, -1) to (1, 1)
    public Vector2Int GetQuadrant(Vector2Int pos)
    {
        Vector2Int quadrant = Vector2Int.zero;

        quadrant.x = (pos.x < _size / 2) ? 1 : -1;
        quadrant.y = (pos.y < _size / 2) ? 1 : -1;
        
        return quadrant;
    }

    public Tile GetTile(Vector2Int coordinates)
    {
        return !ValidCoordinates(coordinates) ? null : rows[coordinates.y].GetTile(coordinates.x);
    }

    private bool ValidCoordinates(Vector2Int coordinates)
    {
        return coordinates.y >= 0 && coordinates.y < _size && coordinates.x >= 0 && coordinates.x < _size;
    }
}
