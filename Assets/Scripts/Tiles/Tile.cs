using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileVisual;
    
    private Vector2Int _coordinates;

    private void Awake()
    {
        _coordinates = Vector2Int.down;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector2Int GetCoordinates() => _coordinates;
    public void SetCoordinatesX(int x) => _coordinates.x = x;
    public void SetCoordinatesY(int y) => _coordinates.y = y;
    public bool CoordinatesSet() => _coordinates != Vector2Int.down;

    public override string ToString()
    {
        return (char)(_coordinates.x + 97) + (_coordinates.y + 1).ToString();
    }
}
