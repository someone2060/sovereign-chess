using System;
using UnityEngine;

public class TileVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer selectedEmptyVisual;
    [SerializeField] private SpriteRenderer selectedPieceVisual;
    [SerializeField] private SpriteRenderer highlightVisual;
    [SerializeField] private Tile tile;
    
    private void Start()
    {
        HideSelectVisuals();
        PieceMover.Instance.OnPieceSelected += PieceMover_OnPieceSelected;
        PieceMover.Instance.OnPieceDeselected += PieceMover_OnPieceDeselected;
    }

    private void PieceMover_OnPieceSelected(object sender, PieceMover.OnPieceEventArgs e)
    {
        if (e.piece.GetTile().Equals(tile))
        {
            highlightVisual.gameObject.SetActive(true);
            return;
        }
        if (!e.legalMoves.Contains(tile.GetCoordinates())) return;
        
        ShowSelectVisuals();
    }

    private void PieceMover_OnPieceDeselected(object sender, EventArgs e)
    {
        HideSelectVisuals();
    }

    private void ShowSelectVisuals()
    {
        if (tile.HasPiece()) selectedPieceVisual.gameObject.SetActive(true);
        else selectedEmptyVisual.gameObject.SetActive(true);
    }

    private void HideSelectVisuals()
    {
        selectedEmptyVisual.gameObject.SetActive(false);
        selectedPieceVisual.gameObject.SetActive(false);
        highlightVisual.gameObject.SetActive(false);
    }
}
