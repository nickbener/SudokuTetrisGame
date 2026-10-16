using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShapesDatabase", menuName = "Configs/ShapesDatabase")]
public class ShapesDatabaseSO : ScriptableObject
{
    [SerializeField] private List<ShapeConfigSO> _allShapes = new List<ShapeConfigSO>();

    public IReadOnlyList<ShapeConfigSO> AllShapes => _allShapes;
}
