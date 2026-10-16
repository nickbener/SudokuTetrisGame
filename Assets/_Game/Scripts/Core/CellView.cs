using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] private Image _blockImage;
    private bool _isOccupied;
    public Vector2Int Coordinates { get; private set; }
    //privite staic bool _isDragging;

    public void Initialize(int x, int y)
    {
        Coordinates = new Vector2Int(x, y);
        SetState(false);
    }

    public void SetState(bool isOccupied)
    {
        _isOccupied = isOccupied;
        _blockImage.gameObject.SetActive(isOccupied);

        if (isOccupied)
        {
            Color c = _blockImage.color;
            c.a = 1f;
            _blockImage.color = c;
        }
    }

    public void SetPreview(bool showPreview)
    {
            if (_isOccupied) return;

        if (showPreview)
        {
            _blockImage.gameObject.SetActive(true);
            Color c = _blockImage.color;
            c.a = 0.4f;
            _blockImage.color = c;
        }
        else
        {
            _blockImage.gameObject.SetActive(false);
        }
    }

    public void PlayClearAnimation()
    {
        _isOccupied = false;
        SetState(false);
    }
}