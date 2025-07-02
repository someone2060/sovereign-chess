using System;
using UnityEngine;

/* Uses InputHandler.cs to check if the player has interacted with any pieces */
public class TileSelector : MonoBehaviour
{
    public static TileSelector Instance { get; private set; }

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
        
        Debug.Log("Clicked " + positionWorld); //TODO DEBUG

        var tile = GetTileOnWorld(positionWorld);
        if (tile is null) return;
        
        Debug.Log("Collision happened with tile at " + tile.GetCoordinates()); //TODO DEBUG
        
        if (!tile.HasPiece()) return;

        tile.GetPiece().SelectPiece();
    }

    public static Tile GetTileOnWorld(Vector2 position)
    {
        var collided = Physics2D.OverlapPoint(position);
        
        return collided?.GetComponent<Tile>();
    }
}
