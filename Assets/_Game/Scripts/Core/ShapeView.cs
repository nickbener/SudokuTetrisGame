using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShapeView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image _blockPrefab;
    public float _cellSize;
    public float _spacing;
    private ShapeModel _model;
    private List<RectTransform> _spawnedBlocks = new List<RectTransform>();
    private float _currentAngle = 0f;

    private Vector3 _startPosition;
    private Transform _startParent;
    private Canvas _parentCanvas;
    public event Action<ShapeView> OnDragStarted;
    public event Action<ShapeView> OnDragging;
    public event Action<ShapeView> OnDragEnded;
    public ShapeModel Model => _model;
    public List<RectTransform> SpawnedBlocks => _spawnedBlocks;

    private bool _isAnimating = false;

    private void Awake()
    {
        _parentCanvas = GetComponentInParent<Canvas>();
    }
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

        int minX = 5, maxX = -1, minY = 5, maxY = -1;

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
                RectTransform rect = block.GetComponent<RectTransform>();
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                _spawnedBlocks.Add(rect);
            }
        }
        ApplyBlockSize(_cellSize, _spacing);
    }

    public void ApplyBlockSize(float cellSize, float spacing)
    {
        if (_model == null) return;

        float step = cellSize + spacing;
        int minX = 5, maxX = -1, minY = 5, maxY = -1;

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

        float centerX = (minX + maxX) / 2f;
        float centerY = (minY + maxY) / 2f;

        int blockIndex = 0;
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (!_model.IsCellActive(x, y)) continue;

                if (blockIndex < _spawnedBlocks.Count)
                {
                    RectTransform rect = _spawnedBlocks[blockIndex];
                    rect.sizeDelta = new Vector2(cellSize, cellSize);
                    rect.anchoredPosition = new Vector2((x - centerX) * step, (y - centerY) * step);
                    blockIndex++;
                }
            }
        }
    }

    public void SetDragScale(float gridCellSize)
    {
        if (gridCellSize <= 0.1f) return;
        float scaleFactor = gridCellSize / _cellSize;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        transform.localEulerAngles = new Vector3(0f, 0f, _currentAngle);
    }

    public void RotateVisual()
    {
        if (_isAnimating) return;
        _currentAngle -= 90f;
        transform.DOKill(); 
        transform.DORotate(new Vector3(0f, 0f, _currentAngle), 0.15f).SetEase(Ease.OutQuad);
    }

    // --- ЛОГИКА DRAG AND DROP ---
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_parentCanvas == null)
            _parentCanvas = GetComponentInParent<Canvas>();

        if (_isAnimating) return;

        _startPosition = transform.position;
        _startParent = transform.parent;
        if (_parentCanvas != null)
        {
            transform.SetParent(_parentCanvas.transform, true);
            transform.localEulerAngles = new Vector3(0f, 0f, _currentAngle); // Фиксируем угол
        }

        OnDragStarted?.Invoke(this);
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        transform.position = eventData.position;
        transform.localEulerAngles = new Vector3(0f, 0f, _currentAngle);
            
        OnDragging?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;
        OnDragEnded?.Invoke(this);
    }

    public void MoveToGridAndFinish(Vector3 targetWorldPos, Action onCompleteCallback)
    {
        _isAnimating = true;
        transform.DOMove(targetWorldPos, 0.12f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _isAnimating = false;
            onCompleteCallback?.Invoke();
        });
    }

    public void ReturnToSlot()
    {
        _isAnimating = true;
        transform.SetParent(_startParent, true);

        transform.DOMove(_startPosition, 0.25f).SetEase(Ease.OutBack);
        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _isAnimating = false;
            transform.localPosition = Vector3.zero; // Доворачиваем ровно в центр слота
            transform.localEulerAngles = new Vector3(0f, 0f, _currentAngle);
        });
    }
}
