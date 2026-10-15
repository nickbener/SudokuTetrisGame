using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private RectTransform _containerRect;
    [SerializeField] private CellView _cellPrefab;

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

    public CellView GetCellView(int x, int y)
    {
        if (x >= 0 && x < _cellViews.GetLength(0) && y >= 0 && y < _cellViews.GetLength(1))
        {
            return _cellViews[x, y];
        }
        return null;
    }
}