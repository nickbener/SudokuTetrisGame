using UnityEngine;
using UnityEngine.UI;

public class ShapeSlotView : MonoBehaviour
{
    public ShapeView _shapeViewPrefab;
    public Transform _spawnPoint;
    public Button _rotateButton;

    private ShapeModel _currentModel;
    private ShapeView _spawnedShapeView;

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
        if (_spawnedShapeView != null)
        {
            Destroy(_spawnedShapeView.gameObject);
        }

        if (_currentModel != null && _shapeViewPrefab != null)
        {
            _spawnedShapeView = Instantiate(_shapeViewPrefab, _spawnPoint);
            _spawnedShapeView.Setup(_currentModel);
        }
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
