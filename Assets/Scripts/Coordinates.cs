using UnityEngine;

public class Coordinates
{
    private Vector2Int _coordinates;

    public Coordinates(int x, int y)
    {
        _coordinates = new Vector2Int(x, y);
    }

    public Coordinates(Vector2Int coordinates)
    {
        _coordinates = coordinates;
    }

    public Coordinates(Coordinates coordinates)
    {
        _coordinates = coordinates.GetVector2Int();
    }
    
    private bool Equals(Coordinates other)
    {
        return _coordinates.Equals(other._coordinates);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Coordinates)obj);
    }

    public override int GetHashCode()
    {
        return _coordinates.GetHashCode();
    }

    public override string ToString()
    {
        return (char)(_coordinates.x + 97) + (_coordinates.y + 1).ToString();
    }
    
    public Vector2Int GetVector2Int() => _coordinates;
    public void SetX(int x) => _coordinates.x = x;
    public void SetY(int y) => _coordinates.y = y;
}
