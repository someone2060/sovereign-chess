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

    // Returns true if there are any pieces targeting this tile hostile to the inputted alignment;
    // no pieces can ever attack neutral alignment, and white/black alignments are hostile to each other  
    public bool IsAttacked(Piece.Alignment alignment)
    {
        if (alignment == Piece.Alignment.Neutral) return false;
        throw new System.NotImplementedException();
    }
}
