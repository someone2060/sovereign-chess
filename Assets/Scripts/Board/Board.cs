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
            rows[i].SetTilesCoordinateX(i);
        }
    }

    public Tile GetTile(Vector2Int coordinates)
    {
        return !ValidCoordinates(coordinates) ? null : rows[coordinates.x].GetTile(coordinates.y);
    }

    private bool ValidCoordinates(Vector2Int coordinates)
    {
        return coordinates.y >= 0 && coordinates.y < _size && coordinates.x >= 0 && coordinates.x < _size;
    }
}
