using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeView : MonoBehaviour
{
    public Image _blockPrefab;
    public float _cellSize;
    public float _spacing;
    private ShapeModel _model;
    private List<GameObject> _spawnedBlocks = new List<GameObject>();
    private float _currentAngle = 0f;

    public void Setup(ShapeModel model)
    {
        _model = model;
        _currentAngle = 0f;
        transform.localEulerAngles = Vector3.zero;
        
        for (int i = 0; i < _spawnedBlocks.Count; i++)
        {
            Destroy(_spawnedBlocks[i]);
        }

        _spawnedBlocks.Clear();
        SpawnBlocks();
    }

    private void SpawnBlocks()
    {
        if (_model == null) return;

        float step = _cellSize + _spacing;
        int minX = 5, maxX = -1;
        int minY = 5, maxY = -1;

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (_model.IsCellActive(x, y))
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }
        if (maxX < minX || maxY < minY) return;
        float centerX = (minX + maxX) / 2f;
        float centerY = (minY + maxY) / 2f;

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (!_model.IsCellActive(x, y)) continue;

                Image block = Instantiate(_blockPrefab, transform);
                float offsetX = x - centerX;
                float offsetY = y - centerY;

                RectTransform rect = block.GetComponent<RectTransform>();
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);

                rect.sizeDelta = new Vector2(_cellSize, _cellSize);
                rect.anchoredPosition = new Vector2(offsetX * step, offsetY * step);

                _spawnedBlocks.Add(block.gameObject);
            }
        }
    }
        
    public void RotateVisual()
    {
        _currentAngle -= 90f;
        transform.localEulerAngles = new Vector3(0f, 0f, _currentAngle);
    }
}
