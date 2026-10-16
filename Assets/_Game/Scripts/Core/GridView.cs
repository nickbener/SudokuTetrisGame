using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    public GridLayoutGroup _gridLayout;
    public RectTransform _containerRect;
     public CellView _cellPrefab;

    public float CurrentCellSize => _gridLayout.cellSize.x;
    public float CurrentSpacing => _gridLayout.spacing.x;

    private CellView[,] _cellViews;

    public void BuildGrid(int width, int height)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _cellViews = new CellView[width, height];
        _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayout.constraintCount = width;

         float containerWidth = _containerRect.rect.width;
        float totalSpacingX = _gridLayout.spacing.x * (width - 1);
        float totalPaddingX = _gridLayout.padding.left + _gridLayout.padding.right;
        float cellSize = (containerWidth - totalSpacingX - totalPaddingX) / width;
        _gridLayout.cellSize = new Vector2(cellSize, cellSize);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                CellView newCell = Instantiate(_cellPrefab, transform);
                newCell.Initialize(x, y);
                _cellViews[x, y] = newCell;
            }
        }
    }

    public CellView GetCellUnderPosition(Vector3 worldPos)
    {
            if (_cellViews == null) return null;

            float maxDist = (_gridLayout.cellSize.x * transform.lossyScale.x) * 0.6f;
            CellView closest = null;
            float minDist = float.MaxValue;

            for (int y = 0; y < _cellViews.GetLength(1); y++)
            {
                for (int x = 0; x < _cellViews.GetLength(0); x++)
                {
                    CellView cell = _cellViews[x, y];
                    float dist = Vector2.Distance(worldPos, cell.transform.position);
                    
                    if (dist < maxDist && dist < minDist)
                    {
                        minDist = dist;
                        closest = cell;
                    }
                }
            }

            return closest;
    }

    // public CellView GetCellView(int x, int y)
    // {
    //     if (x >= 0 && x < _cellViews.GetLength(0) && y >= 0 && y < _cellViews.GetLength(1))
    //     {
    //         return _cellViews[x, y];
    //     }
    //     return null;
    // }
}