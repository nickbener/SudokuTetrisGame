using System;
using UnityEngine;
using Zenject;

public class GameLoopController : IInitializable
{
    private readonly GridModel _gridModel;
        private readonly GridView _gridView;

        public GameLoopController(GridModel gridModel, GridView gridView)
        {
            _gridModel = gridModel;
            _gridView = gridView;
        }

        public void Initialize()
        {
            _gridView.BuildGrid(_gridModel.Width, _gridModel.Height);
        }
}