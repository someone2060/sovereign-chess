using UnityEngine;

public class SovereignTile : Tile
{
    [SerializeField] private SovereignTile partner; // other tile that shares same sovereign colour
    [SerializeField] private SovereignPieceSO sovereignPiece;
    [SerializeField] private Piece[] pieces;

    private void Start()
    {
        tileVisual.color = sovereignPiece.color;
    }
    
    public override void SetPiece(Piece newPiece)
    {
        base.SetPiece(newPiece);
        if (piece is null)
        {
            AlignmentManager.Instance.ChangeAlignment(sovereignPiece, Piece.Alignment.Neutral);
            return;
        }
        AlignmentManager.Instance.ChangeAlignment(sovereignPiece, newPiece.GetAlignment());
    }

    public SovereignPieceSO GetSovereignPiece()
    {
        return sovereignPiece;
    }

    public override bool LegalTile(Piece.Alignment alignment, bool canMove = true, bool canCapture = true)
    {
        if (partner.HasPiece()) return false;
        return base.LegalTile(alignment, canMove, canCapture);
    }
}