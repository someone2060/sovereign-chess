using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Serves as a "hub" for piece alignment management so that each piece doesn't need to link to each individual sovereign tile.
 */
public class AlignmentManager : MonoBehaviour
{
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

    private void Start()
    {
        // PieceMover.Instance.OnPieceSelected += PieceMover_OnPieceSelected; // FOR DEBUGGING
    }

    public void ChangeAlignment(SovereignPieceSO sovereignPiece, Piece.Alignment newAlignment)
    {
        OnAlignmentChange?.Invoke(this, new OnAlignmentChangeEventArgs
        {
            sovereignPiece = sovereignPiece,
            newAlignment = newAlignment
        });
    }
}
