using UnityEngine;

public class ShapeModel
{
    private readonly ShapeConfigSO _config;
    private bool[,] _currentGrid;
    public Vector2Int CenterOffset { get; private set; }
    public ShapeModel(ShapeConfigSO config)
    {
        _config = config;
        _currentGrid = new bool[5, 5];
        
        int minX = 5, maxX = -1, minY = 5, maxY = -1;

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                bool isActive = _config.IsCellActive(x, y);
                _currentGrid[x, y] = isActive;

                if (isActive)
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }

        if (maxX >= minX && maxY >= minY)
        {
            CenterOffset = new Vector2Int((minX + maxX) / 2, (minY + maxY) / 2);
        }
        else
        {
            CenterOffset = new Vector2Int(2, 2);
        }
    }

    public bool IsCellActive(int x, int y)
    {
        if (x < 0 || x >= 5 || y < 0 || y >= 5) return false;
        return _currentGrid[x, y];
    }

    public void Rotate()
    {
        bool[,] newGrid = new bool[5, 5];

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                newGrid[y, 4 - x] = _currentGrid[x, y];
            }
        }

        _currentGrid = newGrid;
    }

    public ShapeModel Clone()
    {
        ShapeModel copy = new ShapeModel(_config);
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                copy._currentGrid[x, y] = this._currentGrid[x, y];
            }
        }
        return copy;
    }
}
