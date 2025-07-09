using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileVisual;
    [SerializeField] private Piece piece;
    
    private Coordinates _coordinates;

    private void Awake()
    {
        _coordinates = new Coordinates(Vector2Int.down);
    }

    public Coordinates GetCoordinates() => _coordinates;
    public void SetCoordinates(Coordinates coordinates)
    {
        _coordinates = new Coordinates(coordinates);
    }
    public bool CoordinatesSet() => !_coordinates.GetVector2Int().Equals(Vector2Int.down);
    
    public bool HasPiece() => piece is not null;
    public Piece GetPiece() => piece;

    public void SetPiece(Piece newPiece)
    {
        if (newPiece is null)
        {
            piece = null;
            return;
        }
        if (piece == newPiece) return;
        piece = newPiece;
        newPiece.SetTile(this);
    }

    public void DestroyPiece()
    {
        Destroy(piece.gameObject);
        piece = null;
    }
}
