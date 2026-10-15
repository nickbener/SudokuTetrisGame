using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] private Image _blockImage;
    public Vector2Int Coordinates { get; private set; }

    public void Initialize(int x, int y)
    {
        Coordinates = new Vector2Int(x, y);
        SetState(false);
    }

    public void SetState(bool isOccupied)
    {
        _blockImage.gameObject.SetActive(isOccupied);
    }
}