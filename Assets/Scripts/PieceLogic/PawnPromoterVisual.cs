using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PawnPromoterVisual : MonoBehaviour
{
    public static PawnPromoterVisual Instance { get; private set; }
    
    [SerializeField] private SpriteRenderer queenSprite;
    [SerializeField] private SpriteRenderer rookSprite;
    [SerializeField] private SpriteRenderer bishopSprite;
    [SerializeField] private SpriteRenderer kingSprite;
    [SerializeField] private SpriteRenderer knightSprite;
    [SerializeField] private LayerMask layerMask;

    private bool _isActive;
    private PromotionColliderVisual _promotionColliderVisual;

    public event EventHandler<OnHoverEventArgs> OnHover;

    public class OnHoverEventArgs : EventArgs
    {
        public PromotionColliderVisual promotionColliderVisual;
    }
    
    private void Awake()
    {
        Instance = this;
        _isActive = false;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        PawnPromoter.Instance.OnShow += PawnPromoter_OnShow;
        PawnPromoter.Instance.OnPawnPromotion += PawnPromoter_OnPawnPromotion;
    }

    private void Update()
    {
        if (!_isActive) return;
        
        Vector2 userPosition = InputHandler.Instance.GetPositionWorld(Camera.main);
        Collider2D collided = Physics2D.OverlapPoint(userPosition, layerMask);
        PromotionColliderVisual promotionColliderVisual = collided?.GetComponent<PromotionColliderVisual>();

        if (promotionColliderVisual is null && _promotionColliderVisual is null) return;
        if (promotionColliderVisual is not null && _promotionColliderVisual is not null)
        {
            if (_promotionColliderVisual.Equals(promotionColliderVisual)) return;
        }
        
        _promotionColliderVisual = promotionColliderVisual;
        OnHover?.Invoke(this, new OnHoverEventArgs{ promotionColliderVisual = _promotionColliderVisual });
    }

    private void PawnPromoter_OnPawnPromotion(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
        _isActive = false;
    }

    private void PawnPromoter_OnShow(object sender, EventArgs e)
    {
        SovereignPieceSO sovereignPiece = PawnPromoter.Instance.GetSovereignPiece();
        queenSprite.sprite = sovereignPiece.queenSprite;
        rookSprite.sprite = sovereignPiece.rookSprite;
        bishopSprite.sprite = sovereignPiece.bishopSprite;
        kingSprite.sprite = sovereignPiece.kingSprite;
        knightSprite.sprite = sovereignPiece.knightSprite;

        gameObject.SetActive(true);
        _isActive = true;
    }
}
