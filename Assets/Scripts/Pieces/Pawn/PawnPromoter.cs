using System;
using System.Collections.Generic;
using UnityEngine;

public class PawnPromoter : MonoBehaviour
{
    public static PawnPromoter Instance { get; private set; }
    
    private SovereignPieceSO _sovereignPiece;
    private static LayerMask _layerMask;
    
    public event EventHandler OnShow;
    public event EventHandler OnHide;
    
    public event EventHandler OnPawnPromotion;
    
    private void Awake()
    {
        Instance = this;
        _layerMask = 1 << LayerMask.NameToLayer("Pawn Promotion");
    }
    
    public SovereignPieceSO GetSovereignPiece() => _sovereignPiece;

    public void PromptPawnPromotion(Pawn pawn, Tile promotionTile)
    {
        transform.position = promotionTile.transform.position;
        _sovereignPiece = pawn.GetSovereignPiece();
        
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
        
        OnShow?.Invoke(this, EventArgs.Empty);
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        Physics2D.OverlapPoint(InputHandler.Instance.GetPositionWorld(Camera.main));
    }
}
