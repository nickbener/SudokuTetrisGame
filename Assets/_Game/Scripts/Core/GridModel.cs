using UnityEngine;

public class GridModel
{
    private readonly int _width;
    private readonly int _height;
    private readonly bool[,] _cells;

    public int Width => _width;
    public int Height => _height;

    public GridModel(GameConfigSO config)
    {
        _width = config.Width;
        _height = config.Height;
        _cells = new bool[_width, _height];
    }

    public bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }

    public bool IsCellOccupied(int x, int y)
    {
        if (!IsValidCoordinate(x, y)) return true;
        return _cells[x, y];
    }

    public void SetCellOccupied(int x, int y, bool isOccupied)
    {
        if (IsValidCoordinate(x, y))
        {
            _cells[x, y] = isOccupied;
        }
    }
}