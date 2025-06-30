using System;
using UnityEngine;

/* Uses InputHandler.cs to check if the player wants to move any piece */
public class PieceSelectInput : MonoBehaviour
{
    public static PieceSelectInput Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputHandler.Instance.OnSelectPerformed += InputHandler_OnSelectPerformed;
    }

    private void InputHandler_OnSelectPerformed(object sender, EventArgs e)
    {
        var positionWorld = InputHandler.Instance.GetPositionWorld(Camera.main);
        var collided = Physics2D.OverlapPoint(positionWorld);
        
        Debug.Log("Clicked " + positionWorld);
        
        var tile = collided?.GetComponent<Tile>();
        if (tile is null) return;
        
        Debug.Log("Collision happened with tile at " + tile.GetCoordinates());
        
        if (!tile.HasPiece()) return;

        tile.GetPiece().SetSelected(true);
    }
}
