using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private List<Row> rows;

    private int _size;

    private void Awake()
    {
        _size = rows.Count;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < rows.Count; i++)
        {
            rows[i].SetTilesCoordinateX(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tile GetTile(Vector2Int coordinates) => rows[coordinates.x].GetTile(coordinates.y);

    public bool ValidCoordinates(Vector2Int coordinates)
    {
        return coordinates.y >= 0 && coordinates.y <= _size && coordinates.x >= 0 && coordinates.x <= _size;
    }
}
