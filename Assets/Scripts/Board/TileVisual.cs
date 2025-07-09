using System;
using UnityEngine;

public class TileVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer selectedEmptyVisual;
    [SerializeField] private SpriteRenderer selectedPieceVisual;
    [SerializeField] private Tile tile;
    
    private void Start()
    {
        HideSelectVisuals();
        TileSelector.Instance.OnPieceSelected += TileSelector_OnPieceSelected;
    }

    private void TileSelector_OnPieceSelected(object sender, TileSelector.OnPieceSelectedEventArgs e)
    {
        if (!e.piece.LegalMoves().Contains(tile.GetCoordinates().GetVector2Int())) return;
        
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
        ShowSelectVisuals();
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        InputHandler.Instance.OnSelectCanceled -= InputHandler_OnSelectCanceled;
        HideSelectVisuals();
    }

    private void ShowSelectVisuals()
    {
        if (tile.HasPiece()) selectedPieceVisual.gameObject.SetActive(true);
        else selectedEmptyVisual.gameObject.SetActive(false);
    }

    private void HideSelectVisuals()
    {
        selectedEmptyVisual.gameObject.SetActive(false);
        selectedPieceVisual.gameObject.SetActive(false);
    }
}
