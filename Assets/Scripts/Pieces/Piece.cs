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

    private bool _selected;

    private void Awake()
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

    public void SelectPiece()
    {
        _selected = true;
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        var selectedTile = TileSelector.GetTileOnWorld(transform.position);
        _selected = false;

        if (selectedTile is null)
        {
            transform.position = tile.transform.position;
            return;
        }
        
        MoveTile(selectedTile);
    }
}
