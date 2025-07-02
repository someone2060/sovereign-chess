using System;
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
    [SerializeField] protected SovereignPieceSO sovereignPiece;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected bool _selected;

    protected void Awake()
    {
        _selected = false;
    }

    protected void Start()
    {
        transform.position = tile.transform.position;
        tile.SetPiece(this);
    }

    // TODO
    protected void Update()
    {
        if (!_selected) return;
        transform.position = InputHandler.Instance.GetPositionWorld(Camera.main);
    }
    
    public Coordinates GetCoordinates() => tile.GetCoordinates();

    public abstract HashSet<Vector2Int> LegalMoves();

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

    public void Select()
    {
        _selected = true;
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        InputHandler.Instance.OnSelectCanceled -= InputHandler_OnSelectCanceled;
        Tile selectedTile = TileSelector.GetTileOnWorld(transform.position);
        _selected = false;

        do
        {
            if (selectedTile is null) break;
            if (selectedTile.HasPiece() && selectedTile.GetPiece() != this)
            {
                selectedTile.DestroyPiece();
            }
            
            MoveTile(selectedTile);
        } while (false);
        
        transform.position = tile.transform.position;
    }
}
