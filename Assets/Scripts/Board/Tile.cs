using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileVisual;
    
    private Coordinates _coordinates;

    private void Awake()
    {
        _coordinates = new Coordinates(Vector2Int.down);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Coordinates GetCoordinates() => _coordinates;
    public bool CoordinatesSet() => !_coordinates.GetCoordinates().Equals(Vector2Int.down);
}
