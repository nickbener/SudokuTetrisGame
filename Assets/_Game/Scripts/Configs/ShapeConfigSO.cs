using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShapeConfig", menuName = "Configs/ShapeConfig")]
public class ShapeConfigSO : ScriptableObject
{

    [HideInInspector] 
    [SerializeField] private bool[] _cells = new bool[25];
    public bool IsCellActive(int x, int y)
    {
        if (x < 0 || x > 4 || y < 0 || y > 4) return false;
        return _cells[y * 5 + x];
    }

    public void SetCellActive(int x, int y, bool isActive)
    {
        if (x >= 0 && x <= 4 && y >= 0 && y <= 4)
        {
            _cells[y * 5 + x] = isActive;
        }
    }

    public bool GetCellActive(int x, int y)
    {
        return _cells[y * 5 + x];
    }
}
