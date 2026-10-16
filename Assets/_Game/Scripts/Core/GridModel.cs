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

    public System.Collections.Generic.List<Vector2Int> CheckAndClearFullLines()
    {
        var clearedCells = new System.Collections.Generic.List<Vector2Int>();
        for (int y = 0; y < _height; y++)
        {
            bool isRowFull = true;
            for (int x = 0; x < _width; x++)
            {
                if (!_cells[x, y])
                {
                    isRowFull = false;
                    break;
                }
            }

            if (isRowFull)
            {
                for (int x = 0; x < _width; x++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (!clearedCells.Contains(pos))
                        clearedCells.Add(pos);
                }
            }
        }
        for (int x = 0; x < _width; x++)
        {
            bool isColFull = true;
            for (int y = 0; y < _height; y++)
            {
                if (!_cells[x, y])
                {
                    isColFull = false;
                    break;
                }
            }

            if (isColFull)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (!clearedCells.Contains(pos))
                        clearedCells.Add(pos);
                }
                }
            }
            foreach (var cellPos in clearedCells)
            {
                _cells[cellPos.x, cellPos.y] = false;
            }

            return clearedCells;
        }
}