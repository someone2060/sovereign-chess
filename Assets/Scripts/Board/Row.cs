using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [SerializeField] private List<Tile> tiles;
    
    private void Start()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].SetX(i);
        }
    }

    public void SetTilesCoordinateY(int x)
    {
        foreach (Tile t in tiles)
        {
            t.SetY(x);
        }
    }

    public Tile GetTile(int x) => tiles[x];
}
