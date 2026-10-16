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

    //public int Limit;

    public GameLoopController(GridModel gridModel, GridView gridView, ShapesSpawner shapesSpawner)
    {
        _gridModel = gridModel;
        _gridView = gridView;
        _shapesSpawner = shapesSpawner;
    }

    public void Initialize()
    {
        _gridView.BuildGrid(_gridModel.Width, _gridModel.Height);
        _slots = GameObject.FindObjectsOfType<ShapeSlotView>();
        foreach (var slot in _slots)
        {
            slot.OnShapeDragStarted += OnShapeDragStarted;
            slot.OnShapeDropped += OnShapeDropped;
        }

        RefillSlots();
    }

    private void RefillSlots()
    {
        SpawnAndDistributeShapes();
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

    private void OnShapeDragStarted(ShapeView shapeView)
    {
        shapeView.ApplyBlockSize(_gridView.CurrentCellSize, _gridView.CurrentSpacing);
    }

    private void OnShapeDropped(ShapeSlotView slot, ShapeView shapeView)
    {
        List<CellView> targetCells = new List<CellView>();
        bool canPlace = true;

        // Проверяем каждый кубик фигуры
        foreach (RectTransform block in shapeView.SpawnedBlocks)
        {
            CellView cellUnderBlock = _gridView.GetCellUnderPosition(block.position);
            if (cellUnderBlock == null)
            {
                canPlace = false;
                break;
            }

            int x = cellUnderBlock.Coordinates.x;
            int y = cellUnderBlock.Coordinates.y;
            if (_gridModel.IsCellOccupied(x, y))
            {
                canPlace = false;
                break;
            }
            if (targetCells.Contains(cellUnderBlock))
            {
                canPlace = false;
                break;
            }
            targetCells.Add(cellUnderBlock);
        }
        if (canPlace && targetCells.Count == shapeView.SpawnedBlocks.Count)
        {
            foreach (CellView cell in targetCells)
            {
                _gridModel.SetCellOccupied(cell.Coordinates.x, cell.Coordinates.y, true);
                cell.SetState(true);
            }
            slot.ClearSlot();
            CheckIfAllSlotsEmpty();
        }
        else
        {
            shapeView.ReturnToSlot();
        }
    }

    private void CheckIfAllSlotsEmpty()
    {
        bool allEmpty = true;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].IsEmpty)
            {
                allEmpty = false;
                break;
            }
        }
        if (allEmpty)
        {
            RefillSlots();
        }
    }

    private void SpawnAndDistributeShapes()
    {
        _currentActiveShapes = _shapesSpawner.SpawnNewThreeShapes();
    }
}