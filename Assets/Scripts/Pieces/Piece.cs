using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public enum Alignment
    {
        Neutral,
        White,
        Black
    }
    
    [SerializeField] protected Tile tile;
    [SerializeField] protected Alignment alignment;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        transform.position = tile.transform.position;
        tile.SetPiece(this);
    }

    // Update is called once per frame
    protected abstract void Update();

    public abstract List<Vector2Int> LegalMoves();

    public void SetAlignment(Alignment alignment) => this.alignment = alignment;

    public void MoveTile(Tile newTile)
    {
        if (tile == newTile) return;
        tile.SetPiece(null);
        tile = newTile;
        newTile.SetPiece(this);
    }

    public bool CanBeMoved(Alignment alignmentMoving)
    {
        if (alignment == Alignment.Neutral)
        {
            return false;
        }
        
        return (alignmentMoving == alignment); 
    }

    public bool CanBeCaptured(Alignment alignmentCapturing)
    {
        if (alignmentCapturing == Alignment.Neutral || alignment == Alignment.Neutral)
        {
            return false;
        }
        
        return (alignmentCapturing != alignment);
    }
}
