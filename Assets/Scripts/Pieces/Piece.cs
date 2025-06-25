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
    
    [SerializeField] private Tile tile;
    [SerializeField] private Alignment alignment;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        transform.position = tile.transform.position;
    }

    // Update is called once per frame
    protected abstract void Update();

    public abstract List<Vector2Int> LegalMoves();

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
