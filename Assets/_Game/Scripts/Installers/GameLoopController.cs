using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

public class GameLoopController : IInitializable, ITickable
{
    private readonly GridModel _gridModel;
    private readonly GridView _gridView;
    private readonly ShapesSpawner _shapesSpawner;
    private readonly GameConfigSO _config;
    private readonly ScoreView _scoreView;
    private readonly ISaveService _saveService;
    private readonly PauseView _pauseView;
    private ShapeSlotView[] _slots; 

    private List<CellView> _currentPreviewCells = new List<CellView>();
    private ShapeView _currentDraggedShape;
    private int _currentScore = 0;
    private List<ShapeModel> _currentActiveShapes;
    private ShapeModel _currentlyDraggedShape;

    //public int Limit;

    public GameLoopController(GridModel gridModel, GridView gridView, ShapesSpawner shapesSpawner, GameConfigSO config, ScoreView scoreView, ISaveService saveService, PauseView pauseView)
    {
        _gridModel = gridModel;
        _gridView = gridView;
        _shapesSpawner = shapesSpawner;
        _config = config;
        _scoreView = scoreView;
        _saveService = saveService;
        _pauseView = pauseView;
    }

    public void Initialize()
    {
        _gridView.BuildGrid(_gridModel.Width, _gridModel.Height);
        _currentScore = 0;
        List<int> topScores = _saveService.LoadTopScores();
        _scoreView.UpdateScore(_currentScore);
        _pauseView.SetPauseState(false);
        _pauseView.ShowGameOver(false);
        _slots = GameObject.FindObjectsOfType<ShapeSlotView>();
        foreach (var slot in _slots)
        {
            slot.OnShapeDragStarted += OnShapeDragStarted;
            slot.OnShapeDragging += OnShapeDragging;
            slot.OnShapeDropped += OnShapeDropped;
        }

        RefillSlots();
    }

    public void Tick()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (_currentDraggedShape != null)
            {
                _currentDraggedShape.Model.Rotate();
                _currentDraggedShape.RotateVisual();
                UpdateHoverPreview(_currentDraggedShape);
            }
        }
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    private bool _isPaused = false;

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        _pauseView.SetPauseState(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
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
    }

    private void OnShapeDragStarted(ShapeView shapeView)
    {
        _currentDraggedShape = shapeView;
        shapeView.SetDragScale(_gridView.CurrentCellSize);
    }

    private void OnShapeDragging(ShapeView shapeView)
    {
        UpdateHoverPreview(shapeView);
    }

    private void UpdateHoverPreview(ShapeView shapeView)
    {
        ClearPreview();
        if (TryGetValidTargetCells(shapeView, out List<CellView> validCells))
        {
            foreach (var cell in validCells)
            {
                cell.SetPreview(true);
                _currentPreviewCells.Add(cell);
            }
        }
    }

    private void ClearPreview()
    {
        for (int i = 0; i < _currentPreviewCells.Count; i++)
        {
            _currentPreviewCells[i].SetPreview(false);
        }
        _currentPreviewCells.Clear();
    }

    private void OnShapeDropped(ShapeSlotView slot, ShapeView shapeView)
    {
        _currentDraggedShape = null;
        ClearPreview();

        if (TryGetValidTargetCells(shapeView, out List<CellView> targetCells))
        {
            foreach (CellView cell in targetCells)
            {
                _gridModel.SetCellOccupied(cell.Coordinates.x, cell.Coordinates.y, true);
                cell.SetState(true);
            }

            slot.ClearSlot();
            List<Vector2Int> clearedCoords = _gridModel.CheckAndClearFullLines();
            if (clearedCoords.Count > 0)
            {
                foreach (Vector2Int coords in clearedCoords)
                {
                    CellView cellToClear = _gridView.GetCellView(coords.x, coords.y);
                    if (cellToClear != null)
                    {
                        cellToClear.PlayClearAnimation();
                    }
                }

                int earnedPoints = clearedCoords.Count * _config.PointsPerСell;
                _currentScore += earnedPoints;
                _scoreView.UpdateScore(_currentScore);
                _saveService.SaveScore(_currentScore);
                
                Debug.Log($"Сгорело ячеек: {clearedCoords.Count}! Получено очков: {earnedPoints}. Всего очков: {_currentScore}");
            }
            CheckIfAllSlotsEmpty();
            CheckForGameOver();
        }
        else
        {
            shapeView.ReturnToSlot();
        }
    }

    private void CheckForGameOver()
      {
          bool canMove = false;
          for (int i = 0; i < _slots.Length; i++)
          {
              if (!_slots[i].IsEmpty)
              {
                  if (_gridModel.CanPlaceShapeAnywhere(_slots[i].CurrentModel))
                  {
                      canMove = true;
                      break;
                  }
              }
          }

          if (!canMove)
          {
              _saveService.SaveScore(_currentScore);
              _pauseView.ShowGameOver(true);
          }
      }

    private bool TryGetValidTargetCells(ShapeView shapeView, out List<CellView> targetCells)
    {
        targetCells = new List<CellView>();

        foreach (RectTransform block in shapeView.SpawnedBlocks)
        {
            CellView cellUnderBlock = _gridView.GetCellUnderPosition(block.position);

            if (cellUnderBlock == null) return false;

            int x = cellUnderBlock.Coordinates.x;
            int y = cellUnderBlock.Coordinates.y;

            if (_gridModel.IsCellOccupied(x, y)) return false;

            if (targetCells.Contains(cellUnderBlock)) return false;

            targetCells.Add(cellUnderBlock);
        }

        return targetCells.Count == shapeView.SpawnedBlocks.Count;
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