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
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        PawnPromoter.Instance.OnShow += PawnPromoter_OnShow;
        PawnPromoter.Instance.OnPawnPromotion += PawnPromoter_OnPawnPromotion;
    }

    private void PawnPromoter_OnPawnPromotion(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
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
    }
    
    
}
