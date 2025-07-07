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
    public Vector2Int GetQuadrant(Vector2Int coords)
    {
        Vector2Int quadrant = Vector2Int.zero;

        quadrant.x = (coords.x < _size / 2) ? 1 : -1;
        quadrant.y = (coords.y < _size / 2) ? 1 : -1;
        
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
    
    public Tile GetTile(Vector2Int coords)
    {
        return !ValidCoordinates(coords) ? null : rows[coords.y].GetTile(coords.x);
    }

    private bool ValidCoordinates(Vector2Int coords)
    {
        return coords.y >= 0 && coords.y < _size && coords.x >= 0 && coords.x < _size;
    }
}
