using System;
using UnityEngine;

public class PawnPromoter : MonoBehaviour
{
    public static PawnPromoter Instance { get; private set; }

    [SerializeField] private Transform pawnParent;
    [SerializeField] private LayerMask layerMask;
    
    private SovereignPieceSO _sovereignPieceSO;
    private Pawn _pawn;
    private Tile _promotionTile;
    
    public event EventHandler OnShow;
    public event EventHandler<OnPawnPromotionEventArgs> OnPawnPromotion;
    public class OnPawnPromotionEventArgs : EventArgs { public Tile tile; }
    
    private void Awake()
    {
        Instance = this;
    }
    
    public SovereignPieceSO GetSovereignPiece() => _sovereignPieceSO;

    public void PromptPawnPromotion(Pawn pawn, Tile promotionTile)
    {
        transform.position = promotionTile.transform.position;
        _sovereignPieceSO = pawn.GetSovereignPiece();
        _pawn = pawn;
        _promotionTile = promotionTile;
        
        InputHandler.Instance.OnSelectCanceled += InputHandler_OnSelectCanceled;
        
        OnShow?.Invoke(this, EventArgs.Empty);
    }

    private void InputHandler_OnSelectCanceled(object sender, EventArgs e)
    {
        InputHandler.Instance.OnSelectCanceled -= InputHandler_OnSelectCanceled;
        
        Collider2D collided = Physics2D.OverlapPoint(
            InputHandler.Instance.GetPositionWorld(Camera.main), layerMask);

        if (collided is null)
        {
            OnPawnPromotion?.Invoke(this, new OnPawnPromotionEventArgs { tile = _pawn.GetTile() });
            return;
        }

        PromotionCollider promotionCollider = collided.GetComponent<PromotionCollider>();
        GameObject piece = promotionCollider.GetPiece().gameObject;

        InstantiateNewPiece(piece);
        _pawn.DestroySelf();
        
        OnPawnPromotion?.Invoke(this, new OnPawnPromotionEventArgs { tile = null });
    }

    private void InstantiateNewPiece(GameObject piece)
    {
        GameObject newPiece = Instantiate(piece, pawnParent);
        newPiece.GetComponent<Piece>().SetAlignment(_pawn.GetAlignment());
        newPiece.GetComponent<Piece>().SetSovereignPiece(_sovereignPieceSO);
        newPiece.GetComponent<Piece>().InitializeSprite();
        if (_promotionTile.HasPiece())
        {
            _promotionTile.DestroyPiece();
        }
        newPiece.GetComponent<Piece>().SetTile(_promotionTile);
        newPiece.GetComponent<Piece>().CentreOnTile();
    }
}
