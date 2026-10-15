using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameLoopController : IInitializable
{
    private readonly GridModel _gridModel;
    private readonly GridView _gridView;
    private readonly ShapesSpawner _shapesSpawner;
    private ShapeSlotView[] _slots; 
    private List<ShapeModel> _currentActiveShapes;
    private ShapeModel _currentlyDraggedShape;

    public GameLoopController(GridModel gridModel, GridView gridView, ShapesSpawner shapesSpawner)
    {
        _gridModel = gridModel;
        _gridView = gridView;
        _shapesSpawner = shapesSpawner;
    }

    public void Initialize()
    {
        _gridView.BuildGrid(_gridModel.Width, _gridModel.Height);
        SpawnAndDistributeShapes();
        _slots = GameObject.FindObjectsOfType<ShapeSlotView>();
        RefillSlots();
    }

    private void RefillSlots()
    {
        List<ShapeModel> newShapes = _shapesSpawner.SpawnNewThreeShapes();

        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < newShapes.Count)
            {
                _slots[i].InitSlot(newShapes[i]);
            }
        }
        
        Debug.Log("Рандомный спавн отработал! Фигурки улетели в слоты.");
    }

    private void SpawnAndDistributeShapes()
    {
        _currentActiveShapes = _shapesSpawner.SpawnNewThreeShapes();
    }
}