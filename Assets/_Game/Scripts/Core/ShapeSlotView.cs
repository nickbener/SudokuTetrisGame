using System;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSlotView : MonoBehaviour
{
    public ShapeView _shapeViewPrefab;
    public Transform _spawnPoint;
    public Button _rotateButton;
    private ShapeModel _currentModel;
    private ShapeView _spawnedShapeView;
    public event Action<ShapeView> OnShapeDragStarted;
    public event Action<ShapeView> OnShapeDragging;
    public event Action<ShapeSlotView, ShapeView> OnShapeDropped;

    public bool IsEmpty => _currentModel == null;
    public ShapeModel CurrentModel => _currentModel;

    private void Awake()
    {
        if (_rotateButton != null)
        {
            _rotateButton.onClick.AddListener(OnRotateClicked);
        }
    }

    public void InitSlot(ShapeModel model)
    {
       _currentModel = model;

        if (_rotateButton != null)
        {
            _rotateButton.gameObject.SetActive(true);
            _rotateButton.interactable = true;
        }

        if (_spawnedShapeView != null)
        {
            Destroy(_spawnedShapeView.gameObject);
        }

        if (_currentModel != null && _shapeViewPrefab != null)
        {
            _spawnedShapeView = Instantiate(_shapeViewPrefab, _spawnPoint);
            _spawnedShapeView.Setup(_currentModel);
            _spawnedShapeView.OnDragStarted += HandleDragStarted;
            _spawnedShapeView.OnDragging += HandleDragging;
            _spawnedShapeView.OnDragEnded += HandleDragEnded;
        }
    }

    private void HandleDragStarted(ShapeView view)
    {
        if (_rotateButton != null)
            _rotateButton.interactable = false;

        OnShapeDragStarted?.Invoke(view);
    }

    private void HandleDragging(ShapeView view)
    {
        OnShapeDragging?.Invoke(view);
    }

    private void HandleDragEnded(ShapeView view)
    {
        OnShapeDropped?.Invoke(this, view);
    }

    public void ClearSlot()
    {
        _currentModel = null;
        if (_spawnedShapeView != null)
        {
            Destroy(_spawnedShapeView.gameObject);
            _spawnedShapeView = null;
        }

        if (_rotateButton != null)
                _rotateButton.gameObject.SetActive(false);
    }

    private void OnRotateClicked()
    {
        if (_currentModel != null && _spawnedShapeView != null)
        {
            _currentModel.Rotate();
            _spawnedShapeView.RotateVisual();
        }
    }

    private void OnDestroy()
    {
        if (_rotateButton != null)
        {
            _rotateButton.onClick.RemoveListener(OnRotateClicked);
        }
    }
}
