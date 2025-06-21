using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileVisual;
    
    private Vector2Int _coordinates;

    private void Awake()
    {
        _coordinates = Vector2Int.zero;
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
    public void SetCoordinates(Vector2Int coordinates) => _coordinates = coordinates;
    public bool CoordinatesSet() => _coordinates != Vector2Int.zero;
}
