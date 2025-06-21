using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [SerializeField] private List<Tile> tiles;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].GetCoordinates().SetY(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTilesCoordinateX(int x)
    {
        foreach (var t in tiles)
        {
            t.GetCoordinates().SetX(x);
        }
    }

    public Tile GetTile(int y) => tiles[y];
}
