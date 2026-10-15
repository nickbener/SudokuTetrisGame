using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShapeConfig", menuName = "Configs/ShapeConfig")]
public class ShapeConfigSO : ScriptableObject
{
    [SerializeField] private List<Vector2Int> _shapeBlocks;
}
