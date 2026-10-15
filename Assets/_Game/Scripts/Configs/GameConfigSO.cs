using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfigSO : ScriptableObject
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int pointsPerСell;

    public int Width => _width;
    public int Height => _width;
    public int PointsPerСell => _width;
}