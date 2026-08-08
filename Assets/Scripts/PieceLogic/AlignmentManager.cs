using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Serves as a "hub" for piece alignment management so that each piece doesn't need to link to each individual sovereign tile.
 */
public class AlignmentManager : MonoBehaviour
{
    [SerializeField] private King whiteKing;
    [SerializeField] private King blackKing;
    
    public event EventHandler<OnAlignmentChangeEventArgs> OnAlignmentChange;
    public class OnAlignmentChangeEventArgs : EventArgs
    {
        public SovereignPieceSO sovereignPiece;
        public Piece.Alignment newAlignment;
    }
    
    public static AlignmentManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeAlignment(SovereignPieceSO sovereignPiece, Piece.Alignment newAlignment)
    {
        if (sovereignPiece == whiteKing.GetSovereignPiece() || sovereignPiece == blackKing.GetSovereignPiece()) return;
        Debug.Log("broadcasting " + newAlignment + " to " + sovereignPiece.id);
        OnAlignmentChange?.Invoke(this, new OnAlignmentChangeEventArgs
        {
            sovereignPiece = sovereignPiece,
            newAlignment = newAlignment
        });
    }
}
