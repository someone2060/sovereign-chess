using System;
using System.Collections.Generic;
using UnityEngine;

public class PawnPromoter : MonoBehaviour
{
    public static PawnPromoter Instance { get; private set; }
    
    private SovereignPieceSO _sovereignPiece;
    private static LayerMask _layerMask;
    private Pawn _pawn;
    private Tile _promotionTile;
    
    public event EventHandler OnShow;
    public event EventHandler<OnPawnPromotionEventArgs> OnPawnPromotion;
    public class OnPawnPromotionEventArgs : EventArgs { public Tile promotionTile; }
    
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
        _pawn = pawn;
        _promotionTile = promotionTile;
        
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
        
        OnShow?.Invoke(this, EventArgs.Empty);
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        InputHandler.Instance.OnSelectCanceled -= InputHandler_OnSelectCanceled;
        
        Collider2D collided = Physics2D.OverlapPoint(
            InputHandler.Instance.GetPositionWorld(Camera.main), _layerMask);

        if (collided is not null)
        {
            PromotionCollider promotionCollider = collided.GetComponent<PromotionCollider>();
            Piece piece = promotionCollider.GetPiece();
            Debug.Log(piece.GetSpriteRenderer().sprite.name);
            OnPawnPromotion?.Invoke(this, new OnPawnPromotionEventArgs { promotionTile = _promotionTile });
        } else
        {
            OnPawnPromotion?.Invoke(this, new OnPawnPromotionEventArgs { promotionTile = _pawn.GetTile() });
        }
    }
}
