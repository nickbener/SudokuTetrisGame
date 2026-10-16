using System.Collections.Generic;
using UnityEngine;

public class ShapesSpawner
{
    private readonly ShapesDatabaseSO _database;
    private List<ShapeModel> _currentActiveShapes;

    public ShapesSpawner(ShapesDatabaseSO database)
    {
        _database = database;
        _currentActiveShapes = new List<ShapeModel>();
    }

    public List<ShapeModel> SpawnNewThreeShapes()
    {
        _currentActiveShapes.Clear();

        if (_database == null || _database.AllShapes.Count == 0)
        {
            return _currentActiveShapes;
        }

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, _database.AllShapes.Count);
            ShapeConfigSO randomConfig = _database.AllShapes[randomIndex];
            ShapeModel shapeModel = new ShapeModel(randomConfig);
            _currentActiveShapes.Add(shapeModel);
        }

        return _currentActiveShapes;
    }
}
