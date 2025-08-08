using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Row : MonoBehaviour, IEnumerable<Tile>
{
    [SerializeField] private List<Tile> tiles;
    
    private void Start()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].SetX(i);
        }
    }
    
    public IEnumerator<Tile> GetEnumerator()
    {
        return tiles.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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
